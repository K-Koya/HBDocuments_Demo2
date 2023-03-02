using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;
using System;

[RequireComponent(typeof(Animator))]
public class AnimatorAssistant : MonoBehaviour
{
    /// <summary>Animatorのパラメーター名 : Speed</summary>
    protected const string _PARAM_NAME_SPEED = "Speed";

    /// <summary>Animatorのパラメーター名 : IsGround</summary>
    protected const string _PARAM_NAME_IS_GROUND = "IsGround";

    /// <summary>Animatorのパラメーター名 : IsArmed</summary>
    protected const string _PARAM_NAME_IS_ARMED = "IsArmed";

    /// <summary>Animatorのパラメーター名 : DoJump</summary>
    protected const string _PARAM_NAME_DO_JUMP = "DoJump";

    /// <summary>Animatorのパラメーター名 : DoAction</summary>
    protected const string _PARAM_NAME_DO_ACTION = "DoAction";

    /// <summary>Animatorのパラメーター名 : DoDamage</summary>
    protected const string _PARAM_NAME_DO_DAMAGE = "DoDamage";

    /// <summary>Animatorのパラメーター名 : AnimationKind</summary>
    protected const string _PARAM_NAME_ANIMATION_KIND = "AnimationKind";


    /// <summary>該当のキャラクターを移動させるコンポーネント</summary>
    protected CharacterMove _Cm = default;

    /// <summary>該当のアニメーター</summary>
    protected Animator _Am = default;

    [SerializeField, Tooltip("戦闘時だけ出現させるオブジェクト")]
    protected GameObject[] _AppearOnAttack = null;

    [SerializeField, Tooltip("アニメーション中に出すエフェクトのプレハブ")]
    protected GameObject[] _EffectPrefs = null;

    /// <summary>エフェクト集</summary>
    protected GameObjectPool[] _Effects = null;

    [SerializeField, Tooltip("エフェクトを出す座標")]
    protected Transform[] _EffectEmitPoints = null;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Cm = GetComponentInParent<CharacterMove>();

        Timeline ti;
        if (TryGetComponent(out ti))
        {
            _Am = ti.animator.component;
        }
        if (!_Am)
        {
            _Am = GetComponent<Animator>();
        }

        if(_EffectPrefs != null && _EffectPrefs.Length > 0)
        {
            _Effects = new GameObjectPool[_EffectPrefs.Length];
            _Effects = Array.ConvertAll(_EffectPrefs, eff => new GameObjectPool(eff, 2));
        }
    }

    #region アニメーションイベント

    /// <summary>アニメーションイベントにて、アニメーション遷移におけるフリーズ回避のため、待機状態にする</summary>
    public void StateCallStaying()
    {
        _Cm.StateCallStaying();
    }

    /// <summary>アニメーションイベントにて、予備動作に入った情報を受け取る</summary>
    public void ProcessCallPreparation()
    {
        _Cm.ProcessCallPreparation();
    }

    /// <summary>アニメーションイベントにて、本動作に入った情報を受け取る</summary>
    public void ProcessCallPlaying()
    {
        _Cm.ProcessCallPlaying();
    }

    /// <summary>アニメーションイベントにて、動作の空き時間になった情報を受け取る</summary>
    public void ProcessCallInterval()
    {
        _Cm.ProcessCallInterval();
    }

    /// <summary>アニメーションイベントにて、動作終了予定の情報を受け取る</summary>
    public void ProcessCallEndSoon()
    {
        _Cm.ProcessCallEndSoon();
    }

    /// <summary>アニメーションイベントにて、エフェクトを発生させる命令を受ける</summary>
    /// <param name="twoIds">上2桁:エフェクトの配列番号 下2桁:射出位置の配列番号</param>
    public void EmitEffect(int twoIds)
    {
        int effectIndex = twoIds / 100;
        int emitIndex = twoIds % 100;

        GameObject obj = _Effects[effectIndex].Instansiate();
        if (obj)
        {
            obj.transform.position = _EffectEmitPoints[emitIndex].position;
            obj.transform.rotation = _EffectEmitPoints[emitIndex].rotation;
        }
    }

    /// <summary>実行中コマンドの打ち出したいオブジェクトをアクティブ化する</summary>
    /// <param name="twoIds">上2桁:攻撃情報テーブルにアクセスしたいカラムID 下2桁:攻撃範囲情報にアクセスしたいカラムID</param>
    public void CommandObjectShootCall(int twoIds)
    {
        _Cm.CommandObjectShootCall(twoIds);
    }

    /// <summary>アニメーションイベントにて、攻撃判定を開始したい旨を受け取る</summary>
    /// <param name="twoIds">上2桁:攻撃情報テーブルにアクセスしたいカラムID 下2桁:攻撃範囲情報にアクセスしたいカラムID</param>
    public void AttackCallStart(int twoIds)
    {
        _Cm.AttackCallStart(twoIds);
    }

    /// <summary>アニメーションイベントにて、攻撃判定を終了したい旨を受け取る</summary>
    public void AttackCallEnd()
    {
        _Cm.AttackCallEnd();
    }

    /// <summary>アニメーションイベントにて、追加実行する指示を出す</summary>
    public void RunningCall()
    {
        _Cm.RunningCall();
    }
    #endregion
}


/// <summary>アニメーションの種類</summary>
public enum AnimationKind : ushort
{
    NoCall = 0,
    Idle,
    Move,
    Jump,
    Fall,

    ShiftSlideFoward = 100,
    ShiftSlideBack,
    ShiftSlideRight,
    ShiftSlideLeft,
    LongTrip,

    GuardGroundFoward = 200,
    GuardGroundBack,
    GuardAirFoward,
    GuardAirBack,

    /* 通常コンボ */
    /// <summary>地上前方近距離</summary>
    ComboGroundFoward = 1000,
    /// <summary>地上前方遠距離</summary>
    ComboGroundFowardFar,
    /// <summary>地上後方</summary>
    ComboGroundBack,
    /// <summary>地上広角</summary>
    ComboGroundWide,
    /// <summary>地上フィニッシュ</summary>
    ComboGroundFinish,
    /// <summary>空中前方近距離</summary>
    ComboAirFoward,
    /// <summary>空中前方遠距離</summary>
    ComboAirFowardFar,
    /// <summary>空中後方</summary>
    ComboAirBack,
    /// <summary>空中広角</summary>
    ComboAirWide,
    /// <summary>空中フィニッシュ</summary>
    ComboAirFinish,

    /* 共通技モーション */
    /// <summary>魔法弾のようなものを発射</summary>
    AttackMagicShoot = 2000,
    /// <summary>レーザーなどをその場から照射</summary>
    AttackLaserShoot = 2001,
    /// <summary>レーザーなどをその場でスイングしつつ照射</summary>
    AttackLaserShootSwing = 2002,

    /// <summary>ラウンドスラッシュ動作</summary>
    AttackRoundSlash = 3000,
    /// <summary>ブレードシュート動作</summary>
    AttackBladeShoot = 3001,


    /*コマンドモーション*/
    /// <summary>スプレーのアイテムを使う動作</summary>
    UseItemSpray = 30000,
    /// <summary>食べるアイテムを使う動作</summary>
    UseItemFood,
    /// <summary>飲むアイテムを使う動作</summary>
    UseItemDrink,

    /// <summary>自分を補助するスキルを発動</summary>
    SupportPowerUp = 50000,
}

/// <summary>ダメージモーションの種類</summary>
public enum DamageKind : byte
{
    /*衝撃なし*/
    /// <summary>ノーリアクション</summary>
    None = 0,

    /*正面*/
    /// <summary>正面から小さな衝撃</summary>
    FrontLittle = 10,
    /// <summary>正面から大きな衝撃</summary>
    FrontBig = 11,

    /*周囲*/
    /// <summary>正面以外から小さな衝撃</summary>
    SroundLittle = 20,
    /// <summary>正面以外から大きな衝撃</summary>
    SroundBig = 21,
}

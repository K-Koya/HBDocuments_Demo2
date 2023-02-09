using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

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

    /// <summary>実行中コマンドの打ち出したいオブジェクトをアクティブ化する</summary>
    /// <param name="index">種類</param>
    public void CommandObjectShootCall(int index)
    {
        _Cm.CommandObjectShootCall(index);
    }

    /// <summary>アニメーションイベントにて、攻撃判定を開始したい旨を受け取る</summary>
    /// <param name="id">AttackPowerTableにアクセスしたいカラムID</param>
    public void AttackCallStart(int id)
    {
        _Cm.AttackCallStart(id);
    }

    /// <summary>アニメーションイベントにて、攻撃判定を終了したい旨を受け取る</summary>
    public void AttackCallEnd()
    {
        _Cm.AttackCallEnd();
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

    UseItemSpray = 300,
    UseItemFood,
    UseItemDrink,

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

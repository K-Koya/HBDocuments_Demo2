using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFireShot : CommandActiveSkillBase
{
    [SerializeField, Tooltip("火炎弾プレハブ")]
    GameObject _FireBallPref = null;

    /// <summary>火炎弾オブジェクトのプール</summary>
    GameObjectPool _FireBalls = null;

    /// <summary>今のコンボの手数</summary>
    byte _Step = 0;

    public CommandFireShot()
    {
        _Name = "ファイアショット";
        _Explain = "火炎弾を照準方向へ放つ。追加入力で5発まで放てるが、放ち終えるまで無防備になる。";
    }

    public override void Initialize()
    {
        if (_FireBallPref)
        {
            _FireBalls = new GameObjectPool(_FireBallPref, 5);
        }
    }

    /// <summary>火炎弾発射メソッド</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    /// <param name="reticleDirection">照準方向</param>
    /// <param name="animKind">要求するアニメーションの種類</param>
    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        //照準方向に向き直る
        rb.transform.forward = Vector3.ProjectOnPlane(reticleDirection, -gravityDirection);

        animKind = AnimationKind.ComboGroundFinish;
        //コンボフィニッシュ
        if (_Step > 4)
        {
            _Step = 1;
        }

        param.State.Kind = MotionState.StateKind.AttackCommand;
        param.State.Process = MotionState.ProcessKind.Preparation;
    }
}

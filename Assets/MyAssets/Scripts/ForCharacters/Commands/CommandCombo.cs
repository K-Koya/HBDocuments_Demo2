using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandCombo : CommandActiveSkillBase
{
    [SerializeField, Tooltip("コンボの手数（フィニッシュを含む）")]
    byte _NumberOfStep = 5;

    /// <summary>今のコンボの手数</summary>
    [SerializeField]
    byte _Step = 0;

    
    public CommandCombo()
    {
        _Name = "通常コンボ";
        _Explain = "通常攻撃。停止時や移動時など特別な行動を起こしていないときに、通常攻撃ボタンで武器を振る。\n攻撃後に続けて振ることもでき、特定回数振ると強めの攻撃が出る。";
    }

    /// <summary>コンボ攻撃を要求するメソッド</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    /// <param name="reticleDirection">照準方向</param>
    /// <param name="animKind">要求するアニメーションの種類</param>
    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        //照準方向に向き直る
        rb.transform.forward = Vector3.ProjectOnPlane(reticleDirection, -gravityDirection);

        //規定数コンボを打ったかで分岐
        //コンボ途中
        if (_Step < _NumberOfStep)
        {
            //走行状態かで分岐
            if (param.State.Kind == MotionState.StateKind.Run)
            {
                animKind = AnimationKind.ComboGroundFowardFar;
            }
            else
            { 
                //キャラクターの正面方向と照準方向の位置関係で分岐
                //前方の場合
                if (Vector3.Dot(param.transform.forward, reticleDirection) > 0f)
                {
                    //照準を合わせている相手がいるかで分岐
                    //いない場合
                    if (param.GazeAt is null)
                    {
                        animKind = AnimationKind.ComboGroundFoward;
                    }
                    //いる場合
                    else
                    {
                        Vector3 reticleNorm = Vector3.Normalize(reticleDirection);
                        //キャラクターの鉛直方向と照準方向の位置関係で分岐
                        //水平に近い
                        if (Vector3.Dot(param.transform.up, reticleNorm) > 0.5f)
                        {
                            //照準を合わせている相手との距離で分岐
                            //近い場合
                            if (reticleDirection.sqrMagnitude < param.Sub.ComboProximityRange * param.Sub.ComboProximityRange)
                            {
                                animKind = AnimationKind.ComboGroundFoward;
                            }
                            //遠い場合
                            else
                            {
                                animKind = AnimationKind.ComboGroundFowardFar;
                            }
                        }
                        //鉛直に近い
                        else
                        {
                            animKind = AnimationKind.ComboAirWide;
                        }
                    }
                }
                //後方の場合
                else
                {
                    animKind = AnimationKind.ComboGroundBack;
                }
            }

            //手数加算
            _Step++;
        }
        //コンボフィニッシュ
        else
        {
            animKind = AnimationKind.ComboGroundFinish;
            _Step = 1;
        }

        param.State.Kind = MotionState.StateKind.ComboNormal;
        param.State.Process = MotionState.ProcessKind.Preparation;
    }

    /// <summary>コンボ手数を0にリセットする</summary>
    public void CountReset()
    {
        _Step = 1;
    }
}

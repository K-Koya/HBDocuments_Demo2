using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCombo : CommandBase, ICSVDataConverter
{
    /// <summary>情報取得対象のCSVファイルパス</summary>
    const string LOAD_CSV_PATH = "CSV/Command/Combo";

    /// <summary>コマンドの種類</summary>
    static CommandKind _Kind = CommandKind.Attack;

    /// <summary>攻撃情報テーブル</summary>
    static AttackPowerColumn[] _AttackPowerTable = null;

    /// <summary>コンボ手数</summary>
    byte _Count = 0;

    /// <summary>今のコンボの手数</summary>
    byte _Step = 0;


    public byte Count { get => _Count; set => _Count = value; }
    public override CommandKind Kind => _Kind;
    protected override AttackPowerColumn[] AttackPowerTable => _AttackPowerTable;


    /// <summary>コンストラクタ</summary>
    public CommandCombo()
    {
        _Kind = CommandKind.Combo;
    }

    public override ushort LoadData()
    {
        CSVToMembers(CSVIO.LoadCSV(LOAD_CSV_PATH));
        return 0;
    }

    public override void Initialize(int layer)
    {

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
        Vector3 reticleNorm = Vector3.Normalize(reticleDirection);

        //規定数コンボを打ったかで分岐
        //コンボ途中
        if (_Step < _Count)
        {
            //走行状態かで分岐
            if (param.State.Kind == MotionState.StateKind.Run)
            {
                animKind = AnimationKind.ComboGroundFowardFar;

                //攻撃方向へ前進
                rb.AddForce(reticleNorm, ForceMode.Impulse);
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
                        animKind = AnimationKind.ComboGroundWide;

                        //攻撃方向へ前進
                        rb.AddForce(reticleNorm, ForceMode.Impulse);
                    }
                    //いる場合
                    else
                    {
                        /*TODO
                        //キャラクターの鉛直方向と照準方向の位置関係で分岐
                        //水平に近い
                        if (Vector3.Dot(param.transform.up, reticleNorm) > 0.75f)
                        {
                            //照準を合わせている相手との距離で分岐
                            //近い場合
                            if (reticleDirection.sqrMagnitude < param.Sub.ComboProximityRange * param.Sub.ComboProximityRange)
                            {
                                animKind = AnimationKind.ComboGroundFoward;

                                //攻撃方向へ前進
                                rb.AddForce(reticleNorm, ForceMode.Impulse);
                            }
                            //遠い場合
                            else
                            {
                                animKind = AnimationKind.ComboGroundFowardFar;

                                //攻撃方向へ前進
                                rb.AddForce(reticleNorm * 2f, ForceMode.Impulse);
                            }
                        }
                        //鉛直に近い
                        else
                        {
                            animKind = AnimationKind.ComboGroundFoward;

                            //攻撃方向へ前進
                            rb.AddForce(reticleNorm, ForceMode.Impulse);
                        }
                        */

                        //照準を合わせている相手との距離で分岐
                        //近い場合
                        if (reticleDirection.sqrMagnitude < param.Sub.ComboProximityRange * param.Sub.ComboProximityRange)
                        {
                            animKind = AnimationKind.ComboGroundFoward;

                            //攻撃方向へ前進
                            rb.AddForce(reticleNorm, ForceMode.Impulse);
                        }
                        //遠い場合
                        else
                        {
                            animKind = AnimationKind.ComboGroundFowardFar;

                            //攻撃方向へ前進
                            rb.AddForce(reticleNorm * 2f, ForceMode.Impulse);
                        }

                        //攻撃方向へ前進
                        rb.AddForce(reticleNorm, ForceMode.Impulse);
                    }
                }
                //後方の場合
                else
                {
                    animKind = AnimationKind.ComboGroundWide;

                    //攻撃方向へ前進
                    rb.AddForce(reticleNorm, ForceMode.Impulse);
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

            //攻撃方向へ前進
            rb.AddForce(reticleNorm * 2f, ForceMode.Impulse);
        }

        param.State.Kind = MotionState.StateKind.ComboNormal;
        param.State.Process = MotionState.ProcessKind.Preparation;
    }

    /// <summary>コンボ手数を0にリセットする</summary>
    public void CountReset()
    {
        _Step = 1;
    }

    public List<string> MembersToCSV()
    {
        throw new System.NotImplementedException();
    }

    public void CSVToMembers(List<string[]> csv)
    {
        _AttackPowerTable = new AttackPowerColumn[csv.Count - 4];
        for (int i = 5; i < csv.Count; i++)
        {
            _AttackPowerTable[i - 5] = new AttackPowerColumn(short.Parse(csv[i][0]), short.Parse(csv[i][1]), short.Parse(csv[i][2]));
        }
    }
}

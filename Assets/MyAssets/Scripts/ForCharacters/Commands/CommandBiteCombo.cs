using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBiteCombo : CommandActiveSkillBase, ICSVDataConverter
{
    /// <summary>情報取得対象のCSVファイルパス</summary>
    const string LOAD_CSV_PATH = "CSV/Command/BiteCombo.csv";

    public override void Initialize()
    {
        CSVToMembers(CSVIO.LoadCSV(LOAD_CSV_PATH));
    }

    /// <summary>コンボ攻撃を要求するメソッド</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    /// <param name="reticleDirection">照準方向</param>
    /// <param name="animKind">要求するアニメーションの種類</param>
    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        animKind = AnimationKind.ComboGroundFoward;

        param.State.Kind = MotionState.StateKind.AttackCommand;
        param.State.Process = MotionState.ProcessKind.Preparation;
    }

    public List<string> MembersToCSV()
    {
        throw new System.NotImplementedException();
    }

    public void CSVToMembers(List<string[]> csv)
    {
        _Name = csv[1][1];
        _Explain= csv[1][2];
        _Count = byte.Parse(csv[1][3]);
        for(int i = 0; i < csv.Count; i++)
        {
            _AttackPowerTable[i] = new AttackPowerColumn(short.Parse(csv[4][0]), short.Parse(csv[4][1]), short.Parse(csv[4][2]));
        }
    }
}

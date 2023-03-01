using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRoundSlash : CommandActiveSkillBase, ICSVDataConverter
{
    /// <summary>情報取得対象のCSVファイルパス</summary>
    const string LOAD_CSV_PATH = "CSV/Command/RoundSlash";

    /// <summary>回転斬撃プレハブのパス</summary>
    const string LOAD_PREF_PATH = "Prefabs/Particles/RoundSlashEdge";

    /// <summary>回転斬撃オブジェクトのプール</summary>
    AttackObjectPool _RoundSlashEdge = null;

    public CommandRoundSlash()
    {
        _Name = "ラウンドスラッシュ";
    }

    public override void Initialize(int layer)
    {
        CSVToMembers(CSVIO.LoadCSV(LOAD_CSV_PATH));

        _RoundSlashEdge = new AttackObjectPool(LOAD_PREF_PATH, layer, 2);
    }

    /// <summary>回転斬撃メソッド</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    /// <param name="reticleDirection">照準方向</param>
    /// <param name="animKind">要求するアニメーションの種類</param>
    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        animKind = AnimationKind.AttackRoundSlash;

        param.State.Kind = MotionState.StateKind.AttackCommand;
        param.State.Process = MotionState.ProcessKind.Preparation;
    }

    /// <summary>回転斬撃生成</summary>
    /// <param name="param">自キャラクターのメインパラメータ</param>
    /// <param name="info">攻撃情報</param>
    /// <param name="emitPoint">射出座標</param>
    public override void ObjectCreation(CharacterParameter param, AttackInformation info, Vector3 emitPoint)
    {
        AttackCollision ac = _RoundSlashEdge.Create(info, Vector3.zero, 0f);
        ac.transform.position = emitPoint;
    }

    public List<string> MembersToCSV()
    {
        throw new System.NotImplementedException();
    }

    public void CSVToMembers(List<string[]> csv)
    {
        _Name = csv[1][1];
        _Explain = csv[1][2];
        _Count = byte.Parse(csv[1][3]);
        _AttackPowerTable = new AttackPowerColumn[csv.Count - 4];
        for (int i = 4; i < csv.Count; i++)
        {
            _AttackPowerTable[i - 4] = new AttackPowerColumn(short.Parse(csv[i][0]), short.Parse(csv[i][1]), short.Parse(csv[i][2]));
        }
    }
}

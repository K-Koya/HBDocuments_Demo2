using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFreezeShot : CommandActiveSkillBase, ICSVDataConverter
{
    /// <summary>情報取得対象のCSVファイルパス</summary>
    const string LOAD_CSV_PATH = "CSV/Command/FreezeShot";

    /// <summary>氷結弾のプレハブのパス</summary>
    const string LOAD_PREF_PATH = "Prefabs/Particles/FreezeBall";

    /// <summary>氷結弾の発射回数の最大値</summary>
    const byte MAX_STEP = 5;

    /// <summary>氷結弾オブジェクトのプール</summary>
    AttackObjectPool _FreezeBalls = null;

    /// <summary>氷結弾の発射回数</summary>
    byte _Step = 0;

    public CommandFreezeShot()
    {
        _Name = "フリーズショット";
    }

    public override void Initialize(CharacterParameter param)
    {
        CSVToMembers(CSVIO.LoadCSV(LOAD_CSV_PATH));

        bool isEnemy = param.gameObject.layer == LayerManager.Instance.Enemy;
        _FreezeBalls = new AttackObjectPool(LOAD_PREF_PATH, isEnemy, 10);

        _Step = 0;
    }

    /// <summary>氷結弾発射メソッド</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    /// <param name="reticleDirection">照準方向</param>
    /// <param name="animKind">要求するアニメーションの種類</param>
    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        //繰り出せる回数は固定
        if (_Step < MAX_STEP)
        {
            animKind = AnimationKind.AttackMagicShoot;

            param.State.Kind = MotionState.StateKind.AttackCommand;
            param.State.Process = MotionState.ProcessKind.Preparation;

            _Step++;
        }
    }

    /// <summary>氷結弾生成</summary>
    /// <param name="param">自キャラクターのメインパラメータ</param>
    /// <param name="info">攻撃情報</param>
    /// <param name="emitPoint">射出座標</param>
    public override void ObjectCreation(CharacterParameter param, AttackInformation info, Vector3 emitPoint)
    {
        Vector3 direction = Vector3.Normalize(param.ReticlePoint - emitPoint);
        AttackCollision ac = _FreezeBalls.Create(info, direction, 0.1f);
        ac.transform.position = emitPoint;
    }

    /// <summary>コンボ手数を0にリセットする</summary>
    public override void PostProcess(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, ref AnimationKind animKind)
    {
        _Step = 0;
    }

    public List<string> MembersToCSV()
    {
        throw new System.NotImplementedException();
    }

    public void CSVToMembers(List<string[]> csv)
    {
        _Name = csv[1][1];
        _Explain = csv[1][2];
        _Step = byte.Parse(csv[1][3]);
        _AttackPowerTable = new AttackPowerColumn[csv.Count - 4];
        for (int i = 4; i < csv.Count; i++)
        {
            _AttackPowerTable[i - 4] = new AttackPowerColumn(short.Parse(csv[i][0]), short.Parse(csv[i][1]), short.Parse(csv[i][2]));
        }
    }
}

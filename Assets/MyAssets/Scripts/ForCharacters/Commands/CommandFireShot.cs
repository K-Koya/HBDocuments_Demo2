using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFireShot : CommandBase, ICSVDataConverter
{
    /// <summary>情報取得対象のCSVファイルパス</summary>
    const string LOAD_CSV_PATH = "CSV/Command/FireShot";

    /// <summary>火炎弾プレハブのパス</summary>
    const string LOAD_PREF_PATH = "Prefabs/Particles/FireBall";

    /// <summary>火炎弾の発射回数の最大値</summary>
    const byte MAX_STEP = 5;

    /// <summary>火炎弾の発射回数</summary>
    byte _Step = 0;

    /// <summary>火炎弾オブジェクトのプール</summary>
    AttackObjectPool _FireBalls = null;

    /// <summary>コマンドID</summary>
    static ushort _Id = 0;

    /// <summary>コマンド名</summary>
    static string _Name = null;

    /// <summary>コマンド説明</summary>
    static string _Explain = null;

    /// <summary>コマンドの種類</summary>
    static CommandKind _Kind = CommandKind.Attack;

    /// <summary>攻撃情報テーブル</summary>
    static AttackPowerColumn[] _AttackPowerTable = null;

    /// <summary>消費MP</summary>
    byte _MPCost = 0;



    public override ushort Id => _Id;
    public override string Name => _Name;
    public override string Explain => _Explain;
    public override CommandKind Kind => _Kind;
    public override byte MPCost => _MPCost;
    protected override AttackPowerColumn[] AttackPowerTable => _AttackPowerTable;


    

    public override ushort LoadData()
    {
        CSVToMembers(CSVIO.LoadCSV(LOAD_CSV_PATH));
        return _Id;
    }

    public override void Initialize(int layer)
    {
        _FireBalls = new AttackObjectPool(LOAD_PREF_PATH, layer, 10);

        _Step = 0;
    }

    /// <summary>火炎弾発射メソッド</summary>
    /// <param name="param">該当キャラクターのパラメータ</param>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="gravityDirection">重力方向</param>
    /// <param name="reticleDirection">照準方向</param>
    /// <param name="animKind">要求するアニメーションの種類</param>
    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        //繰り出せる回数は固定
        if(_Step < MAX_STEP)
        {
            animKind = AnimationKind.AttackMagicShoot;

            param.State.Kind = MotionState.StateKind.AttackCommand;
            param.State.Process = MotionState.ProcessKind.Preparation;

            _Step++;
        }
    }

    /// <summary>火炎弾生成</summary>
    /// <param name="param">自キャラクターのメインパラメータ</param>
    /// <param name="info">攻撃情報</param>
    /// <param name="emitPoint">射出座標</param>
    public override void ObjectCreation(CharacterParameter param, AttackInformation info, Vector3 emitPoint)
    {
        Vector3 direction = Vector3.Normalize(param.ReticlePoint - emitPoint);
        AttackCollision ac = _FireBalls.Create(info, direction, 10f);
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
        _Id = ushort.Parse(csv[1][0]);
        _Name = csv[1][1];
        _Explain = csv[1][2];
        _MPCost = byte.Parse(csv[1][3]);
        _AttackPowerTable = new AttackPowerColumn[csv.Count - 4];
        for (int i = 4; i < csv.Count; i++)
        {
            _AttackPowerTable[i - 4] = new AttackPowerColumn(short.Parse(csv[i][0]), short.Parse(csv[i][1]), short.Parse(csv[i][2]));
        }
    }
}

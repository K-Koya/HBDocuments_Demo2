using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRoundSlash : CommandBase, ICSVDataConverter
{
    /// <summary>���擾�Ώۂ�CSV�t�@�C���p�X</summary>
    const string LOAD_CSV_PATH = "CSV/Command/RoundSlash";

    /// <summary>��]�a���v���n�u�̃p�X</summary>
    const string LOAD_PREF_PATH = "Prefabs/Particles/RoundSlashEdge";

    /// <summary>��]�a���I�u�W�F�N�g�̃v�[��</summary>
    AttackObjectPool _RoundSlashEdge = null;

    /// <summary>�R�}���hID</summary>
    static ushort _Id = 0;

    /// <summary>�R�}���h��</summary>
    static string _Name = null;

    /// <summary>�R�}���h����</summary>
    static string _Explain = null;

    /// <summary>�R�}���h�̎��</summary>
    static CommandKind _Kind = CommandKind.Attack;

    /// <summary>�U�����e�[�u��</summary>
    static AttackPowerColumn[] _AttackPowerTable = null;

    /// <summary>����MP</summary>
    byte _MPCost = 0;



    public override ushort Id => _Id;
    public override string Name => _Name;
    public override string Explain => _Explain;
    public override CommandKind Kind => _Kind;
    public override byte MPCost => _MPCost;
    protected override AttackPowerColumn[] AttackPowerTable => _AttackPowerTable;

    public CommandRoundSlash()
    {
        _Name = "���E���h�X���b�V��";
        _Kind = CommandKind.Attack;
    }

    public override ushort LoadData()
    {
        CSVToMembers(CSVIO.LoadCSV(LOAD_CSV_PATH));
        return _Id;
    }

    public override void Initialize(int layer)
    {
        _RoundSlashEdge = new AttackObjectPool(LOAD_PREF_PATH, layer, 2);
    }

    /// <summary>��]�a�����\�b�h</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    /// <param name="reticleDirection">�Ə�����</param>
    /// <param name="animKind">�v������A�j���[�V�����̎��</param>
    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        animKind = AnimationKind.AttackRoundSlash;

        param.State.Kind = MotionState.StateKind.AttackCommand;
        param.State.Process = MotionState.ProcessKind.Preparation;
    }

    /// <summary>��]�a������</summary>
    /// <param name="param">���L�����N�^�[�̃��C���p�����[�^</param>
    /// <param name="info">�U�����</param>
    /// <param name="emitPoint">�ˏo���W</param>
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

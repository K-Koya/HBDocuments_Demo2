using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandJumpTurn : CommandActiveSkillBase, ICSVDataConverter
{
    /// <summary>���擾�Ώۂ�CSV�t�@�C���p�X</summary>
    const string LOAD_CSV_PATH = "CSV/Command/JumpTurn";

    /// <summary>�Ռ��g�v���n�u�̃p�X</summary>
    const string LOAD_PREF_PATH = "Prefabs/Particles/ShockWave";

    /// <summary>�Ռ��g�I�u�W�F�N�g�̃v�[��</summary>
    AttackObjectPool _Shockwave = null;

    public CommandJumpTurn()
    {
        _Name = "�W�����v�^�[��";
    }

    public override void Initialize(CharacterParameter param)
    {
        CSVToMembers(CSVIO.LoadCSV(LOAD_CSV_PATH));

        bool isEnemy = param.gameObject.layer == LayerManager.Instance.Enemy;
        _Shockwave = new AttackObjectPool(LOAD_PREF_PATH, isEnemy, 1);
    }

    /// <summary>�Ռ��g���˃��\�b�h</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    /// <param name="reticleDirection">�Ə�����</param>
    /// <param name="animKind">�v������A�j���[�V�����̎��</param>
    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        animKind = AnimationKind.ComboGroundWide;

        param.State.Kind = MotionState.StateKind.AttackCommand;
        param.State.Process = MotionState.ProcessKind.Preparation;
    }

    /// <summary>�Ռ��g����</summary>
    /// <param name="param">���L�����N�^�[�̃��C���p�����[�^</param>
    /// <param name="info">�U�����</param>
    /// <param name="emitPoint">�ˏo���W</param>
    public override void ObjectCreation(CharacterParameter param, AttackInformation info, Vector3 emitPoint)
    {
        AttackCollision ac = _Shockwave.Create(info, Vector3.zero, 0f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFreezeShot : CommandActiveSkillBase, ICSVDataConverter
{
    /// <summary>���擾�Ώۂ�CSV�t�@�C���p�X</summary>
    const string LOAD_CSV_PATH = "CSV/Command/FreezeShot";

    /// <summary>�X���e�̃v���n�u�̃p�X</summary>
    const string LOAD_PREF_PATH = "Prefabs/Particles/FreezeBall";

    /// <summary>�X���e�̔��ˉ񐔂̍ő�l</summary>
    const byte MAX_STEP = 5;

    /// <summary>�X���e�I�u�W�F�N�g�̃v�[��</summary>
    AttackObjectPool _FreezeBalls = null;

    /// <summary>�X���e�̔��ˉ�</summary>
    byte _Step = 0;

    public CommandFreezeShot()
    {
        _Name = "�t���[�Y�V���b�g";
    }

    public override void Initialize(CharacterParameter param)
    {
        CSVToMembers(CSVIO.LoadCSV(LOAD_CSV_PATH));

        bool isEnemy = param.gameObject.layer == LayerManager.Instance.Enemy;
        _FreezeBalls = new AttackObjectPool(LOAD_PREF_PATH, isEnemy, 10);

        _Step = 0;
    }

    /// <summary>�X���e���˃��\�b�h</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    /// <param name="reticleDirection">�Ə�����</param>
    /// <param name="animKind">�v������A�j���[�V�����̎��</param>
    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        //�J��o����񐔂͌Œ�
        if (_Step < MAX_STEP)
        {
            animKind = AnimationKind.AttackMagicShoot;

            param.State.Kind = MotionState.StateKind.AttackCommand;
            param.State.Process = MotionState.ProcessKind.Preparation;

            _Step++;
        }
    }

    /// <summary>�X���e����</summary>
    /// <param name="param">���L�����N�^�[�̃��C���p�����[�^</param>
    /// <param name="info">�U�����</param>
    /// <param name="emitPoint">�ˏo���W</param>
    public override void ObjectCreation(CharacterParameter param, AttackInformation info, Vector3 emitPoint)
    {
        Vector3 direction = Vector3.Normalize(param.ReticlePoint - emitPoint);
        AttackCollision ac = _FreezeBalls.Create(info, direction, 0.1f);
        ac.transform.position = emitPoint;
    }

    /// <summary>�R���{�萔��0�Ƀ��Z�b�g����</summary>
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

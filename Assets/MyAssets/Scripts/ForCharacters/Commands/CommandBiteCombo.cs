using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBiteCombo : CommandActiveSkillBase, ICSVDataConverter
{
    /// <summary>���擾�Ώۂ�CSV�t�@�C���p�X</summary>
    const string LOAD_CSV_PATH = "CSV/Command/BiteCombo.csv";

    public override void Initialize()
    {
        CSVToMembers(CSVIO.LoadCSV(LOAD_CSV_PATH));
    }

    /// <summary>�R���{�U����v�����郁�\�b�h</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    /// <param name="reticleDirection">�Ə�����</param>
    /// <param name="animKind">�v������A�j���[�V�����̎��</param>
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

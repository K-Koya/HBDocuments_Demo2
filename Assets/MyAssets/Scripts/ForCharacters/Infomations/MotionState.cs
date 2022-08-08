using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�{�L�����N�^�[���ǂ̂悤�ȓ�������Ă��邩���܂Ƃ߂�N���X</summary>
[System.Serializable]
public class MotionState
{
    [SerializeField, Tooltip("�s���̎��")]
    StateKind _State = StateKind.Stay;

    [SerializeField, Tooltip("���̍s���̒i�K")]
    ProcessKind _Process = ProcessKind.NotPlaying;

    /// <summary>�s���̎��</summary>
    public StateKind State { get => _State; set => _State = value; }
    /// <summary>���̍s���̒i�K</summary>
    public ProcessKind Process { get => _Process; set => _Process = value; }

    /// <summary>�s���̎�ނ������񋓑�</summary>
    public enum StateKind : byte
    {
        /// <summary>�ҋ@���̏��</summary>
        Stay = 0,
        /// <summary>���s���̏��</summary>
        Walk,
        /// <summary>���s���̏��</summary>
        Run,
        /// <summary>�ʏ�W�����v���̏��</summary>
        JumpNoraml,
        /// <summary>�ʏ헎�����̏��</summary>
        FallNoraml,
        /// <summary>�Z�������(�V�t�g�X���C�h)���̏��</summary>
        DodgeShort,
        /// <summary>���������(�����O�g���b�v)���̏�</summary>
        DodgeLong,
        /// <summary>�K�[�h���̏��</summary>
        Guard,
        /// <summary>�ʏ�R���{���̏��</summary>
        ComboNormal,
        /// <summary>�R���{�t�B�j�b�V�����̏��</summary>
        ComboFinish,
    }

    /// <summary>�e�s���ɂ��āA���̓���̂ǂ̋ǖʂ��������񋓑�</summary>
    public enum ProcessKind : byte
    {
        /// <summary>�����{</summary>
        NotPlaying = 0,
        /// <summary>���{�҂�</summary>
        StandBy,
        /// <summary>�{����O�̗\�����쒆</summary>
        Preparation,
        /// <summary>�{���쒆</summary>
        Playing,
        /// <summary>�{���썇�Ԃ̋󂫎���</summary>
        Interval,
        /// <summary>�{����I�����O</summary>
        EndSoon,
        /// <summary>���{��̌�</summary>
        PostProcess,
    }
}

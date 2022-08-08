using UnityEngine;

/// <summary>�v���C���[�������̓R���s���[�^�[�̓��͏�Ԃ�Ԃ����ۃ��\�b�h�W</summary>
public interface IController
{
    /// <summary>�ړ����͂̏��</summary>
    public InputState Move();
    /// <summary>�ړ����͂̕����E�傫��</summary>
    public Vector2 MoveDirection();
    /// <summary>�W�����v���͂̏��</summary>
    public InputState Jump();
    /// <summary>�ʏ�U�����͂̏��</summary>
    public InputState Attack();
    /// <summary>�Ə��R�}���h���͂̏��</summary>
    public InputState AimCommand();
    /// <summary>�Z�������(�V�t�g�X���C�h)�̓��͏��</summary>
    public InputState DodgeShort();
    /// <summary>�Z�������(�V�t�g�X���C�h)�̓��͏��</summary>
    public InputState DodgeLong();
    /// <summary>�K�[�h�̓��͏��</summary>
    public InputState Guard();
}

/// <summary>�Y���̓��͏�</summary>
public enum InputState : byte
{
    /// <summary>������Ă��Ȃ�</summary>
    Untouched = 0,
    /// <summary>�����ꂽ����</summary>
    PushDown,
    /// <summary>�����Ă����</summary>
    Pushing,
    /// <summary>����������</summary>
    PushUp,
}

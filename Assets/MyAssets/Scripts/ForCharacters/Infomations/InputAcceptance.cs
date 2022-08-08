using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>����ۏ�Ԃ��Ǘ�����N���X</summary>
[System.Serializable]
public class InputAcceptance
{
    /// <summary>true : ���͈ړ����󂯕t����</summary>
    public bool _CanMove = true;

    /// <summary>true : ���͂ŃW�����v���󂯕t����</summary>
    public bool _CanJump = true;

    /// <summary>true : �V�t�g�X���C�h�i�Z��������j���󂯕t����</summary>
    public bool _CanShiftSlide = true;

    /// <summary>true : �����O�g���b�v�i����������j���󂯕t����</summary>
    public bool _CanLongTrip = true;

    /// <summary>true : �K�[�h���󂯕t����</summary>
    public bool _CanGurad = true;

    /// <summary>true : �ʏ�R���{���󂯕t����</summary>
    public bool _CanComboNormal = true;

    /// <summary>true : �R���{�t�B�j�b�V�����󂯕t����</summary>
    public bool _CanComboFinish = false;
}

using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckEditColumn : MonoBehaviour
{
    [SerializeField, Tooltip("�\������R�}���hID")]
    ushort _CommandId = 0;

    [SerializeField, Tooltip("�t���[���̔w�i")]
    Image _BackGround = null;

    [SerializeField, Tooltip("�R�}���h���e�L�X�g")]
    TMP_Text _Name = null;

    /// <summary>�R�}���h�������\��</summary>
    public Action<ushort> CreateExplainMethod = null;

    /// <summary>�f�b�L���̃R�}���h�u������</summary>
    public Action<ushort> ReplaceMethod = null;


    /// <summary>DeckEditList����Ăяo���ăJ�������\�����郁�\�b�h</summary>
    /// <param name="id">�R�}���hID</param>
    /// <param name="name">�R�}���h��</param>
    /// <param name="back">�R�}���h�t���[���̔w�i</param>
    /// <param name="onFocus">�J�����Ƀt�H�[�J�X����������ɍs�����\�b�h</param>
    /// <param name="onPush">�J�����������ꂽ���ɍs�����\�b�h</param>
    public void CreateColumn(ushort id, string name, Color back, Action<ushort> onFocus, Action<ushort> onPush)
    {
        _CommandId = id;
        _Name.text = name;
        _BackGround.color = back;
        CreateExplainMethod = onFocus;
        ReplaceMethod = onPush;
    }

    /// <summary>�R�}���h�������\��</summary>
    public void CreateExplain()
    {
        CreateExplainMethod?.Invoke(_CommandId);
    }

    /// <summary>�{�^���������ꂽ���̏���</summary>
    public void OnPush()
    {
        ReplaceMethod?.Invoke(_CommandId);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    public System.Action<ushort> CreateExplainMethod = null;

    /// <summary>DeckEditList����Ăяo���ăJ�������\�����郁�\�b�h</summary>
    /// <param name="id">�R�}���hID</param>
    /// <param name="name">�R�}���h��</param>
    /// <param name="back">�R�}���h�t���[���̔w�i</param>
    /// <param name="action">�R�}���h�������\�����郁�\�b�h</param>
    public void CreateColumn(ushort id, string name, Color back, System.Action<ushort> action)
    {
        _CommandId = id;
        _Name.text = name;
        _BackGround.color = back;
        CreateExplainMethod = action;
    }

    /// <summary>�R�}���h�������\��</summary>
    public void CreateExplain()
    {
        CreateExplainMethod?.Invoke(_CommandId);
    }
}

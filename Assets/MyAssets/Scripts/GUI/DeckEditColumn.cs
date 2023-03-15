using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckEditColumn : MonoBehaviour
{
    [SerializeField, Tooltip("表示するコマンドID")]
    ushort _CommandId = 0;

    [SerializeField, Tooltip("フレームの背景")]
    Image _BackGround = null;

    [SerializeField, Tooltip("コマンド名テキスト")]
    TMP_Text _Name = null;

    /// <summary>コマンド説明を構成</summary>
    public System.Action<ushort> CreateExplainMethod = null;

    /// <summary>DeckEditListから呼び出してカラムを構成するメソッド</summary>
    /// <param name="id">コマンドID</param>
    /// <param name="name">コマンド名</param>
    /// <param name="back">コマンドフレームの背景</param>
    /// <param name="action">コマンド説明を構成するメソッド</param>
    public void CreateColumn(ushort id, string name, Color back, System.Action<ushort> action)
    {
        _CommandId = id;
        _Name.text = name;
        _BackGround.color = back;
        CreateExplainMethod = action;
    }

    /// <summary>コマンド説明を構成</summary>
    public void CreateExplain()
    {
        CreateExplainMethod?.Invoke(_CommandId);
    }
}

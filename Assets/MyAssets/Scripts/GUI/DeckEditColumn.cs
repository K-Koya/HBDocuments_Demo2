using System.Collections;
using System.Collections.Generic;
using System;
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
    public Action<ushort> CreateExplainMethod = null;

    /// <summary>デッキ中のコマンド置き換え</summary>
    public Action<ushort> ReplaceMethod = null;


    /// <summary>DeckEditListから呼び出してカラムを構成するメソッド</summary>
    /// <param name="id">コマンドID</param>
    /// <param name="name">コマンド名</param>
    /// <param name="back">コマンドフレームの背景</param>
    /// <param name="onFocus">カラムにフォーカスが乗った時に行うメソッド</param>
    /// <param name="onPush">カラムが押された時に行うメソッド</param>
    public void CreateColumn(ushort id, string name, Color back, Action<ushort> onFocus, Action<ushort> onPush)
    {
        _CommandId = id;
        _Name.text = name;
        _BackGround.color = back;
        CreateExplainMethod = onFocus;
        ReplaceMethod = onPush;
    }

    /// <summary>コマンド説明を構成</summary>
    public void CreateExplain()
    {
        CreateExplainMethod?.Invoke(_CommandId);
    }

    /// <summary>ボタンが押された時の処理</summary>
    public void OnPush()
    {
        ReplaceMethod?.Invoke(_CommandId);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckEditList : MonoBehaviour
{
    /// <summary>MPコストの単位</summary>
    const string UNIT_FOR_COST = "MP";

    /// <summary>アイテム個数の単位</summary>
    const string UNIT_FOR_COUNT = "Inventry";


    [SerializeField, Tooltip("選択可能なコマンドのリストのカラムプレハブ")]
    GameObject _UnlockedColumnPref = null;
    
    [SerializeField, Tooltip("選択可能なコマンドのリストカラムの格納場所")]
    GameObject _UnlockedListArea = null;


    [SerializeField, Tooltip("コマンド背景色:アタックコマンド")]
    Color _BackgroundAttack = Color.red;

    [SerializeField, Tooltip("コマンド背景色:サポートコマンド")]
    Color _BackgroundSupport = Color.blue;

    [SerializeField, Tooltip("コマンド背景色:アイテムコマンド")]
    Color _BackgroundItem = Color.green;

    [SerializeField, Tooltip("コマンドリストのカラム")]
    DeckEditColumn[] _DeckCommandColumns = null;

    /// <summary>選択可能なコマンドのリストカラム</summary>
    DeckEditColumn[] _UnlockedCommandColumns = null;

    [SerializeField, Tooltip("選択したコマンドの背景")]
    Image[] _PickUpBackgrounds = null;

    [SerializeField, Tooltip("選択したコマンド名テキスト")]
    TMP_Text _PickUpCommandName = null;

    [SerializeField, Tooltip("選択したコマンドのMPや個数の単位表示")]
    TMP_Text _PickUpCommandUnit = null;

    [SerializeField, Tooltip("選択したコマンドのMPや個数の表示")]
    TMP_Text _PickUpCommandCount = null;

    [SerializeField, Tooltip("選択したコマンドの説明テキスト")]
    TMP_Text _PickUpCommandExplain = null;


    /// <summary>デッキ登録中のコマンドID</summary>
    List<ushort> _CommandIDs = null;


    /// <summary>デッキ登録中のコマンドカラムのうち、押されたカラムの要素番号</summary>
    int _PushedIndexOnDeckList = -1;

    /// <summary>開放済みコマンドのカラムのうち、押されたカラムのコマンドID</summary>
    int _PushedIdOnUnlockedList = -1;


    // Start is called before the first frame update
    void Start()
    {
        //装備済みコマンドリスト構成
        IReadOnlyList<ushort> ids = SaveDataManager.Instance.GetDeckedCommand("Kana"/*TODO*/);
        _CommandIDs = new List<ushort>();
        for (int i = 0; i < ids.Count; i++)
        {
            _CommandIDs.Add(ids[i]);
            CommandBase command = CommandDictionary.Instance.Commands[ids[i]];
            BuildListColumn(command, _DeckCommandColumns[i], DeckReplaceTo);
        }

        //装備可能コマンドリスト構成
        ids = SaveDataManager.Instance.GetUnlockedCommand("Kana"/*TODO*/);
        _UnlockedCommandColumns = new DeckEditColumn[ids.Count];
        for (int i = 0; i < ids.Count; i++)
        {
            CommandBase command = CommandDictionary.Instance.Commands[ids[i]];
            GameObject obj = Instantiate(_UnlockedColumnPref);
            obj.transform.SetParent(_UnlockedListArea.transform);
            _UnlockedCommandColumns[i] = obj.GetComponent<DeckEditColumn>();
            BuildListColumn(command, _UnlockedCommandColumns[i], DeckReplaceFrom);
        }
    }

    /// <summary>コマンドリストのカラムの表示を設定</summary>
    /// <param name="command">コマンドデータ</param>
    /// <param name="column">カラム制御</param>
    /// <param name="onPushMethod">カラム押下時の実行メソッド</param>
    void BuildListColumn(CommandBase command, DeckEditColumn column, System.Action<ushort> onPushMethod)
    {
        switch (command.Kind)
        {
            case CommandKind.Attack:
                column.CreateColumn(command.Id, command.Name, _BackgroundAttack, PickUpExplain, onPushMethod);
                break;
            case CommandKind.Support:
                column.CreateColumn(command.Id, command.Name, _BackgroundSupport, PickUpExplain, onPushMethod);
                break;
            case CommandKind.Item:
                column.CreateColumn(command.Id, command.Name, _BackgroundItem, PickUpExplain, onPushMethod);
                break;
            case CommandKind.SupportHeal:
                column.CreateColumn(command.Id, command.Name, _BackgroundSupport, PickUpExplain, onPushMethod);
                break;
            case CommandKind.ItemHeal:
                column.CreateColumn(command.Id, command.Name, _BackgroundItem, PickUpExplain, onPushMethod);
                break;
            case CommandKind.Passive:
                column.CreateColumn(command.Id, command.Name, _BackgroundItem, PickUpExplain, onPushMethod);
                break;
            default:
                column.CreateColumn(command.Id, command.Name, Color.white, PickUpExplain, onPushMethod);
                break;
        }
    }

    /// <summary>開放済みコマンドリスト向けのデッキ変更指示</summary>
    /// <param name="id">コマンドのID</param>
    void DeckReplaceFrom(ushort id)
    {
        if(_PushedIdOnUnlockedList > -1)
        {
            _PushedIdOnUnlockedList = -1;
            _PushedIndexOnDeckList = -1;
        }
        else if(_PushedIndexOnDeckList > -1)
        {
            _CommandIDs[_PushedIndexOnDeckList] = id;
            CommandBase command = CommandDictionary.Instance.Commands[_CommandIDs[_PushedIndexOnDeckList]];
            BuildListColumn(command, _DeckCommandColumns[_PushedIndexOnDeckList], DeckReplaceTo);

            _PushedIdOnUnlockedList = -1;
            _PushedIndexOnDeckList = -1;
        }
        else
        {
            _PushedIdOnUnlockedList = id;
            _PushedIndexOnDeckList = -1;
        }
    }

    /// <summary>デッキ選択済みリスト向けのデッキ変更指示</summary>
    /// <param name="id">コマンドのID</param>
    void DeckReplaceTo(ushort id)
    {
        int index = _CommandIDs.IndexOf(id);

        if (_PushedIndexOnDeckList > -1)
        {
            ushort cache = _CommandIDs[_PushedIndexOnDeckList];
            _CommandIDs[_PushedIndexOnDeckList] = _CommandIDs[index];
            _CommandIDs[index] = cache;

            CommandBase command = CommandDictionary.Instance.Commands[_CommandIDs[_PushedIndexOnDeckList]];
            BuildListColumn(command, _DeckCommandColumns[_PushedIndexOnDeckList], DeckReplaceTo);
            command = CommandDictionary.Instance.Commands[_CommandIDs[index]];
            BuildListColumn(command, _DeckCommandColumns[index], DeckReplaceTo);

            _PushedIdOnUnlockedList = -1;
            _PushedIndexOnDeckList = -1;
        }
        else if (_PushedIdOnUnlockedList > -1)
        {            
            _CommandIDs[index] = (ushort)_PushedIdOnUnlockedList;
            CommandBase command = CommandDictionary.Instance.Commands[_CommandIDs[index]];
            BuildListColumn(command, _DeckCommandColumns[index], DeckReplaceTo);

            _PushedIdOnUnlockedList = -1;
            _PushedIndexOnDeckList = -1;
        }
        else
        {
            _PushedIndexOnDeckList = index;
            _PushedIdOnUnlockedList = -1;
        }
    }

    /// <summary>説明欄に情報を表示する</summary>
    /// <param name="id">コマンドID</param>
    void PickUpExplain(ushort id)
    {
        CommandBase command = CommandDictionary.Instance.Commands[id];

        _PickUpCommandName.text = command.Name;

        switch (command.Kind)
        {
            case CommandKind.Attack:
                Array.ForEach(_PickUpBackgrounds, bg => bg.color = _BackgroundAttack);
                _PickUpCommandUnit.text = UNIT_FOR_COST;
                _PickUpCommandCount.text = command.MPCost.ToString();
                break;
            case CommandKind.Support:
                Array.ForEach(_PickUpBackgrounds, bg => bg.color = _BackgroundSupport);
                _PickUpCommandUnit.text = UNIT_FOR_COST;
                _PickUpCommandCount.text = command.MPCost.ToString();
                break;
            case CommandKind.Item:
                Array.ForEach(_PickUpBackgrounds, bg => bg.color = _BackgroundItem);
                _PickUpCommandUnit.text = UNIT_FOR_COUNT;
                _PickUpCommandCount.text = command.MaxInventry.ToString();
                break;
            case CommandKind.SupportHeal:
                Array.ForEach(_PickUpBackgrounds, bg => bg.color = _BackgroundSupport);
                _PickUpCommandUnit.text = UNIT_FOR_COST;
                _PickUpCommandCount.text = command.MPCost.ToString();
                break;
            case CommandKind.ItemHeal:
                Array.ForEach(_PickUpBackgrounds, bg => bg.color = _BackgroundItem);
                _PickUpCommandUnit.text = UNIT_FOR_COUNT;
                _PickUpCommandCount.text = command.MaxInventry.ToString();
                break;
            case CommandKind.Passive:
                _PickUpCommandUnit.text = "";
                _PickUpCommandCount.text = "";
                break;
            default:
                Array.ForEach(_PickUpBackgrounds, bg => bg.color = Color.white);
                _PickUpCommandUnit.text = "";
                _PickUpCommandCount.text = "";
                break;
        }

        _PickUpCommandExplain.text = command.Explain;
    }

    /// <summary>確定して反映</summary>
    public void Confirm()
    {
        SaveDataManager.Instance.SetDeckedCommand("Kana"/*TODO*/, _CommandIDs);
    }
}

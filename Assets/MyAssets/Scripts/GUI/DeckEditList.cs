using System;
using System.Collections;
using System.Collections.Generic;
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


    /// <summary>デッキ登録中のコマンド</summary>
    CommandBase[] _Commands = null;

    /// <summary>デッキ登録可能なコマンド</summary>
    CommandBase[] _UnlockedCommands = null;


    // Start is called before the first frame update
    void Start()
    {
        //装備済みコマンドリスト構成
        IReadOnlyList<ushort> ids = SaveDataManager.Instance.GetDeckedCommand("Kana"/*TODO*/);
        _Commands = new CommandBase[ids.Count];
        for (int i = 0; i < _Commands.Length; i++)
        {
            _Commands[i] = CommandDictionary.Instance.Commands[ids[i]];
            BuildListColumn(_Commands[i], _DeckCommandColumns[i]);
        }

        //装備可能コマンドリスト構成
        ids = SaveDataManager.Instance.GetUnlockedCommand("Kana"/*TODO*/);
        _UnlockedCommands = new CommandBase[ids.Count];
        _UnlockedCommandColumns = new DeckEditColumn[ids.Count];
        for (int i = 0; i < _UnlockedCommands.Length; i++)
        {
            _UnlockedCommands[i] = CommandDictionary.Instance.Commands[ids[i]];
            GameObject obj = Instantiate(_UnlockedColumnPref);
            obj.transform.SetParent(_UnlockedListArea.transform);
            _UnlockedCommandColumns[i] = obj.GetComponent<DeckEditColumn>();
            BuildListColumn(_UnlockedCommands[i], _UnlockedCommandColumns[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>コマンドリストのカラムの表示を設定</summary>
    /// <param name="command">コマンドデータ</param>
    /// <param name="column">カラム制御</param>
    /// <param name="name">名前</param>
    void BuildListColumn(CommandBase command, DeckEditColumn column)
    {
        switch (command.Kind)
        {
            case CommandKind.Attack:
                column.CreateColumn(command.Id, command.Name, _BackgroundAttack, PickUpExplain);
                break;
            case CommandKind.Support:
                column.CreateColumn(command.Id, command.Name, _BackgroundSupport, PickUpExplain);
                break;
            case CommandKind.Item:
                column.CreateColumn(command.Id, command.Name, _BackgroundItem, PickUpExplain);
                break;
            case CommandKind.SupportHeal:
                column.CreateColumn(command.Id, command.Name, _BackgroundSupport, PickUpExplain);
                break;
            case CommandKind.ItemHeal:
                column.CreateColumn(command.Id, command.Name, _BackgroundItem, PickUpExplain);
                break;
            case CommandKind.Passive:
                column.CreateColumn(command.Id, command.Name, _BackgroundItem, PickUpExplain);
                break;
            default: break;
        }
    }

    /// <summary>説明欄に情報を表示する</summary>
    /// <param name="id">コマンドID</param>
    public void PickUpExplain(ushort id)
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
            default: break;
        }

        _PickUpCommandExplain.text = command.Explain;
    }
}

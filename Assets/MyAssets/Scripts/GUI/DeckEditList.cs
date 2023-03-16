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
    /// <summary>MP�R�X�g�̒P��</summary>
    const string UNIT_FOR_COST = "MP";

    /// <summary>�A�C�e�����̒P��</summary>
    const string UNIT_FOR_COUNT = "Inventry";


    [SerializeField, Tooltip("�I���\�ȃR�}���h�̃��X�g�̃J�����v���n�u")]
    GameObject _UnlockedColumnPref = null;
    
    [SerializeField, Tooltip("�I���\�ȃR�}���h�̃��X�g�J�����̊i�[�ꏊ")]
    GameObject _UnlockedListArea = null;


    [SerializeField, Tooltip("�R�}���h�w�i�F:�A�^�b�N�R�}���h")]
    Color _BackgroundAttack = Color.red;

    [SerializeField, Tooltip("�R�}���h�w�i�F:�T�|�[�g�R�}���h")]
    Color _BackgroundSupport = Color.blue;

    [SerializeField, Tooltip("�R�}���h�w�i�F:�A�C�e���R�}���h")]
    Color _BackgroundItem = Color.green;

    [SerializeField, Tooltip("�R�}���h���X�g�̃J����")]
    DeckEditColumn[] _DeckCommandColumns = null;

    /// <summary>�I���\�ȃR�}���h�̃��X�g�J����</summary>
    DeckEditColumn[] _UnlockedCommandColumns = null;

    [SerializeField, Tooltip("�I�������R�}���h�̔w�i")]
    Image[] _PickUpBackgrounds = null;

    [SerializeField, Tooltip("�I�������R�}���h���e�L�X�g")]
    TMP_Text _PickUpCommandName = null;

    [SerializeField, Tooltip("�I�������R�}���h��MP����̒P�ʕ\��")]
    TMP_Text _PickUpCommandUnit = null;

    [SerializeField, Tooltip("�I�������R�}���h��MP����̕\��")]
    TMP_Text _PickUpCommandCount = null;

    [SerializeField, Tooltip("�I�������R�}���h�̐����e�L�X�g")]
    TMP_Text _PickUpCommandExplain = null;


    /// <summary>�f�b�L�o�^���̃R�}���hID</summary>
    List<ushort> _CommandIDs = null;


    /// <summary>�f�b�L�o�^���̃R�}���h�J�����̂����A�����ꂽ�J�����̗v�f�ԍ�</summary>
    int _PushedIndexOnDeckList = -1;

    /// <summary>�J���ς݃R�}���h�̃J�����̂����A�����ꂽ�J�����̃R�}���hID</summary>
    int _PushedIdOnUnlockedList = -1;


    // Start is called before the first frame update
    void Start()
    {
        //�����ς݃R�}���h���X�g�\��
        IReadOnlyList<ushort> ids = SaveDataManager.Instance.GetDeckedCommand("Kana"/*TODO*/);
        _CommandIDs = new List<ushort>();
        for (int i = 0; i < ids.Count; i++)
        {
            _CommandIDs.Add(ids[i]);
            CommandBase command = CommandDictionary.Instance.Commands[ids[i]];
            BuildListColumn(command, _DeckCommandColumns[i], DeckReplaceTo);
        }

        //�����\�R�}���h���X�g�\��
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

    /// <summary>�R�}���h���X�g�̃J�����̕\����ݒ�</summary>
    /// <param name="command">�R�}���h�f�[�^</param>
    /// <param name="column">�J��������</param>
    /// <param name="onPushMethod">�J�����������̎��s���\�b�h</param>
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

    /// <summary>�J���ς݃R�}���h���X�g�����̃f�b�L�ύX�w��</summary>
    /// <param name="id">�R�}���h��ID</param>
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

    /// <summary>�f�b�L�I���ς݃��X�g�����̃f�b�L�ύX�w��</summary>
    /// <param name="id">�R�}���h��ID</param>
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

    /// <summary>�������ɏ���\������</summary>
    /// <param name="id">�R�}���hID</param>
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

    /// <summary>�m�肵�Ĕ��f</summary>
    public void Confirm()
    {
        SaveDataManager.Instance.SetDeckedCommand("Kana"/*TODO*/, _CommandIDs);
    }
}

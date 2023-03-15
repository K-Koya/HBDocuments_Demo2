using System;
using System.Collections;
using System.Collections.Generic;
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


    /// <summary>�f�b�L�o�^���̃R�}���h</summary>
    CommandBase[] _Commands = null;

    /// <summary>�f�b�L�o�^�\�ȃR�}���h</summary>
    CommandBase[] _UnlockedCommands = null;


    // Start is called before the first frame update
    void Start()
    {
        //�����ς݃R�}���h���X�g�\��
        IReadOnlyList<ushort> ids = SaveDataManager.Instance.GetDeckedCommand("Kana"/*TODO*/);
        _Commands = new CommandBase[ids.Count];
        for (int i = 0; i < _Commands.Length; i++)
        {
            _Commands[i] = CommandDictionary.Instance.Commands[ids[i]];
            BuildListColumn(_Commands[i], _DeckCommandColumns[i]);
        }

        //�����\�R�}���h���X�g�\��
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

    /// <summary>�R�}���h���X�g�̃J�����̕\����ݒ�</summary>
    /// <param name="command">�R�}���h�f�[�^</param>
    /// <param name="column">�J��������</param>
    /// <param name="name">���O</param>
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

    /// <summary>�������ɏ���\������</summary>
    /// <param name="id">�R�}���hID</param>
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

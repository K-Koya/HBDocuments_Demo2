using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCommandList : MonoBehaviour
{
    /// <summary>MP�R�X�g�̒P��</summary>
    const string UNIT_FOR_COST = "mp";

    /// <summary>�A�C�e�����̒P��</summary>
    const string UNIT_FOR_COUNT = "�~";

    [SerializeField, Tooltip("�R�}���h�w�i�F:�A�^�b�N�R�}���h")]
    Color _BackgroundAttack = Color.red;

    [SerializeField, Tooltip("�R�}���h�w�i�F:�T�|�[�g�R�}���h")]
    Color _BackgroundSupport = Color.blue;

    [SerializeField, Tooltip("�R�}���h�w�i�F:�A�C�e���R�}���h")]
    Color _BackgroundItem = Color.green;

    [SerializeField, Tooltip("�R�}���h���X�g�̔w�i")]
    Image[] _ListBackground = null;

    [SerializeField, Tooltip("�R�}���h���X�g���ɃR�}���h����\������e�L�X�g")]
    TMP_Text[] _ListCommandName = null;

    [SerializeField, Tooltip("�R�}���h���X�g���ɁAMP�R�X�g����Ȃǂ̏��l��\������e�L�X�g")]
    TMP_Text[] _ListCounter = null;

    /// <summary>�v���C���[�̏����R�}���h</summary>
    CommandBase[] _Commands = null;

    // Start is called before the first frame update
    void Start()
    {
        //�v���C���[�̏����R�}���h���擾
        CommandHolder[] holders = FindObjectsOfType<CommandHolder>();
        foreach (CommandHolder holder in holders)
        {
            if (holder.CompareTag(TagManager.Instance.Player))
            {
                _Commands = holder.ActiveSkills;
                break;
            }
        }

        Display();
    }

    // Update is called once per frame
    void Update()
    {
        Reflect();
    }

    /// <summary>�\��</summary>
    void Display()
    {
        for (int i = 0; i < _Commands.Length; i++)
        {
            switch (_Commands[i].Kind)
            {
                case CommandKind.Attack:
                    _ListBackground[i].color = _BackgroundAttack;
                    _ListCounter[i].text = $"{UNIT_FOR_COST}{_Commands[i].MPCost}";
                    break;

                case CommandKind.Support:
                    _ListBackground[i].color = _BackgroundSupport;
                    _ListCounter[i].text = $"{UNIT_FOR_COST}{_Commands[i].MPCost}";
                    break;

                case CommandKind.Item:
                    _ListBackground[i].color = _BackgroundItem;
                    _ListCounter[i].text = $"{UNIT_FOR_COUNT}{_Commands[i].CurrentInventory}";
                    break;

                case CommandKind.ItemHeal:
                    _ListBackground[i].color = _BackgroundItem;
                    _ListCounter[i].text = $"{UNIT_FOR_COUNT}{_Commands[i].CurrentInventory}";
                    break;

                case CommandKind.SupportHeal:
                    _ListBackground[i].color = _BackgroundSupport;
                    _ListCounter[i].text = $"{UNIT_FOR_COST}{_Commands[i].MPCost}";
                    break;
                default:
                    _ListCounter[i].text = "";
                    break;
            }

            _ListCommandName[i].text = _Commands[i].Name;
        }
    }

    /// <summary>�ύX�̔��f</summary>
    void Reflect()
    {
        for (int i = 0; i < _Commands.Length; i++)
        {
            switch (_Commands[i].Kind)
            {
                case CommandKind.Attack:
                    _ListBackground[i].color = _BackgroundAttack;
                    _ListCounter[i].text = $"{UNIT_FOR_COST}{_Commands[i].MPCost}";
                    break;

                case CommandKind.Support:
                    _ListBackground[i].color = _BackgroundSupport;
                    _ListCounter[i].text = $"{UNIT_FOR_COST}{_Commands[i].MPCost}";
                    break;

                case CommandKind.Item:
                    _ListBackground[i].color = _BackgroundItem;
                    _ListCounter[i].text = $"{UNIT_FOR_COUNT}{_Commands[i].CurrentInventory}";
                    break;

                case CommandKind.ItemHeal:
                    _ListBackground[i].color = _BackgroundItem;
                    _ListCounter[i].text = $"{UNIT_FOR_COUNT}{_Commands[i].CurrentInventory}";
                    break;

                case CommandKind.SupportHeal:
                    _ListBackground[i].color = _BackgroundSupport;
                    _ListCounter[i].text = $"{UNIT_FOR_COST}{_Commands[i].MPCost}";
                    break;
                default:
                    _ListCounter[i].text = "";
                    break;
            }

            _ListCommandName[i].text = _Commands[i].Name;
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCommandList : MonoBehaviour
{
    /// <summary>MPコストの単位</summary>
    const string UNIT_FOR_COST = "mp";

    /// <summary>アイテム個数の単位</summary>
    const string UNIT_FOR_COUNT = "×";

    [SerializeField, Tooltip("コマンド背景色:アタックコマンド")]
    Color _BackgroundAttack = Color.red;

    [SerializeField, Tooltip("コマンド背景色:サポートコマンド")]
    Color _BackgroundSupport = Color.blue;

    [SerializeField, Tooltip("コマンド背景色:アイテムコマンド")]
    Color _BackgroundItem = Color.green;

    [SerializeField, Tooltip("コマンドリストの背景")]
    Image[] _ListBackground = null;

    [SerializeField, Tooltip("コマンドリスト中にコマンド名を表示するテキスト")]
    TMP_Text[] _ListCommandName = null;

    [SerializeField, Tooltip("コマンドリスト中に、MPコストや個数などの情報値を表示するテキスト")]
    TMP_Text[] _ListCounter = null;

    /// <summary>プレイヤーの所持コマンド</summary>
    CommandActiveSkillBase[] _Commands = null;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーの所持コマンドを取得
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

    /// <summary>表示</summary>
    void Display()
    {
        for (int i = 0; i < _Commands.Length; i++)
        {
            switch (_Commands[i].Kind)
            {
                case CommandActiveSkillBase.CommandKind.Attack:
                    _ListBackground[i].color = _BackgroundAttack;
                    _ListCounter[i].text = UNIT_FOR_COST;
                    break;

                case CommandActiveSkillBase.CommandKind.Support:
                    _ListBackground[i].color = _BackgroundSupport;
                    _ListCounter[i].text = UNIT_FOR_COST;
                    break;

                case CommandActiveSkillBase.CommandKind.Item:
                    _ListBackground[i].color = _BackgroundItem;
                    _ListCounter[i].text = UNIT_FOR_COUNT;
                    break;

                case CommandActiveSkillBase.CommandKind.ItemHeal:
                    _ListBackground[i].color = _BackgroundItem;
                    _ListCounter[i].text = UNIT_FOR_COUNT;
                    break;

                case CommandActiveSkillBase.CommandKind.SupportHeal:
                    _ListBackground[i].color = _BackgroundSupport;
                    _ListCounter[i].text = UNIT_FOR_COST;
                    break;
            }

            _ListCommandName[i].text = _Commands[i].Name;
            _ListCounter[i].text += _Commands[i].Count.ToString();
        }
    }

    /// <summary>変更の反映</summary>
    void Reflect()
    {
        for (int i = 0; i < _Commands.Length; i++)
        {
            switch (_Commands[i].Kind)
            {
                case CommandActiveSkillBase.CommandKind.Attack:
                    _ListBackground[i].color = _BackgroundAttack;
                    _ListCounter[i].text = UNIT_FOR_COST;
                    break;

                case CommandActiveSkillBase.CommandKind.Support:
                    _ListBackground[i].color = _BackgroundSupport;
                    _ListCounter[i].text = UNIT_FOR_COST;
                    break;

                case CommandActiveSkillBase.CommandKind.Item:
                    _ListBackground[i].color = _BackgroundItem;
                    _ListCounter[i].text = UNIT_FOR_COUNT;
                    break;

                case CommandActiveSkillBase.CommandKind.ItemHeal:
                    _ListBackground[i].color = _BackgroundItem;
                    _ListCounter[i].text = UNIT_FOR_COUNT;
                    break;

                case CommandActiveSkillBase.CommandKind.SupportHeal:
                    _ListBackground[i].color = _BackgroundSupport;
                    _ListCounter[i].text = UNIT_FOR_COST;
                    break;
            }

            _ListCommandName[i].text = _Commands[i].Name;
            _ListCounter[i].text += _Commands[i].Count.ToString();
        }
    }
}

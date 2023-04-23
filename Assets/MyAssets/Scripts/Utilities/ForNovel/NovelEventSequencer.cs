using UnityEngine;
using System.Collections.Generic;

public class NovelEventSequencer : MonoBehaviour, ICSVDataConverter
{
    [SerializeField, Tooltip("文字表示コンポーネントをアサイン")]
    MessagePrinter _printer = null;
    [SerializeField, Tooltip("暗転処理コンポーネントをアサイン")]
    BlackoutController _situationController = null;

    [SerializeField, Tooltip("読みだすCSVファイルのパス")]
    string _csvPath = "";

    [SerializeField, Tooltip("イベント実行するアニメーターをアサイン")]
    Animator[] _animators = null;

    /// <summary>ノベルイベント</summary>
    List<NovelEventContainer> novelEventContainers = null;

    // _messages フィールドから表示する現在のメッセージのインデックス。
    // 何も指していない場合は -1 とする。
    private int _currentIndex = -1;

    private void Start()
    {
        //CSVファイル読み出し
        CSVToMembers(CSVIO.LoadCSV(_csvPath));
        MoveNext();
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        if(InputUtility.GetDownDecide)
        {
            if (_printer.IsPrinting)
            {
                _printer.Skip();
                NovelEventContainer container = novelEventContainers[_currentIndex];
                if(container.IsSkipable) _animators[container.CharacterId].SetTrigger("DoSkip");
                //_situationController?.SkipBlackout();
            }
            else 
            { 
                MoveNext();
            }
        }
    }

    /// <summary>
    /// 次のページに進む。
    /// 次のページが存在しない場合は無視する。
    /// </summary>
    private void MoveNext()
    {
        if (novelEventContainers is null or { Count: 0 }) { return; }

        if (_currentIndex + 1 < novelEventContainers.Count)
        {
            _currentIndex++;
            NovelEventContainer container = novelEventContainers[_currentIndex];
            _printer?.ShowMessage(container.Sentence, container.CharacterName, container.SpeedRatio);
            if(container.IsAnimationStep) _animators[container.CharacterId].SetTrigger("GoNext");
        }
        else
        {
            _situationController?.DoBlackout();
        }
    }

    public List<string> MembersToCSV()
    {
        throw new System.NotImplementedException();
    }

    public void CSVToMembers(List<string[]> csv)
    {
        novelEventContainers = new List<NovelEventContainer>();

        for (int i = 1; i < csv.Count; i++)
        {
            NovelEventContainer container = new NovelEventContainer
                (
                int.Parse(csv[i][0]),
                short.Parse(csv[i][1]),
                csv[i][2],
                float.Parse(csv[i][3]),
                bool.Parse(csv[i][4]),
                csv[i][5],
                bool.Parse(csv[i][6])
                ) ;
            novelEventContainers.Add(container);
        }
    }
}

public class NovelEventContainer
{
    /// <summary>順番</summary>
    int _order = 0;

    /// <summary>キャラクター識別番号</summary>
    short _characterId = 0;

    /// <summary>キャラクター名</summary>
    string _characterName = null;

    /// <summary>文字表示速度倍率</summary>
    float _speedRatio = 1f;

    /// <summary>true : 文字送りスキップが可能</summary>
    bool _isSkipable = false;

    /// <summary>表示するテキスト</summary>
    string _sentence = "";

    /// <summary>true : 次のアニメーションに進む</summary>
    bool _isAnimationStep = false;

    /// <summary>順番</summary>
    public int Order { get => _order; }
    /// <summary>キャラクター識別番号</summary>
    public short CharacterId { get => _characterId; }
    /// <summary>キャラクター名</summary>
    public string CharacterName { get => _characterName; }
    /// <summary>文字表示速度倍率</summary>
    public float SpeedRatio { get => _speedRatio; }
    /// <summary>true : 文字送りスキップが可能</summary>
    public bool IsSkipable { get => _isSkipable; }
    /// <summary>表示するテキスト</summary>
    public string Sentence { get => _sentence; }
    /// <summary>true : 次のアニメーションに進む</summary>
    public bool IsAnimationStep { get => _isAnimationStep; }

    /// <summary>ノベルゲーム向け情報格納庫</summary>
    /// <param name="order">順番</param>
    /// <param name="characterId">キャラクター識別番号</param>
    /// <param name="characterName">キャラクター名</param>
    /// <param name="speedRatio">文字表示速度倍率</param>
    /// <param name="isSkipable">true : 文字送りスキップが可能</param>
    public NovelEventContainer(int order, short characterId, string characterName, float speedRatio, bool isSkipable, string sentence, bool isAnimationStep)
    {
        _order = order;
        _characterId = characterId;
        _characterName = characterName;
        _speedRatio = speedRatio;
        _isSkipable = isSkipable;
        _sentence = sentence;
        _isAnimationStep = isAnimationStep;
    }
}
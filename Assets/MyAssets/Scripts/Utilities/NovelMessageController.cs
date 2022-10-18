using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ノベルゲームの会話パートのように、文章を順々に表示、ボタンで次にすすむなどを制御する
/// </summary>
public class NovelMessageController : MonoBehaviour
{
    [SerializeField, Tooltip("メッセージを表示するテキストオブジェクト")]
    TextMeshProUGUI _MessageText = null;

    [SerializeField, Tooltip("発言者の名前を表示するテキストオブジェクト")]
    TextMeshProUGUI _NameText = null;


    /// <summary>メッセージウィンドウをクリックしたときに立たせるフラグ</summary>
    bool _IsClickedMessageWindow = false;

    /// <summary>文章めくり要求フラグ</summary>
    bool _IsRequestProceedNextMessage = false;

    /// <summary>遅延表示をするフラグ</summary>
    bool _IsDelaySkip = false;

    [SerializeField, Tooltip("アクション実行許可フラグ")]
    bool _IsRunnableAction = false;

    [SerializeField, Tooltip("全アクション実行済みフラグ")]
    bool _IsRunAllActions = false;

    /// <summary>コルーチン内の文字表示遅延に利用するクラスインスタンス</summary>
    WaitForSeconds _TranscriptionDelay = null;


    /// <summary>
    /// 文章格納庫
    /// </summary>
    [System.Serializable]
    public class MessageContainer
    {
        [SerializeField, Tooltip("どういった発言か、その名前")]
        string _Name = "名前";

        [SerializeField, Tooltip("この発言をする者の名前")]
        string _Whose = "この発言をする者の名前";

        [SerializeField, Tooltip("表示する文章"), TextArea(1,10)]
        string _Sentence = "文章";

        [SerializeField, Tooltip("文章の表示速度")]
        float _Speed = 0.05f;

        /// <summary>表示中の文章</summary>
        string _DisclosuredSentence = "";

        /// <summary>文章をすべて表示し終えた</summary>
        bool _IsDisclosuredAll = false;


        public MessageContainer()
        {
            _Name = "名前";
            _Whose = "この発言をする者の名前";
            _Sentence = "文章";
            _DisclosuredSentence = "";
            _Speed = 0.05f;
            _IsDisclosuredAll = false;
        }

        /// <summary>どういった発言か、その名前</summary>
        public string Whose { get => _Whose; }
        /// <summary>この発言をする者の名前</summary>
        public string DisclosuredSentence { get => _DisclosuredSentence; }
        /// <summary>表示する文章</summary>
        public bool IsDisclosuredAll { get => _IsDisclosuredAll; }
        /// <summary>文章の表示速度</summary>
        public float Speed { get => _Speed; }


        /// <summary>文章表示</summary>
        public void Show()
        {
            //文字列がなければ、全文表示した扱いとする
            _IsDisclosuredAll = (_Sentence.Length <= 0);

            //文章をすべて表示し終えるまで、
            if (!_IsDisclosuredAll)
            {
                //「表示中の文章」に「表示中の文章」の長さ+1番目のsentenceの文字を追加する
                _DisclosuredSentence += _Sentence[_DisclosuredSentence.Length];

                //「表示中の文章」と「文章の本文」の長さが一致したら「文章をすべて表示し終えた」状態とする
                _IsDisclosuredAll = (_DisclosuredSentence.Length == _Sentence.Length);
            }
        }
    }
    [SerializeField, Tooltip("メッセージ格納庫")]
    MessageContainer[] messageContainer = default;

    /// <summary>アクション実行許可フラグ</summary>
    public bool IsRunnableAction { set => _IsRunnableAction = value; }
    /// <summary>全アクション実行済みフラグ</summary>
    public bool IsRunAllActions { get => _IsRunAllActions; }


    /// <summary>文章を作成</summary>
    IEnumerator CreateMessage()
    {
        //アクションを実行するフラグが立つまで待つ
        while (!_IsRunnableAction)
        {
            //次のフレームへ
            yield return null;
        }

        foreach (MessageContainer mc in messageContainer)
        {
            //表示文字列初期化
            if(_MessageText) _MessageText.text = "";
            if(_NameText) _NameText.text = mc.Whose;

            //遅延秒数設定
            _TranscriptionDelay = new WaitForSeconds(mc.Speed);

            //文字の遅延表示
            while (!mc.IsDisclosuredAll)
            {
                mc.Show();
                _MessageText.text = mc.DisclosuredSentence;

                //遅延を待たずに表示
                if (_IsDelaySkip) continue;
                yield return _TranscriptionDelay;
            }

            //ボタンが押されるまで待つ
            do
            {
                //次のフレームへ
                yield return null;
            } while (!_IsRequestProceedNextMessage);

            //遅延スキップフラグを折る
            _IsDelaySkip = false;
        }

        //全アクション実行済みフラグを立てる
        _IsRunAllActions = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //「文章を作成」コルーチン実行
        StartCoroutine(CreateMessage());
    }

    // Update is called once per frame
    void Update()
    {
        RequestProceedNextMessage();

        //文章表示遅延フラグを立てる
        if (_IsRunnableAction && !_IsRunAllActions && !_IsDelaySkip && _IsRequestProceedNextMessage)
        {
            _IsDelaySkip = true;
        }
    }


    /// <summary>
    /// ノベルメッセージ表示を操作するボタン入力を集約するメソッド
    /// </summary>
    void RequestProceedNextMessage()
    {
        _IsRequestProceedNextMessage = (_IsClickedMessageWindow || InputUtility.GetDownAttack);
        _IsClickedMessageWindow = false;
    }

    /// <summary>
    /// 画面上のボタン入力により操作するためのメソッド
    /// ButtonのOnClickにて呼び出す
    /// </summary>
    public void ClickedMessageWindow()
    {
        _IsClickedMessageWindow = true;
    }
}

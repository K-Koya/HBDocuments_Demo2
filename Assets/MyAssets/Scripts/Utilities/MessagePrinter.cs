using TMPro;
using UnityEngine;

public class MessagePrinter : MonoBehaviour
{
    [SerializeField, Tooltip("メッセージ表示用テキストコンポーネントをアサイン")]
    TMP_Text _textUi = null;

    [SerializeField, Tooltip("発言者表示用テキストコンポーネントをアサイン")]
    TMP_Text _speakerUi = null;

    [SerializeField, Tooltip("発言予定文字列")]
    string _message = "";

    [SerializeField, Tooltip("表示速度の標準値")]
    float _speed = 0.05f;

    /// <summary>文字を表示してからの経過時間</summary>
    float _elapsed = 0;

    /// <summary>文字毎の待ち時間</summary>
    float _interval = 0;

    // _message フィールドから表示する現在の文字インデックス
    int _currentIndex = -1;

    /// <summary>
    /// 文字出力中かどうか。
    /// </summary>
    public bool IsPrinting { get => _currentIndex + 1 < _message.Length; }

    private void Update()
    {
        if (_textUi is null || _message is null || _currentIndex + 1 >= _message.Length) { return; }

        _elapsed += Time.deltaTime;
        if (_elapsed > _interval)
        {
            _elapsed = 0;
            _currentIndex++;
            _textUi.text += _message[_currentIndex];
        }
    }

    /// <summary>指定のメッセージを表示する</summary>
    /// <param name="message">テキストとして表示するメッセージ<param>
    /// <param name="speaker">発言者名<param>
    /// <param name="speedRatio">表示速度倍率<param>
    public void ShowMessage(string message, string speaker, float speedRatio)
    {
        _textUi.text = "";
        _speakerUi.text = speaker;
        _message = message;
        _interval = _speed / speedRatio;
        _currentIndex = -1;
    }

    /// <summary>現在再生中の文字出力を省略する</summary>
    public void Skip()
    {
        _textUi.text = _message;
        _currentIndex = _message.Length;
    }
}
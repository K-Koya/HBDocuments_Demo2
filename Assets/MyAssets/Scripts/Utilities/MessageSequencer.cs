using UnityEngine;

public class MessageSequencer : MonoBehaviour
{
    [SerializeField]
    MessagePrinter _printer = default;
    [SerializeField]
    MessageSituationController _situationController = null;

    [SerializeField, TextArea(1,4)]
    string[] _messages = default;

    // _messages フィールドから表示する現在のメッセージのインデックス。
    // 何も指していない場合は -1 とする。
    private int _currentIndex = -1;

    private void Start()
    {
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
                _situationController.Skip();
            }
            else 
            { 
                MoveNext();
                _situationController?.DoBlackout();
            }
        }
    }

    /// <summary>
    /// 次のページに進む。
    /// 次のページが存在しない場合は無視する。
    /// </summary>
    private void MoveNext()
    {
        if (_messages is null or { Length: 0 }) { return; }

        if (_currentIndex + 1 < _messages.Length)
        {
            _currentIndex++;
            _printer?.ShowMessage(_messages[_currentIndex]);
        }
    }
}
using TMPro;
using UnityEngine;

public class MessagePrinter : MonoBehaviour
{
    [SerializeField, Tooltip("���b�Z�[�W�\���p�e�L�X�g�R���|�[�l���g���A�T�C��")]
    TMP_Text _textUi = null;

    [SerializeField, Tooltip("�����ҕ\���p�e�L�X�g�R���|�[�l���g���A�T�C��")]
    TMP_Text _speakerUi = null;

    [SerializeField, Tooltip("�����\�蕶����")]
    string _message = "";

    [SerializeField, Tooltip("�\�����x�̕W���l")]
    float _speed = 0.05f;

    /// <summary>������\�����Ă���̌o�ߎ���</summary>
    float _elapsed = 0;

    /// <summary>�������̑҂�����</summary>
    float _interval = 0;

    // _message �t�B�[���h����\�����錻�݂̕����C���f�b�N�X
    int _currentIndex = -1;

    /// <summary>
    /// �����o�͒����ǂ����B
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

    /// <summary>�w��̃��b�Z�[�W��\������</summary>
    /// <param name="message">�e�L�X�g�Ƃ��ĕ\�����郁�b�Z�[�W<param>
    /// <param name="speaker">�����Җ�<param>
    /// <param name="speedRatio">�\�����x�{��<param>
    public void ShowMessage(string message, string speaker, float speedRatio)
    {
        _textUi.text = "";
        _speakerUi.text = speaker;
        _message = message;
        _interval = _speed / speedRatio;
        _currentIndex = -1;
    }

    /// <summary>���ݍĐ����̕����o�͂��ȗ�����</summary>
    public void Skip()
    {
        _textUi.text = _message;
        _currentIndex = _message.Length;
    }
}
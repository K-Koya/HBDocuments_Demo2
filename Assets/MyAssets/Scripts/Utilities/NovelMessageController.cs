using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �m�x���Q�[���̉�b�p�[�g�̂悤�ɁA���͂����X�ɕ\���A�{�^���Ŏ��ɂ����ނȂǂ𐧌䂷��
/// </summary>
public class NovelMessageController : MonoBehaviour
{
    [SerializeField, Tooltip("���b�Z�[�W��\������e�L�X�g�I�u�W�F�N�g")]
    TextMeshProUGUI _MessageText = null;

    [SerializeField, Tooltip("�����҂̖��O��\������e�L�X�g�I�u�W�F�N�g")]
    TextMeshProUGUI _NameText = null;


    /// <summary>���b�Z�[�W�E�B���h�E���N���b�N�����Ƃ��ɗ�������t���O</summary>
    bool _IsClickedMessageWindow = false;

    /// <summary>���͂߂���v���t���O</summary>
    bool _IsRequestProceedNextMessage = false;

    /// <summary>�x���\��������t���O</summary>
    bool _IsDelaySkip = false;

    [SerializeField, Tooltip("�A�N�V�������s���t���O")]
    bool _IsRunnableAction = false;

    [SerializeField, Tooltip("�S�A�N�V�������s�ς݃t���O")]
    bool _IsRunAllActions = false;

    /// <summary>�R���[�`�����̕����\���x���ɗ��p����N���X�C���X�^���X</summary>
    WaitForSeconds _TranscriptionDelay = null;


    /// <summary>
    /// ���͊i�[��
    /// </summary>
    [System.Serializable]
    public class MessageContainer
    {
        [SerializeField, Tooltip("�ǂ��������������A���̖��O")]
        string _Name = "���O";

        [SerializeField, Tooltip("���̔���������҂̖��O")]
        string _Whose = "���̔���������҂̖��O";

        [SerializeField, Tooltip("�\�����镶��"), TextArea(1,10)]
        string _Sentence = "����";

        [SerializeField, Tooltip("���͂̕\�����x")]
        float _Speed = 0.05f;

        /// <summary>�\�����̕���</summary>
        string _DisclosuredSentence = "";

        /// <summary>���͂����ׂĕ\�����I����</summary>
        bool _IsDisclosuredAll = false;


        public MessageContainer()
        {
            _Name = "���O";
            _Whose = "���̔���������҂̖��O";
            _Sentence = "����";
            _DisclosuredSentence = "";
            _Speed = 0.05f;
            _IsDisclosuredAll = false;
        }

        /// <summary>�ǂ��������������A���̖��O</summary>
        public string Whose { get => _Whose; }
        /// <summary>���̔���������҂̖��O</summary>
        public string DisclosuredSentence { get => _DisclosuredSentence; }
        /// <summary>�\�����镶��</summary>
        public bool IsDisclosuredAll { get => _IsDisclosuredAll; }
        /// <summary>���͂̕\�����x</summary>
        public float Speed { get => _Speed; }


        /// <summary>���͕\��</summary>
        public void Show()
        {
            //�����񂪂Ȃ���΁A�S���\�����������Ƃ���
            _IsDisclosuredAll = (_Sentence.Length <= 0);

            //���͂����ׂĕ\�����I����܂ŁA
            if (!_IsDisclosuredAll)
            {
                //�u�\�����̕��́v�Ɂu�\�����̕��́v�̒���+1�Ԗڂ�sentence�̕�����ǉ�����
                _DisclosuredSentence += _Sentence[_DisclosuredSentence.Length];

                //�u�\�����̕��́v�Ɓu���̖͂{���v�̒�������v������u���͂����ׂĕ\�����I�����v��ԂƂ���
                _IsDisclosuredAll = (_DisclosuredSentence.Length == _Sentence.Length);
            }
        }
    }
    [SerializeField, Tooltip("���b�Z�[�W�i�[��")]
    MessageContainer[] messageContainer = default;

    /// <summary>�A�N�V�������s���t���O</summary>
    public bool IsRunnableAction { set => _IsRunnableAction = value; }
    /// <summary>�S�A�N�V�������s�ς݃t���O</summary>
    public bool IsRunAllActions { get => _IsRunAllActions; }


    /// <summary>���͂��쐬</summary>
    IEnumerator CreateMessage()
    {
        //�A�N�V���������s����t���O�����܂ő҂�
        while (!_IsRunnableAction)
        {
            //���̃t���[����
            yield return null;
        }

        foreach (MessageContainer mc in messageContainer)
        {
            //�\�������񏉊���
            if(_MessageText) _MessageText.text = "";
            if(_NameText) _NameText.text = mc.Whose;

            //�x���b���ݒ�
            _TranscriptionDelay = new WaitForSeconds(mc.Speed);

            //�����̒x���\��
            while (!mc.IsDisclosuredAll)
            {
                mc.Show();
                _MessageText.text = mc.DisclosuredSentence;

                //�x����҂����ɕ\��
                if (_IsDelaySkip) continue;
                yield return _TranscriptionDelay;
            }

            //�{�^�����������܂ő҂�
            do
            {
                //���̃t���[����
                yield return null;
            } while (!_IsRequestProceedNextMessage);

            //�x���X�L�b�v�t���O��܂�
            _IsDelaySkip = false;
        }

        //�S�A�N�V�������s�ς݃t���O�𗧂Ă�
        _IsRunAllActions = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //�u���͂��쐬�v�R���[�`�����s
        StartCoroutine(CreateMessage());
    }

    // Update is called once per frame
    void Update()
    {
        RequestProceedNextMessage();

        //���͕\���x���t���O�𗧂Ă�
        if (_IsRunnableAction && !_IsRunAllActions && !_IsDelaySkip && _IsRequestProceedNextMessage)
        {
            _IsDelaySkip = true;
        }
    }


    /// <summary>
    /// �m�x�����b�Z�[�W�\���𑀍삷��{�^�����͂��W�񂷂郁�\�b�h
    /// </summary>
    void RequestProceedNextMessage()
    {
        _IsRequestProceedNextMessage = (_IsClickedMessageWindow || InputUtility.GetDownAttack);
        _IsClickedMessageWindow = false;
    }

    /// <summary>
    /// ��ʏ�̃{�^�����͂ɂ�葀�삷�邽�߂̃��\�b�h
    /// Button��OnClick�ɂČĂяo��
    /// </summary>
    public void ClickedMessageWindow()
    {
        _IsClickedMessageWindow = true;
    }
}

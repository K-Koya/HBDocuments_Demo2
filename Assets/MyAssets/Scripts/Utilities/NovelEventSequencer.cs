using UnityEngine;
using System.Collections.Generic;

public class NovelEventSequencer : MonoBehaviour, ICSVDataConverter
{
    [SerializeField, Tooltip("�����\���R���|�[�l���g���A�T�C��")]
    MessagePrinter _printer = null;
    [SerializeField, Tooltip("�Ó]�����R���|�[�l���g���A�T�C��")]
    BlackoutController _situationController = null;

    [SerializeField, Tooltip("�ǂ݂���CSV�t�@�C���̃p�X")]
    string _csvPath = "";

    [SerializeField, Tooltip("�C�x���g���s����A�j���[�^�[���A�T�C��")]
    Animator[] _animators = null;

    /// <summary>�m�x���C�x���g</summary>
    List<NovelEventContainer> novelEventContainers = null;

    // _messages �t�B�[���h����\�����錻�݂̃��b�Z�[�W�̃C���f�b�N�X�B
    // �����w���Ă��Ȃ��ꍇ�� -1 �Ƃ���B
    private int _currentIndex = -1;

    private void Start()
    {
        //CSV�t�@�C���ǂݏo��
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
    /// ���̃y�[�W�ɐi�ށB
    /// ���̃y�[�W�����݂��Ȃ��ꍇ�͖�������B
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
    /// <summary>����</summary>
    int _order = 0;

    /// <summary>�L�����N�^�[���ʔԍ�</summary>
    short _characterId = 0;

    /// <summary>�L�����N�^�[��</summary>
    string _characterName = null;

    /// <summary>�����\�����x�{��</summary>
    float _speedRatio = 1f;

    /// <summary>true : ��������X�L�b�v���\</summary>
    bool _isSkipable = false;

    /// <summary>�\������e�L�X�g</summary>
    string _sentence = "";

    /// <summary>true : ���̃A�j���[�V�����ɐi��</summary>
    bool _isAnimationStep = false;

    /// <summary>����</summary>
    public int Order { get => _order; }
    /// <summary>�L�����N�^�[���ʔԍ�</summary>
    public short CharacterId { get => _characterId; }
    /// <summary>�L�����N�^�[��</summary>
    public string CharacterName { get => _characterName; }
    /// <summary>�����\�����x�{��</summary>
    public float SpeedRatio { get => _speedRatio; }
    /// <summary>true : ��������X�L�b�v���\</summary>
    public bool IsSkipable { get => _isSkipable; }
    /// <summary>�\������e�L�X�g</summary>
    public string Sentence { get => _sentence; }
    /// <summary>true : ���̃A�j���[�V�����ɐi��</summary>
    public bool IsAnimationStep { get => _isAnimationStep; }

    /// <summary>�m�x���Q�[���������i�[��</summary>
    /// <param name="order">����</param>
    /// <param name="characterId">�L�����N�^�[���ʔԍ�</param>
    /// <param name="characterName">�L�����N�^�[��</param>
    /// <param name="speedRatio">�����\�����x�{��</param>
    /// <param name="isSkipable">true : ��������X�L�b�v���\</param>
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
using System;
using UnityEditor.Hardware;
using UnityEngine;

public class CommandHolder : MonoBehaviour
{
    /// <summary>�A�N�e�B�u�X�L���R�}���h�̐�</summary>
    const byte NUMBER_OF_ACTIVE_SKILL = 4;

    #region �����o
    [SerializeField, Tooltip("�R���{�萔")]
    byte _ComboCount = 5;

    [SerializeField, Tooltip("�A�N�e�B�u�X�L���R�}���h��ID")]
    int[] _ActiveSkillsId = null;

    /// <summary>�W�����v�p�R�}���h</summary>
    MotionJumpBase _CommandJump = null;

    /// <summary>�Z��������R�}���h</summary>
    MotionShiftSlideBase _CommandShiftSlide = null;

    /// <summary>����������R�}���h</summary>
    MotionLongTripBase _CommandLongTrip = null;

    /// <summary>�A�N�e�B�u�X�L���R�}���h�̃��X�g</summary>
    CommandBase[] _ActiveSkills = null;

    /// <summary>�R���{�R�}���h</summary>
    CommandCombo _CommandCombo = null;

    /// <summary>�R���{�R�}���h�̂������s���̃R�}���h</summary>
    CommandBase _Running = null;

    /// <summary>�ЂƂO�̎��s���̃R�}���h</summary>
    CommandBase _BeforeRunning = null;
    #endregion

    #region �v���p�e�B
    /// <summary>�W�����v�p�R�}���h</summary>
    public MotionJumpBase Jump { get => _CommandJump; }
    /// <summary>�Z��������R�}���h</summary>
    public MotionShiftSlideBase ShiftSlide { get => _CommandShiftSlide; }
    /// <summary>����������R�}���h</summary>
    public MotionLongTripBase LongTrip { get => _CommandLongTrip; }
    /// <summary>�A�N�e�B�u�X�L���R�}���h�̃��X�g</summary>
    public CommandBase[] ActiveSkills { get => _ActiveSkills; }
    /// <summary>�R���{�R�}���h</summary>
    public CommandCombo Combo { get => _CommandCombo; }
    /// <summary>�A�N�e�B�u�X�L���R�}���h�E�R���{�R�}���h�̂������s���̃R�}���h</summary>
    public CommandBase Running { get => _Running; }
    #endregion

    #region �Q�b�^�[���\�b�h
    /// <summary>�R���{�R�}���h���擾���Ď��s��Ԃɂ���</summary>
    /// <returns>�R���{�R�}���h</returns>
    public CommandCombo GetComboForRun()
    {
        _Running = _CommandCombo;
        return _CommandCombo;
    }

    /// <summary>�A�N�e�B�u�X�L���R�}���h���擾���Ď��s��Ԃɂ���</summary>
    /// <param name="index">�X�L�����X�g�ɃA�N�Z�X���邽�߂̓Y�����ԍ�</param>
    /// <returns>�A�N�e�B�u�X�L���R�}���h</returns>
    public CommandBase GetActiveSkillForRun(int index)
    {
        _Running = ActiveSkills[index];
        return _Running;
    }

    /// <summary>�A�N�e�B�u�X�L���R�}���h���擾���Ď��s�j������</summary>
    /// <returns>�A�N�e�B�u�X�L���R�}���h</returns>
    public CommandBase GetActiveSkillForPostProcess()
    {
        CommandBase cashe = _Running;
        _Running = null;
        return cashe;
    }

    #endregion

    void Start()
    {
        CharacterParameter param = GetComponentInParent<CharacterParameter>();
        CharacterMove cm = GetComponentInParent<CharacterMove>();

        int layerSet = gameObject.layer == LayerMask.NameToLayer(LayerManager.Instance.NameEnemy)
                        ? LayerMask.NameToLayer(LayerManager.Instance.NameEnemyAttacker)
                        : LayerMask.NameToLayer(LayerManager.Instance.NameAlliesAttacker);

        //�e��R�}���h��null�����e���Ȃ�
        if (_CommandJump is null) 
            _CommandJump = new MotionJumpBase();
        if (_CommandShiftSlide is null) 
            _CommandShiftSlide = new MotionShiftSlideBase();
        if (_CommandLongTrip is null) 
            _CommandLongTrip = new MotionLongTripBase();

        //�A�N�e�B�u�X�L���R�}���h�͏�����Ɏ��߁Anull�����e���Ȃ�
        _ActiveSkills = new CommandBase[NUMBER_OF_ACTIVE_SKILL];
        if (_ActiveSkills.Length != NUMBER_OF_ACTIVE_SKILL)
        {
            Array.Resize(ref _ActiveSkills, NUMBER_OF_ACTIVE_SKILL);
        }
        for(int i = 0; i < NUMBER_OF_ACTIVE_SKILL; i++)
        {
            //�S�R�}���h�i�[�ɂ���擾
            _ActiveSkills[i] = CommandDictionary.Instance.CloneCommand((ushort)_ActiveSkillsId[i]);

            //�e�X�L���R�}���h�����������s
            _ActiveSkills[i].Initialize(layerSet);
        }

        //�S�R�}���h�i�[�ɂ���擾
        _CommandCombo = CommandDictionary.Instance.CloneCombo;
        _CommandCombo.Count = _ComboCount;
        _CommandCombo.Initialize(layerSet);
    }

    void Update()
    {
        //Debug.Log($"{_ActiveSkills[0]} {_ActiveSkills[1]} {_ActiveSkills[2]} {_ActiveSkills[3]}");
    }

    /// <summary>�R�[�h����R�}���h���Z�b�g���郁�\�b�h</summary>
    /// <param name="commands">�Ώۂ̃A�N�e�B�u�X�L���R�}���h</param>
    public void SetActiveSkills(params CommandBase[] commands)
    {
        _ActiveSkills = new CommandBase[NUMBER_OF_ACTIVE_SKILL];
        for(int i = 0; i < NUMBER_OF_ACTIVE_SKILL; i++)
        {
            _ActiveSkills[i] = commands[i];
        }
    }
}

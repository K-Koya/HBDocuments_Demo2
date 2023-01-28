using System;
using UnityEngine;

public class CommandHolder : MonoBehaviour
{
    /// <summary>�A�N�e�B�u�X�L���R�}���h�̐�</summary>
    const byte NUMBER_OF_ACTIVE_SKILL = 4;

    #region �����o
    [Header("�����R�}���h���")]
    [SerializeField, Tooltip("�W�����v�p�R�}���h")]
    CommandJumpBase _CommandJump = new CommandJumpBase();

    [SerializeField, Tooltip("�Z��������R�}���h")]
    CommandShiftSlideBase _CommandShiftSlide = new CommandShiftSlideBase();

    [SerializeField, Tooltip("����������R�}���h")]
    CommandLongTripBase _CommandLongTrip = new CommandLongTripBase();

    [SerializeReference, SelectableSerializeReference, Tooltip("�A�N�e�B�u�X�L���R�}���h�̃��X�g")]
    CommandActiveSkillBase[] _ActiveSkills = null;

    [SerializeField, Tooltip("�R���{�R�}���h")]
    CommandCombo _CommandCombo = new CommandCombo();

    [Header("���s���A�N�e�B�u�X�L���R�}���h���")]
    [SerializeField, Tooltip("�A�N�e�B�u�X�L���R�}���h�E�R���{�R�}���h�̂������s���̃R�}���h")]
    CommandActiveSkillBase _Running = null;
    #endregion

    #region �v���p�e�B
    /// <summary>�W�����v�p�R�}���h</summary>
    public CommandJumpBase Jump { get => _CommandJump; }
    /// <summary>�Z��������R�}���h</summary>
    public CommandShiftSlideBase ShiftSlide { get => _CommandShiftSlide; }
    /// <summary>����������R�}���h</summary>
    public CommandLongTripBase LongTrip { get => _CommandLongTrip; }
    /// <summary>�A�N�e�B�u�X�L���R�}���h�̃��X�g</summary>
    public CommandActiveSkillBase[] ActiveSkills { get => _ActiveSkills; }
    /// <summary>�R���{�R�}���h</summary>
    public CommandCombo Combo { get => _CommandCombo; }
    /// <summary>�A�N�e�B�u�X�L���R�}���h�E�R���{�R�}���h�̂������s���̃R�}���h</summary>
    public CommandActiveSkillBase Running { get => _Running; }
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
    public CommandActiveSkillBase GetActiveSkillForRun(int index)
    {
        _Running = ActiveSkills[index];
        return _Running;
    }
    #endregion

    void Start()
    {
        //�e��R�}���h��null�����e���Ȃ�
        if (_CommandJump is null) 
            _CommandJump = new CommandJumpBase();
        if (_CommandShiftSlide is null) 
            _CommandShiftSlide = new CommandShiftSlideBase();
        if (_CommandLongTrip is null) 
            _CommandLongTrip = new CommandLongTripBase();

        //�A�N�e�B�u�X�L���R�}���h�͏�����Ɏ��߁Anull�����e���Ȃ�
        if (_ActiveSkills.Length != NUMBER_OF_ACTIVE_SKILL)
        {
            Array.Resize(ref _ActiveSkills, NUMBER_OF_ACTIVE_SKILL);
        }
        for(int i = 0; i < NUMBER_OF_ACTIVE_SKILL; i++)
        {
            if (_ActiveSkills[i] is null)
                _ActiveSkills[i] = new CommandActiveSkillBase();

            //�e�X�L���R�}���h�����������s
            _ActiveSkills[i].Initialize();
        }

        //�e��R�}���h�����������s
        _CommandJump.Initialize();
        _CommandShiftSlide.Initialize();
        _CommandLongTrip.Initialize();
    }
}

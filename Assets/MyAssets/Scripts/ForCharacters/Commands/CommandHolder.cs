using System;
using UnityEngine;

public class CommandHolder : MonoBehaviour
{
    /// <summary>�A�N�e�B�u�X�L���R�}���h�̐�</summary>
    const byte NUMBER_OF_ACTIVE_SKILL = 4;

    #region �����o
    [SerializeField, Tooltip("�W�����v�p�R�}���h")]
    CommandJumpBase _CommandJump = new CommandJumpBase();

    [SerializeField, Tooltip("�Z��������R�}���h")]
    CommandShiftSlideBase _CommandShiftSlide = new CommandShiftSlideBase();

    [SerializeField, Tooltip("����������R�}���h")]
    CommandLongTripBase _CommandLongTrip = new CommandLongTripBase();

    [SerializeField, Tooltip("�A�N�e�B�u�X�L���R�}���h�̃��X�g")]
    CommandActiveSkillBase[] _ActiveSkills = new CommandActiveSkillBase[NUMBER_OF_ACTIVE_SKILL];

    [SerializeField, Tooltip("�R���{���R���|�[�l���g")]
    CommandCombo _CommandCombo = new CommandCombo();

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
    /// <summary>�R���{���R���|�[�l���g</summary>
    public CommandCombo Combo { get => _CommandCombo; }
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
        }
    }
}

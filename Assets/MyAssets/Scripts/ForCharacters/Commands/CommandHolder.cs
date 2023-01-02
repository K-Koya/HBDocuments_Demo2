using System;
using UnityEngine;

public class CommandHolder : MonoBehaviour
{
    /// <summary>アクティブスキルコマンドの数</summary>
    const byte NUMBER_OF_ACTIVE_SKILL = 4;

    #region メンバ
    [SerializeField, Tooltip("ジャンプ用コマンド")]
    CommandJumpBase _CommandJump = new CommandJumpBase();

    [SerializeField, Tooltip("短距離回避コマンド")]
    CommandShiftSlideBase _CommandShiftSlide = new CommandShiftSlideBase();

    [SerializeField, Tooltip("長距離回避コマンド")]
    CommandLongTripBase _CommandLongTrip = new CommandLongTripBase();

    [SerializeField, Tooltip("アクティブスキルコマンドのリスト")]
    CommandActiveSkillBase[] _ActiveSkills = new CommandActiveSkillBase[NUMBER_OF_ACTIVE_SKILL];

    [SerializeField, Tooltip("コンボ情報コンポーネント")]
    CommandCombo _CommandCombo = new CommandCombo();

    #endregion

    #region プロパティ
    /// <summary>ジャンプ用コマンド</summary>
    public CommandJumpBase Jump { get => _CommandJump; }
    /// <summary>短距離回避コマンド</summary>
    public CommandShiftSlideBase ShiftSlide { get => _CommandShiftSlide; }
    /// <summary>長距離回避コマンド</summary>
    public CommandLongTripBase LongTrip { get => _CommandLongTrip; }
    /// <summary>アクティブスキルコマンドのリスト</summary>
    public CommandActiveSkillBase[] ActiveSkills { get => _ActiveSkills; }
    /// <summary>コンボ情報コンポーネント</summary>
    public CommandCombo Combo { get => _CommandCombo; }
    #endregion


    void Start()
    {
        //各種コマンドはnullを許容しない
        if (_CommandJump is null) 
            _CommandJump = new CommandJumpBase();
        if (_CommandShiftSlide is null) 
            _CommandShiftSlide = new CommandShiftSlideBase();
        if (_CommandLongTrip is null) 
            _CommandLongTrip = new CommandLongTripBase();

        //アクティブスキルコマンドは上限数に収め、nullを許容しない
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

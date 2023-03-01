using System;
using UnityEditor.Hardware;
using UnityEngine;

public class CommandHolder : MonoBehaviour
{
    /// <summary>アクティブスキルコマンドの数</summary>
    const byte NUMBER_OF_ACTIVE_SKILL = 4;

    #region メンバ
    [Header("所持コマンド情報")]
    [SerializeField, Tooltip("ジャンプ用コマンド")]
    CommandJumpBase _CommandJump = null;

    [SerializeField, Tooltip("短距離回避コマンド")]
    CommandShiftSlideBase _CommandShiftSlide = null;

    [SerializeField, Tooltip("長距離回避コマンド")]
    CommandLongTripBase _CommandLongTrip = null;

    [SerializeReference, SelectableSerializeReference, Tooltip("アクティブスキルコマンドのリスト")]
    CommandActiveSkillBase[] _ActiveSkills = null;

    [SerializeField, Tooltip("コンボコマンド")]
    CommandCombo _CommandCombo = null;

    [Header("実行中アクティブスキルコマンド情報")]
    [SerializeField, Tooltip("アクティブスキルコマンド・コンボコマンドのうち実行中のコマンド")]
    CommandActiveSkillBase _Running = null;

    /// <summary>ひとつ前の実行中のコマンド</summary>
    CommandActiveSkillBase _BeforeRunning = null;
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
    /// <summary>コンボコマンド</summary>
    public CommandCombo Combo { get => _CommandCombo; }
    /// <summary>アクティブスキルコマンド・コンボコマンドのうち実行中のコマンド</summary>
    public CommandActiveSkillBase Running { get => _Running; }
    #endregion

    #region ゲッターメソッド
    /// <summary>コンボコマンドを取得して実行状態にする</summary>
    /// <returns>コンボコマンド</returns>
    public CommandCombo GetComboForRun()
    {
        _Running = _CommandCombo;
        return _CommandCombo;
    }

    /// <summary>アクティブスキルコマンドを取得して実行状態にする</summary>
    /// <param name="index">スキルリストにアクセスするための添え字番号</param>
    /// <returns>アクティブスキルコマンド</returns>
    public CommandActiveSkillBase GetActiveSkillForRun(int index)
    {
        _Running = ActiveSkills[index];
        return _Running;
    }

    /// <summary>アクティブスキルコマンドを取得して実行破棄する</summary>
    /// <returns>アクティブスキルコマンド</returns>
    public CommandActiveSkillBase GetActiveSkillForPostProcess()
    {
        CommandActiveSkillBase cashe = _Running;
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

            //各スキルコマンドを初期化実行
            _ActiveSkills[i].Initialize(layerSet);
        }

        //各種コマンドを初期化実行
        _CommandJump.Initialize(layerSet);
        _CommandShiftSlide.Initialize(layerSet);
        _CommandLongTrip.Initialize(layerSet);
    }

    /// <summary>コードからコマンドをセットするメソッド</summary>
    /// <param name="commands">対象のアクティブスキルコマンド</param>
    public void SetActiveSkills(params CommandActiveSkillBase[] commands)
    {
        _ActiveSkills = new CommandActiveSkillBase[NUMBER_OF_ACTIVE_SKILL];
        for(int i = 0; i < NUMBER_OF_ACTIVE_SKILL; i++)
        {
            _ActiveSkills[i] = commands[i];
        }
    }

    CommandActiveSkillBase ActiveSkillSelector(int id)
    {
        CommandActiveSkillBase returnal = null;

        switch (id)
        {
            case 1:
                returnal = new CommandFireShot();
                break;
            case 2:
                returnal = new CommandFreezeShot();
                break;
            case 3:

                break;
            case 4:

                break;


            case 500:

                break;

            case 900:

                break;
        }

        return returnal;
    }
}

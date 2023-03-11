using System;
using UnityEditor.Hardware;
using UnityEngine;

public class CommandHolder : MonoBehaviour
{
    /// <summary>アクティブスキルコマンドの数</summary>
    const byte NUMBER_OF_ACTIVE_SKILL = 4;

    #region メンバ
    [SerializeField, Tooltip("アクティブスキルコマンドのID")]
    int[] _ActiveSkillsId = null;

    [Header("所持コマンド情報")]
    [SerializeField, Tooltip("ジャンプ用コマンド")]
    MotionJumpBase _CommandJump = null;

    [SerializeField, Tooltip("短距離回避コマンド")]
    MotionShiftSlideBase _CommandShiftSlide = null;

    [SerializeField, Tooltip("長距離回避コマンド")]
    MotionLongTripBase _CommandLongTrip = null;

    [SerializeReference, SelectableSerializeReference, Tooltip("アクティブスキルコマンドのリスト")]
    CommandBase[] _ActiveSkills = null;

    [SerializeField, Tooltip("コンボコマンド")]
    CommandCombo _CommandCombo = null;

    [Header("実行中アクティブスキルコマンド情報")]
    [SerializeField, Tooltip("アクティブスキルコマンド・コンボコマンドのうち実行中のコマンド")]
    CommandBase _Running = null;

    /// <summary>ひとつ前の実行中のコマンド</summary>
    CommandBase _BeforeRunning = null;
    #endregion

    #region プロパティ
    /// <summary>ジャンプ用コマンド</summary>
    public MotionJumpBase Jump { get => _CommandJump; }
    /// <summary>短距離回避コマンド</summary>
    public MotionShiftSlideBase ShiftSlide { get => _CommandShiftSlide; }
    /// <summary>長距離回避コマンド</summary>
    public MotionLongTripBase LongTrip { get => _CommandLongTrip; }
    /// <summary>アクティブスキルコマンドのリスト</summary>
    public CommandBase[] ActiveSkills { get => _ActiveSkills; }
    /// <summary>コンボコマンド</summary>
    public CommandCombo Combo { get => _CommandCombo; }
    /// <summary>アクティブスキルコマンド・コンボコマンドのうち実行中のコマンド</summary>
    public CommandBase Running { get => _Running; }
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
    public CommandBase GetActiveSkillForRun(int index)
    {
        _Running = ActiveSkills[index];
        return _Running;
    }

    /// <summary>アクティブスキルコマンドを取得して実行破棄する</summary>
    /// <returns>アクティブスキルコマンド</returns>
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

        //各種コマンドはnullを許容しない
        if (_CommandJump is null) 
            _CommandJump = new MotionJumpBase();
        if (_CommandShiftSlide is null) 
            _CommandShiftSlide = new MotionShiftSlideBase();
        if (_CommandLongTrip is null) 
            _CommandLongTrip = new MotionLongTripBase();
        if(_CommandCombo is null)
            _CommandCombo = new CommandCombo();


        //アクティブスキルコマンドは上限数に収め、nullを許容しない
        _ActiveSkills = new CommandBase[NUMBER_OF_ACTIVE_SKILL];
        if (_ActiveSkills.Length != NUMBER_OF_ACTIVE_SKILL)
        {
            Array.Resize(ref _ActiveSkills, NUMBER_OF_ACTIVE_SKILL);
        }
        for(int i = 0; i < NUMBER_OF_ACTIVE_SKILL; i++)
        {
            //全コマンド格納庫から取得
            _ActiveSkills[i] = CommandDictionary.Instance.CloneCommand((ushort)_ActiveSkillsId[i]);

            //各スキルコマンドを初期化実行
            _ActiveSkills[i].Initialize(layerSet);
        }
    }

    void Update()
    {
        //Debug.Log($"{_ActiveSkills[0]} {_ActiveSkills[1]} {_ActiveSkills[2]} {_ActiveSkills[3]}");
    }

    /// <summary>コードからコマンドをセットするメソッド</summary>
    /// <param name="commands">対象のアクティブスキルコマンド</param>
    public void SetActiveSkills(params CommandBase[] commands)
    {
        _ActiveSkills = new CommandBase[NUMBER_OF_ACTIVE_SKILL];
        for(int i = 0; i < NUMBER_OF_ACTIVE_SKILL; i++)
        {
            _ActiveSkills[i] = commands[i];
        }
    }
}

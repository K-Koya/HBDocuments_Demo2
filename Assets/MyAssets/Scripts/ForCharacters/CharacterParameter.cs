using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>�L�����N�^�[�̏����܂Ƃ߂�R���|�[�l���g</summary>
[RequireComponent(typeof(Timeline))]
abstract public class CharacterParameter : MonoBehaviour
{
    #region �L�����Ԃ̃A�N�Z�b�T
    /// <summary>�v���C���[�L�����N�^�[���i�[</summary>
    protected static CharacterParameter _Player = null;

    /// <summary>�����L�����N�^�[���i�[</summary>
    protected static List<CharacterParameter> _Allies = new List<CharacterParameter>(5);

    /// <summary>�G�L�����N�^�[���i�[</summary>
    protected static List<CharacterParameter> _Enemies = new List<CharacterParameter>(12);
    #endregion

    #region ���C���p�����[�^
    [SerializeField, Tooltip("�ő�HP")]
    protected short _HPMaximum = 1000;

    [SerializeField, Tooltip("���݂�HP")]
    protected short _HPCurrent = 1000;

    [SerializeField, Tooltip("�ő�MP")]
    protected float _MPMaximum = 10f;

    [SerializeField, Tooltip("���݂�MP")]
    protected float _MPCurrent = 10f;


    /// <summary>�ő�HP</summary>
    public short HPMaximum { get => _HPMaximum; }
    /// <summary>���݂�HP</summary>
    public short HPCurrent { get => _HPCurrent; }
    /// <summary>�ő�MP</summary>
    public float MPMaximum { get => _MPMaximum; }
    /// <summary>���݂�MP</summary>
    public float MPCurrent { get => _MPCurrent; }

    #endregion

    #region �T�u�p�����[�^
    [SerializeField, Tooltip("�Ə��˒���")]
    protected float _LockMaxRange = 30f;

    [SerializeField, Tooltip("�ʏ�R���{�˒���")]
    protected float _ComboProximityRange = 5f;

    [SerializeField, Tooltip("���s�ō���")]
    protected float _LimitSpeedWalk = 2f;

    [SerializeField, Tooltip("���s�ō���")]
    protected float _LimitSpeedRun = 5f;


    /// <summary>�Ə��˒���</summary>
    public float LockMaxRange { get => _LockMaxRange; }
    /// <summary>�ʏ�R���{�˒���</summary>
    public float ComboProximityRange { get => _ComboProximityRange; }
    /// <summary>���s�ō���</summary>
    public float LimitSpeedWalk { get => _LimitSpeedWalk; }
    /// <summary>���s�ō���</summary>
    public float LimitSpeedRun { get => _LimitSpeedRun; }

    #endregion

    #region �����o
    [SerializeField, Tooltip("�L�����N�^�[�̖ڐ��ʒu")]
    protected Transform _EyePoint = null;

    /// <summary>���Y�L�����N�^�[�������Ԏ��R���|�[�l���g</summary>
    protected Timeline _Tl = null;

    /// <summary>true : �L�����N�^�[�̌����ƈړ������𓯊�����</summary>
    protected bool _IsSyncDirection = true;

    /// <summary>�L�����N�^�[���ʕ������</summary>
    protected Vector3 _Direction = default;

    [SerializeField, Tooltip("�U�����󂯂�Ώۂ̃��C���[")]
    protected LayerMask _HostilityLayer = default;

    /// <summary>�������Ă��鑊��̃p�����[�^</summary>
    protected CharacterParameter _GazeAt = null;

    /// <summary>�L�����N�^�[�̓����蔻��R���C�_�[</summary>
    protected Collider _HitArea = null;

    [SerializeField, Tooltip("�L�����N�^�[�̍s�����")]
    protected MotionState _State = null;

    [SerializeField, Tooltip("����ۏ��")]
    protected InputAcceptance _Acceptance = null;

    

    /// <summary>�U���͈͏��</summary>
    protected List<AttackArea> _AttackAreas = new List<AttackArea>(10);

    /// <summary>�ł��ŋߎ󂯂��U���̏��</summary>
    protected AttackInformation _GaveAttack = null;
    #endregion


    #region �v���p�e�B
    /// <summary>�v���C���[�L�����N�^�[���i�[</summary>
    public static CharacterParameter Player => _Player;
    /// <summary>�����L�����N�^�[�����X�g������Ă���</summary>
    public static IReadOnlyList<CharacterParameter> Allies => _Allies;
    /// <summary>�G�L�����N�^�[�����X�g������Ă���</summary>
    public static IReadOnlyList<CharacterParameter> Enemies => _Enemies;
    /// <summary>�L�����N�^�[�̖ڐ��ʒu</summary>
    public Transform EyePoint { get => _EyePoint; set => _EyePoint = value; }
    /// <summary>�{�L�����N�^�[�̎��ԏ��</summary>
    public Timeline Tl { get => _Tl; }
    /// <summary>true : �L�����N�^�[�̌����ƈړ������𓯊�����</summary>
    public bool IsSyncDirection { get => _IsSyncDirection; set => _IsSyncDirection = value; }
    /// <summary>�L�����N�^�[���ʕ������</summary>
    public Vector3 Direction { get => _Direction; set => _Direction = value; }
    /// <summary>�U�����󂯂�Ώۂ̃��C���[</summary>
    public LayerMask HostilityLayer { get => _HostilityLayer; }
    /// <summary>�������Ă��鑊��̃p�����[�^</summary>
    public CharacterParameter GazeAt { get => _GazeAt; set => _GazeAt = value; }
    /// <summary>�L�����N�^�[�̓����蔻��R���C�_�[</summary>
    public Collider HitArea { get => _HitArea; }
    /// <summary>�L�����N�^�[�̍s�����</summary>
    public MotionState State { get => _State; }
    /// <summary>����ۏ��</summary>
    public InputAcceptance Can { get => _Acceptance; }
    /// <summary>�U���͈͏��</summary>
    public List<AttackArea> AttackAreas => _AttackAreas;
    /// <summary>�ł��ŋߎ󂯂��U���̏��</summary>
    public AttackInformation GaveAttack { get => _GaveAttack; set => _GaveAttack = value; }
    #endregion

    /// <summary>�{�N���X�̐ÓI�����o�Ɏ��R���|�[�l���g��o�^�����郁�\�b�h</summary>
    abstract protected void RegisterStaticReference();

    /// <summary>�{�N���X�̐ÓI�����o���玩�R���|�[�l���g�𖕏����郁�\�b�h</summary>
    abstract protected void EraseStaticReference();

    void Awake()
    {
        _State = new MotionState();
        _Acceptance = new InputAcceptance();

        RegisterStaticReference();
    }

    void OnDestroy()
    {
        EraseStaticReference();
    }

    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Tl = GetComponent<Timeline>();
        _HitArea = GetComponent<Collider>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        SetAcceptant();
    }

    /// <summary>����ۏ���l�X�ȃX�e�[�g��Ԃ���ݒ肷��</summary>
    void SetAcceptant()
    {
        switch (_State.Kind)
        {
            case MotionState.StateKind.Stay:
            case MotionState.StateKind.Walk:
                _Acceptance.Move = true;
                _Acceptance.Jump = true;
                _Acceptance.ShiftSlide = true;
                _Acceptance.LongTrip = true;
                _Acceptance.Gurad = true;
                _Acceptance.ComboNormal = true;
                _Acceptance.ComboFinish = false;
                _IsSyncDirection = true;

                break;
            case MotionState.StateKind.Run:
                _Acceptance.Move = true;
                _Acceptance.Jump = true;
                _Acceptance.ShiftSlide = false;
                _Acceptance.LongTrip = true;
                _Acceptance.Gurad = true;
                _Acceptance.ComboNormal = true;
                _Acceptance.ComboFinish = false;
                _IsSyncDirection = true;

                break;
            case MotionState.StateKind.JumpNoraml:
            case MotionState.StateKind.FallNoraml:
                _Acceptance.Move = true;
                _Acceptance.Jump = false;
                _Acceptance.ShiftSlide = false;
                _Acceptance.LongTrip = false;
                _Acceptance.Gurad = false;
                _Acceptance.ComboNormal = true;
                _Acceptance.ComboFinish = false;
                _IsSyncDirection = true;

                break;
            case MotionState.StateKind.ShiftSlide:
                switch (_State.Process)
                {
                    case MotionState.ProcessKind.Preparation:
                    case MotionState.ProcessKind.Playing:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;
                        _IsSyncDirection = false;
                        break;

                    case MotionState.ProcessKind.EndSoon:
                        _State.Kind = MotionState.StateKind.Stay;
                        _State.Process = MotionState.ProcessKind.Playing;
                        _Acceptance.Move = true;
                        _Acceptance.Jump = true;
                        _Acceptance.ShiftSlide = true;
                        _Acceptance.LongTrip = true;
                        _Acceptance.Gurad = true;
                        _Acceptance.ComboNormal = true;
                        _Acceptance.ComboFinish = false;
                        _IsSyncDirection = true;
                        break;

                }

                break;
            case MotionState.StateKind.LongTrip:
                switch (_State.Process)
                {
                    case MotionState.ProcessKind.Preparation:
                    case MotionState.ProcessKind.Playing:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;
                        _IsSyncDirection = false;
                        break;

                    case MotionState.ProcessKind.EndSoon:
                        _State.Kind = MotionState.StateKind.FallNoraml;
                        _State.Process = MotionState.ProcessKind.Playing;
                        _Acceptance.Move = true;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = true;
                        _Acceptance.ComboFinish = false;
                        _IsSyncDirection = true;
                        break;
                }

                break;
            case MotionState.StateKind.Guard:
                _Acceptance.Move = false;
                _Acceptance.Jump = false;
                _Acceptance.ShiftSlide = false;
                _Acceptance.LongTrip = false;
                _Acceptance.Gurad = true;
                _Acceptance.ComboNormal = false;
                _Acceptance.ComboFinish = false;

                break;
            case MotionState.StateKind.ComboNormal:

                _IsSyncDirection = true;
                switch (_State.Process)
                {
                    case MotionState.ProcessKind.Interval:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = true;
                        _Acceptance.ComboFinish = true;

                        break;
                    case MotionState.ProcessKind.EndSoon:
                        _State.Kind = MotionState.StateKind.Stay;
                        _State.Process = MotionState.ProcessKind.Playing;
                        _Acceptance.Move = true;
                        _Acceptance.Jump = true;
                        _Acceptance.ShiftSlide = true;
                        _Acceptance.LongTrip = true;
                        _Acceptance.Gurad = true;
                        _Acceptance.ComboNormal = true;
                        _Acceptance.ComboFinish = false;

                        break;
                    default:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;

                        break;
                }
                break;
            case MotionState.StateKind.Hurt:
                switch (_State.Process)
                {
                    case MotionState.ProcessKind.Preparation:
                    case MotionState.ProcessKind.Playing:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;
                        _IsSyncDirection = false;
                        break;

                    case MotionState.ProcessKind.EndSoon:
                        _State.Kind = MotionState.StateKind.FallNoraml;
                        _State.Process = MotionState.ProcessKind.Playing;
                        _Acceptance.Move = true;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = true;
                        _Acceptance.ComboFinish = false;
                        _IsSyncDirection = true;
                        break;
                }
                break;

            default: break;
        }
    }
}

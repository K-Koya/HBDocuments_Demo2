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

    #region �����o
    [SerializeField, Tooltip("�L�����N�^�[�̖ڐ��ʒu")]
    protected Transform _EyePoint = default;

    [SerializeField, Tooltip("���ԂƏW�܂�ۂɂƂ鋗��")]
    protected float _AliseGatherRange = 5f;

    [SerializeField, Tooltip("�����̋ߐڍU���̎˒�:������")]
    protected float _AttackRangeFar = 7f;

    [SerializeField, Tooltip("�����̋ߐڍU���̎˒�:�ߋ���")]
    protected float _AttackRangeMiddle = 5f;

    [SerializeField, Tooltip("�G�ւ̒ǐՂ��p�����锻�苗��")]
    protected float _ChaseEnemyDistance = 40f;

    /// <summary>���Y�L�����N�^�[�������Ԏ��R���|�[�l���g</summary>
    protected Timeline _Tl = default;

    /// <summary>true : �L�����N�^�[�̌����ƈړ������𓯊�����</summary>
    protected bool _IsSyncDirection = true;

    /// <summary>�L�����N�^�[���ʕ������</summary>
    protected Vector3 _Direction = default;

    [SerializeField, Tooltip("�U���𓖂Ă�Ώۂ̃��C���[")]
    protected LayerMask _HostilityLayer = default;

    /// <summary>�L�����N�^�[�̓����蔻��R���C�_�[</summary>
    protected Collider _HitArea = null;

    [SerializeField, Tooltip("�L�����N�^�[�̍s�����")]
    protected MotionState _State = default;

    [SerializeField, Tooltip("����ۏ��")]
    protected InputAcceptance _Acceptance = default;

    [SerializeField, Tooltip("���s�ō���")]
    protected float _LimitSpeedWalk = 2f;

    [SerializeField, Tooltip("���s�ō���")]
    protected float _LimitSpeedRun = 5f;

    /// <summary>�U���͈͏��</summary>
    protected List<AttackArea> _AttackAreas = new List<AttackArea>(10);
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
    /// <summary>�U���𓖂Ă�Ώۂ̃��C���[</summary>
    public LayerMask HostilityLayer { get => _HostilityLayer; }
    /// <summary>�L�����N�^�[�̓����蔻��R���C�_�[</summary>
    public Collider HitArea { get => _HitArea; }
    /// <summary>���ԂƏW�܂�ۂɂƂ鋗��</summary>
    public float AliseGatherRange { get => _AliseGatherRange; }
    /// <summary>�����̋ߐڍU���̎˒�:������</summary>
    public float AttackRangeFar { get => _AttackRangeFar; }
    /// <summary>�����̋ߐڍU���̎˒�:�ߋ���</summary>
    public float AttackRangeMiddle { get => _AttackRangeMiddle; }
    /// <summary>�G�ւ̒ǐՂ��p�����锻�苗��</summary>
    public float ChaseEnemyDistance { get => _ChaseEnemyDistance; }
    /// <summary>�L�����N�^�[�̍s�����</summary>
    public MotionState State { get => _State; }
    /// <summary>����ۏ��</summary>
    public InputAcceptance Can { get => _Acceptance; }
    /// <summary>���s�ō���</summary>
    public float LimitSpeedWalk { get => _LimitSpeedWalk; }
    /// <summary>���s�ō���</summary>
    public float LimitSpeedRun { get => _LimitSpeedRun; }
    /// <summary>�U���͈͏��</summary>
    public List<AttackArea> AttackAreas => _AttackAreas;
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

            default: break;
        }
    }

    void OnDrawGizmos()
    {
        /*
        if(_AttackAreas != null && _AttackAreas.Count > 0)
        {
            Gizmos.color = Color.red;
            foreach(AttackArea at in _AttackAreas)
            {
                Gizmos.DrawWireSphere(at.CenterPos, at.Radius);
                if(at.GetType() == typeof(AttackAreaCapsule))
                {
                    Gizmos.DrawWireSphere((at as AttackAreaCapsule).CenterPos2, at.Radius);
                }
            }
        }

        //�G���Q�[�W�����g�������o��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRangeMiddle);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRangeFar);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ChaseEnemyDistance);
        */
    }
}

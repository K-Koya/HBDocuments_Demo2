using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>�L�����N�^�[�̏����܂Ƃ߂�R���|�[�l���g</summary>
[RequireComponent(typeof(Timeline))]
abstract public class CharacterParameter : MonoBehaviour
{
    #region �萔
    /// <summary>�󂯂��U��ID�̗������Ƃ��</summary>
    public const byte NUMBER_OF_RECORD_GAVE_ATTACK_ID = 8;

    #endregion

    #region �L�����Ԃ̃A�N�Z�b�T
    /// <summary>�v���C���[�L�����N�^�[���i�[</summary>
    protected static CharacterParameter _Player = null;

    /// <summary>�����L�����N�^�[���i�[</summary>
    protected static List<CharacterParameter> _Allies = new List<CharacterParameter>(5);

    /// <summary>�G�L�����N�^�[���i�[</summary>
    protected static List<CharacterParameter> _Enemies = new List<CharacterParameter>(12);
    #endregion

    #region ���C���p�����[�^
    [SerializeField, Tooltip("�L�����N�^�[���F�A���t�@�x�b�g�\�L�iCSV�t�@�C�����[�h�ɗ��p�j")]
    protected string _NameAlphabet = "name";

    [SerializeField, Tooltip("�L�����N�^�[���F�J�^�J�i")]
    protected string _Name = "���O";

    [SerializeField, Tooltip("���C���p�����[�^")]
    protected MainParameter _Main = null;

    [SerializeField, Tooltip("���݂�HP")]
    protected short _HPCurrent = 1000;

    [SerializeField, Tooltip("�ő�MP")]
    protected float _MPMaximum = 100f;

    [SerializeField, Tooltip("���݂�MP")]
    protected float _MPCurrent = 100f;
    #endregion

    #region �T�u�p�����[�^
    [System.Serializable]
    public class SubParameter
    {
        [SerializeField, Tooltip("�Ə��˒���")]
        float _LockMaxRange = 30f;

        [SerializeField, Tooltip("�ʏ�R���{�˒���")]
        float _ComboProximityRange = 5f;

        [SerializeField, Tooltip("���s�ō���")]
        float _LimitSpeedWalk = 2f;

        [SerializeField, Tooltip("���s�ō���")]
        float _LimitSpeedRun = 5f;


        /// <summary>�Ə��˒���</summary>
        public float LockMaxRange { get => _LockMaxRange; }
        /// <summary>�ʏ�R���{�˒���</summary>
        public float ComboProximityRange { get => _ComboProximityRange; }
        /// <summary>���s�ō���</summary>
        public float LimitSpeedWalk { get => _LimitSpeedWalk; }
        /// <summary>���s�ō���</summary>
        public float LimitSpeedRun { get => _LimitSpeedRun; }
    }

    [SerializeField, Tooltip("�T�u�p�����[�^")]
    SubParameter _Sub = new SubParameter();
    #endregion

    #region �����o
    [SerializeField, Tooltip("�L�����N�^�[�̖ڐ��ʒu")]
    protected Transform _EyePoint = null;

    /// <summary>�Ə���̈ʒu</summary>
    protected Vector3 _ReticlePoint = Vector3.zero;

    /// <summary>���Y�L�����N�^�[�������Ԏ��R���|�[�l���g</summary>
    protected Timeline _Tl = null;

    /// <summary>�L�����N�^�[�̌������</summary>
    protected Vector3 _CharacterDirection = default;

    /// <summary>�L�����N�^�[�̈ړ��������</summary>
    protected Vector3 _MoveDirection = default;

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


    [SerializeField, Tooltip("�I�u�W�F�N�g�̎ˏo�ʒu���")]
    protected Transform[] _EmitPoints = null;

    [System.Serializable]
    public class CollidersForOneWeapon
    {
        [SerializeField]
        public AttackCollision[] _AttackCollisions = null;
    }
    [SerializeField, Tooltip("�U���͈͏��")]
    protected CollidersForOneWeapon[] _AttackAreas = null;


    /// <summary>�󂯂��U����ID����</summary>
    protected Queue<byte> _GaveAttackIDs = new Queue<byte>(NUMBER_OF_RECORD_GAVE_ATTACK_ID);
    #endregion


    #region �v���p�e�B
    /// <summary>�v���C���[�L�����N�^�[���i�[</summary>
    public static CharacterParameter Player => _Player;
    /// <summary>�����L�����N�^�[�����X�g������Ă���</summary>
    public static IReadOnlyList<CharacterParameter> Allies => _Allies;
    /// <summary>�G�L�����N�^�[�����X�g������Ă���</summary>
    public static IReadOnlyList<CharacterParameter> Enemies => _Enemies;

    /// <summary>�L�����N�^�[���F�A���t�@�x�b�g�\�L</summary>
    public string NameAlphabet => _NameAlphabet;
    /// <summary>�L�����N�^�[���F�J�^�J�i</summary>
    public string Name => _Name;
    /// <summary>���C���p�����[�^</summary>
    public MainParameter Main => _Main;
    /// <summary>�ő��HP</summary>
    public virtual short HPMaximum => _Main.HPMaximum;
    /// <summary>���݂�HP</summary>
    public virtual short HPCurrent => _HPCurrent;
    /// <summary>�ő�MP</summary>
    public virtual float MPMaximum => _MPMaximum;
    /// <summary>���݂�MP</summary>
    public virtual float MPCurrent => _MPCurrent;
    /// <summary>�T�u�p�����[�^</summary>
    public SubParameter Sub => _Sub;

    /// <summary>�L�����N�^�[�̖ڐ��ʒu</summary>
    public Transform EyePoint { get => _EyePoint; set => _EyePoint = value; }
    /// <summary>�Ə���̈ʒu</summary>
    public Vector3 ReticlePoint { get => _ReticlePoint; set => _ReticlePoint = value; }
    /// <summary>�{�L�����N�^�[�̎��ԏ��</summary>
    public Timeline Tl { get => _Tl; }
    /// <summary>�L�����N�^�[�̌������</summary>
    public Vector3 CharacterDirection { get => _CharacterDirection; set => _CharacterDirection = value; }
    /// <summary>�L�����N�^�[�̈ړ��������</summary>
    public Vector3 MoveDirection { get => _MoveDirection; set => _MoveDirection = value; }
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

    /// <summary>�I�u�W�F�N�g�̎ˏo�ʒu���</summary>
    public Transform[] EmitPoints => _EmitPoints;
    /// <summary>�U���͈͏��</summary>
    public CollidersForOneWeapon[] AttackAreas => _AttackAreas;
    /// <summary>�󂯂��U����ID����</summary>
    public Queue<byte> GaveAttackIDs => _GaveAttackIDs;
    #endregion

    /// <summary>�{�N���X�̐ÓI�����o�Ɏ��R���|�[�l���g��o�^�����郁�\�b�h</summary>
    abstract protected void RegisterStaticReference();

    /// <summary>�{�N���X�̐ÓI�����o���玩�R���|�[�l���g�𖕏����郁�\�b�h</summary>
    abstract protected void EraseStaticReference();


    void Awake()
    {
        _Main = new MainParameter();
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

        //��U��������������
        for(int i = 0; i < NUMBER_OF_RECORD_GAVE_ATTACK_ID; i++)
        {
            _GaveAttackIDs.Enqueue(byte.MaxValue);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        SetAcceptant();
    }

    /// <summary>�_���[�W����</summary>
    /// <param name="damage">��_���[�W�l</param>
    public virtual void GaveDamage(int damage)
    {
        _HPCurrent -= (short)damage;
    }

    /// <summary>�񕜏���</summary>
    /// <param name="ratioOfHP">�ő�HP�ɑ΂���񕜗ʊ���</param>
    public virtual void GaveHeal(float ratioOfHP)
    {
        short recover = (short)(ratioOfHP * _Main.HPMaximum);
        _HPCurrent += recover;
        _HPCurrent = _HPCurrent > _Main.HPMaximum ? _Main.HPMaximum : _HPCurrent;
        Debug.Log($"{_Name}�ɁAHP��{recover}�񕜂������");

        GameObject eff = EffectManager.Instance.HealEffects.Instansiate();
        eff.transform.position = _EyePoint.position;
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
                _Acceptance.Command = true;

                break;
            case MotionState.StateKind.Run:
                _Acceptance.Move = true;
                _Acceptance.Jump = true;
                _Acceptance.ShiftSlide = false;
                _Acceptance.LongTrip = true;
                _Acceptance.Gurad = true;
                _Acceptance.ComboNormal = true;
                _Acceptance.ComboFinish = false;
                _Acceptance.Command = true;

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
                _Acceptance.Command = true;

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
                        _Acceptance.Command = false;
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
                        _Acceptance.Command = true;
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
                        _Acceptance.Command = false;
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
                        _Acceptance.Command = true;
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
                _Acceptance.Command = false;

                break;
            case MotionState.StateKind.ComboNormal:

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
                        _Acceptance.Command = false;

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
                        _Acceptance.Command = true;

                        break;
                    default:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;
                        _Acceptance.Command = false;

                        break;
                }

                break;
            case MotionState.StateKind.AttackCommand:
            case MotionState.StateKind.HealCommand:
            case MotionState.StateKind.SupportCommand:
                switch (_State.Process)
                {
                    case MotionState.ProcessKind.Interval:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;
                        _Acceptance.Command = true;

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
                        _Acceptance.Command = true;

                        break;
                    default:
                        _Acceptance.Move = false;
                        _Acceptance.Jump = false;
                        _Acceptance.ShiftSlide = false;
                        _Acceptance.LongTrip = false;
                        _Acceptance.Gurad = false;
                        _Acceptance.ComboNormal = false;
                        _Acceptance.ComboFinish = false;
                        _Acceptance.Command = false;

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
                        _Acceptance.Command = false;
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
                        _Acceptance.Command = true;
                        break;
                }
                break;

            default: break;
        }
    }
}

[System.Serializable]
public class MainParameter
{
    [SerializeField, Tooltip("�ő�HP")]
    short _HPMaximum = 1000;

    [SerializeField, Tooltip("���ڍU��")]
    short _Attack = 1000;

    [SerializeField, Tooltip("���ږh��")]
    short _Defense = 1000;

    [SerializeField, Tooltip("�ԐڍU��")]
    short _Magic = 1000;

    [SerializeField, Tooltip("�Ԑږh��")]
    short _Shield = 1000;

    [SerializeField, Tooltip("�q����")]
    short _Rapid = 1000;

    [SerializeField, Tooltip("�Z�p��")]
    short _Technique = 1000;

    [SerializeField, Tooltip("���q")]
    short _Luck = 1000;

    /// <summary>�ő�HP</summary>
    public short HPMaximum { get => _HPMaximum; }
    /// <summary>���ڍU��</summary>
    public short Attack { get => _Attack; }
    /// <summary>���ږh��</summary>
    public short Defense { get => _Defense; }
    /// <summary>�ԐڍU��</summary>
    public short Magic { get => _Magic; }
    /// <summary>�Ԑږh��</summary>
    public short Shield { get => _Shield; }
    /// <summary>�q����</summary>
    public short Rapid { get => _Rapid; }
    /// <summary>�Z�p��</summary>
    public short Technique { get => _Technique; }
    /// <summary>���q</summary>
    public short Luck { get => _Luck; }
}
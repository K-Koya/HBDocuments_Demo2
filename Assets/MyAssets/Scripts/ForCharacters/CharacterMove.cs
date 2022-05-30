using System;
using UnityEngine;
using Chronos;

[RequireComponent(typeof(Rigidbody), typeof(Timeline))]
abstract public class CharacterMove : MonoBehaviour
{
    #region �萔
    /// <summary>���x��0�ł���Ƃ݂Ȃ����l</summary>
    protected const float VELOCITY_ZERO_BORDER = 0.5f;
    #endregion

    #region �����o
    [SerializeField, Tooltip("�L�����N�^�[�̖ڐ��ʒu")]
    protected Transform _EyePoint = default;

    /// <summary>���Y�L�����N�^�[�������Ԏ��R���|�[�l���g</summary>
    protected Timeline _Tl = default;

    /// <summary>�L�����N�^�[���ʕ������</summary>
    protected Vector3 _CharacterDirection = default;

    /// <summary>true : �W�����v����</summary>
    protected bool _JumpFlag = false;

    /// <summary>���n���������R���|�[�l���g</summary>
    GroundChecker _GroundChecker = default;


    /// <summary>�ړ��p���\�b�h</summary>
    protected Action Move = default;


    #endregion

    #region �v���p�e�B
    /// <summary>�L�����N�^�[�̖ڐ��ʒu</summary>
    public Transform EyePoint { get => _EyePoint; set => _EyePoint = value; }
    /// <summary>True : ���n���Ă���</summary>
    public bool IsGround => _GroundChecker.IsGround;
    /// <summary>�d�͕���</summary>
    protected Vector3 GravityDirection => _GroundChecker.GravityDirection;
    /// <summary>�ړ����x</summary>
    abstract public float Speed { get; }
    /// <summary>�W�����v����t���O</summary>
    public bool JumpFlag => _JumpFlag;
    #endregion




    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Tl = GetComponent<Timeline>();
        _GroundChecker = GetComponent<GroundChecker>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
    }
}

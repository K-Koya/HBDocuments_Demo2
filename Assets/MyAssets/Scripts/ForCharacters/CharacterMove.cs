using System;
using UnityEngine;
using Chronos;

[RequireComponent(typeof(Rigidbody), typeof(GroundChecker), typeof(Timeline))]
public class CharacterMove : MonoBehaviour
{
    #region �萔
    /// <summary>���x��0�ł���Ƃ݂Ȃ����l</summary>
    protected const float VELOCITY_ZERO_BORDER = 2f;
    #endregion

    #region �����o
    [SerializeField, Tooltip("�L�����N�^�[�̖ڐ��ʒu")]
    protected Transform _EyePoint = default;

    /// <summary>���Y�L�����N�^�[�������Ԏ��R���|�[�l���g</summary>
    protected Timeline _Tl = default;


    /// <summary>�ړ��������</summary>
    protected Vector3 _MoveDirection = default;

    /// <summary>�L�����N�^�[���ʕ������</summary>
    protected Vector3 _CharacterDirection = default;



    /// <summary>���n���������R���|�[�l���g</summary>
    GroundChecker _GroundChecker = default;

    /// <summary>�ړ��p���\�b�h</summary>
    protected Action Move = default;
    #endregion

    #region �v���p�e�B
    /// <summary>�L�����N�^�[�̖ڐ��ʒu</summary>
    public Transform EyePoint { get => _EyePoint; set => _EyePoint = value; }
    /// <summary>True : ���n���Ă���</summary>
    protected bool IsGround => _GroundChecker.IsGround;
    /// <summary>�d�͕���</summary>
    protected Vector3 GravityDirection => _GroundChecker.GravityDirection;

    

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

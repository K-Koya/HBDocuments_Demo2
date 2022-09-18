using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterParameter), typeof(ComputerMove))]
public class ComputerAIBase : MonoBehaviour
{
    #region �萔�l
    /// <summary>�ړI�n�ɓ��������Ƃ݂Ȃ�����</summary>
    const float _ARRAIVAL_DISTANCE_BASE = 0.75f;

    /// <summary>�ēx�ړI�n��ڎw���Ĉړ������悤�Ƃ��鋗��</summary>
    const float _RESET_ARRAIVAL_DISTANCE = 2f;

    #endregion

    #region �����o
    /// <summary>�����̎�����̊�ɂȂ�ʒu</summary>
    Vector3 _BasePosition = default;

    /// <summary>�L�����N�^�[�̎����</summary>
    CharacterParameter _Param = null;

    /// <summary>���ڃL�����N�^�[�̎����</summary>
    CharacterParameter _TargetParam = null;

    /// <summary>�L�����N�^�[���ړ�������@�\���W�񂵂��R���|�[�l���g</summary>
    ComputerMove _Move = null;

    /// <summary>�^�[�Q�b�g���ӂ�����������]���</summary>
    Quaternion _RelativeOrbitQuat = Quaternion.identity;

    /// <summary>�v�l���\�b�h</summary>
    System.Action Think = null;

    /// <summary>�s�����\�b�h</summary>
    System.Action Movement = null;

    [SerializeField, Tooltip("�����̐����Ԋu")]
    float _CreateRandomInterval = 1f;

    [SerializeField, Tooltip("������ŋ��ꏊ��ς��悤�Ƃ���m��(0�`100)")]
    sbyte _DetectAthorPlaceRatio = 5;

    [SerializeField, Tooltip("���R�s������ꍇ�̍s���͈�")]
    float _WanderingDistance = 8f;
    #endregion

    #region �R���s���[�^�[�p���͏��


    #endregion

    #region �v���p�e�B

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _BasePosition = transform.position;

        _Param = GetComponent<CharacterParameter>();
        _Move = GetComponent<ComputerMove>();

        _TargetParam = CharacterParameter.Player;

        Think = ApproachToVigilanceMiddle;
        Movement = Trip;
    }

    // Update is called once per frame
    void Update()
    {
        Think?.Invoke();
        Movement?.Invoke();

        //Debug.Log($"Think : {Think.Method.Name} & Movement : {Movement.Method.Name}");
    }
    
    /// <summary>�ړI�n���B����</summary>
    /// <param name="destination">�ړI�n</param>
    /// <returns>true : ��������</returns>
    bool GetArrival(Vector3 destination, float buffer = 0f)
    {
        float sqrArrivalDistance = Mathf.Pow(_ARRAIVAL_DISTANCE_BASE + buffer + _Move.Speed * _Move.Speed * 0.5f, 2);
        return Vector3.SqrMagnitude(destination - transform.position) < sqrArrivalDistance;
    }

    /// <summary>�v�l���\�b�h : ���낤�낷��</summary>
    void Wandering()
    {
        _Move.MovePower = _Param.LimitSpeedWalk;

        //�s����Ԃ��ҋ@�Ȃ�ʂ̈ړ�����l����悤�Ɏv��
        if (Movement == Staying)
        {
            if ((_DetectAthorPlaceRatio / 100.0f) > Random.value)
            {
                //�ړ�����w�肵�������ňړ�
                _Move.Destination = _BasePosition + new Vector3(Random.Range(1, _WanderingDistance), 0f, Random.Range(1, _WanderingDistance));
                Movement = Trip;
            }
        }
        //�s����Ԃ��ړ����Ȃ�ړI�n�t�߂֓����������m�F
        else if (Movement == Trip)
        {
            if (GetArrival(_Move.DestinationOnNavMesh))
            {
                _Move.Destination = null;
                Movement = Staying;
            }
        }
    }

    /// <summary>�v�l���\�b�h : �Ώۑ���̉������U�����x�����Ă��낤�낷��悤�ȓ���</summary>
    void VigilanceMiddle()
    {
        _Move.MovePower = _Param.LimitSpeedWalk;

        //�s����Ԃ��ҋ@�Ȃ�ʂ̈ړ�����l����悤�Ɏv��
        if (Movement == Staying || Movement == Trip)
        {
            if ((_DetectAthorPlaceRatio / 100.0f) > Random.value)
            {
                //�ړ���̑��Έʒu���w��
                _RelativeOrbitQuat = Quaternion.AngleAxis(Random.Range(-45f, 45f), _TargetParam.transform.up);
                Movement = Trip;
            }
            else if (GetArrival(_Move.DestinationOnNavMesh))
            {
                _RelativeOrbitQuat = Quaternion.identity;
                _Move.Destination = null;
                Movement = Staying;
            }
        }
        else
        {
            Movement = Staying;
        }

        if (_RelativeOrbitQuat != Quaternion.identity)
        {
            Vector3 _RelativeVigilancePoint = Vector3.Normalize(transform.position - _TargetParam.transform.position) * _TargetParam.AttackRangeMiddle;
            _RelativeVigilancePoint = _RelativeOrbitQuat * _RelativeVigilancePoint;
            _Move.Destination = _RelativeVigilancePoint + _TargetParam.transform.position;
        }
    }

    /// <summary>�v�l���\�b�h : �Ώۂɐڋ߂��A���͂��x�����Ȃ��炤���</summary>
    void ApproachToVigilanceMiddle()
    {
        //�Ώۂ����Ȃ���Η��E
        if (!_TargetParam)
        {
            return;
        }

        float sqrDistance = Vector3.SqrMagnitude(transform.position - _TargetParam.transform.position);

        //�Ώۂ��ǐՎ˒���艓����΁A�^�[�Q�b�g�w����������������
        if (sqrDistance > _Param.ChaseEnemyDistance * _Param.ChaseEnemyDistance)
        {
            Wandering();
        }
        //�Ώۂ��Ώۂ̉������U���˒���艓����΁A�^�[�Q�b�g�߂����Ĉړ�����
        else if (sqrDistance > _TargetParam.AttackRangeFar * _TargetParam.AttackRangeFar)
        {
            _Move.MovePower = _Param.LimitSpeedRun;
            Movement = Approach;
        }
        //�Ώۂ��Ώۂ̉������U���˒����߂Â��Ύ��͂������
        else
        {
            _Move.MovePower = _Param.LimitSpeedWalk;
            _Move.Destination = null;
            VigilanceMiddle();
        }
    }

    /// <summary>�s�����\�b�h : ���̏�ŉ������Ȃ�����</summary>
    void Staying()
    {

    }

    /// <summary>�s�����\�b�h : �w�肵���n�_�߂����Ĉړ����铮��</summary>
    void Trip()
    {

    }

    /// <summary>�s�����\�b�h : �Ώۂɐڋ߂���悤�ȓ���</summary>
    void Approach()
    {
        _Move.Destination = _TargetParam.transform.position;
    }

}

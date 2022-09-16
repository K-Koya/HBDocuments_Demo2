using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    /// <summary>�������ꂽ����</summary>
    float _Capricious = 0f;
        
    /// <summary>�v�l���\�b�h</summary>
    System.Action Think = null;

    /// <summary>�s�����\�b�h</summary>
    System.Action Movement = null;

    [SerializeField, Tooltip("�����̐����Ԋu")]
    float _CreateRandomInterval = 1f;

    [SerializeField, Tooltip("������ŋ��ꏊ��ς��悤�Ƃ���m��(0�`100)")]
    sbyte _DetectAthorPlaceRatio = 20;

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

        Think = OrbitWandering;
        Movement = Trip;
    }

    // Update is called once per frame
    void Update()
    {
        Think?.Invoke();
        Movement?.Invoke();
    }

    void OnEnable()
    {
        StartCoroutine(CreateRandom());
    }

    /// <summary>�w��Ԋu�ŗ�������点��R���[�`��</summary>
    IEnumerator CreateRandom()
    {
        yield return null;
        while (true)
        {
            _Capricious = Random.value;
            yield return _Param.Tl.WaitForSeconds(_CreateRandomInterval);
        }
    }

    /// <summary>�ړI�n���B����</summary>
    /// <param name="destination">�ړI�n</param>
    /// <returns>true : ��������</returns>
    bool GetArrival(Vector3 destination, float buffer = 0f)
    {
        float sqrArrivalDistance = Mathf.Pow(_ARRAIVAL_DISTANCE_BASE + buffer + _Move.Speed * 0.75f, 2);
        return Vector3.SqrMagnitude(destination - transform.position) < sqrArrivalDistance;
    }

    /// <summary>�v�l���\�b�h : ���낤�낷��</summary>
    void Wandering()
    {
        //�s����Ԃ��ҋ@�Ȃ�ʂ̈ړ�����l����悤�Ɏv��
        if(Movement == Staying)
        {
            if ((_DetectAthorPlaceRatio / 100.0f) > _Capricious)
            {
                //�ړ�����w�肵�������ňړ�
                _Move.Destination = _BasePosition + new Vector3(Random.Range(1, _WanderingDistance), 0f, Random.Range(1, _WanderingDistance));
                Movement = Trip;
            }
        }
        //�s����Ԃ��ړ����Ȃ�ړI�n�t�߂֓����������m�F
        else if (Movement == Trip)
        {
            if(GetArrival(_Move.DestinationOnNavMesh))
            {
                _Move.Destination = null;
                Movement = Staying;
            }
        }
    }

    /// <summary>�v�l���\�b�h : �^�[�Q�b�g�𒆐S�ɉ~����������</summary>
    void OrbitWandering()
    {
        //���񃋁[�g��񂪂Ȃ���Η��E
        if (!_Param.Orbit || !_Param.Orbit.IsDefined)
        {
            return;
        }


    }

    /// <summary>�v�l���\�b�h : �Ώۂɐڋ߂��A���͂��x�����Ȃ��炤���</summary>
    void ApproachToOrbitVigilance()
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
            Movement = Approach;
        }
        //�Ώۂ��Ώۂ̉������U���˒����߂Â��Ύ��͂������
        else
        {
            OrbitWandering();
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

    /// <summary>�s�����\�b�h : �Ώۑ���̉������U�����x�����Ď��͂��ړ�����悤�ȓ���</summary>
    void OrbitVigilanceFar()
    {
        
    }
}

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
    protected Vector3 _BasePosition = default;

    /// <summary>�L�����N�^�[�̎����</summary>
    protected CharacterParameter _Param = null;

    /// <summary>���ڃL�����N�^�[�̎����</summary>
    protected CharacterParameter _TargetParam = null;

    /// <summary>�L�����N�^�[���ړ�������@�\���W�񂵂��R���|�[�l���g</summary>
    protected ComputerMove _Move = null;

    /// <summary>�^�[�Q�b�g���ӂ�����������]���</summary>
    protected Quaternion _RelativeOrbitQuat = Quaternion.identity;

    /// <summary>�ړ�����</summary>
    protected float _MoveTime = 0f;

    /// <summary>�ړ����Ԃ̐�������</summary>
    protected float _MoveTimeLimit = 10f;

    /// <summary>�v�l���\�b�h</summary>
    protected System.Action Think = null;

    /// <summary>�s�����\�b�h</summary>
    protected System.Action Movement = null;

    /// <summary>�v�l���\�b�h�̊��荞�ݎ��ɕێ�����L���b�V��</summary>
    protected System.Action ThinkCash = null;
        
    [SerializeField, Tooltip("������ŋ��ꏊ��ς��悤�Ƃ���m��(0�`100)")]
    protected sbyte _DetectAthorPlaceRatio = 5;

    [SerializeField, Tooltip("���R�s������ꍇ�̍s���͈�")]
    protected float _WanderingDistance = 8f;

    [SerializeField, Tooltip("�G��ǂ�������ꍇ�̒Ǐ]�ϐ�")]
    protected FollowControl _FollowEnemy;

    #endregion

    /// <summary>�^�[�Q�b�g�t�߂��ړ�����Ƃ��̓Z���x������ۊǂ���\����</summary>
    [System.Serializable]
    protected struct FollowControl
    {
        [SerializeField, Tooltip("�^�[�Q�b�g�ɑ΂������")]
        float _AttractInfluence;

        [SerializeField, Tooltip("�^�[�Q�b�g�ɑ΂���˗�")]
        float _RepulsionInfluence;

        public FollowControl(float attract, float repulsion)
        {
            _AttractInfluence = attract;
            _RepulsionInfluence = repulsion;
        }

        /// <summary>�����ƃ^�[�Q�b�g�̋�������ŏI�I�Ȉ��́E�˗͂��v�Z</summary>
        /// <param name="sqrDistance">�����ƃ^�[�Q�b�g�̋�����2��</param>
        /// <param name="attractDecay">���͂̉e���͈�</param>
        /// <param name="repulsionDecay">�˗͂̉e���͈�</param>
        /// <returns>����(��)or�˗�(��)</returns>
        public float FollowInfluence(float sqrDistance, float attractDecay, float repulsionDecay)
        {
            //���i�[�h�E�W���[���Y�E�|�e���V�����Ef
            return (_RepulsionInfluence / Mathf.Pow(sqrDistance, repulsionDecay - 1f)) - (_AttractInfluence / Mathf.Pow(sqrDistance, attractDecay - 1f));
        }
    }

    #region �R���s���[�^�[�p���͏��


    #endregion

    #region �v���p�e�B

    #endregion


    // Start is called before the first frame update
    protected virtual void Start()
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

        _MoveTime += Time.deltaTime;
    }

    /// <summary>�ړI�n���B����</summary>
    /// <param name="destination">�ړI�n</param>
    /// <returns>true : ��������</returns>
    protected bool GetArrival(Vector3 destination, float buffer = 0f)
    {
        float sqrArrivalDistance = Mathf.Pow(_ARRAIVAL_DISTANCE_BASE + buffer + _Move.Speed * _Move.Speed * 0.5f, 2);
        return Vector3.SqrMagnitude(destination - transform.position) < sqrArrivalDistance;
    }

    /// <summary>�v�l���\�b�h : �悯�悤�Ƃ���</summary>
    void Avoidance()
    {
        Movement = Trip;
        if (GetArrival(_Move.DestinationOnNavMesh))
        {
            _Move.Destination = null;
            Movement = Staying;
        }

        //���A
        if (_TargetParam.AttackAreas == null || _TargetParam.AttackAreas.Count < 1)
        {
            Think = ThinkCash;
        }
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
                //�ړ���Ɛ������Ԃ��w�肵�������ňړ�
                _Move.Destination = _BasePosition + new Vector3(Random.Range(1, _WanderingDistance), 0f, Random.Range(1, _WanderingDistance));
                _MoveTimeLimit = Random.Range(5f, 20f);
                _MoveTime = 0f;
                Movement = Trip;
            }
        }
        //�s����Ԃ��ړ����Ȃ琧�����Ԃ��o�������ړI�n�t�߂֓����������m�F
        else if (Movement == Trip)
        {
            if (_MoveTimeLimit < _MoveTime || GetArrival(_Move.DestinationOnNavMesh))
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

        //�G�Α��肪�U���������Ȃ�A�͈͊O�֓����悤�Ƃ���
        if (_TargetParam.AttackAreas != null && _TargetParam.AttackAreas.Count > 0)
        {
            Vector3 outDirection = Vector3.zero;
            foreach (AttackArea at in _TargetParam.AttackAreas)
            {
                Vector3? buffer = at.InsideArea(_Param.HitArea);
                outDirection = buffer == null ? outDirection : (Vector3)buffer;
            }

            if(outDirection.sqrMagnitude > 0f)
            {
                _Move.MovePower = _Param.LimitSpeedRun;
                _Move.Destination = transform.position + outDirection * 2f;
                ThinkCash = Think;
                Think = Avoidance;

                return;
            }
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
    protected void Staying()
    {

    }

    /// <summary>�s�����\�b�h : �w�肵���n�_�߂����Ĉړ����铮��</summary>
    protected void Trip()
    {

    }

    /// <summary>�s�����\�b�h : �^�[�Q�b�g�ɑ΂��ēZ�����悤�Ɉړ����铮��</summary>
    protected void FollowEnemy()
    {
        
    }

    /// <summary>�s�����\�b�h : �Ώۂɐڋ߂���悤�ȓ���</summary>
    protected void Approach()
    {
        _Move.Destination = _TargetParam.transform.position;
    }
}

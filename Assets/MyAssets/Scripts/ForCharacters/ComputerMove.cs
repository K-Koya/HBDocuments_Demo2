using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Chronos;

[RequireComponent(typeof(NavMeshAgent))]
public class ComputerMove : CharacterMove
{
    /// <summary>��������擾���\�b�h</summary>
    protected System.Action GetCondition = null;

    /// <summary>�s���̕��򃁃\�b�h</summary>
    protected System.Action Think = null;

    /// <summary>�����̎�����̊�ɂȂ�ʒu</summary>
    protected Vector3 _BasePosition = default;

    /// <summary>�s������</summary>
    protected float _MoveTime = 0f;

    /// <summary>�s���̐�������</summary>
    protected float _MoveTimeLimit = 10f;

    #region �i�r���b�V���p�����o
    [SerializeField, Tooltip("�ړI�n�ɐڋ߂����Ƃ݂Ȃ�����")]
    float _CloseDistance = 3f;

    /// <summary>���Y�L�����N�^�[�̈ړ�����</summary>
    NavMeshAgent _Nav = null;

    /// <summary>�ړ�����W</summary>
    Vector3? _Destination = null;

    /// <summary>�͂�������␳�l</summary>
    Vector3 _ForceCorrection = Vector3.zero;

    /// <summary>�ړ�����߂�R���[�`��</summary>
    Coroutine _SetDestinationCoroutine = null;

    /// <summary>true : �ړ����NavMesh��Ɍ�����ꂽ</summary>
    bool _IsFoundDestination = false;

    /// <summary>true : �ړ�����W�ɐڋ߂���</summary>
    bool _IsCloseDestination = true;
    #endregion

    #region �v���p�e�B
    /// <summary>�ړ�����W</summary>
    public Vector3? Destination { set => _Destination = value; }
    /// <summary>�͂�������␳�l</summary>
    public Vector3 ForceCorrection { set => _ForceCorrection = value; }
    /// <summary>�i�r���b�V����ɂ�����ړ�����W</summary>
    public Vector3 DestinationOnNavMesh { get => _Nav.destination; }
    /// <summary>true : �ړ����NavMesh��Ɍ�����ꂽ</summary>
    public bool IsFoundDestination { get => _IsFoundDestination; }
    /// <summary>true : �ړ�����W�ɐڋ߂���</summary>
    public bool IsCloseDestination { get => _IsCloseDestination;}
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


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        Timeline tl = GetComponent<Timeline>();
        _Nav = tl.navMeshAgent.component;
        _Nav.isStopped = true;

        Movement = MoveByNavMesh;

        //TODO
        _Param.GazeAt = CharacterParameter.Player;
    }

    // Update is called once per frame
    protected override void Update()
    {
        Damaged?.Invoke();
        GetCondition?.Invoke();
        Think?.Invoke();

        base.Update();
    }

    /// <summary>�i�r���b�V���𗘗p�����ړ����\�b�h</summary>
    void MoveByNavMesh()
    {
        _IsCloseDestination = false;
        if (_Destination == null)
        {
            _SetDestinationCoroutine = null;
        }
        //�ړI�n�w�肪����΃R���[�`�������s
        else
        {
            //�ړI�n�ɂ����Ȃ�R���[�`���͎~�߂�
            if (Vector3.SqrMagnitude((Vector3)_Destination - transform.position) < _CloseDistance * _CloseDistance)
            {
                _IsCloseDestination = true;
                _SetDestinationCoroutine = null;
                _Nav.ResetPath();
                _Destination = null;
            }
            //�R���[�`�������s�ł���Ύ��s
            else if (_SetDestinationCoroutine == null)
            {
                _SetDestinationCoroutine = StartCoroutine(DestinationSetOnAgent());
            }            
        }

        //�o�H�p�X�ꗗ���A�ɂ߂ċ߂����łȂ��A���߂̈ʒu���擾����
        Vector3 currentNextPassing = transform.position;
        foreach (Vector3 passing in _Nav.path.corners)
        {
            if(Vector3.SqrMagnitude(passing - transform.position) > 0.01f)
            {
                currentNextPassing = passing;
                break;
            }
        }

        //�ړ�����W���w�肵�Ă���΁A���߂̒ʉ߃|�C���g�Ɍ����ė͂�������
        if (!_Param.Can.Move && _Destination == null)
        {
            _Param.MoveDirection = Vector3.zero;
        }
        else
        {
            _Param.MoveDirection = Vector3.Normalize(currentNextPassing - transform.position);
        }

        //�ړ��͕␳�����Z
        _Param.MoveDirection += _ForceCorrection;

        //���͂�����Έړ��͂̏���
        if (_Param.MoveDirection.sqrMagnitude > 0f)
        {
            //�ړ����͂̑傫�����擾
            _MoveInputRate = _Param.MoveDirection.magnitude;
            //�ړ��������擾
            _Param.MoveDirection *= 1f / _MoveInputRate;
            //���x������������
            if(_Param.State.Kind == MotionState.StateKind.Walk)
            {
                _MovePower = _Param.Sub.LimitSpeedWalk;
            }
            else if(_Param.State.Kind == MotionState.StateKind.Run)
            {
                _MovePower = _Param.Sub.LimitSpeedRun;
            }
        }
        else
        {
            _MoveInputRate = 0f;
            _Param.MoveDirection = Vector3.zero;
        }

        //�d�͕����ȊO�ňړ��ʐ������������ꍇ�A�u���[�L�ʂ��v�Z����
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -VelocityOnPlane.normalized * 0.2f;
        }
    }

    /// <summary>NavMeshAgent��Destination�Ɉ��Ԋu�ŖړI�n���w������R���[�`��</summary>
    IEnumerator DestinationSetOnAgent()
    {
        while (true)
        {
            _IsFoundDestination = false;

            if (_Destination == null)
            {
                yield return null;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast((Vector3)_Destination + Vector3.up * 0.2f, Vector3.down, out hit, 10f, LayerManager.Instance.AllGround))
                {
                    _Nav.destination = hit.point;
                    _IsFoundDestination = true;
                }

                yield return _Param.Tl.WaitForSeconds(0.2f);
            }
        }
    }

    /// <summary>�i�r���b�V���ɂ��ړ��o�H�������o��</summary>
    void OnDrawGizmos()
    {
        if (_Nav && _Nav.enabled)
        {
            Gizmos.color = Color.red;
            var prefPos = transform.position;

            foreach (var pos in _Nav.path.corners)
            {
                Gizmos.DrawLine(prefPos, pos);
                prefPos = pos;
            }
        }
    }
}

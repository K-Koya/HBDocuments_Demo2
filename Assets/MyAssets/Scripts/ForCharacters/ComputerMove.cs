using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Chronos;

[RequireComponent(typeof(NavMeshAgent))]
public class ComputerMove : CharacterMove
{
    #region �����o
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
    #endregion

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _Nav = _Param.Tl.navMeshAgent.component;
        _Nav.isStopped = true;

        Move = MoveByNavMesh;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>�i�r���b�V���𗘗p�����ړ����\�b�h</summary>
    void MoveByNavMesh()
    {
        //�ړI�n�w�肪����΃R���[�`�������s
        if (_Destination == null)
        {
            _SetDestinationCoroutine = null;
        }
        else
        {
            if (_SetDestinationCoroutine == null)
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
            _Param.Direction = Vector3.zero;
        }
        else
        {
            _Param.Direction = Vector3.Normalize(currentNextPassing - transform.position);
        }

        //�ړ��͕␳�����Z
        _Param.Direction += _ForceCorrection;

        //���͂�����Έړ��͂̏���
        if (_Param.Direction.sqrMagnitude > 0f)
        {
            //�ړ����͂̑傫�����擾
            _MoveInputRate = _Param.Direction.magnitude;
            //�ړ��������擾
            _Param.Direction *= 1f / _MoveInputRate;
        }
        else
        {
            _MoveInputRate = 0f;
            _Param.Direction = Vector3.zero;
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

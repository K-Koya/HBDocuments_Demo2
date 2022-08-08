using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterParameter))]
public class EnemyAIBase : MonoBehaviour
{
    #region �����o
    /// <summary>�L�����N�^�[�̎����</summary>
    CharacterParameter _Param = null;

    /// <summary>���Y�L�����N�^�[�̈ړ�����</summary>
    NavMeshAgent _Nav = null;

    /// <summary>�O�t���[���̈ʒu���W</summary>
    Vector3 _BeforeFramePosition = Vector3.zero;

    /// <summary>true : �ړI�n�ɓ������Ă���</summary>
    bool _IsArrival = true;

    /// <summary>�ړ�����W</summary>
    Vector3? _Destination = null;
    #endregion

    #region �v���p�e�B
    /// <summary>�ړ�����W</summary>
    public Vector3? Destination { set => _Destination = value; }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _Param = GetComponent<CharacterParameter>();

        _Nav = _Param.Tl.navMeshAgent.component;
        _Nav.isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        MoveByNavMesh();
    }

    /// <summary>�i�r���b�V���𗘗p�����ړ��w�胁�\�b�h</summary>
    void MoveByNavMesh()
    {
        //�ړI�n�w��
        _Destination = CharacterParameter.Player.transform.position;
        StartCoroutine(DestinationSetOnAgent());

        //�o�H�p�X�ꗗ���A�ɂ߂ċ߂����łȂ��A���߂̈ʒu���擾����
        Vector3 currentNextPassing = transform.position;
        foreach (Vector3 passing in _Nav.path.corners)
        {
            if (Vector3.SqrMagnitude(passing - transform.position) > 0.01f)
            {
                currentNextPassing = passing;
                break;
            }
        }

        //���߂̒ʉ߃|�C���g�Ɍ����ė͂�������
        _Param.Direction = Vector3.Normalize(currentNextPassing - transform.position);
    }


    /// <summary>NavMeshAgent��Destination�Ɉ��Ԋu�ŖړI�n���w������R���[�`��</summary>
    IEnumerator DestinationSetOnAgent()
    {
        while (true)
        {
            if (_Destination == null)
            {
                yield return null;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast((Vector3)_Destination + Vector3.up * 0.2f, Vector3.down, out hit, 10f, LayerManager.Ins.AllGround))
                {
                    _Nav.destination = hit.point;
                }

                yield return _Param.Tl.WaitForSeconds(0.2f);
            }
        }
    }

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

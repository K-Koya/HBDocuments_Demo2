using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Chronos;
using DG.Tweening;

public class AlliesMove : CharacterMove
{
    #region �����o
    /// <summary>���Y�L�����N�^�[�̕��������R���|�[�l���g</summary>
    RigidbodyTimeline3D _Rb = null;

    /// <summary>���Y�L�����N�^�[�̈ړ�����</summary>
    NavMeshAgent _Nav = null;

    /// <summary>�O�t���[���̈ʒu���W</summary>
    Vector3 _BeforeFramePosition = Vector3.zero;

    /// <summary>�ړ���</summary>
    Vector3 _NavVelocity = Vector3.zero;

    /// <summary>���݂̈ړ�����</summary>
    UsingMoveMode _MoveMode = UsingMoveMode.AddTransform;

    /// <summary>�ړ�����W</summary>
    Vector3? _Destination = null;

    /// <summary>�������Ă���O��</summary>
    Vector3? _ForceAdditon = null;
    #endregion

    #region �v���p�e�B
    /// <summary>Rigidbody��velocity���ړ��������ʂɊ��Z��������</summary>
    Vector3 VelocityOnPlane => Vector3.ProjectOnPlane(_NavVelocity, -GravityDirection);


    public override bool IsGround
    {
        get
        {
            bool isGround = base.IsGround;
            if (!_Nav.isStopped) isGround = _Nav.isOnNavMesh;   

            return isGround;
        }
    }

    /// <summary>�ړ����x</summary>
    public override float Speed => VelocityOnPlane.magnitude / _Tl.deltaTime;

    /// <summary>���݂̈ړ�����</summary>
    public UsingMoveMode MoveMode { get => _MoveMode; }

    /// <summary>�ړ�����W</summary>
    public Vector3? Destination { set => _Destination = value; }

    /// <summary>�������Ă���O��</summary>
    public Vector3? ForceAdditon { set => _ForceAdditon = value; }
    #endregion

    protected override void RegisterStaticReference()
    {
        _Allies.Add(this);
    }

    protected override void EraseStaticReference()
    {
        _Allies.Remove(this);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _Rb = _Tl.rigidbody;
        _Rb.useGravity = false;

        _Nav = _Tl.navMeshAgent.component;

        _BeforeFramePosition = transform.position;

        Move = MoveByNavMesh;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        _NavVelocity = transform.position - _BeforeFramePosition;
        _BeforeFramePosition = transform.position;
    }

    /// <summary>�i�r���b�V���ɂ��ړ����\�b�h</summary>
    void MoveByNavMesh()
    {
        //�͂��������ĉ����o����Ă��鎞
        if (_Nav.isStopped)
        {
            //�������Ă���O�͂��������Ȃ���
            if(_Rb.velocity.sqrMagnitude < 0.04f)
            {
                _Rb.velocity = Vector3.zero;
                _Nav.isStopped = false;
            }
        }

        //OffMeshLink�ɏ������
        if (_Nav.isOnOffMeshLink)
        {
            OffMeshLinkData linkData = _Nav.currentOffMeshLinkData;

            //OffMeshLink�����������̏ꍇ�̏�������
            switch (linkData.linkType)
            {
                //�������郊���N
                case OffMeshLinkType.LinkTypeDropDown:



                    break;
                //���ꂽ���ɃW�����v���郊���N
                case OffMeshLinkType.LinkTypeJumpAcross:



                    break;
                //�蓮�쐬�E�ݒ�̃����N
                default:

                    //�蓮�����N�ɂ���tag�ŕ��򂷂�
                    OffMeshLink link = linkData.offMeshLink;
                    //�W�����v���ŏ��~�肷��i���̃����N
                    if (link.CompareTag(TagManager.Ins.OffMeshLinkJumpStep))
                    {

                    }
                    //���ꂽ���ɃW�����v���郊���N
                    else if (link.CompareTag(TagManager.Ins.OffMeshLinkJumpFar))
                    {

                    }

                    break;
            }

        }

        _Destination = Player.transform.position;
        StartCoroutine(DestinationSetOnAgent());
    }

    /// <summary>���W�b�h�{�f�B�ɂ��ړ����\�b�h</summary>
    void MoveByRigidbody()
    {

    }

    /// <summary>�w������֔�΂��悤�ȗ͂�������</summary>
    /// <param name="force">�w��̗͂̌����Ƒ傫��</param>
    public void AddForceImpulse(Vector3 force)
    {
        _Nav.isStopped = true;
    }

    /// <summary>�w��ʒu����w��̗͂ŊO���֔�΂��悤�ȗ͂�������</summary>
    /// <param name="source">�w��ʒu</param>
    /// <param name="power">>�w��̗͂̑傫��</param>
    public void AddForceExplode(Vector3 source, float power)
    {
        _Nav.isStopped = true;
    }

    /// <summary>NavMeshAgent��Destination�Ɉ��Ԋu�ŖړI�n���w������R���[�`��</summary>
    IEnumerator DestinationSetOnAgent()
    {
        while (true)
        {
            if(_Destination == null)
            {
                _Nav.isStopped = true;
                _Rb.isKinematic = false;

                yield return null;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast((Vector3)_Destination + Vector3.up * 0.2f, Vector3.down, out hit, 10f, LayerManager.Ins.AllGround))
                {
                    _Nav.destination = hit.point;
                }

                yield return _Tl.WaitForSeconds(0.2f);
            }
        }
    }

    /// <summary>���݂̈ړ����샂�[�h</summary>
    public enum UsingMoveMode
    {
        /// <summary>���������𗘗p</summary>
        Rigidbody,
        /// <summary>���W�ړ�</summary>
        AddTransform,
        /// <summary>�i�r���b�V���ŗU��</summary>
        NavMesh,
    }
}

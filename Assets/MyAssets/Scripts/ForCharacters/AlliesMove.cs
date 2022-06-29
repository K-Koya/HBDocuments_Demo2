using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Chronos;
using DG.Tweening;

public class AlliesMove : CharacterMove
{
    #region メンバ
    /// <summary>当該キャラクターの物理挙動コンポーネント</summary>
    RigidbodyTimeline3D _Rb = null;

    /// <summary>当該キャラクターの移動制御</summary>
    NavMeshAgent _Nav = null;

    /// <summary>前フレームの位置座標</summary>
    Vector3 _BeforeFramePosition = Vector3.zero;

    /// <summary>移動量</summary>
    Vector3 _NavVelocity = Vector3.zero;

    /// <summary>現在の移動方式</summary>
    UsingMoveMode _MoveMode = UsingMoveMode.AddTransform;

    /// <summary>移動先座標</summary>
    Vector3? _Destination = null;

    /// <summary>かかっている外力</summary>
    Vector3? _ForceAdditon = null;
    #endregion

    #region プロパティ
    /// <summary>Rigidbodyのvelocityを移動方向平面に換算したもの</summary>
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

    /// <summary>移動速度</summary>
    public override float Speed => VelocityOnPlane.magnitude / _Tl.deltaTime;

    /// <summary>現在の移動方式</summary>
    public UsingMoveMode MoveMode { get => _MoveMode; }

    /// <summary>移動先座標</summary>
    public Vector3? Destination { set => _Destination = value; }

    /// <summary>かかっている外力</summary>
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

    /// <summary>ナビメッシュによる移動メソッド</summary>
    void MoveByNavMesh()
    {
        //力がかかって押し出されている時
        if (_Nav.isStopped)
        {
            //かかっている外力が小さくなった
            if(_Rb.velocity.sqrMagnitude < 0.04f)
            {
                _Rb.velocity = Vector3.zero;
                _Nav.isStopped = false;
            }
        }

        //OffMeshLinkに乗った時
        if (_Nav.isOnOffMeshLink)
        {
            OffMeshLinkData linkData = _Nav.currentOffMeshLinkData;

            //OffMeshLinkが自動生成の場合の処理分岐
            switch (linkData.linkType)
            {
                //落下するリンク
                case OffMeshLinkType.LinkTypeDropDown:



                    break;
                //離れた所にジャンプするリンク
                case OffMeshLinkType.LinkTypeJumpAcross:



                    break;
                //手動作成・設定のリンク
                default:

                    //手動リンクにつけたtagで分岐する
                    OffMeshLink link = linkData.offMeshLink;
                    //ジャンプ等で上り降りする段差のリンク
                    if (link.CompareTag(TagManager.Ins.OffMeshLinkJumpStep))
                    {

                    }
                    //離れた所にジャンプするリンク
                    else if (link.CompareTag(TagManager.Ins.OffMeshLinkJumpFar))
                    {

                    }

                    break;
            }

        }

        _Destination = Player.transform.position;
        StartCoroutine(DestinationSetOnAgent());
    }

    /// <summary>リジッドボディによる移動メソッド</summary>
    void MoveByRigidbody()
    {

    }

    /// <summary>指定方向へ飛ばすような力をかける</summary>
    /// <param name="force">指定の力の向きと大きさ</param>
    public void AddForceImpulse(Vector3 force)
    {
        _Nav.isStopped = true;
    }

    /// <summary>指定位置から指定の力で外側へ飛ばすような力をかける</summary>
    /// <param name="source">指定位置</param>
    /// <param name="power">>指定の力の大きさ</param>
    public void AddForceExplode(Vector3 source, float power)
    {
        _Nav.isStopped = true;
    }

    /// <summary>NavMeshAgentのDestinationに一定間隔で目的地を指示するコルーチン</summary>
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

    /// <summary>現在の移動動作モード</summary>
    public enum UsingMoveMode
    {
        /// <summary>物理挙動を利用</summary>
        Rigidbody,
        /// <summary>座標移動</summary>
        AddTransform,
        /// <summary>ナビメッシュで誘導</summary>
        NavMesh,
    }
}

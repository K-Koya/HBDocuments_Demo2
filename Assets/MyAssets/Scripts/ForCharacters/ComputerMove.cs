using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Chronos;

[RequireComponent(typeof(NavMeshAgent))]
public class ComputerMove : CharacterMove
{
    /// <summary>分岐条件取得メソッド</summary>
    protected System.Action GetCondition = null;

    /// <summary>行動の分岐メソッド</summary>
    protected System.Action Think = null;

    /// <summary>自分の持ち場の基準になる位置</summary>
    protected Vector3 _BasePosition = default;

    /// <summary>行動時間</summary>
    protected float _MoveTime = 0f;

    /// <summary>行動の制限時間</summary>
    protected float _MoveTimeLimit = 10f;

    #region ナビメッシュ用メンバ
    [SerializeField, Tooltip("目的地に接近したとみなす距離")]
    float _CloseDistance = 3f;

    /// <summary>当該キャラクターの移動制御</summary>
    NavMeshAgent _Nav = null;

    /// <summary>移動先座標</summary>
    Vector3? _Destination = null;

    /// <summary>力をかける補正値</summary>
    Vector3 _ForceCorrection = Vector3.zero;

    /// <summary>移動先を定めるコルーチン</summary>
    Coroutine _SetDestinationCoroutine = null;

    /// <summary>true : 移動先をNavMesh上に見つけられた</summary>
    bool _IsFoundDestination = false;

    /// <summary>true : 移動先座標に接近した</summary>
    bool _IsCloseDestination = true;
    #endregion

    #region プロパティ
    /// <summary>移動先座標</summary>
    public Vector3? Destination { set => _Destination = value; }
    /// <summary>力をかける補正値</summary>
    public Vector3 ForceCorrection { set => _ForceCorrection = value; }
    /// <summary>ナビメッシュ上における移動先座標</summary>
    public Vector3 DestinationOnNavMesh { get => _Nav.destination; }
    /// <summary>true : 移動先をNavMesh上に見つけられた</summary>
    public bool IsFoundDestination { get => _IsFoundDestination; }
    /// <summary>true : 移動先座標に接近した</summary>
    public bool IsCloseDestination { get => _IsCloseDestination;}
    #endregion



    /// <summary>ターゲット付近を移動するときの纏わり度合いを保管する構造体</summary>
    [System.Serializable]
    protected struct FollowControl
    {
        [SerializeField, Tooltip("ターゲットに対する引力")]
        float _AttractInfluence;

        [SerializeField, Tooltip("ターゲットに対する斥力")]
        float _RepulsionInfluence;

        public FollowControl(float attract, float repulsion)
        {
            _AttractInfluence = attract;
            _RepulsionInfluence = repulsion;
        }

        /// <summary>自分とターゲットの距離から最終的な引力・斥力を計算</summary>
        /// <param name="sqrDistance">自分とターゲットの距離の2乗</param>
        /// <param name="attractDecay">引力の影響範囲</param>
        /// <param name="repulsionDecay">斥力の影響範囲</param>
        /// <returns>引力(正)or斥力(負)</returns>
        public float FollowInfluence(float sqrDistance, float attractDecay, float repulsionDecay)
        {
            //レナード・ジョーンズ・ポテンシャル・f
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

    /// <summary>ナビメッシュを利用した移動メソッド</summary>
    void MoveByNavMesh()
    {
        _IsCloseDestination = false;
        if (_Destination == null)
        {
            _SetDestinationCoroutine = null;
        }
        //目的地指定があればコルーチンを実行
        else
        {
            //目的地についたならコルーチンは止める
            if (Vector3.SqrMagnitude((Vector3)_Destination - transform.position) < _CloseDistance * _CloseDistance)
            {
                _IsCloseDestination = true;
                _SetDestinationCoroutine = null;
                _Nav.ResetPath();
                _Destination = null;
            }
            //コルーチン未実行であれば実行
            else if (_SetDestinationCoroutine == null)
            {
                _SetDestinationCoroutine = StartCoroutine(DestinationSetOnAgent());
            }            
        }

        //経路パス一覧より、極めて近すぎでない、直近の位置を取得する
        Vector3 currentNextPassing = transform.position;
        foreach (Vector3 passing in _Nav.path.corners)
        {
            if(Vector3.SqrMagnitude(passing - transform.position) > 0.01f)
            {
                currentNextPassing = passing;
                break;
            }
        }

        //移動先座標を指定していれば、直近の通過ポイントに向けて力をかける
        if (!_Param.Can.Move && _Destination == null)
        {
            _Param.MoveDirection = Vector3.zero;
        }
        else
        {
            _Param.MoveDirection = Vector3.Normalize(currentNextPassing - transform.position);
        }

        //移動力補正を合算
        _Param.MoveDirection += _ForceCorrection;

        //入力があれば移動力の処理
        if (_Param.MoveDirection.sqrMagnitude > 0f)
        {
            //移動入力の大きさを取得
            _MoveInputRate = _Param.MoveDirection.magnitude;
            //移動方向を取得
            _Param.MoveDirection *= 1f / _MoveInputRate;
            //速度制限をかける
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

        //重力方向以外で移動量成分があった場合、ブレーキ量を計算する
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -VelocityOnPlane.normalized * 0.2f;
        }
    }

    /// <summary>NavMeshAgentのDestinationに一定間隔で目的地を指示するコルーチン</summary>
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

    /// <summary>ナビメッシュによる移動経路を書き出し</summary>
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

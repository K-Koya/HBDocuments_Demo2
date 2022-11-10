using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterParameter), typeof(ComputerMove))]
public class ComputerAIBase : MonoBehaviour
{
    #region 定数値
    /// <summary>目的地に到着したとみなす距離</summary>
    const float _ARRAIVAL_DISTANCE_BASE = 0.75f;

    /// <summary>再度目的地を目指して移動させようとする距離</summary>
    const float _RESET_ARRAIVAL_DISTANCE = 2f;

    #endregion

    #region メンバ
    /// <summary>自分の持ち場の基準になる位置</summary>
    protected Vector3 _BasePosition = default;

    /// <summary>キャラクターの持つ情報</summary>
    protected CharacterParameter _Param = null;

    /// <summary>注目キャラクターの持つ情報</summary>
    protected CharacterParameter _TargetParam = null;

    /// <summary>キャラクターを移動させる機能を集約したコンポーネント</summary>
    protected ComputerMove _Move = null;

    /// <summary>ターゲット周辺をうろつかせる回転情報</summary>
    protected Quaternion _RelativeOrbitQuat = Quaternion.identity;

    /// <summary>移動時間</summary>
    protected float _MoveTime = 0f;

    /// <summary>移動時間の制限時間</summary>
    protected float _MoveTimeLimit = 10f;

    /// <summary>思考メソッド</summary>
    protected System.Action Think = null;

    /// <summary>行動メソッド</summary>
    protected System.Action Movement = null;

    /// <summary>思考メソッドの割り込み時に保持するキャッシュ</summary>
    protected System.Action ThinkCash = null;
        
    [SerializeField, Tooltip("うろつきで居場所を変えようとする確率(0～100)")]
    protected sbyte _DetectAthorPlaceRatio = 5;

    [SerializeField, Tooltip("自由行動する場合の行動範囲")]
    protected float _WanderingDistance = 8f;

    [SerializeField, Tooltip("敵を追いかける場合の追従変数")]
    protected FollowControl _FollowEnemy;

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

    #region コンピューター用入力情報


    #endregion

    #region プロパティ

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

    /// <summary>目的地到達判定</summary>
    /// <param name="destination">目的地</param>
    /// <returns>true : 到着した</returns>
    protected bool GetArrival(Vector3 destination, float buffer = 0f)
    {
        float sqrArrivalDistance = Mathf.Pow(_ARRAIVAL_DISTANCE_BASE + buffer + _Move.Speed * _Move.Speed * 0.5f, 2);
        return Vector3.SqrMagnitude(destination - transform.position) < sqrArrivalDistance;
    }

    /// <summary>思考メソッド : よけようとする</summary>
    void Avoidance()
    {
        Movement = Trip;
        if (GetArrival(_Move.DestinationOnNavMesh))
        {
            _Move.Destination = null;
            Movement = Staying;
        }

        //復帰
        if (_TargetParam.AttackAreas == null || _TargetParam.AttackAreas.Count < 1)
        {
            Think = ThinkCash;
        }
    }

    /// <summary>思考メソッド : うろうろする</summary>
    void Wandering()
    {
        _Move.MovePower = _Param.LimitSpeedWalk;

        //行動状態が待機なら別の移動先を考えるように思案
        if (Movement == Staying)
        {
            if ((_DetectAthorPlaceRatio / 100.0f) > Random.value)
            {
                //移動先と制限時間を指定したうえで移動
                _Move.Destination = _BasePosition + new Vector3(Random.Range(1, _WanderingDistance), 0f, Random.Range(1, _WanderingDistance));
                _MoveTimeLimit = Random.Range(5f, 20f);
                _MoveTime = 0f;
                Movement = Trip;
            }
        }
        //行動状態が移動中なら制限時間が経ったか目的地付近へ到着したか確認
        else if (Movement == Trip)
        {
            if (_MoveTimeLimit < _MoveTime || GetArrival(_Move.DestinationOnNavMesh))
            {
                _Move.Destination = null;
                Movement = Staying;
            }
        }
    }

    /// <summary>思考メソッド : 対象相手の遠距離攻撃も警戒してうろうろするような動作</summary>
    void VigilanceMiddle()
    {
        _Move.MovePower = _Param.LimitSpeedWalk;

        //行動状態が待機なら別の移動先を考えるように思案
        if (Movement == Staying || Movement == Trip)
        {
            if ((_DetectAthorPlaceRatio / 100.0f) > Random.value)
            {
                //移動先の相対位置を指定
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

    /// <summary>思考メソッド : 対象に接近し、周囲を警戒しながらうろつく</summary>
    void ApproachToVigilanceMiddle()
    {
        //対象がいなければ離脱
        if (!_TargetParam)
        {
            return;
        }

        //敵対相手が攻撃しそうなら、範囲外へ逃げようとする
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

        //対象が追跡射程より遠ければ、ターゲット指定を解除しうろつき化
        if (sqrDistance > _Param.ChaseEnemyDistance * _Param.ChaseEnemyDistance)
        {
            Wandering();
        }
        //対象が対象の遠距離攻撃射程より遠ければ、ターゲットめがけて移動する
        else if (sqrDistance > _TargetParam.AttackRangeFar * _TargetParam.AttackRangeFar)
        {
            _Move.MovePower = _Param.LimitSpeedRun;
            Movement = Approach;
        }
        //対象が対象の遠距離攻撃射程より近づけば周囲をうろつく
        else
        {
            _Move.MovePower = _Param.LimitSpeedWalk;
            _Move.Destination = null;
            VigilanceMiddle();
        }
    }

    /// <summary>行動メソッド : その場で何もしない動作</summary>
    protected void Staying()
    {

    }

    /// <summary>行動メソッド : 指定した地点めがけて移動する動作</summary>
    protected void Trip()
    {

    }

    /// <summary>行動メソッド : ターゲットに対して纏わりつくように移動する動作</summary>
    protected void FollowEnemy()
    {
        
    }

    /// <summary>行動メソッド : 対象に接近するような動作</summary>
    protected void Approach()
    {
        _Move.Destination = _TargetParam.transform.position;
    }
}

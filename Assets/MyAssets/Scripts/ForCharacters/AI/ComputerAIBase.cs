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
    Vector3 _BasePosition = default;

    /// <summary>キャラクターの持つ情報</summary>
    CharacterParameter _Param = null;

    /// <summary>注目キャラクターの持つ情報</summary>
    CharacterParameter _TargetParam = null;

    /// <summary>キャラクターを移動させる機能を集約したコンポーネント</summary>
    ComputerMove _Move = null;

    /// <summary>ターゲット周辺をうろつかせる回転情報</summary>
    Quaternion _RelativeOrbitQuat = Quaternion.identity;

    /// <summary>思考メソッド</summary>
    System.Action Think = null;

    /// <summary>行動メソッド</summary>
    System.Action Movement = null;

    [SerializeField, Tooltip("乱数の生成間隔")]
    float _CreateRandomInterval = 1f;

    [SerializeField, Tooltip("うろつきで居場所を変えようとする確率(0～100)")]
    sbyte _DetectAthorPlaceRatio = 5;

    [SerializeField, Tooltip("自由行動する場合の行動範囲")]
    float _WanderingDistance = 8f;
    #endregion

    #region コンピューター用入力情報


    #endregion

    #region プロパティ

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
    
    /// <summary>目的地到達判定</summary>
    /// <param name="destination">目的地</param>
    /// <returns>true : 到着した</returns>
    bool GetArrival(Vector3 destination, float buffer = 0f)
    {
        float sqrArrivalDistance = Mathf.Pow(_ARRAIVAL_DISTANCE_BASE + buffer + _Move.Speed * _Move.Speed * 0.5f, 2);
        return Vector3.SqrMagnitude(destination - transform.position) < sqrArrivalDistance;
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
                //移動先を指定したうえで移動
                _Move.Destination = _BasePosition + new Vector3(Random.Range(1, _WanderingDistance), 0f, Random.Range(1, _WanderingDistance));
                Movement = Trip;
            }
        }
        //行動状態が移動中なら目的地付近へ到着したか確認
        else if (Movement == Trip)
        {
            if (GetArrival(_Move.DestinationOnNavMesh))
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
    void Staying()
    {

    }

    /// <summary>行動メソッド : 指定した地点めがけて移動する動作</summary>
    void Trip()
    {

    }

    /// <summary>行動メソッド : 対象に接近するような動作</summary>
    void Approach()
    {
        _Move.Destination = _TargetParam.transform.position;
    }

}

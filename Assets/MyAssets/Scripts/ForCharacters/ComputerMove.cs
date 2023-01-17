using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Chronos;

[RequireComponent(typeof(NavMeshAgent))]
public class ComputerMove : CharacterMove
{
    #region メンバ
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

    /// <summary>ナビメッシュを利用した移動メソッド</summary>
    void MoveByNavMesh()
    {
        //目的地指定があればコルーチンを実行
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
            _Param.Direction = Vector3.zero;
        }
        else
        {
            _Param.Direction = Vector3.Normalize(currentNextPassing - transform.position);
        }

        //移動力補正を合算
        _Param.Direction += _ForceCorrection;

        //入力があれば移動力の処理
        if (_Param.Direction.sqrMagnitude > 0f)
        {
            //移動入力の大きさを取得
            _MoveInputRate = _Param.Direction.magnitude;
            //移動方向を取得
            _Param.Direction *= 1f / _MoveInputRate;
        }
        else
        {
            _MoveInputRate = 0f;
            _Param.Direction = Vector3.zero;
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

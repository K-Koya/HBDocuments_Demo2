using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterParameter))]
public class EnemyAIBase : MonoBehaviour
{
    #region メンバ
    /// <summary>キャラクターの持つ情報</summary>
    CharacterParameter _Param = null;

    /// <summary>当該キャラクターの移動制御</summary>
    NavMeshAgent _Nav = null;

    /// <summary>前フレームの位置座標</summary>
    Vector3 _BeforeFramePosition = Vector3.zero;

    /// <summary>true : 目的地に到着している</summary>
    bool _IsArrival = true;

    /// <summary>移動先座標</summary>
    Vector3? _Destination = null;
    #endregion

    #region プロパティ
    /// <summary>移動先座標</summary>
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

    /// <summary>ナビメッシュを利用した移動指定メソッド</summary>
    void MoveByNavMesh()
    {
        //目的地指定
        _Destination = CharacterParameter.Player.transform.position;
        StartCoroutine(DestinationSetOnAgent());

        //経路パス一覧より、極めて近すぎでない、直近の位置を取得する
        Vector3 currentNextPassing = transform.position;
        foreach (Vector3 passing in _Nav.path.corners)
        {
            if (Vector3.SqrMagnitude(passing - transform.position) > 0.01f)
            {
                currentNextPassing = passing;
                break;
            }
        }

        //直近の通過ポイントに向けて力をかける
        _Param.Direction = Vector3.Normalize(currentNextPassing - transform.position);
    }


    /// <summary>NavMeshAgentのDestinationに一定間隔で目的地を指示するコルーチン</summary>
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

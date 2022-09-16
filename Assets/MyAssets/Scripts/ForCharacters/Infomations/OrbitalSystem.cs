using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalSystem : MonoBehaviour
{
    #region メンバ
    [SerializeField, Tooltip("周回中継ポイントのうち、ワールド正面方向に位置するものの配列番号")]
    sbyte _WayPointIndexFoward = 0;

    [SerializeField, Tooltip("周回中継ポイントのうち、ワールド背後方向に位置するものの配列番号")]
    sbyte _WayPointIndexBack = 6;

    [SerializeField, Tooltip("周回中継ポイントのうち、ワールド右方向に位置するものの配列番号")]
    sbyte _WayPointIndexRight = 3;

    [SerializeField, Tooltip("周回中継ポイントのうち、ワールド左方向に位置するものの配列番号")]
    sbyte _WayPointIndexLeft = 9;

    [SerializeField, Tooltip("周回させるための中継ポイント(先頭と後尾でループできるように設定する)")]
    Transform[] _WayPoints = null;

    #endregion

    #region プロパティ
    /// <summary>true : 定義されている</summary>
    public bool IsDefined { get => _WayPoints != null || _WayPoints.Length > 0; }
    /// <summary>ワールド正面方向を取得</summary>
    public Vector3 GetForward { get => _WayPoints[_WayPointIndexFoward].position; }
    /// <summary>ワールド背後方向を取得</summary>
    public Vector3 GetBack { get => _WayPoints[_WayPointIndexBack].position; }
    /// <summary>ワールド右方向を取得</summary>
    public Vector3 GetRight { get => _WayPoints[_WayPointIndexRight].position; }
    /// <summary>ワールド左方向を取得</summary>
    public Vector3 GetLeft { get => _WayPoints[_WayPointIndexLeft].position; }


    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        if(_WayPoints == null || _WayPoints.Length < 1)
        {
            _WayPoints = GetComponentsInChildren<Transform>();
        }
    }

    /// <summary>fromに最も近いWayPointを取得する</summary>
    /// <param name="from">基準座標</param>
    public Vector3 GetClosist(Vector3 from)
    {
        sbyte index = 0;
        float sqrDistance = float.MaxValue;
        for (sbyte i = 0; i < _WayPoints.Length; i++)
        {
            float val = Vector3.SqrMagnitude(from - _WayPoints[i].position);
            if (val < sqrDistance)
            {
                sqrDistance = val;
                index = i;
            }
        }

        return _WayPoints[index].position;
    }

    /// <summary>fromに最も近い所から次のWayPointを取得する</summary>
    /// <param name="from">基準座標</param>
    public Vector3 GetClosistAfter(Vector3 from)
    {
        sbyte index = 0;
        float sqrDistance = float.MaxValue;
        for (sbyte i = 0; i < _WayPoints.Length; i++)
        {
            float val = Vector3.SqrMagnitude(from - _WayPoints[i].position);
            if (val < sqrDistance)
            {
                sqrDistance = val;
                index = i;
            }
        }

        return _WayPoints[index - 1 > _WayPoints.Length ? 0 : index + 1].position;
    }

    /// <summary>fromに最も近い所から手前のWayPointを取得する</summary>
    /// <param name="from">基準座標</param>
    public Vector3 GetClosistBefore(Vector3 from)
    {
        sbyte index = 0;
        float sqrDistance = float.MaxValue;
        for (sbyte i = 0; i < _WayPoints.Length; i++)
        {
            float val = Vector3.SqrMagnitude(from - _WayPoints[i].position);
            if (val < sqrDistance)
            {
                sqrDistance = val;
                index = i;
            }
        }

        return _WayPoints[index - 1 < 1 ? _WayPoints.Length - 1 : index - 1].position;
    }
}

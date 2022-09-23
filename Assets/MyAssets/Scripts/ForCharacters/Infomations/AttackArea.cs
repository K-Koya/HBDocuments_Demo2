using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract public class AttackArea
{
    /// <summary>サイズ半径</summary>
    protected float _Radius = 1f;

    /// <summary>中心点</summary>
    protected Vector3 _CenterPos = default;


    /// <summary>サイズ半径</summary>
    public float Radius { get => _Radius; }
    /// <summary>中心点</summary>
    public Vector3 CenterPos { get => _CenterPos; }


    public AttackArea(float radius, Vector3 center)
    {
        _Radius = radius;
        _CenterPos = center;
    }

    /// <summary>攻撃エリア内にownコライダーがいるか判定し、いれば中心点からの方向を取得</summary>
    /// <param name="own">判定するコライダー</param>
    /// <returns>中心点からの方向</returns>
    abstract public Vector3? InsideArea(Collider own);

    /// <summary>攻撃エリアを作成</summary>
    abstract public CharacterParameter[] EmitArea(LayerMask layer);
}

public class AttackAreaCapsule : AttackArea
{
    /// <summary>二つ目の中心点</summary>
    Vector3 _CenterPos2 = default;

    /// <summary>中心点</summary>
    public Vector3 CenterPos2 { get => _CenterPos2; }


    public AttackAreaCapsule(float radius, Vector3 center1, Vector3 center2) : base(radius, center1)
    {
        _CenterPos2 = center2;
    }

    /// <summary>攻撃エリア内にownコライダーがいるか判定し、いれば中心点からの方向を取得</summary>
    /// <param name="own">判定するコライダー</param>
    /// <returns>中心点からの方向</returns>
    public override Vector3? InsideArea(Collider own)
    {
        Vector3? returnal = null;

        Collider[] hits = Physics.OverlapCapsule(_CenterPos, _CenterPos2, _Radius);
        foreach (Collider hit in hits)
        {
            if(hit == own)
            {
                returnal = own.transform.position - Vector3.Lerp(_CenterPos, _CenterPos2, 0.5f);
                break;
            }
        }

        return returnal;
    }

    /// <summary>攻撃エリアを作成</summary>
    /// <returns>ヒットした相手のパラメーター</returns>
    public override CharacterParameter[] EmitArea(LayerMask layer)
    {
        Collider[] hits = Physics.OverlapCapsule(_CenterPos, _CenterPos2, _Radius, layer);
        CharacterParameter[] returnal = hits.Select(col => col.GetComponent<CharacterParameter>()).ToArray();

        return returnal;
    }
}

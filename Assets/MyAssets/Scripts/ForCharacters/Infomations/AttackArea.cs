using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract public class AttackArea
{
    /// <summary>�T�C�Y���a</summary>
    protected float _Radius = 1f;

    /// <summary>���S�_</summary>
    protected Vector3 _CenterPos = default;


    /// <summary>�T�C�Y���a</summary>
    public float Radius { get => _Radius; }
    /// <summary>���S�_</summary>
    public Vector3 CenterPos { get => _CenterPos; }


    public AttackArea(float radius, Vector3 center)
    {
        _Radius = radius;
        _CenterPos = center;
    }

    /// <summary>�U���G���A����own�R���C�_�[�����邩���肵�A����Β��S�_����̕������擾</summary>
    /// <param name="own">���肷��R���C�_�[</param>
    /// <returns>���S�_����̕���</returns>
    abstract public Vector3? InsideArea(Collider own);

    /// <summary>�U���G���A���쐬</summary>
    abstract public CharacterParameter[] EmitArea(LayerMask layer);
}

public class AttackAreaCapsule : AttackArea
{
    /// <summary>��ڂ̒��S�_</summary>
    Vector3 _CenterPos2 = default;

    /// <summary>���S�_</summary>
    public Vector3 CenterPos2 { get => _CenterPos2; }


    public AttackAreaCapsule(float radius, Vector3 center1, Vector3 center2) : base(radius, center1)
    {
        _CenterPos2 = center2;
    }

    /// <summary>�U���G���A����own�R���C�_�[�����邩���肵�A����Β��S�_����̕������擾</summary>
    /// <param name="own">���肷��R���C�_�[</param>
    /// <returns>���S�_����̕���</returns>
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

    /// <summary>�U���G���A���쐬</summary>
    /// <returns>�q�b�g��������̃p�����[�^�[</returns>
    public override CharacterParameter[] EmitArea(LayerMask layer)
    {
        Collider[] hits = Physics.OverlapCapsule(_CenterPos, _CenterPos2, _Radius, layer);
        CharacterParameter[] returnal = hits.Select(col => col.GetComponent<CharacterParameter>()).ToArray();

        return returnal;
    }
}

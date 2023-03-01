using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    /// <summary>����</summary>
    Vector3 _Direction = Vector3.forward;

    /// <summary>��s���x</summary>
    protected float _Speed = 0.1f;

    void FixedUpdate()
    {
        transform.position += _Direction * _Speed;
    }
}

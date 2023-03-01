using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    /// <summary>ï˚å¸</summary>
    Vector3 _Direction = Vector3.forward;

    /// <summary>îÚçsë¨ìx</summary>
    protected float _Speed = 0.1f;

    void FixedUpdate()
    {
        transform.position += _Direction * _Speed;
    }
}

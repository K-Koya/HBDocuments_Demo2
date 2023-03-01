using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShot : MonoBehaviour
{
    /// <summary>方向</summary>
    Vector3 _Direction = Vector3.forward;

    /// <summary>飛行速度</summary>
    protected float _Speed = 0.1f;

    [SerializeField, Tooltip("飛行中のエフェクト")]
    GameObject _EffectOnFrying = null;

    [SerializeField, Tooltip("着弾し爆発するときのエフェクト")]
    GameObject _EffectOnExplode = null;

    [SerializeField, Tooltip("持続時間")]
    float _LifeTime = 10f;

    /// <summary>経過時間</summary>
    float _Time = 0f;

    void Update()
    {
        if(_Time  < 0f)
        {
            if(_Time  > -100f)
            {
                Explode();
                _Time = -101f;
            }
        }
        else
        {
            _Time -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if(_Time > 0f)
        {
            transform.position += _Direction * _Speed;
        }
    }

    public void Create(Vector3 direction, float speed)
    {
        _Time = _LifeTime;
        _EffectOnFrying?.SetActive(true);
        _EffectOnExplode?.SetActive(false);

        _Direction = direction;
        _Speed = speed;
    }

    void Explode()
    {
        _EffectOnFrying?.SetActive(false);
        _EffectOnExplode?.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
        _Time = -101f;
    }
}

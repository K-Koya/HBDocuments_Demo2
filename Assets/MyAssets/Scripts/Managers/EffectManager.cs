using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [Header("以下に共通パーティクルのプレハブをアサイン")]

    [SerializeField, Tooltip("回復用エフェクト")]
    GameObject _HealEffectPref = null;

    [SerializeField, Tooltip("ダメージ用エフェクト（中）")]
    GameObject _HitEffectMiddlePref = null;


    /// <summary>回復用エフェクト</summary>
    GameObjectPool _HealEffects = null;

    /// <summary>ダメージ用エフェクト（中）</summary>
    GameObjectPool _HitEffectMiddles = null;


    /// <summary>回復用エフェクト</summary>
    public GameObjectPool HealEffects => _HealEffects;
    /// <summary>ダメージ用エフェクト（中）</summary>
    public GameObjectPool HitEffectMiddles => _HitEffectMiddles;


    void Start()
    {
        _HealEffects = new GameObjectPool(_HealEffectPref, 10);
        _HitEffectMiddles = new GameObjectPool(_HitEffectMiddlePref, 10);
    }
}

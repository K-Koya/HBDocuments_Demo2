using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [Header("�ȉ��ɋ��ʃp�[�e�B�N���̃v���n�u���A�T�C��")]

    [SerializeField, Tooltip("�񕜗p�G�t�F�N�g")]
    GameObject _HealEffectPref = null;

    [SerializeField, Tooltip("�_���[�W�p�G�t�F�N�g�i���j")]
    GameObject _HitEffectMiddlePref = null;


    /// <summary>�񕜗p�G�t�F�N�g</summary>
    GameObjectPool _HealEffects = null;

    /// <summary>�_���[�W�p�G�t�F�N�g�i���j</summary>
    GameObjectPool _HitEffectMiddles = null;


    /// <summary>�񕜗p�G�t�F�N�g</summary>
    public GameObjectPool HealEffects => _HealEffects;
    /// <summary>�_���[�W�p�G�t�F�N�g�i���j</summary>
    public GameObjectPool HitEffectMiddles => _HitEffectMiddles;


    void Start()
    {
        _HealEffects = new GameObjectPool(_HealEffectPref, 10);
        _HitEffectMiddles = new GameObjectPool(_HitEffectMiddlePref, 10);
    }
}

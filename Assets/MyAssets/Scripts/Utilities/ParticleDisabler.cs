using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisabler : MonoBehaviour
{
    [SerializeField, Tooltip("パーティクルエフェクトの大本の親オブジェクト")]
    GameObject _ParticleParent = null;

    private void OnParticleSystemStopped()
    {
        _ParticleParent?.gameObject.SetActive(false);
    }
}
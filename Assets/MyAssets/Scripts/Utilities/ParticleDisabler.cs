using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisabler : MonoBehaviour
{
    [SerializeField, Tooltip("�p�[�e�B�N���G�t�F�N�g�̑�{�̐e�I�u�W�F�N�g")]
    GameObject _ParticleParent = null;

    private void OnParticleSystemStopped()
    {
        _ParticleParent?.gameObject.SetActive(false);
    }
}
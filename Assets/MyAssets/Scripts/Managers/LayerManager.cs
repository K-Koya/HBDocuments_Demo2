using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : Singleton<LayerManager>
{
    #region �����o
    [SerializeField, Tooltip("�n�ʃ��C��")]
    LayerMask _Ground = default;
    [SerializeField, Tooltip("�J���������蔲����n�ʃ��C��")]
    LayerMask _SeeThroughGround = default;
    [SerializeField, Tooltip("�G���C��")]
    LayerMask _Enemy = default;
    [SerializeField, Tooltip("�v���C���[�̃��C��")]
    LayerMask _Player = default;
    [SerializeField, Tooltip("�������C��")]
    LayerMask _Allies = default;
    #endregion

    #region �v���p�e�B
    /// <summary>�n�ʃ��C��</summary>
    public LayerMask Ground { get => _Ground; }
    /// <summary>�J���������蔲����n�ʃ��C��</summary>
    public LayerMask SeeThroughGround { get => _SeeThroughGround; }
    /// <summary>�G���C��</summary>
    public LayerMask Enemy { get => _Enemy; }
    /// <summary>�v���C���[�̃��C��</summary>
    public LayerMask Player { get => _Player; }
    /// <summary>�������C��</summary>
    public LayerMask Allies { get => _Allies; }
    /// <summary>�S�Ă̒n�ʃ��C��</summary>
    public LayerMask AllGround { get => _Ground | _SeeThroughGround; }
    /// <summary>�S�ẴL�����N�^�[�̃��C��</summary>
    public LayerMask AllCharacter { get => _Enemy | _Player | _Allies; }
    /// <summary>�S�Ă̖����L�����N�^�[�̃��C��</summary>
    public LayerMask AllAllies { get => _Allies | _Player; }

    #endregion
}
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : Singleton<TagManager>
{
    #region �����o
    [SerializeField, Tooltip("���C���J�����̃^�O")]
    string _MainCamera = "MainCamera";
    [SerializeField, Tooltip("�v���C���[�̃^�O")]
    string _Player = "Player";
    [SerializeField, Tooltip("�G�̃^�O")]
    string _Enemy = "Enemy";
    [SerializeField, Tooltip("�����L�����N�^�[�̃^�O")]
    string _Allies = "Allies";
    [SerializeField, Tooltip("OffMeshLink�̂����A�i������艺�肷�郂�m�̃^�O")]
    string _OffMeshLinkJumpStep = "OffMeshLinkJumpStep";
    [SerializeField, Tooltip("OffMeshLink�̂����A�������ɃW�����v���郂�m�̃^�O")]
    string _OffMeshLinkJumpFar = "OffMeshLinkJumpFar";
    #endregion

    #region �v���p�e�B
    /// <summary>���C���J�����̃^�O</summary>
    public string MainCamera { get => _MainCamera; }
    /// <summary>�v���C���[�̃^�O</summary>
    public string Player { get => _Player; }
    /// <summary>�G�̃^�O</summary>
    public string Enemy { get => _Enemy; }
    /// <summary>�����L�����N�^�[�̃^�O</summary>
    public string Allies { get => _Allies; }
    /// <summary>OffMeshLink�̂����A�i������艺�肷�郂�m�̃^�O</summary>
    public string OffMeshLinkJumpStep { get => _OffMeshLinkJumpStep; }
    /// <summary>OffMeshLink�̂����A�������ɃW�����v���郂�m�̃^�O</summary>
    public string OffMeshLinkJumpFar { get => _OffMeshLinkJumpFar; }
    
    #endregion
}

public static partial class ForRegisterExtensionMethods
{
    public static bool CompareTags(this GameObject gameObject, params string[] tags)
    {
        return tags.Count(tag => gameObject.CompareTag(tag)) > 0;
    }
} 

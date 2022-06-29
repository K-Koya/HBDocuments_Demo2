using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : Singleton<TagManager>
{
    #region メンバ
    [SerializeField, Tooltip("メインカメラのタグ")]
    string _MainCamera = "MainCamera";
    [SerializeField, Tooltip("プレイヤーのタグ")]
    string _Player = "Player";
    [SerializeField, Tooltip("敵のタグ")]
    string _Enemy = "Enemy";
    [SerializeField, Tooltip("味方キャラクターのタグ")]
    string _Allies = "Allies";
    [SerializeField, Tooltip("OffMeshLinkのうち、段差を上り下りするモノのタグ")]
    string _OffMeshLinkJumpStep = "OffMeshLinkJumpStep";
    [SerializeField, Tooltip("OffMeshLinkのうち、遠距離にジャンプするモノのタグ")]
    string _OffMeshLinkJumpFar = "OffMeshLinkJumpFar";
    #endregion

    #region プロパティ
    /// <summary>メインカメラのタグ</summary>
    public string MainCamera { get => _MainCamera; }
    /// <summary>プレイヤーのタグ</summary>
    public string Player { get => _Player; }
    /// <summary>敵のタグ</summary>
    public string Enemy { get => _Enemy; }
    /// <summary>味方キャラクターのタグ</summary>
    public string Allies { get => _Allies; }
    /// <summary>OffMeshLinkのうち、段差を上り下りするモノのタグ</summary>
    public string OffMeshLinkJumpStep { get => _OffMeshLinkJumpStep; }
    /// <summary>OffMeshLinkのうち、遠距離にジャンプするモノのタグ</summary>
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

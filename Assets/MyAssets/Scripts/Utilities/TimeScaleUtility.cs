using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary> 時間経過速度を制御する </summary>
[RequireComponent(typeof(Timekeeper))]
public class TimeScaleUtility : Singleton<TimeScaleUtility>
{
    #region メンバ
    /// <summary> すべてのグローバルクロックを制御するタイムキーパー </summary>
    Timekeeper _Tk = default;

    [Header("以下、派生メンバー")]
    [SerializeField, Tooltip("登録されているGlobalclock名をここにアサイン")]
    GlobalclockName _GlobalclockName = new GlobalclockName();

    /// <summary>true : ポーズフラグ</summary>
    static bool _IsPausing = false;

    #endregion

    #region プロパティ
    /// <summary>true : ポーズフラグ</summary>
    public static bool IsPausing { get => _IsPausing; }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _Tk = GetComponent<Timekeeper>();
    }

    // Update is called once per frame
    void Update()
    {
        //ポーズ機能
        if (InputUtility.GetDownPause)
        {
            if (_IsPausing)
            {
                _Tk.Clock(_GlobalclockName.Pausable).paused = false;
                _IsPausing = false;
            }
            else
            {
                _Tk.Clock(_GlobalclockName.Pausable).paused = true;
                _IsPausing = true;
            }
        }
    }


    /// <summary> 登録されているGlobalclock名をまとめる構造体 </summary>
    [Serializable]
    public struct GlobalclockName
    {
        #region メンバー
        [SerializeField] string _Root;
        [SerializeField] string _Pausable;
        [SerializeField] string _FreeLookCamera;
        [SerializeField] string _Character;
        [SerializeField] string _Allies;
        [SerializeField] string _Player;
        [SerializeField] string _Friend;
        [SerializeField] string _Enemy;
        [SerializeField] string _Unpausable;
        #endregion

        #region プロパティ
        public string Root { get => _Root; }
        public string Pausable { get => _Pausable; }
        public string FreeLookCamera { get => _FreeLookCamera; }
        public string Character { get => _Character; }
        public string Allies { get => _Allies; }
        public string Player { get => _Player; }
        public string Friend { get => _Friend; }
        public string Enemy { get => _Enemy; }
        public string Unpausable { get => _Unpausable; }
        #endregion
    }
}

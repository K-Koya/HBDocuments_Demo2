using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ゲーム全体の流れを制御するコンポーネント </summary>
public class GameManager : Singleton<GameManager>
{
    #region メンバ
    /// <summary>True : ポーズ中である</summary>
    bool _IsPausing = false;



    #endregion

    #region プロパティ
    /// <summary>True : ポーズ中である</summary>
    public bool IsPausing { get => _IsPausing; }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>ポーズボタンでポーズ制御するメソッド</summary>
    void DoPausing()
    {

    }
}

/// <summary> ゲームの状況 </summary>
public enum GameState : byte
{
    Title = 0,
    TitleDemo,
}

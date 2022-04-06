using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ゲーム全体の流れを制御するコンポーネント </summary>
public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/// <summary> ゲームの状況 </summary>
public enum GameState : byte
{
    Title = 0,
    TitleDemo,
}

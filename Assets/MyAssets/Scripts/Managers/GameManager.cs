using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �Q�[���S�̗̂���𐧌䂷��R���|�[�l���g </summary>
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

/// <summary> �Q�[���̏� </summary>
public enum GameState : byte
{
    Title = 0,
    TitleDemo,
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �Q�[���S�̗̂���𐧌䂷��R���|�[�l���g </summary>
public class GameManager : Singleton<GameManager>
{
    #region �����o
    /// <summary>True : �|�[�Y���ł���</summary>
    bool _IsPausing = false;



    #endregion

    #region �v���p�e�B
    /// <summary>True : �|�[�Y���ł���</summary>
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

    /// <summary>�|�[�Y�{�^���Ń|�[�Y���䂷�郁�\�b�h</summary>
    void DoPausing()
    {

    }
}

/// <summary> �Q�[���̏� </summary>
public enum GameState : byte
{
    Title = 0,
    TitleDemo,
}

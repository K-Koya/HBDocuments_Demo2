using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBase : MonoBehaviour
{
    [SerializeField, Tooltip("コマンド名")]
    protected string _Name = "";

    [SerializeField, Tooltip("コマンド説明文"), TextArea(4,10)]
    protected string _Explain = "";



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

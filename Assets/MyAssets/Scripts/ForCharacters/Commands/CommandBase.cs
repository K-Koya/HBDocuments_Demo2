using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBase : MonoBehaviour
{
    [SerializeField, Tooltip("�R�}���h��")]
    protected string _Name = "";

    [SerializeField, Tooltip("�R�}���h������"), TextArea(4,10)]
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

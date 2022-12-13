using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageSituationController : MonoBehaviour
{
    [SerializeField]
    Image _blackout = null;

    [SerializeField]
    float _blackoutDuring = 3f;
    float _blackoutTimer = 0f;

    public bool IsBlackouted { get => !(_blackoutTimer > 0f); }
       
    // Update is called once per frame
    void Update()
    {
        if(_blackoutTimer > 0f)
        {
            _blackoutTimer -= Time.deltaTime;
            float alpha = _blackoutTimer / _blackoutDuring;
            if (IsBlackouted)
            {
                alpha = 0f;
            }
            _blackout.color = new Color(_blackout.color.r, _blackout.color.g, _blackout.color.b, alpha);
        }    
    }

    public void DoBlackout()
    {
        _blackoutTimer = _blackoutDuring;
        _blackout.color = new Color(_blackout.color.r, _blackout.color.g, _blackout.color.b, 1f);
    }

    public void Skip()
    {
        _blackoutTimer = -1f;
        _blackout.color = new Color(_blackout.color.r, _blackout.color.g, _blackout.color.b, 0f);
    }
}

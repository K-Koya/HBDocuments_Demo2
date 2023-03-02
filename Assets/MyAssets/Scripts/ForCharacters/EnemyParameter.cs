using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParameter : CharacterParameter
{
    protected override void EraseStaticReference()
    {
        _Enemies.Remove(this);
    }

    protected override void RegisterStaticReference()
    {
        _Enemies.Add(this);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //Æ€•ûŒü‚Ì’²®
        if (_GazeAt)
        {
            Vector3 direction = Vector3.Normalize(_GazeAt.transform.position - _EyePoint.transform.position);
            if (Vector3.Dot(_EyePoint.transform.forward, direction) > 0f)
            {
                _ReticlePoint = _GazeAt.EyePoint.transform.position;
            }
            else
            {
                _ReticlePoint = _EyePoint.transform.position + _EyePoint.transform.forward * Sub.LockMaxRange;
            }
        }
        else
        {
            _ReticlePoint = _EyePoint.transform.position + _EyePoint.transform.forward * Sub.LockMaxRange;
        }
    }
}

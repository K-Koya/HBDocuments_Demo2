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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameter : CharacterParameter
{
    protected override void EraseStaticReference()
    {
        _Player = null;
    }

    protected override void RegisterStaticReference()
    {
        _Player = this;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _HostilityLayer = LayerManager.Instance.Enemy;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}

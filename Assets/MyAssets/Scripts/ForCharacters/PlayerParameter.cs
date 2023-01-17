using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameter : CharacterParameter
{
    #region メンバ
    /// <summary>照準器の位置</summary>
    Vector3 _ReticlePoint = Vector3.zero;
    #endregion

    #region プロパティ
    public Vector3 ReticlePoint { get => _ReticlePoint; set => _ReticlePoint = value; }
    #endregion

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParameter : CharacterParameter
{

    [Header("以下 Enemy 専用")]
    [SerializeField, Tooltip("強敵用:HP強化倍率")]
    sbyte _EnemyHPCorrection = 0;

    /// <summary>HP強化倍率の現在値</summary>
    sbyte _EnemyHPCorrectionCurrent = 0;


    /// <summary>最大のHP</summary>
    public override short HPMaximum { get => (short)(_Main.HPMaximum * _EnemyHPCorrection); }
    /// <summary>現在のHP</summary>
    public override short HPCurrent { get => (short)(_HPCurrent + _Main.HPMaximum * _EnemyHPCorrectionCurrent); }


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

        _EnemyHPCorrectionCurrent = _EnemyHPCorrection;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //照準方向の調整
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

    public override void GaveDamage(int damage)
    {
        //HP強化倍率値が残っていて、現HPより大きいダメージを受けた
        if(_EnemyHPCorrectionCurrent > 0 && damage > _HPCurrent)
        {
            //被ダメージがHP一本分よりも大きいダメージなら、その本数分だけ減らす
            int div = damage / _HPCurrent;
            int dam = damage - (_HPCurrent * div);

            _HPCurrent = (short)(Main.HPMaximum - dam);
            _EnemyHPCorrectionCurrent -= (sbyte)div;
        }
        else
        {
            _HPCurrent -= (short)damage;
        }



        //HPが0以下になった
        if (_EnemyHPCorrectionCurrent < 0 || _HPCurrent < 1)
        {
            _HPCurrent = 0;
            _EnemyHPCorrectionCurrent = 0;
        }
    }
}

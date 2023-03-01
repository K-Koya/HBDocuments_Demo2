using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParameter : CharacterParameter
{

    [Header("�ȉ� Enemy ��p")]
    [SerializeField, Tooltip("���G�p:HP�����{��")]
    sbyte _EnemyHPCorrection = 0;

    /// <summary>HP�����{���̌��ݒl</summary>
    sbyte _EnemyHPCorrectionCurrent = 0;


    /// <summary>�ő��HP</summary>
    public override short HPMaximum { get => (short)(_Main.HPMaximum * _EnemyHPCorrection); }
    /// <summary>���݂�HP</summary>
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

        //�Ə������̒���
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
        //HP�����{���l���c���Ă��āA��HP���傫���_���[�W���󂯂�
        if(_EnemyHPCorrectionCurrent > 0 && damage > _HPCurrent)
        {
            //��_���[�W��HP��{�������傫���_���[�W�Ȃ�A���̖{�����������炷
            int div = damage / _HPCurrent;
            int dam = damage - (_HPCurrent * div);

            _HPCurrent = (short)(Main.HPMaximum - dam);
            _EnemyHPCorrectionCurrent -= (sbyte)div;
        }
        else
        {
            _HPCurrent -= (short)damage;
        }



        //HP��0�ȉ��ɂȂ���
        if (_EnemyHPCorrectionCurrent < 0 || _HPCurrent < 1)
        {
            _HPCurrent = 0;
            _EnemyHPCorrectionCurrent = 0;
        }
    }
}

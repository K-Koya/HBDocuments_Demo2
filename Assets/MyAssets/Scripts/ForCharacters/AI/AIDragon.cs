using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDragon : ComputerMove
{
    /// <summary>�߂����鑊��ɑ΂��铮����I������m��(%)</summary>
    byte _RatioOfNearExcess = 0;

    /// <summary>�ߋ����ōs�����铮����I������m��(%)</summary>
    byte _RatioOfNearMovement = 0;

    /// <summary>�U���̂������͂ȍU����I������m��(%)</summary>
    byte _RatioOfPowerfullAttack = 0;

    /// <summary>true : �Y���s���̏������������ς�ł���</summary>
    bool _IsInitialized = false;

    /// <summary>true : �ЂƂO�̍s�����ړ�������</summary>
    bool _WasMove = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        GetCondition = GetConditionOnBattleOneTarget;
        Think = ThinkOnBattleOneTarget;
        Act = KeepPoint;

        _Param.State.Kind = MotionState.StateKind.Stay;
        _Param.State.Process = MotionState.ProcessKind.Playing;

        _MoveTimeLimit= 0;
        _MoveTime = 0;
    }

    #region ��������̎擾���\�b�h
    /// <summary>1�̂�W�I�Ƃ���퓬���̍s���𕪊򂷂�������擾���郁�\�b�h</summary>
    void GetConditionOnBattleOneTarget()
    {
        EnemyParameter enemyParam = _Param as EnemyParameter;

        float sqrTargetDistance = enemyParam.Sub.LockMaxRange;
        if(_Param.GazeAt) sqrTargetDistance = Vector3.SqrMagnitude(_Param.GazeAt.transform.position - enemyParam.transform.position);
        float ratioHP = enemyParam.HPCurrent / enemyParam.HPMaximum;

        //�����ߋ���
        if(Mathf.Pow(enemyParam.Sub.ComboProximityRange, 2f) < sqrTargetDistance)
        {
            _RatioOfNearExcess = 90;
            _RatioOfNearMovement = 9;
        }
        //�˒��O
        else if(Mathf.Pow(enemyParam.Sub.LockMaxRange, 2f) > sqrTargetDistance)
        {
            _RatioOfNearExcess = 0;
            _RatioOfNearMovement = 10;
        }
        //���悢����
        else
        {
            //����
            if(Mathf.Pow(enemyParam.Sub.LockMaxRange / 2f, 2f) > sqrTargetDistance)
            {
                _RatioOfNearExcess = 5;
                _RatioOfNearMovement = 20;
            }
            //�߂�
            else
            {
                _RatioOfNearExcess = 20;
                _RatioOfNearMovement = 60;
            }
        }

        //�̗͑���
        if(ratioHP > 0.6f)
        {
            _RatioOfPowerfullAttack = 0;
        }
        else
        {
            _RatioOfPowerfullAttack = 60;
        }
    }

    #endregion


    #region �s���̕��򃁃\�b�h
    /// <summary>1�̂�W�I�Ƃ���퓬���̍s�����򃁃\�b�h</summary>
    void ThinkOnBattleOneTarget()
    {
        _DoAction = false;

        switch (_Param.State.Kind)
        {
            //�������̒��n�`�F�b�N
            case MotionState.StateKind.FallNoraml:
            case MotionState.StateKind.JumpNoraml:

                if (IsGround) _CommandHolder.Jump.LandingProcess(_Param);
                break;

            //������̈ړ��̓`�F�b�N
            case MotionState.StateKind.ShiftSlide:

                _CommandHolder.ShiftSlide.ShiftSlidePostProcess(_Param, _Rb.component, GravityDirection);
                break;
            case MotionState.StateKind.LongTrip:

                _CommandHolder.LongTrip.LongTripPostProcess(_Param, _Rb.component, GravityDirection);
                break;

            //��_���[�W�`�F�b�N
            case MotionState.StateKind.Hurt:


                break;
        }

        //���̍s��������
        if (Act is null)
        {
            _MoveTime = 0f;
            _IsInitialized = true;
            _WasMove = !_WasMove;
            //���͂ȍs��
            if (_RatioOfPowerfullAttack > Random.value * 100)
            {
                //�����ߋ���
                if (_RatioOfNearExcess < Random.value * 100)
                {
                    Act = _WasMove ? JumpTurn : Backwards;
                }
                //�ߋ���
                else if (_RatioOfNearMovement < Random.value * 100)
                {
                    Act = _WasMove ? BiteCombo : Wandering;
                }
                //������
                else
                {
                    Act = _WasMove ? FireShot : Approach;
                }
            }
            //���ʊ��̍s��
            else
            {
                float rand = Random.value;
                if(rand < 0.3f)
                {
                    Act = BiteCombo;
                }
                else if(rand < 0.6f)
                {
                    Act = JumpTurn;
                }
                else if(rand < 0.9f)
                {
                    Act = FireShot;                }
                else
                {
                    Act = KeepPoint;
                }

                /*
                //�����ߋ���
                if (_RatioOfNearExcess < Random.value * 100)
                {
                    Act = _WasMove ? JumpTurn : RunAway;
                }
                //�ߋ���
                else if (_RatioOfNearMovement < Random.value * 100)
                {
                    Act = _WasMove ? FireShot : Backwards;
                }
                //������
                else
                {
                    Act = _WasMove ? FireShot : Approach;
                }
                */
            }
        }
        //�s�����Ԃ��v��
        else
        {
            _MoveTime -= Time.deltaTime;
        }
    }

    #endregion

    #region �ړ����\�b�h
    /// <summary>�������ɂ��̏�ɋ������郁�\�b�h</summary>
    void KeepPoint()
    {
        //���񏈗�
        if (_IsInitialized)
        {
            _Param.State.Kind = MotionState.StateKind.Stay;
            _Param.State.Process = MotionState.ProcessKind.Playing;
            Destination = null;
            _MoveTime = 3f;
            _IsInitialized = false;
        }

        //���Ԑ؂�ŏI������
        if (_MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>�v���C���[�ɑ����Đڋ߂��悤�Ƃ��郁�\�b�h</summary>
    void Approach()
    {
        //���񏈗�
        if (_IsInitialized)
        {
            _Param.State.Kind = MotionState.StateKind.Run;
            _Param.State.Process = MotionState.ProcessKind.Playing;
            Destination = _Param.GazeAt.transform.position;
            _MoveTime = 10f;
            _IsInitialized = false;
        }

        _Param.CharacterDirection = _Param.MoveDirection;

        //�ړI�n�ɓ������邩���Ԑ؂�ŏI������
        if (IsCloseDestination || _MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>�v���C���[���瑖���ċ�����u�����Ƃ��郁�\�b�h</summary>
    void RunAway()
    {
        //���񏈗�
        if (_IsInitialized)
        {
            _Param.State.Kind = MotionState.StateKind.Run;
            _Param.State.Process = MotionState.ProcessKind.Playing;
            Destination = _Param.GazeAt.transform.position;
            _MoveTime = 10f;
            _IsInitialized = false;
        }

        _Param.CharacterDirection = _Param.MoveDirection;

        //�ړI�n�ɓ������邩���Ԑ؂�ŏI������
        if (IsCloseDestination || _MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>�v���C���[�����i���������郁�\�b�h</summary>
    void Backwards()
    {
        //���񏈗�
        if (_IsInitialized)
        {
            _Param.State.Kind = MotionState.StateKind.Run;
            _Param.State.Process = MotionState.ProcessKind.Playing;
            Destination = _Param.GazeAt.transform.position;
            _MoveTime = 5f;
            _IsInitialized = false;
        }

        _Param.CharacterDirection = Vector3.Normalize(_Param.GazeAt.transform.position - _Param.EyePoint.transform.position);

        //�ړI�n�ɓ������邩���Ԑ؂�ŏI������
        if (IsCloseDestination || _MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>�v���C���[�t�߂�����ėl�q�������������\�b�h</summary>
    void Wandering()
    {
        //���񏈗�
        if (_IsInitialized)
        {
            _Param.State.Kind = MotionState.StateKind.Walk;
            _Param.State.Process = MotionState.ProcessKind.Playing;
            Destination = _Param.GazeAt.transform.position + new Vector3(Random.value * 10f, 0f, Random.value * 10f);
            _MoveTime = Random.Range(1f, 10f);
            _IsInitialized = false;
        }

        if(Random.value < 0.05f)
        {
            Destination = _Param.GazeAt.transform.position + new Vector3(Random.value * 10f, 0f, Random.value * 10f);
        }

        //���Ԑ؂�ŏI������
        if (_MoveTime <= 0f)
        {
            Act = null;
        }
    }
    #endregion

    #region �s�����\�b�h
    /// <summary>�A���Ŋ��݂��čŌ�Ƀ_�C�u����R���{�U�����s�����\�b�h</summary>
    void BiteCombo()
    {
        Vector3 dir = Vector3.Normalize(_Param.GazeAt.EyePoint.position - _Param.EyePoint.position);

        //���񏈗�
        if (_IsInitialized)
        {
            _CommandHolder.GetActiveSkillForRun(1).DoRun(_Param, _Rb.component, GravityDirection, dir, ref _AnimKind);
            Destination = null;
            _DoAction = true;
            _MoveTime = 8f;
            _IsInitialized = false;
        }

        CharacterRotation(dir, -GravityDirection, 10f);

        //���Ԑ؂�ŃR���{����
        if (_MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>���̏�ŃW�����v���Ȃ��炱����Ɍ�������A���n�Œn�Ȃ炵�U�������郁�\�b�h</summary>
    void JumpTurn()
    {
        Vector3 dir = Vector3.Normalize(_Param.GazeAt.EyePoint.position - _Param.EyePoint.position);

        //���񏈗�
        if (_IsInitialized)
        {
            _CommandHolder.GetActiveSkillForRun(2).DoRun(_Param, _Rb.component, GravityDirection, dir, ref _AnimKind);
            _DoAction = true;
            Destination = null;
            _MoveTime = 6f;
            _IsInitialized = false;
        }

        CharacterRotation(dir, -GravityDirection, 90f);

        //���Ԑ؂�ŏI������
        if (_MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>�Ή��e���ō��łT�A�����˂��郁�\�b�h</summary>
    void FireShot()
    {
        Vector3 dir = Vector3.Normalize(_Param.GazeAt.EyePoint.position - _Param.EyePoint.position);

        //���񏈗�
        if (_IsInitialized)
        {
            _CommandHolder.GetActiveSkillForRun(0).DoRun(_Param, _Rb.component, GravityDirection, dir, ref _AnimKind);
            _DoAction = true;
            Destination = null;
            _MoveTime = 4f;
            _IsInitialized = false;
        }

        //�ǉ����͎�t
        if(_Param.State.Process == MotionState.ProcessKind.Interval)
        {
            //���肪�����Ȃ�ǉ��Ŏ��s
            if(_RatioOfNearExcess / 100f < Random.value)
            {
                _CommandHolder.GetActiveSkillForRun(0).DoRun(_Param, _Rb.component, GravityDirection, dir, ref _AnimKind);
                _DoAction = true;
            }
        }

        //���Ԑ؂�ŏI������
        if (_MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>���ʍ��E����������悤�ɉ���f�����\�b�h</summary>
    void FireBreath()
    {

    }
    #endregion
}

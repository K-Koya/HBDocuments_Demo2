using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDragon : ComputerMove
{
    /// <summary>近すぎる相手に対する動きを選択する確率(%)</summary>
    byte _RatioOfNearExcess = 0;

    /// <summary>近距離で行動する動きを選択する確率(%)</summary>
    byte _RatioOfNearMovement = 0;

    /// <summary>攻撃のうち強力な攻撃を選択する確率(%)</summary>
    byte _RatioOfPowerfullAttack = 0;

    /// <summary>true : 該当行動の初期化処理が済んでいる</summary>
    bool _IsInitialized = false;

    /// <summary>true : ひとつ前の行動が移動だった</summary>
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

    #region 分岐条件の取得メソッド
    /// <summary>1体を標的とする戦闘時の行動を分岐する条件を取得するメソッド</summary>
    void GetConditionOnBattleOneTarget()
    {
        EnemyParameter enemyParam = _Param as EnemyParameter;

        float sqrTargetDistance = enemyParam.Sub.LockMaxRange;
        if(_Param.GazeAt) sqrTargetDistance = Vector3.SqrMagnitude(_Param.GazeAt.transform.position - enemyParam.transform.position);
        float ratioHP = enemyParam.HPCurrent / enemyParam.HPMaximum;

        //超至近距離
        if(Mathf.Pow(enemyParam.Sub.ComboProximityRange, 2f) < sqrTargetDistance)
        {
            _RatioOfNearExcess = 90;
            _RatioOfNearMovement = 9;
        }
        //射程外
        else if(Mathf.Pow(enemyParam.Sub.LockMaxRange, 2f) > sqrTargetDistance)
        {
            _RatioOfNearExcess = 0;
            _RatioOfNearMovement = 10;
        }
        //程よい距離
        else
        {
            //遠目
            if(Mathf.Pow(enemyParam.Sub.LockMaxRange / 2f, 2f) > sqrTargetDistance)
            {
                _RatioOfNearExcess = 5;
                _RatioOfNearMovement = 20;
            }
            //近め
            else
            {
                _RatioOfNearExcess = 20;
                _RatioOfNearMovement = 60;
            }
        }

        //体力多め
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


    #region 行動の分岐メソッド
    /// <summary>1体を標的とする戦闘時の行動分岐メソッド</summary>
    void ThinkOnBattleOneTarget()
    {
        _DoAction = false;

        switch (_Param.State.Kind)
        {
            //落下等の着地チェック
            case MotionState.StateKind.FallNoraml:
            case MotionState.StateKind.JumpNoraml:

                if (IsGround) _CommandHolder.Jump.LandingProcess(_Param);
                break;

            //回避時の移動力チェック
            case MotionState.StateKind.ShiftSlide:

                _CommandHolder.ShiftSlide.ShiftSlidePostProcess(_Param, _Rb.component, GravityDirection);
                break;
            case MotionState.StateKind.LongTrip:

                _CommandHolder.LongTrip.LongTripPostProcess(_Param, _Rb.component, GravityDirection);
                break;

            //被ダメージチェック
            case MotionState.StateKind.Hurt:


                break;
        }

        //次の行動を決定
        if (Act is null)
        {
            _MoveTime = 0f;
            _IsInitialized = true;
            _WasMove = !_WasMove;
            //強力な行動
            if (_RatioOfPowerfullAttack > Random.value * 100)
            {
                //超至近距離
                if (_RatioOfNearExcess < Random.value * 100)
                {
                    Act = _WasMove ? JumpTurn : Backwards;
                }
                //近距離
                else if (_RatioOfNearMovement < Random.value * 100)
                {
                    Act = _WasMove ? BiteCombo : Wandering;
                }
                //遠距離
                else
                {
                    Act = _WasMove ? FireShot : Approach;
                }
            }
            //普通寄りの行動
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
                //超至近距離
                if (_RatioOfNearExcess < Random.value * 100)
                {
                    Act = _WasMove ? JumpTurn : RunAway;
                }
                //近距離
                else if (_RatioOfNearMovement < Random.value * 100)
                {
                    Act = _WasMove ? FireShot : Backwards;
                }
                //遠距離
                else
                {
                    Act = _WasMove ? FireShot : Approach;
                }
                */
            }
        }
        //行動時間を計測
        else
        {
            _MoveTime -= Time.deltaTime;
        }
    }

    #endregion

    #region 移動メソッド
    /// <summary>動かずにその場に居続けるメソッド</summary>
    void KeepPoint()
    {
        //初回処理
        if (_IsInitialized)
        {
            _Param.State.Kind = MotionState.StateKind.Stay;
            _Param.State.Process = MotionState.ProcessKind.Playing;
            Destination = null;
            _MoveTime = 3f;
            _IsInitialized = false;
        }

        //時間切れで終了処理
        if (_MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>プレイヤーに走って接近しようとするメソッド</summary>
    void Approach()
    {
        //初回処理
        if (_IsInitialized)
        {
            _Param.State.Kind = MotionState.StateKind.Run;
            _Param.State.Process = MotionState.ProcessKind.Playing;
            Destination = _Param.GazeAt.transform.position;
            _MoveTime = 10f;
            _IsInitialized = false;
        }

        _Param.CharacterDirection = _Param.MoveDirection;

        //目的地に到着するか時間切れで終了処理
        if (IsCloseDestination || _MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>プレイヤーから走って距離を置こうとするメソッド</summary>
    void RunAway()
    {
        //初回処理
        if (_IsInitialized)
        {
            _Param.State.Kind = MotionState.StateKind.Run;
            _Param.State.Process = MotionState.ProcessKind.Playing;
            Destination = _Param.GazeAt.transform.position;
            _MoveTime = 10f;
            _IsInitialized = false;
        }

        _Param.CharacterDirection = _Param.MoveDirection;

        //目的地に到着するか時間切れで終了処理
        if (IsCloseDestination || _MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>プレイヤーから後進しつつ向き直るメソッド</summary>
    void Backwards()
    {
        //初回処理
        if (_IsInitialized)
        {
            _Param.State.Kind = MotionState.StateKind.Run;
            _Param.State.Process = MotionState.ProcessKind.Playing;
            Destination = _Param.GazeAt.transform.position;
            _MoveTime = 5f;
            _IsInitialized = false;
        }

        _Param.CharacterDirection = Vector3.Normalize(_Param.GazeAt.transform.position - _Param.EyePoint.transform.position);

        //目的地に到着するか時間切れで終了処理
        if (IsCloseDestination || _MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>プレイヤー付近を歩いて様子をうかがうメソッド</summary>
    void Wandering()
    {
        //初回処理
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

        //時間切れで終了処理
        if (_MoveTime <= 0f)
        {
            Act = null;
        }
    }
    #endregion

    #region 行動メソッド
    /// <summary>連続で噛みついて最後にダイブするコンボ攻撃を行うメソッド</summary>
    void BiteCombo()
    {
        Vector3 dir = Vector3.Normalize(_Param.GazeAt.EyePoint.position - _Param.EyePoint.position);

        //初回処理
        if (_IsInitialized)
        {
            _CommandHolder.GetActiveSkillForRun(1).DoRun(_Param, _Rb.component, GravityDirection, dir, ref _AnimKind);
            Destination = null;
            _DoAction = true;
            _MoveTime = 8f;
            _IsInitialized = false;
        }

        CharacterRotation(dir, -GravityDirection, 10f);

        //時間切れでコンボ入力
        if (_MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>その場でジャンプしながらこちらに向き直り、着地で地ならし攻撃をするメソッド</summary>
    void JumpTurn()
    {
        Vector3 dir = Vector3.Normalize(_Param.GazeAt.EyePoint.position - _Param.EyePoint.position);

        //初回処理
        if (_IsInitialized)
        {
            _CommandHolder.GetActiveSkillForRun(2).DoRun(_Param, _Rb.component, GravityDirection, dir, ref _AnimKind);
            _DoAction = true;
            Destination = null;
            _MoveTime = 6f;
            _IsInitialized = false;
        }

        CharacterRotation(dir, -GravityDirection, 90f);

        //時間切れで終了処理
        if (_MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>火炎弾を最高で５連発発射するメソッド</summary>
    void FireShot()
    {
        Vector3 dir = Vector3.Normalize(_Param.GazeAt.EyePoint.position - _Param.EyePoint.position);

        //初回処理
        if (_IsInitialized)
        {
            _CommandHolder.GetActiveSkillForRun(0).DoRun(_Param, _Rb.component, GravityDirection, dir, ref _AnimKind);
            _DoAction = true;
            Destination = null;
            _MoveTime = 4f;
            _IsInitialized = false;
        }

        //追加入力受付
        if(_Param.State.Process == MotionState.ProcessKind.Interval)
        {
            //相手が遠くなら追加で実行
            if(_RatioOfNearExcess / 100f < Random.value)
            {
                _CommandHolder.GetActiveSkillForRun(0).DoRun(_Param, _Rb.component, GravityDirection, dir, ref _AnimKind);
                _DoAction = true;
            }
        }

        //時間切れで終了処理
        if (_MoveTime <= 0f)
        {
            Act = null;
        }
    }

    /// <summary>正面左右を往復するように炎を吐くメソッド</summary>
    void FireBreath()
    {

    }
    #endregion
}

using System.Collections.Generic;
using UnityEngine;
using Chronos;


[RequireComponent(typeof(Rigidbody), typeof(CharacterParameter))]
abstract public class CharacterMove : MonoBehaviour
{
    #region 定数
    /// <summary>速度が0であるとみなす数値</summary>
    protected const float VELOCITY_ZERO_BORDER = 0.5f;
    #endregion

    #region メンバ
    /// <summary>移動向けに力をかける時の力の大きさ</summary>
    float _MovePower = 3.0f;

    /// <summary>結果の移動速度</summary>
    float _Speed = 0.0f;

    /// <summary>true : 攻撃アニメーションが終了した</summary>
    protected bool _IsAttackEnd = true;

    /// <summary>true : コンボ攻撃入力</summary>
    protected bool _DoCombo = false;

    /// <summary>キャラクターの持つ情報</summary>
    protected CharacterParameter _Param = null;

    /// <summary>当該キャラクターの物理挙動コンポーネント</summary>
    protected RigidbodyTimeline3D _Rb = null;

    /// <summary>True : ジャンプ直後</summary>
    protected bool _JumpFlag = false;

    /// <summary>着地判定をするコンポーネント</summary>
    GroundChecker _GroundChecker = default;

    /// <summary>移動入力の大きさ</summary>
    protected float _MoveInputRate = 0f;

    /// <summary>かけるブレーキ力</summary>
    protected Vector3 _ForceOfBrake = Vector3.zero;

    /// <summary>移動用メソッド</summary>
    protected System.Action Move = null;

    /// <summary>攻撃用メソッド</summary>
    protected System.Action Attack = null;

    #endregion

    #region プロパティ
    /// <summary>true : コンボ攻撃入力があった(直後フラグを折る)</summary>
    public bool DoCombo => _DoCombo;
    /// <summary>True : 着地している</summary>
    public bool IsGround => _GroundChecker.IsGround;
    /// <summary>重力方向</summary>
    protected Vector3 GravityDirection => _GroundChecker.GravityDirection;
    /// <summary>Rigidbodyのvelocityを移動方向平面に換算したもの</summary>
    protected Vector3 VelocityOnPlane => Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);
    /// <summary>移動向けに力をかける時の力の大きさ</summary>
    public float MovePower { set => _MovePower = value; }
    /// <summary>結果の移動速度</summary>
    public float Speed => _Speed;
    /// <summary>ジャンプ直後フラグ</summary>
    public bool JumpFlag => _JumpFlag;
    
    #endregion



    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Param = GetComponent<CharacterParameter>();
        _GroundChecker = GetComponent<GroundChecker>();

        _Rb = _Param.Tl.rigidbody;
        _Rb.useGravity = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //timeScaleが0ならポーズ中
        if (!(_Param.Tl.timeScale > 0f)) return;

        //速度測定
        _Speed = VelocityOnPlane.magnitude;
        Move?.Invoke();
        Attack?.Invoke();
    }

    void FixedUpdate()
    {
        //着地しているか否かで分岐
        //着地中
        if (IsGround)
        {
            //移動力がかかっている
            if (_MoveInputRate > 0f)
            {
                //回転する
                CharacterRotation(_Param.Direction, -GravityDirection, 360f);

                //力をかける
                _Rb.AddForce(transform.forward * _MoveInputRate * _MovePower, ForceMode.Acceleration);

                //速度(向き)を、入力方向へ設定
                _Rb.velocity = Quaternion.FromToRotation(Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection), transform.forward) * _Rb.velocity;

                //重力をかける
                _Rb.AddForce(GravityDirection * 2f, ForceMode.Acceleration);
            }
            //移動力がなければ、現在の速度が閾値を下回った時に0にする
            else
            {
                if (VelocityOnPlane.sqrMagnitude < VELOCITY_ZERO_BORDER)
                {
                    _Rb.velocity = Vector3.Project(_Rb.velocity, -GravityDirection);
                }

                //重力をかける
                _Rb.AddForce(GravityDirection * 1f, ForceMode.Acceleration);
            }
        }
        //空中
        else
        {
            //移動力がかかっている
            if (_MoveInputRate > 0f)
            {
                //回転する
                CharacterRotation(_Param.Direction, -GravityDirection, 90f);

                //力をかける
                _Rb.AddForce(_Param.Direction * _MoveInputRate * _MovePower, ForceMode.Acceleration);
            }

            //重力をかける
            _Rb.AddForce(GravityDirection * 9.8f, ForceMode.Acceleration);
        }

        //速度減衰をかける
        if (_ForceOfBrake.sqrMagnitude > 0f)
        {
            _Rb.AddForce(_ForceOfBrake, ForceMode.Acceleration);
        }
    }


    /// <summary> キャラクターを指定向きに回転させる </summary>
    /// <param name="targetDirection">目標向き</param>
    /// <param name="up">上方向（Vector.Zeroなら上方向を指定しない）</param>
    /// <param name="rotateSpeed">回転速度</param>
    protected void CharacterRotation(Vector3 targetDirection, Vector3 up, float rotateSpeed)
    {
        if (targetDirection.sqrMagnitude > 0.0f)
        {
            Vector3 trunDirection = transform.right;
            Quaternion charDirectionQuaternion = Quaternion.identity;
            if (up.sqrMagnitude > 0f) charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f), up);
            else charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, charDirectionQuaternion, rotateSpeed * Time.deltaTime);
        }
    }

    
    /// <summary>アニメーションイベントにて、攻撃アニメーション開始情報を受け取る</summary>
    public void AttackStartCall()
    {
        _IsAttackEnd = false;
        _Param.State.Process = MotionState.ProcessKind.Playing;

        //攻撃を作成
        foreach(AttackArea at in _Param.AttackAreas)
        {
            CharacterParameter[] attackHits = at.EmitArea(_Param.HostilityLayer);
        }
    }

    /// <summary>アニメーションイベントにて、攻撃アニメーションの攻撃部分の終了情報を受け取る</summary>
    public void AttackEndCall()
    {
        _Param.AttackAreas.Clear();
        _Param.State.Process = MotionState.ProcessKind.Interval;
    }

    /// <summary>アニメーションイベントにて、コンボフィニッシュアニメーションの攻撃部分の終了情報を受け取る</summary>
    public void ComboFinishEndCall()
    {
        _Param.State.Process = MotionState.ProcessKind.EndSoon;
    }

    /// <summary>コンボ追加入力受付</summary>
    public void ComboAcceptCall()
    {
        _IsAttackEnd = true;
    }

    /// <summary>アニメーションイベントにて、攻撃アニメーションそのものの終了情報を受け取る</summary>
    public void AttackAnimationEndCall()
    {
        _IsAttackEnd = true;
        _Param.State.Process = MotionState.ProcessKind.NotPlaying;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

public class PlayerMove : CharacterMove
{
    #region メンバ
    /// <summary>MainCameraの位置等の情報</summary>
    Transform _MainCameraTransform = default;

    /// <summary>当該キャラクターの物理挙動コンポーネント</summary>
    RigidbodyTimeline3D _Rb = default;

    /// <summary>移動入力の大きさ</summary>
    float _CurrentMovePower = 0f;

    /// <summary>かけるブレーキ力</summary>
    Vector3 _ForceOfBrake = Vector3.zero;
    #endregion


    #region プロパティ
    /// <summary>Rigidbodyのvelocityを移動方向平面に換算したもの</summary>
    Vector3 VelocityOnPlane => Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);

    /// <summary>移動速度</summary>
    public override float Speed => VelocityOnPlane.magnitude;
    #endregion

    protected override void Start()
    {
        base.Start();

        _Rb = _Tl.rigidbody;
        _Rb.useGravity = false;
        _MainCameraTransform = Camera.main.transform;

        Move = MoveOnPlane;
    }

    void FixedUpdate()
    {
        //timeScaleが0ならポーズ中
        if (!(_Tl.timeScale > 0f)) return;

        //着地しているか否かで分岐
        //着地中
        if (IsGround)
        {
            //移動力がかかっている
            if (_CurrentMovePower > 0f)
            {
                //回転する
                CharacterRotation(_CharacterDirection, -GravityDirection, 360f);

                //力をかける
                _Rb.AddForce(transform.forward * _CurrentMovePower * 5f, ForceMode.Acceleration);

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
            if (_CurrentMovePower > 0f)
            {
                //回転する
                CharacterRotation(_CharacterDirection, -GravityDirection, 90f);

                //力をかける
                _Rb.AddForce(_CharacterDirection * _CurrentMovePower * 5f, ForceMode.Acceleration);
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



    /// <summary>重力方向の逐一変化をしない移動メソッド</summary>
    void MoveOnPlane()
    {
        //前方向と右方向を取得し、移動入力値を反映
        Vector3 vertical = Vector3.ProjectOnPlane(_MainCameraTransform.forward, -GravityDirection);
        vertical = vertical.normalized * InputUtility.GetAxis2DMoveDirection.y;
        Vector3 horizontal = Vector3.ProjectOnPlane(_MainCameraTransform.right, -GravityDirection);
        horizontal = horizontal.normalized * InputUtility.GetAxis2DMoveDirection.x;

        _CharacterDirection = vertical + horizontal;

        //入力があれば移動力の処理
        if(_CharacterDirection.sqrMagnitude > 0)
        {
            //移動入力の大きさを取得
            _CurrentMovePower = _CharacterDirection.magnitude;
            //移動方向を取得
            _CharacterDirection *= 1 / _CurrentMovePower;
        }
        else
        {
            _CurrentMovePower = 0f;
            _CharacterDirection = Vector3.zero;
        }

        //重力方向以外で移動量成分があった場合、ブレーキ量を計算する
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -horizontal * 0.2f;
        }


        //ジャンプ力減衰
        if (!InputUtility.GetJump && !IsGround && Vector3.Angle(-GravityDirection, _Rb.velocity) < 90f)
        {
            _Rb.velocity = Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);
        }

        //着地時にジャンプ処理
        _JumpFlag = false;
        if (IsGround && InputUtility.GetDownJump)
        {
            _Rb.AddForce(-GravityDirection * 7f, ForceMode.VelocityChange);
            _JumpFlag = true;
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
}

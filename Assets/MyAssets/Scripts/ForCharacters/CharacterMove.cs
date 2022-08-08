using System;
using System.Collections.Generic;
using UnityEngine;
using Chronos;


[RequireComponent(typeof(Rigidbody), typeof(CharacterParameter))]
public class CharacterMove : MonoBehaviour
{
    #region 定数
    /// <summary>速度が0であるとみなす数値</summary>
    protected const float VELOCITY_ZERO_BORDER = 0.5f;
    #endregion

    #region メンバ
    [SerializeField, Tooltip("移動操作における正面方向を指定")]
    MoveForwardMode _MoveForwardMode = MoveForwardMode.WorldTransformZ;

    /// <summary>MainCameraの位置等の情報</summary>
    Transform _MainCameraTransform = default;

    /// <summary>入力状況</summary>
    IController _Controller = null;

    /// <summary>キャラクターの持つ情報</summary>
    protected CharacterParameter _Param = null;

    /// <summary>当該キャラクターの物理挙動コンポーネント</summary>
    protected RigidbodyTimeline3D _Rb = null;

    /// <summary>true : ジャンプ直後</summary>
    protected bool _JumpFlag = false;

    /// <summary>着地判定をするコンポーネント</summary>
    GroundChecker _GroundChecker = default;

    /// <summary>移動入力の大きさ</summary>
    protected float _CurrentMovePower = 0f;

    /// <summary>かけるブレーキ力</summary>
    protected Vector3 _ForceOfBrake = Vector3.zero;


    /// <summary>移動用メソッド</summary>
    protected Action Move = default;

    #endregion

    #region プロパティ

    public CharacterParameter Status => _Param;
    /// <summary>True : 着地している</summary>
    public bool IsGround => _GroundChecker.IsGround;
    /// <summary>重力方向</summary>
    protected Vector3 GravityDirection => _GroundChecker.GravityDirection;
    /// <summary>Rigidbodyのvelocityを移動方向平面に換算したもの</summary>
    protected Vector3 VelocityOnPlane => Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);
    /// <summary>移動速度</summary>
    public float Speed => VelocityOnPlane.magnitude;
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

        _MainCameraTransform = Camera.main.transform;
        _Controller = GetComponent<IController>();

        Move = MoveOnPlane;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //timeScaleが0ならポーズ中
        if (!(_Param.Tl.timeScale > 0f)) return;

        Move?.Invoke();
    }

    void FixedUpdate()
    {
        //着地しているか否かで分岐
        //着地中
        if (IsGround)
        {
            //移動力がかかっている
            if (_CurrentMovePower > 0f)
            {
                //回転する
                CharacterRotation(_Param.Direction, -GravityDirection, 360f);

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
                CharacterRotation(_Param.Direction, -GravityDirection, 90f);

                //力をかける
                _Rb.AddForce(_Param.Direction * _CurrentMovePower * 5f, ForceMode.Acceleration);
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

    /// <summary>移動方向を入力値から計算して求める</summary>
    /// <param name="input">入力値</param>
    /// <returns>移動方向</returns>
    Vector3 CalculateMoveDirection(Vector2 input)
    {
        //前方向と右方向を取得し、移動入力値を反映
        Vector3 vertical = Vector3.ProjectOnPlane(_MainCameraTransform.forward, -GravityDirection);
        vertical = vertical.normalized * input.y;
        Vector3 horizontal = Vector3.ProjectOnPlane(_MainCameraTransform.right, -GravityDirection);
        horizontal = horizontal.normalized * input.x;

        return vertical + horizontal;
    }

    /// <summary>重力方向の逐一変化をしない移動メソッド</summary>
    void MoveOnPlane()
    {
        //移動操作における正面方向の指定によって、正面方向情報を書き換える
        switch (_MoveForwardMode)
        {
            case MoveForwardMode.MainCameraTransformZ:
                _Param.Direction = CalculateMoveDirection(_Controller.MoveDirection());
                break;
            default: break;
        }

        //入力があれば移動力の処理
        if (_Param.Direction.sqrMagnitude > 0)
        {
            //移動入力の大きさを取得
            _CurrentMovePower = _Param.Direction.magnitude;
            //移動方向を取得
            _Param.Direction *= 1 / _CurrentMovePower;
        }
        else
        {
            _CurrentMovePower = 0f;
            _Param.Direction = Vector3.zero;
        }

        //重力方向以外で移動量成分があった場合、ブレーキ量を計算する
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -VelocityOnPlane.normalized * 0.2f;
        }


        //ジャンプ力減衰
        if (_Controller.Jump() == InputState.Untouched && !IsGround && Vector3.Angle(-GravityDirection, _Rb.velocity) < 90f)
        {
            _Rb.velocity = Vector3.ProjectOnPlane(_Rb.velocity, -GravityDirection);
        }

        //着地時にジャンプ処理
        _JumpFlag = false;
        if (IsGround && _Controller.Jump() == InputState.PushDown)
        {
            _Rb.AddForce(-GravityDirection * 7f, ForceMode.VelocityChange);
            _JumpFlag = true;
        }
    }


    /// <summary>移動操作における正面方向モード</summary>
    public enum MoveForwardMode : byte
    {
        /// <summary>ワールド座標空間のZ軸の正方向</summary>
        WorldTransformZ,
        /// <summary>MainCameraの正面方向</summary>
        MainCameraTransformZ,
    }
}

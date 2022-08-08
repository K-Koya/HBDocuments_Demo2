using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

/// <summary>
/// InputManagerで使われているボタン名の文字列を管理
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputUtility : Singleton<InputUtility>
{
    #region InputActionのActionsキー
    [Header("以下、InputActionのActionsに登録した名前を登録")]
    [SerializeField, Tooltip("InputActionにおける、スタートボタン名")]
    string _ButtonNameStart = "Start";

    [SerializeField, Tooltip("InputActionにおける、ポーズメニューボタン名")]
    string _ButtonNamePause = "Pause";

    [SerializeField, Tooltip("InputActionにおける、リストメニューボタン名")]
    string _ButtonNameOption = "Option";

    [SerializeField, Tooltip("InputActionにおける、移動方向の入力名")]
    string _StickNameMoveDirection = "MoveDirection";

    [SerializeField, Tooltip("InputActionにおける、カメラ移動入力名")]
    string _StickNameCameraMove = "CameraMove";

    [SerializeField, Tooltip("InputActionにおける、ジャンプ入力名")]
    string _ButtonNameJump = "Jump";

    [SerializeField, Tooltip("InputActionにおける、特殊コマンド入力名")]
    string _ButtonNameAimCommand = "AimCommand";

    [SerializeField, Tooltip("InputActionにおける、攻撃入力名")]
    string _ButtonNameAttack = "Attack";

    [SerializeField, Tooltip("InputActionにおける、ガード入力名")]
    string _ButtonNameGuard = "Guard";

    [SerializeField, Tooltip("InputActionにおける、回避入力名")]
    string _ButtonNameDodge = "Dodge";
    #endregion
    /*
    #region コントローラー振動用メンバ
    /// <summary> コントローラー </summary>
    //static Gamepad _Gamepad = default;

    [Header("コントローラー振動用に用いるパラメーター")]
    [SerializeField, Range(0, 1), Tooltip("コントローラーの右側の振動の強さ")]
    float _RightShakePower = 0.5f;

    /// <summary>DOTween保管用 : コントローラーの右側の振動の強さ</summary>
    float _TweenRightShakePower = 0f;

    [SerializeField, Range(0, 1), Tooltip("コントローラーの左側の振動の強さ")]
    float _LeftShakePower = 0.5f;

    /// <summary>DOTween保管用 : コントローラーの左側の振動の強さ</summary>
    float _TweenLeftShakePower = 0f;

    [SerializeField, Tooltip("コントローラーの振動させる時間")]
    float _ShakeInterval = 0.75f;

    [SerializeField, Tooltip("コントローラーの振動していない時間")]
    float _UnShakeInterval = 0.75f;

    [SerializeField, Tooltip("DOTweenを使って細かい振動をさせる場合の使うEasingタイプ")]
    Ease _TweenShakeModeRight = Ease.Linear;

    [SerializeField, Tooltip("DOTweenを使って大きな振動をさせる場合の使うEasingタイプ")]
    Ease _TweenShakeModeLeft = Ease.Linear;
    #endregion
    */

    #region InputAction
    /// <summary> スタートボタンの入力状況 </summary>
    static InputAction _StartAction = default;

    /// <summary> ポーズメニュー起動の入力状況 </summary>
    static InputAction _PauseAction = default;

    /// <summary> リストメニュー起動の入力状況 </summary>
    static InputAction _OptionAction = default;

    /// <summary> 移動操作の入力状況 </summary>
    static InputAction _MoveDirectionAction = default;

    /// <summary> カメラ移動操作の入力状況 </summary>
    static InputAction _CameraMoveAction = default;

    /// <summary> ジャンプの入力状況 </summary>
    static InputAction _JumpAction = default;

    /// <summary> 特殊コマンドボタンの入力状況 </summary>
    static InputAction _AimCommandAction = default;

    /// <summary> 攻撃ボタンの入力状況 </summary>
    static InputAction _AttackAction = default;

    /// <summary> ガードボタンの入力状況 </summary>
    static InputAction _GuardAction = default;

    /// <summary> 回避ボタンの入力状況 </summary>
    static InputAction _DodgeAction = default;
    #endregion

    #region プロパティ
    /// <summary> スタートボタンの入力状況 </summary>
    static public InputAction StartAction { get => _StartAction; }
    /// <summary> ポーズメニュー起動の入力状況 </summary>
    static public InputAction PauseAction { get => _PauseAction; }
    /// <summary> リストメニュー起動の入力状況 </summary>
    static public InputAction OptionAction { get => _OptionAction; }
    /// <summary> 移動操作の入力状況 </summary>
    static public InputAction MoveDirectionAction { get => _MoveDirectionAction; }
    /// <summary> カメラ移動操作の入力状況 </summary>
    static public InputAction CameraMoveAction { get => _CameraMoveAction; }
    /// <summary> ジャンプの入力状況 </summary>
    static public InputAction JumpAction { get => _JumpAction; }
    /// <summary> 特殊コマンドボタンの入力状況 </summary>
    static public InputAction AimCommandAction { get => _AimCommandAction; }
    /// <summary> 攻撃ボタンの入力状況 </summary>
    static public InputAction AttackAction { get => _AttackAction; }
    /// <summary> ガードボタンの入力状況 </summary>
    static public InputAction GuardAction { get => _GuardAction; }
    /// <summary> 回避ボタンの入力状況 </summary>
    static public InputAction DodgeAction { get => _DodgeAction; }


    /// <summary> スタートボタン押下直後 </summary>
    static public bool GetDownStart { get => _StartAction.triggered; }
    /// <summary> ポーズボタン押下直後 </summary>
    static public bool GetDownPause { get => _PauseAction.triggered; }
    /// <summary> リストメニューボタン押下直後 </summary>
    static public bool GetDownOption { get => _OptionAction.triggered; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //入力を関連付け
        PlayerInput input = GetComponent<PlayerInput>();
        InputActionMap actionMap = input.currentActionMap;
        _StartAction = actionMap[_ButtonNameStart];
        _PauseAction = actionMap[_ButtonNamePause];
        _OptionAction = actionMap[_ButtonNameOption];
        _MoveDirectionAction = actionMap[_StickNameMoveDirection];
        _CameraMoveAction = actionMap[_StickNameCameraMove];
        _JumpAction = actionMap[_ButtonNameJump];
        _AimCommandAction = actionMap[_ButtonNameAimCommand];
        _AttackAction = actionMap[_ButtonNameAttack];
    }
    /*
        void OnDestroy()
        {
            StopShakeController();
        }

        /// <summary> コントローラーの振動を促す。ただし、数値は0～1の範囲で。範囲を超える場合はClampする。 </summary>
        /// <param name="leftPower">左側のモーター強度</param>
        /// <param name="rightPower">右側のモーター強度</param>
        static public void SimpleShakeController(float leftPower, float rightPower)
        {
            if (_Gamepad == null) _Gamepad = Gamepad.current;
            if (_Gamepad != null)
            {
                _Gamepad.SetMotorSpeeds(Mathf.Clamp01(leftPower), Mathf.Clamp01(rightPower));
            }
        }

        /// <summary>コントローラーの振動を止める</summary>
        static public void StopShakeController()
        {
            if (_Gamepad == null) _Gamepad = Gamepad.current;
            if (_Gamepad != null)
            {
                _Gamepad.SetMotorSpeeds(0f, 0f);
            }
        }

        /// <summary>コントローラーの振動を一定間隔で</summary>
        IEnumerator PalusShake()
        {
            while (enabled)
            {
                _TweenLeftShakePower = _LeftShakePower;
                _TweenRightShakePower = _RightShakePower;

                yield return new WaitForSeconds(_ShakeInterval);

                _TweenLeftShakePower = 0f;
                _TweenRightShakePower = 0f;

                yield return new WaitForSeconds(_UnShakeInterval);
            }
        }

        /// <summary>コントローラーの振動を一定間隔で</summary>
        IEnumerator PalusTweenShake()
        {
            while (enabled)
            {
                DOTween.To(() => _TweenLeftShakePower, f => _TweenLeftShakePower = f, 0, _ShakeInterval).SetEase(_TweenShakeModeLeft);
                DOTween.To(() => _TweenRightShakePower, f => _TweenRightShakePower = f, 0, _ShakeInterval).SetEase(_TweenShakeModeRight);

                yield return new WaitForSeconds(_ShakeInterval);

                _TweenLeftShakePower = 0f;
                _TweenRightShakePower = 0f;

                yield return new WaitForSeconds(_UnShakeInterval);

                _TweenLeftShakePower = _LeftShakePower;
                _TweenRightShakePower = _RightShakePower;
            }
        }

        /// <summary>コントローラーの振動を一定間隔で</summary>
        IEnumerator PalusTestShake()
        {
            while (enabled)
            {
                Sequence seqLeft = DOTween.Sequence();
                Sequence seqRight = DOTween.Sequence();
                seqLeft.Append(DOTween.To(() => _TweenLeftShakePower, f => _TweenLeftShakePower = f, 0, _ShakeInterval).SetEase(Ease.InCubic));
                seqRight.Append(DOTween.To(() => _TweenRightShakePower, f => _TweenRightShakePower = f, _RightShakePower, _ShakeInterval / 2f).SetEase(Ease.InCubic));
                seqRight.Append(DOTween.To(() => _TweenRightShakePower, f => _TweenRightShakePower = f, 0, _ShakeInterval / 2f).SetEase(Ease.OutCubic));
                seqLeft.Play();
                seqRight.Play();

                yield return seqRight.WaitForCompletion();

                _TweenLeftShakePower = 0f;
                _TweenRightShakePower = 0f;

                yield return new WaitForSeconds(_UnShakeInterval);

                _TweenLeftShakePower = _LeftShakePower;
            }
        }
    */
}

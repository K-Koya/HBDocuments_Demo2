using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [SerializeField, Tooltip("InputActionにおける、移動入力名")]
    string _StickNameMove = "Move";

    [SerializeField, Tooltip("InputActionにおける、カメラ移動入力名")]
    string _StickNameCameraMove = "CameraMove";

    [SerializeField, Tooltip("InputActionにおける、ジャンプ入力名")]
    string _ButtonNameJump = "Jump";

    [SerializeField, Tooltip("InputActionにおける、特殊コマンド入力名")]
    string _ButtonNameCommand = "Command";

    [SerializeField, Tooltip("InputActionにおける、攻撃入力名")]
    string _ButtonNameAttack = "Attack";
    #endregion

    #region コントローラー振動用メンバ
    /// <summary> コントローラー </summary>
    static Gamepad _Gamepad = default;

    [SerializeField, Range(0, 1), Tooltip("コントローラーの右側の振動の強さ")]
    float _RightShakePower = 0.5f;

    [SerializeField, Range(0, 1), Tooltip("コントローラーの左側の振動の強さ")]
    float _LeftShakePower = 0.5f;

    [SerializeField, Tooltip("コントローラーの振動の間隔")]
    float _ShakeInterval = 0.2f;
    #endregion

    #region InputAction
    /// <summary> スタートボタンの入力状況 </summary>
    static InputAction _StartAction = default;

    /// <summary> ポーズメニュー起動の入力状況 </summary>
    static InputAction _PauseAction = default;

    /// <summary> リストメニュー起動の入力状況 </summary>
    static InputAction _OptionAction = default;

    /// <summary> 移動操作の入力状況 </summary>
    static InputAction _MoveAction = default;

    /// <summary> カメラ移動操作の入力状況 </summary>
    static InputAction _CameraMoveAction = default;

    /// <summary> ジャンプの入力状況 </summary>
    static InputAction _JumpAction = default;

    /// <summary> 特殊コマンドボタンの入力状況 </summary>
    static InputAction _CommandAction = default;

    /// <summary> 攻撃ボタンの入力状況 </summary>
    static InputAction _AttackAction = default;
    #endregion

    #region プロパティ
    /// <summary> スタートボタン押下直後 </summary>
    static public bool GetDownStart { get => _StartAction.triggered; }
    /// <summary> ポーズボタン押下直後 </summary>
    static public bool GetDownPause { get => _PauseAction.triggered; }
    /// <summary> リストメニューボタン押下直後 </summary>
    static public bool GetDownOption { get => _OptionAction.triggered; }
    /// <summary> 移動操作の二次元値 </summary>
    static public Vector2 GetAxis2DMove { get => _MoveAction.ReadValue<Vector2>(); }
    /// <summary> カメラ移動操作の二次元値 </summary>
    static public Vector2 GetAxis2DCameraMove { get => _CameraMoveAction.ReadValue<Vector2>(); }
    /// <summary> ジャンプボタン押下直後 </summary>
    static public bool GetDownJump { get => _JumpAction.triggered; }
    /// <summary> ジャンプボタン押下中 </summary>
    static public bool GetJump { get => _JumpAction.IsPressed(); }
    /// <summary> 特殊コマンドボタン押下直後 </summary>
    static public bool GetDownCommand { get => _CommandAction.triggered; }
    /// <summary> 特殊コマンドボタン押下中 </summary>
    static public bool GetCommand { get => _CommandAction.IsPressed(); }
    /// <summary> 攻撃ボタン押下直後 </summary>
    static public bool GetDownAttack { get => _AttackAction.triggered; }
    /// <summary> 攻撃ボタン押下中 </summary>
    static public bool GetAttack { get => _AttackAction.IsPressed(); }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //ゲームパッド情報を取得
        _Gamepad = Gamepad.current;

        StartCoroutine(PalusShake());
    }

    // Update is called once per frame
    void Update()
    {
        //入力を関連付け
        PlayerInput input = GetComponent<PlayerInput>();
        InputActionMap actionMap = input.currentActionMap;
        _StartAction = actionMap[_ButtonNameStart];
        _PauseAction = actionMap[_ButtonNamePause];
        _OptionAction = actionMap[_ButtonNameOption];
        _MoveAction = actionMap[_StickNameMove];
        _CameraMoveAction = actionMap[_StickNameCameraMove];
        _JumpAction = actionMap[_ButtonNameJump];
        _CommandAction = actionMap[_ButtonNameCommand];
        _AttackAction = actionMap[_ButtonNameAttack];
    }

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
            SimpleShakeController(_LeftShakePower, _RightShakePower);

            yield return new WaitForSeconds(_ShakeInterval);

            StopShakeController();

            yield return new WaitForSeconds(_ShakeInterval);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputManager�Ŏg���Ă���{�^�����̕�������Ǘ�
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputUtility : Singleton<InputUtility>
{
    #region InputAction��Actions�L�[
    [Header("�ȉ��AInputAction��Actions�ɓo�^�������O��o�^")]
    [SerializeField, Tooltip("InputAction�ɂ�����A�X�^�[�g�{�^����")]
    string _ButtonNameStart = "Start";

    [SerializeField, Tooltip("InputAction�ɂ�����A�|�[�Y���j���[�{�^����")]
    string _ButtonNamePause = "Pause";

    [SerializeField, Tooltip("InputAction�ɂ�����A���X�g���j���[�{�^����")]
    string _ButtonNameOption = "Option";

    [SerializeField, Tooltip("InputAction�ɂ�����A�ړ����͖�")]
    string _StickNameMove = "Move";

    [SerializeField, Tooltip("InputAction�ɂ�����A�J�����ړ����͖�")]
    string _StickNameCameraMove = "CameraMove";

    [SerializeField, Tooltip("InputAction�ɂ�����A�W�����v���͖�")]
    string _ButtonNameJump = "Jump";

    [SerializeField, Tooltip("InputAction�ɂ�����A����R�}���h���͖�")]
    string _ButtonNameCommand = "Command";

    [SerializeField, Tooltip("InputAction�ɂ�����A�U�����͖�")]
    string _ButtonNameAttack = "Attack";
    #endregion

    #region �R���g���[���[�U���p�����o
    /// <summary> �R���g���[���[ </summary>
    static Gamepad _Gamepad = default;

    [SerializeField, Range(0, 1), Tooltip("�R���g���[���[�̉E���̐U���̋���")]
    float _RightShakePower = 0.5f;

    [SerializeField, Range(0, 1), Tooltip("�R���g���[���[�̍����̐U���̋���")]
    float _LeftShakePower = 0.5f;

    [SerializeField, Tooltip("�R���g���[���[�̐U���̊Ԋu")]
    float _ShakeInterval = 0.2f;
    #endregion

    #region InputAction
    /// <summary> �X�^�[�g�{�^���̓��͏� </summary>
    static InputAction _StartAction = default;

    /// <summary> �|�[�Y���j���[�N���̓��͏� </summary>
    static InputAction _PauseAction = default;

    /// <summary> ���X�g���j���[�N���̓��͏� </summary>
    static InputAction _OptionAction = default;

    /// <summary> �ړ�����̓��͏� </summary>
    static InputAction _MoveAction = default;

    /// <summary> �J�����ړ�����̓��͏� </summary>
    static InputAction _CameraMoveAction = default;

    /// <summary> �W�����v�̓��͏� </summary>
    static InputAction _JumpAction = default;

    /// <summary> ����R�}���h�{�^���̓��͏� </summary>
    static InputAction _CommandAction = default;

    /// <summary> �U���{�^���̓��͏� </summary>
    static InputAction _AttackAction = default;
    #endregion

    #region �v���p�e�B
    /// <summary> �X�^�[�g�{�^���������� </summary>
    static public bool GetDownStart { get => _StartAction.triggered; }
    /// <summary> �|�[�Y�{�^���������� </summary>
    static public bool GetDownPause { get => _PauseAction.triggered; }
    /// <summary> ���X�g���j���[�{�^���������� </summary>
    static public bool GetDownOption { get => _OptionAction.triggered; }
    /// <summary> �ړ�����̓񎟌��l </summary>
    static public Vector2 GetAxis2DMove { get => _MoveAction.ReadValue<Vector2>(); }
    /// <summary> �J�����ړ�����̓񎟌��l </summary>
    static public Vector2 GetAxis2DCameraMove { get => _CameraMoveAction.ReadValue<Vector2>(); }
    /// <summary> �W�����v�{�^���������� </summary>
    static public bool GetDownJump { get => _JumpAction.triggered; }
    /// <summary> �W�����v�{�^�������� </summary>
    static public bool GetJump { get => _JumpAction.IsPressed(); }
    /// <summary> ����R�}���h�{�^���������� </summary>
    static public bool GetDownCommand { get => _CommandAction.triggered; }
    /// <summary> ����R�}���h�{�^�������� </summary>
    static public bool GetCommand { get => _CommandAction.IsPressed(); }
    /// <summary> �U���{�^���������� </summary>
    static public bool GetDownAttack { get => _AttackAction.triggered; }
    /// <summary> �U���{�^�������� </summary>
    static public bool GetAttack { get => _AttackAction.IsPressed(); }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //�Q�[���p�b�h�����擾
        _Gamepad = Gamepad.current;

        StartCoroutine(PalusShake());
    }

    // Update is called once per frame
    void Update()
    {
        //���͂��֘A�t��
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

    /// <summary> �R���g���[���[�̐U���𑣂��B�������A���l��0�`1�͈̔͂ŁB�͈͂𒴂���ꍇ��Clamp����B </summary>
    /// <param name="leftPower">�����̃��[�^�[���x</param>
    /// <param name="rightPower">�E���̃��[�^�[���x</param>
    static public void SimpleShakeController(float leftPower, float rightPower)
    {
        if (_Gamepad == null) _Gamepad = Gamepad.current;
        if (_Gamepad != null)
        {
            _Gamepad.SetMotorSpeeds(Mathf.Clamp01(leftPower), Mathf.Clamp01(rightPower));
        }
    }

    /// <summary>�R���g���[���[�̐U�����~�߂�</summary>
    static public void StopShakeController()
    {
        if (_Gamepad == null) _Gamepad = Gamepad.current;
        if (_Gamepad != null)
        {
            _Gamepad.SetMotorSpeeds(0f, 0f);
        }
    }

    /// <summary>�R���g���[���[�̐U�������Ԋu��</summary>
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

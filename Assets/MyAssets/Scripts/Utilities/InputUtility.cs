using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

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

    [SerializeField, Tooltip("InputAction�ɂ�����A�ړ������̓��͖�")]
    string _StickNameMoveDirection = "MoveDirection";

    [SerializeField, Tooltip("InputAction�ɂ�����A�J�����ړ����͖�")]
    string _StickNameCameraMove = "CameraMove";

    [SerializeField, Tooltip("InputAction�ɂ�����A�W�����v���͖�")]
    string _ButtonNameJump = "Jump";

    [SerializeField, Tooltip("InputAction�ɂ�����A����R�}���h���͖�")]
    string _ButtonNameAimCommand = "AimCommand";

    [SerializeField, Tooltip("InputAction�ɂ�����A�U�����͖�")]
    string _ButtonNameAttack = "Attack";

    [SerializeField, Tooltip("InputAction�ɂ�����A�K�[�h���͖�")]
    string _ButtonNameGuard = "Guard";

    [SerializeField, Tooltip("InputAction�ɂ�����A�����͖�")]
    string _ButtonNameDodge = "Dodge";
    #endregion
    /*
    #region �R���g���[���[�U���p�����o
    /// <summary> �R���g���[���[ </summary>
    //static Gamepad _Gamepad = default;

    [Header("�R���g���[���[�U���p�ɗp����p�����[�^�[")]
    [SerializeField, Range(0, 1), Tooltip("�R���g���[���[�̉E���̐U���̋���")]
    float _RightShakePower = 0.5f;

    /// <summary>DOTween�ۊǗp : �R���g���[���[�̉E���̐U���̋���</summary>
    float _TweenRightShakePower = 0f;

    [SerializeField, Range(0, 1), Tooltip("�R���g���[���[�̍����̐U���̋���")]
    float _LeftShakePower = 0.5f;

    /// <summary>DOTween�ۊǗp : �R���g���[���[�̍����̐U���̋���</summary>
    float _TweenLeftShakePower = 0f;

    [SerializeField, Tooltip("�R���g���[���[�̐U�������鎞��")]
    float _ShakeInterval = 0.75f;

    [SerializeField, Tooltip("�R���g���[���[�̐U�����Ă��Ȃ�����")]
    float _UnShakeInterval = 0.75f;

    [SerializeField, Tooltip("DOTween���g���čׂ����U����������ꍇ�̎g��Easing�^�C�v")]
    Ease _TweenShakeModeRight = Ease.Linear;

    [SerializeField, Tooltip("DOTween���g���đ傫�ȐU����������ꍇ�̎g��Easing�^�C�v")]
    Ease _TweenShakeModeLeft = Ease.Linear;
    #endregion
    */

    #region InputAction
    /// <summary> �X�^�[�g�{�^���̓��͏� </summary>
    static InputAction _StartAction = default;

    /// <summary> �|�[�Y���j���[�N���̓��͏� </summary>
    static InputAction _PauseAction = default;

    /// <summary> ���X�g���j���[�N���̓��͏� </summary>
    static InputAction _OptionAction = default;

    /// <summary> �ړ�����̓��͏� </summary>
    static InputAction _MoveDirectionAction = default;

    /// <summary> �J�����ړ�����̓��͏� </summary>
    static InputAction _CameraMoveAction = default;

    /// <summary> �W�����v�̓��͏� </summary>
    static InputAction _JumpAction = default;

    /// <summary> ����R�}���h�{�^���̓��͏� </summary>
    static InputAction _AimCommandAction = default;

    /// <summary> �U���{�^���̓��͏� </summary>
    static InputAction _AttackAction = default;

    /// <summary> �K�[�h�{�^���̓��͏� </summary>
    static InputAction _GuardAction = default;

    /// <summary> ����{�^���̓��͏� </summary>
    static InputAction _DodgeAction = default;
    #endregion

    #region �v���p�e�B
    /// <summary> �X�^�[�g�{�^���̓��͏� </summary>
    static public InputAction StartAction { get => _StartAction; }
    /// <summary> �|�[�Y���j���[�N���̓��͏� </summary>
    static public InputAction PauseAction { get => _PauseAction; }
    /// <summary> ���X�g���j���[�N���̓��͏� </summary>
    static public InputAction OptionAction { get => _OptionAction; }
    /// <summary> �ړ�����̓��͏� </summary>
    static public InputAction MoveDirectionAction { get => _MoveDirectionAction; }
    /// <summary> �J�����ړ�����̓��͏� </summary>
    static public InputAction CameraMoveAction { get => _CameraMoveAction; }
    /// <summary> �W�����v�̓��͏� </summary>
    static public InputAction JumpAction { get => _JumpAction; }
    /// <summary> ����R�}���h�{�^���̓��͏� </summary>
    static public InputAction AimCommandAction { get => _AimCommandAction; }
    /// <summary> �U���{�^���̓��͏� </summary>
    static public InputAction AttackAction { get => _AttackAction; }
    /// <summary> �K�[�h�{�^���̓��͏� </summary>
    static public InputAction GuardAction { get => _GuardAction; }
    /// <summary> ����{�^���̓��͏� </summary>
    static public InputAction DodgeAction { get => _DodgeAction; }


    /// <summary> �X�^�[�g�{�^���������� </summary>
    static public bool GetDownStart { get => _StartAction.triggered; }
    /// <summary> �|�[�Y�{�^���������� </summary>
    static public bool GetDownPause { get => _PauseAction.triggered; }
    /// <summary> ���X�g���j���[�{�^���������� </summary>
    static public bool GetDownOption { get => _OptionAction.triggered; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //���͂��֘A�t��
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
                _TweenLeftShakePower = _LeftShakePower;
                _TweenRightShakePower = _RightShakePower;

                yield return new WaitForSeconds(_ShakeInterval);

                _TweenLeftShakePower = 0f;
                _TweenRightShakePower = 0f;

                yield return new WaitForSeconds(_UnShakeInterval);
            }
        }

        /// <summary>�R���g���[���[�̐U�������Ԋu��</summary>
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

        /// <summary>�R���g���[���[�̐U�������Ԋu��</summary>
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

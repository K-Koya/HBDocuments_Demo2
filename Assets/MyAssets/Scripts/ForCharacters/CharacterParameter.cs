using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>�L�����N�^�[�̏����܂Ƃ߂�R���|�[�l���g</summary>
[RequireComponent(typeof(Timeline))]
abstract public class CharacterParameter : MonoBehaviour
{
    #region �L�����Ԃ̃A�N�Z�b�T

    /// <summary>�v���C���[�L�����N�^�[���i�[</summary>
    protected static CharacterParameter _Player = null;

    /// <summary>�����L�����N�^�[���i�[</summary>
    protected static List<CharacterParameter> _Allies = new List<CharacterParameter>();

    /// <summary>�G�L�����N�^�[���i�[</summary>
    protected static List<CharacterParameter> _Enemies = new List<CharacterParameter>();
    #endregion

    #region �����o
    [SerializeField, Tooltip("�L�����N�^�[�̖ڐ��ʒu")]
    protected Transform _EyePoint = default;

    /// <summary>���Y�L�����N�^�[�������Ԏ��R���|�[�l���g</summary>
    protected Timeline _Tl = default;

    /// <summary>�L�����N�^�[���ʕ������</summary>
    protected Vector3 _CharacterDirection = default;

    #endregion


    #region �v���p�e�B
    /// <summary>�v���C���[�L�����N�^�[���i�[</summary>
    public static CharacterParameter Player => _Player;
    /// <summary>�����L�����N�^�[�����X�g������Ă���</summary>
    public static IReadOnlyList<CharacterParameter> Allies => _Allies;
    /// <summary>�G�L�����N�^�[�����X�g������Ă���</summary>
    public static IReadOnlyList<CharacterParameter> Enemies => _Enemies;
    /// <summary>�L�����N�^�[�̖ڐ��ʒu</summary>
    public Transform EyePoint { get => _EyePoint; set => _EyePoint = value; }
    /// <summary>�{�L�����N�^�[�̎��ԏ��</summary>
    public Timeline Tl { get => _Tl; }
    /// <summary>�L�����N�^�[���ʕ������</summary>
    public Vector3 Direction { get => _CharacterDirection; set => _CharacterDirection = value; }
    #endregion

    /// <summary>�{�N���X�̐ÓI�����o�Ɏ��R���|�[�l���g��o�^�����郁�\�b�h</summary>
    abstract protected void RegisterStaticReference();

    /// <summary>�{�N���X�̐ÓI�����o���玩�R���|�[�l���g�𖕏����郁�\�b�h</summary>
    abstract protected void EraseStaticReference();

    void Awake()
    {
        RegisterStaticReference();
    }

    void OnDestroy()
    {
        EraseStaticReference();
    }

    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Tl = GetComponent<Timeline>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}

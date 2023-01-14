using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutController : MonoBehaviour
{
    #region �����o�[
    [Header("�Ó]�i���]�j�c�[��")]
    [SerializeField, Tooltip("�Ó]�p���h��摜")]
    Image _Blackout = null;

    /// <summary>�Ó]�ɔ�₷����</summary>
    float _BlackoutDuring = 3f;

    /// <summary>�Ó]�p�^�C�}�[</summary>
    float _BlackoutTimer = 0f;

    /// <summary>true : ���]��������</summary>
    bool _UseWhiteout = false;
    #endregion


    #region �v���p�e�B
    /// <summary>true : �Ó]�i���]�j����������</summary>
    public bool IsBlackouted { get => !(_BlackoutTimer > 0f); }
    #endregion


    // Update is called once per frame
    void Update()
    {
        //�Ó]�i���]�j����
        if(!IsBlackouted)
        {
            _BlackoutTimer -= Time.deltaTime;
            float alpha = _BlackoutTimer / _BlackoutDuring;
            if (_UseWhiteout)
            {
                if (IsBlackouted)
                {
                    alpha = 0f;
                }
            }
            else
            {
                alpha = 1f - alpha;
                if (IsBlackouted)
                {
                    alpha = 1f;
                }
            }
            _Blackout.color = new Color(_Blackout.color.r, _Blackout.color.g, _Blackout.color.b, alpha);
        }


    }

    /// <summary>�Ó]����</summary>
    /// <param name="during">�|���鎞��(s)</param>
    public void DoBlackout(float during = 1f)
    {
        //�Ó]�������Ȃ痣�E
        if (!IsBlackouted) return;

        _BlackoutDuring = during;
        _BlackoutTimer = _BlackoutDuring;
        _Blackout.color = new Color(_Blackout.color.r, _Blackout.color.g, _Blackout.color.b, 1f);
        _UseWhiteout = false;
    }

    /// <summary>���]����</summary>
    /// <param name="during">�|���鎞��(s)</param>
    public void DoWhiteout(float during = 1f)
    {
        //���]�������Ȃ痣�E
        if (!IsBlackouted) return;

        _BlackoutDuring = during;
        _BlackoutTimer = _BlackoutDuring;
        _Blackout.color = new Color(_Blackout.color.r, _Blackout.color.g, _Blackout.color.b, 0f);
        _UseWhiteout = true;
    }

    /// <summary>�Ó]�i���]�j�����̃X�L�b�v�v��</summary>
    public void SkipBlackout()
    {
        _BlackoutTimer = -1f;
        float alpha = _UseWhiteout ? 0f : 1f;
        _Blackout.color = new Color(_Blackout.color.r, _Blackout.color.g, _Blackout.color.b, alpha);
    }
}

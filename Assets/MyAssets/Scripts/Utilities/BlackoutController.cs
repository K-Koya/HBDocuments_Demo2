using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutController : MonoBehaviour
{
    #region メンバー
    [Header("暗転（明転）ツール")]
    [SerializeField, Tooltip("暗転用黒塗り画像")]
    Image _Blackout = null;

    /// <summary>暗転に費やす時間</summary>
    float _BlackoutDuring = 3f;

    /// <summary>暗転用タイマー</summary>
    float _BlackoutTimer = 0f;

    /// <summary>true : 明転をさせる</summary>
    bool _UseWhiteout = false;
    #endregion


    #region プロパティ
    /// <summary>true : 暗転（明転）が完了した</summary>
    public bool IsBlackouted { get => !(_BlackoutTimer > 0f); }
    #endregion


    // Update is called once per frame
    void Update()
    {
        //暗転（明転）処理
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

    /// <summary>暗転処理</summary>
    /// <param name="during">掛ける時間(s)</param>
    public void DoBlackout(float during = 1f)
    {
        //暗転処理中なら離脱
        if (!IsBlackouted) return;

        _BlackoutDuring = during;
        _BlackoutTimer = _BlackoutDuring;
        _Blackout.color = new Color(_Blackout.color.r, _Blackout.color.g, _Blackout.color.b, 1f);
        _UseWhiteout = false;
    }

    /// <summary>明転処理</summary>
    /// <param name="during">掛ける時間(s)</param>
    public void DoWhiteout(float during = 1f)
    {
        //明転処理中なら離脱
        if (!IsBlackouted) return;

        _BlackoutDuring = during;
        _BlackoutTimer = _BlackoutDuring;
        _Blackout.color = new Color(_Blackout.color.r, _Blackout.color.g, _Blackout.color.b, 0f);
        _UseWhiteout = true;
    }

    /// <summary>暗転（明転）処理のスキップ要求</summary>
    public void SkipBlackout()
    {
        _BlackoutTimer = -1f;
        float alpha = _UseWhiteout ? 0f : 1f;
        _Blackout.color = new Color(_Blackout.color.r, _Blackout.color.g, _Blackout.color.b, alpha);
    }
}

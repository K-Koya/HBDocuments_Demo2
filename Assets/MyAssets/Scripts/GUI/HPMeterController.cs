using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Chronos;

public class HPMeterController : MonoBehaviour 
{
    /// <summary>プレイヤーのステータス</summary>
    CharacterParameter _Param = null;

    /// <summary>時間軸コンポーネント</summary>
    Timeline _Tl = null;

    [SerializeField, Tooltip("プレイヤーHPの実数表示Imageコンポーネント")]
    Image _HpMeterNowImg = default;
    
    [SerializeField, Tooltip("プレイヤーHPの余白表示Imageコンポーネント")]
    Image _HpMeterBlankImg = default;

    /// <summary>HPの余白表示のためのHP値保管</summary>
    float _BeforeHPRatio = 0.0f;


    // Use this for initialization
    void Start () 
    {
        _Param = FindObjectOfType<PlayerParameter>();
        _Tl = GetComponent<Timeline>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        SyncHPMeter();
    }

    /// <summary>HPメーターをプレイヤーのHPとして反映させる</summary>
    private void SyncHPMeter()
    {
        short hpMaximum = _Param.Main.HPMaximum;
        short hpCurrent = _Param.HPCurrent;
        //怯み中はBlank部分を表示する
        bool doAppearBlankHP = _Param.State.Kind is MotionState.StateKind.Hurt;

        //HPの割合値を計算
        float hpRatio = hpCurrent / (float)hpMaximum;

        //HP実数値のメーターを設定
        _HpMeterNowImg.fillAmount = hpRatio;

        //HPに応じていい感じに 青→緑→黄→赤→赤黒 に変化させていくための演算
        float hue = (4.0f * hpRatio - 1.0f) / 6.0f;
        float val = 0.9f;
        if (hue < 0.0f)
        {
            val += hue;
            hue = 0.0f;
        }
        _HpMeterNowImg.color = Color.HSVToRGB(hue, 1.0f, val);

        //HPの余白表示が表示されている状態で、余白部分を減らすフラグが立っていれば減少処理
        if (_BeforeHPRatio > hpRatio)
        {
            if (doAppearBlankHP) _BeforeHPRatio = Mathf.Clamp(_BeforeHPRatio - _Tl.deltaTime, hpRatio, hpMaximum);
        }
        else
        {
            _BeforeHPRatio = hpRatio;
        }

        //余白部分を設定
        _HpMeterBlankImg.fillAmount = _BeforeHPRatio;
    }
}

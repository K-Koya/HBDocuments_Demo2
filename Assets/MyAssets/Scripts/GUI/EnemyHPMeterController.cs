using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Chronos;

public class EnemyHPMeterController : MonoBehaviour {

    /// <summary>対象キャラクターのメインステータス</summary>
    CharacterParameter.MainParameter _Param = null;


    /// <summary>時間軸コンポーネント</summary>
    Timeline _Tl = null;

    [SerializeField, Tooltip("プレイヤーHPの実数表示Imageコンポーネント")]
    Image _HPCurrentMeterImg = default;
    
    [SerializeField, Tooltip("プレイヤーHPの余白表示Imageコンポーネント")]
    Image _HPBlankMeterImg = default;


    /// <summary>
    /// HPの余白表示のためのHP値保管
    /// </summary>
    float beforeHPRatio = 0.0f;


    // Use this for initialization
    void Start () 
    {
        _Param = GetComponentInParent<CharacterParameter>().Main;
        _Tl = GetComponentInParent<Timeline>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        SyncHPGauge();
    }

    /// <summary>
    /// HPゲージを敵のHPとして反映させる
    /// </summary>
    private void SyncHPGauge()
    {
        short hPMaximum = _Param.HPMaximum;
        short hPCurrent = _Param.HPCurrent;

        //HPの割合値を計算
        float hpRatio = hPCurrent / (float)hPMaximum;

        //HP実数値のゲージを設定
        _HPCurrentMeterImg.fillAmount = hpRatio;

        //HPに応じていい感じに 青→緑→黄→赤→赤黒 に変化させていくための演算
        float hue = (4.0f * hpRatio - 1.0f) / 6.0f;
        float val = 0.9f;
        if (hue < 0.0f)
        {
            val += hue;
            hue = 0.0f;
        }
        _HPCurrentMeterImg.color = Color.HSVToRGB(hue, 1.0f, val);

        //HPの余白表示が表示されている状態で、余白部分を減らすフラグが立っていれば減少処理
        if (beforeHPRatio > hpRatio)
        {
            beforeHPRatio = Mathf.Clamp(beforeHPRatio - (0.5f * _Tl.deltaTime), hpRatio, hPMaximum);
        }
        else
        {
            beforeHPRatio = hpRatio;
        }

        //余白部分を設定
        _HPBlankMeterImg.fillAmount = beforeHPRatio;
    }
}

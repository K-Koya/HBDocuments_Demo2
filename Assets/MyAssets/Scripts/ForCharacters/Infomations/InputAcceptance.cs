using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>操作可否状態を管理するクラス</summary>
[System.Serializable]
public class InputAcceptance
{
    /// <summary>true : 入力移動を受け付ける</summary>
    public bool _CanMove = true;

    /// <summary>true : 入力でジャンプを受け付ける</summary>
    public bool _CanJump = true;

    /// <summary>true : シフトスライド（短距離回避）を受け付ける</summary>
    public bool _CanShiftSlide = true;

    /// <summary>true : ロングトリップ（長距離回避）を受け付ける</summary>
    public bool _CanLongTrip = true;

    /// <summary>true : ガードを受け付ける</summary>
    public bool _CanGurad = true;

    /// <summary>true : 通常コンボを受け付ける</summary>
    public bool _CanComboNormal = true;

    /// <summary>true : コンボフィニッシュを受け付ける</summary>
    public bool _CanComboFinish = false;
}

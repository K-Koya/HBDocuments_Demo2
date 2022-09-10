/// <summary>操作可否状態を管理するクラス</summary>
[System.Serializable]
public class InputAcceptance
{
    /// <summary>true : 入力移動を受け付ける</summary>
    public bool _Move = true;

    /// <summary>true : 入力でジャンプを受け付ける</summary>
    public bool _Jump = true;

    /// <summary>true : シフトスライド（短距離回避）を受け付ける</summary>
    public bool _ShiftSlide = true;

    /// <summary>true : ロングトリップ（長距離回避）を受け付ける</summary>
    public bool _LongTrip = true;

    /// <summary>true : ガードを受け付ける</summary>
    public bool _Gurad = true;

    /// <summary>true : 通常コンボを受け付ける</summary>
    public bool _ComboNormal = true;

    /// <summary>true : コンボフィニッシュを受け付ける</summary>
    public bool _ComboFinish = false;
}

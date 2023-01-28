/// <summary>操作可否状態を管理するクラス</summary>
[System.Serializable]
public class InputAcceptance
{
    /// <summary>true : 入力移動を受け付ける</summary>
    public bool Move = true;

    /// <summary>true : 入力でジャンプを受け付ける</summary>
    public bool Jump = true;

    /// <summary>true : シフトスライド（短距離回避）を受け付ける</summary>
    public bool ShiftSlide = true;

    /// <summary>true : ロングトリップ（長距離回避）を受け付ける</summary>
    public bool LongTrip = true;

    /// <summary>true : ガードを受け付ける</summary>
    public bool Gurad = true;

    /// <summary>true : 通常コンボを受け付ける</summary>
    public bool ComboNormal = true;

    /// <summary>true : コンボフィニッシュを受け付ける</summary>
    public bool ComboFinish = false;

    /// <summary>true : コマンド入力を受け付ける</summary>
    public bool Command = true;
}

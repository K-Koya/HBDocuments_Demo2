using UnityEngine;

/// <summary>プレイヤーもしくはコンピューターの入力状態を返す抽象メソッド集</summary>
public interface IController
{
    /// <summary>移動入力の状態</summary>
    public InputState Move();
    /// <summary>移動入力の方向・大きさ</summary>
    public Vector2 MoveDirection();
    /// <summary>ジャンプ入力の状態</summary>
    public InputState Jump();
    /// <summary>通常攻撃入力の状態</summary>
    public InputState Attack();
    /// <summary>照準コマンド入力の状態</summary>
    public InputState AimCommand();
    /// <summary>短距離回避(シフトスライド)の入力状態</summary>
    public InputState DodgeShort();
    /// <summary>短距離回避(シフトスライド)の入力状態</summary>
    public InputState DodgeLong();
    /// <summary>ガードの入力状態</summary>
    public InputState Guard();
}

/// <summary>該当の入力状況</summary>
public enum InputState : byte
{
    /// <summary>押されていない</summary>
    Untouched = 0,
    /// <summary>押された直後</summary>
    PushDown,
    /// <summary>押している間</summary>
    Pushing,
    /// <summary>離した直後</summary>
    PushUp,
}

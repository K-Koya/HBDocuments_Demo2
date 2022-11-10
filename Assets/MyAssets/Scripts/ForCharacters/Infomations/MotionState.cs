using UnityEngine;

/// <summary>本キャラクターがどのような動作をしているかをまとめるクラス</summary>
[System.Serializable]
public class MotionState
{
    [SerializeField, Tooltip("行動の種類")]
    StateKind _Kind = StateKind.Stay;

    [SerializeField, Tooltip("その行動の段階")]
    ProcessKind _Process = ProcessKind.NotPlaying;

    /// <summary>行動の種類</summary>
    public StateKind Kind { get => _Kind; set => _Kind = value; }
    /// <summary>その行動の段階</summary>
    public ProcessKind Process { get => _Process; set => _Process = value; }

    /// <summary>行動の種類を示す列挙体</summary>
    public enum StateKind : byte
    {
        /// <summary>待機中の状態</summary>
        Stay = 0,
        /// <summary>歩行中の状態</summary>
        Walk,
        /// <summary>走行中の状態</summary>
        Run,
        /// <summary>通常ジャンプ中の状態</summary>
        JumpNoraml,
        /// <summary>通常落下中の状態</summary>
        FallNoraml,
        /// <summary>短距離回避(シフトスライド)中の状態</summary>
        ShiftSlide,
        /// <summary>長距離回避(ロングトリップ)中の状態</summary>
        LongTrip,
        /// <summary>ガード中の状態</summary>
        Guard,
        /// <summary>通常コンボ中の状態</summary>
        ComboNormal,
    }

    /// <summary>各行動について、その動作のどの局面かを示す列挙体</summary>
    public enum ProcessKind : byte
    {
        /// <summary>未実施</summary>
        NotPlaying = 0,
        /// <summary>本動作前の予備動作中(他の操作は不能)</summary>
        Preparation,
        /// <summary>本動作中</summary>
        Playing,
        /// <summary>本動作合間の空き時間(他の操作は一部受け付ける)</summary>
        Interval,
        /// <summary>本動作終了直前</summary>
        EndSoon
    }
}

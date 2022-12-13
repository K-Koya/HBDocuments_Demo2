using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterParameter), typeof(ComputerMove))]
public class ComputerAIBase : MonoBehaviour
{
    #region メンバ
    /// <summary>自分の持ち場の基準になる位置</summary>
    protected Vector3 _BasePosition = default;

    /// <summary>キャラクターの持つ情報</summary>
    protected CharacterParameter _Param = null;

    /// <summary>注目キャラクターの持つ情報</summary>
    protected CharacterParameter _TargetParam = null;

    /// <summary>キャラクターを移動させる機能を集約したコンポーネント</summary>
    protected ComputerMove _Move = null;

    /// <summary>true : 右方向に周回する（周回移動時）</summary>
    protected bool _IsSurroundRight = false;

    /// <summary>移動時間</summary>
    protected float _MoveTime = 0f;

    /// <summary>移動時間の制限時間</summary>
    protected float _MoveTimeLimit = 10f;

    /// <summary>行動メソッド</summary>
    protected System.Action Think = null;

    /// <summary>移動メソッド</summary>
    protected System.Action Movement = null;

    /// <summary>行動メソッドの割り込み時に保持するキャッシュ</summary>
    protected System.Action ThinkCash = null;

    /// <summary>行動パターンメソッド</summary>
    protected System.Action Pattern = null;

    #endregion

    /// <summary>ターゲット付近を移動するときの纏わり度合いを保管する構造体</summary>
    [System.Serializable]
    protected struct FollowControl
    {
        [SerializeField, Tooltip("ターゲットに対する引力")]
        float _AttractInfluence;

        [SerializeField, Tooltip("ターゲットに対する斥力")]
        float _RepulsionInfluence;

        public FollowControl(float attract, float repulsion)
        {
            _AttractInfluence = attract;
            _RepulsionInfluence = repulsion;
        }

        /// <summary>自分とターゲットの距離から最終的な引力・斥力を計算</summary>
        /// <param name="sqrDistance">自分とターゲットの距離の2乗</param>
        /// <param name="attractDecay">引力の影響範囲</param>
        /// <param name="repulsionDecay">斥力の影響範囲</param>
        /// <returns>引力(正)or斥力(負)</returns>
        public float FollowInfluence(float sqrDistance, float attractDecay, float repulsionDecay)
        {
            //レナード・ジョーンズ・ポテンシャル・f
            return (_RepulsionInfluence / Mathf.Pow(sqrDistance, repulsionDecay - 1f)) - (_AttractInfluence / Mathf.Pow(sqrDistance, attractDecay - 1f));
        }
    }


    protected virtual void Start()
    {

    }


}

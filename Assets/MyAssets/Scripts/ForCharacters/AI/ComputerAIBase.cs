using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterParameter), typeof(ComputerMove))]
public class ComputerAIBase : MonoBehaviour
{
    #region �����o
    /// <summary>�����̎�����̊�ɂȂ�ʒu</summary>
    protected Vector3 _BasePosition = default;

    /// <summary>�L�����N�^�[�̎����</summary>
    protected CharacterParameter _Param = null;

    /// <summary>���ڃL�����N�^�[�̎����</summary>
    protected CharacterParameter _TargetParam = null;

    /// <summary>�L�����N�^�[���ړ�������@�\���W�񂵂��R���|�[�l���g</summary>
    protected ComputerMove _Move = null;

    /// <summary>true : �E�����Ɏ��񂷂�i����ړ����j</summary>
    protected bool _IsSurroundRight = false;

    /// <summary>�ړ�����</summary>
    protected float _MoveTime = 0f;

    /// <summary>�ړ����Ԃ̐�������</summary>
    protected float _MoveTimeLimit = 10f;

    /// <summary>�s�����\�b�h</summary>
    protected System.Action Think = null;

    /// <summary>�ړ����\�b�h</summary>
    protected System.Action Movement = null;

    /// <summary>�s�����\�b�h�̊��荞�ݎ��ɕێ�����L���b�V��</summary>
    protected System.Action ThinkCash = null;

    /// <summary>�s���p�^�[�����\�b�h</summary>
    protected System.Action Pattern = null;

    #endregion

    /// <summary>�^�[�Q�b�g�t�߂��ړ�����Ƃ��̓Z���x������ۊǂ���\����</summary>
    [System.Serializable]
    protected struct FollowControl
    {
        [SerializeField, Tooltip("�^�[�Q�b�g�ɑ΂������")]
        float _AttractInfluence;

        [SerializeField, Tooltip("�^�[�Q�b�g�ɑ΂���˗�")]
        float _RepulsionInfluence;

        public FollowControl(float attract, float repulsion)
        {
            _AttractInfluence = attract;
            _RepulsionInfluence = repulsion;
        }

        /// <summary>�����ƃ^�[�Q�b�g�̋�������ŏI�I�Ȉ��́E�˗͂��v�Z</summary>
        /// <param name="sqrDistance">�����ƃ^�[�Q�b�g�̋�����2��</param>
        /// <param name="attractDecay">���͂̉e���͈�</param>
        /// <param name="repulsionDecay">�˗͂̉e���͈�</param>
        /// <returns>����(��)or�˗�(��)</returns>
        public float FollowInfluence(float sqrDistance, float attractDecay, float repulsionDecay)
        {
            //���i�[�h�E�W���[���Y�E�|�e���V�����Ef
            return (_RepulsionInfluence / Mathf.Pow(sqrDistance, repulsionDecay - 1f)) - (_AttractInfluence / Mathf.Pow(sqrDistance, attractDecay - 1f));
        }
    }


    protected virtual void Start()
    {

    }


}

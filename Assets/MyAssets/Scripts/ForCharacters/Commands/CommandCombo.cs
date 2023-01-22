using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandCombo : CommandActiveSkillBase
{
    [SerializeField, Tooltip("�R���{�̎萔�i�t�B�j�b�V�����܂ށj")]
    byte _NumberOfStep = 5;

    /// <summary>���̃R���{�̎萔</summary>
    [SerializeField]
    byte _Step = 0;

    
    public CommandCombo()
    {
        _Name = "�ʏ�R���{";
        _Explain = "�ʏ�U���B��~����ړ����ȂǓ��ʂȍs�����N�����Ă��Ȃ��Ƃ��ɁA�ʏ�U���{�^���ŕ����U��B\n�U����ɑ����ĐU�邱�Ƃ��ł��A����񐔐U��Ƌ��߂̍U�����o��B";
    }

    /// <summary>�R���{�U����v�����郁�\�b�h</summary>
    /// <param name="param">�Y���L�����N�^�[�̃p�����[�^</param>
    /// <param name="rb">���W�b�h�{�f�B</param>
    /// <param name="gravityDirection">�d�͕���</param>
    /// <param name="reticleDirection">�Ə�����</param>
    /// <param name="animKind">�v������A�j���[�V�����̎��</param>
    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        //�Ə������Ɍ�������
        rb.transform.forward = Vector3.ProjectOnPlane(reticleDirection, -gravityDirection);

        //�K�萔�R���{��ł������ŕ���
        //�R���{�r��
        if (_Step < _NumberOfStep)
        {
            //���s��Ԃ��ŕ���
            if (param.State.Kind == MotionState.StateKind.Run)
            {
                animKind = AnimationKind.ComboGroundFowardFar;
            }
            else
            { 
                //�L�����N�^�[�̐��ʕ����ƏƏ������̈ʒu�֌W�ŕ���
                //�O���̏ꍇ
                if (Vector3.Dot(param.transform.forward, reticleDirection) > 0f)
                {
                    //�Ə������킹�Ă��鑊�肪���邩�ŕ���
                    //���Ȃ��ꍇ
                    if (param.GazeAt is null)
                    {
                        animKind = AnimationKind.ComboGroundFoward;
                    }
                    //����ꍇ
                    else
                    {
                        Vector3 reticleNorm = Vector3.Normalize(reticleDirection);
                        //�L�����N�^�[�̉��������ƏƏ������̈ʒu�֌W�ŕ���
                        //�����ɋ߂�
                        if (Vector3.Dot(param.transform.up, reticleNorm) > 0.5f)
                        {
                            //�Ə������킹�Ă��鑊��Ƃ̋����ŕ���
                            //�߂��ꍇ
                            if (reticleDirection.sqrMagnitude < param.Sub.ComboProximityRange * param.Sub.ComboProximityRange)
                            {
                                animKind = AnimationKind.ComboGroundFoward;
                            }
                            //�����ꍇ
                            else
                            {
                                animKind = AnimationKind.ComboGroundFowardFar;
                            }
                        }
                        //�����ɋ߂�
                        else
                        {
                            animKind = AnimationKind.ComboAirWide;
                        }
                    }
                }
                //����̏ꍇ
                else
                {
                    animKind = AnimationKind.ComboGroundBack;
                }
            }

            //�萔���Z
            _Step++;
        }
        //�R���{�t�B�j�b�V��
        else
        {
            animKind = AnimationKind.ComboGroundFinish;
            _Step = 1;
        }

        param.State.Kind = MotionState.StateKind.ComboNormal;
        param.State.Process = MotionState.ProcessKind.Preparation;
    }

    /// <summary>�R���{�萔��0�Ƀ��Z�b�g����</summary>
    public void CountReset()
    {
        _Step = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCombo : CommandBase, ICSVDataConverter
{
    /// <summary>���擾�Ώۂ�CSV�t�@�C���p�X</summary>
    const string LOAD_CSV_PATH = "CSV/Command/Combo";

    /// <summary>�R�}���h�̎��</summary>
    static CommandKind _Kind = CommandKind.Attack;

    /// <summary>�U�����e�[�u��</summary>
    static AttackPowerColumn[] _AttackPowerTable = null;

    /// <summary>�R���{�萔</summary>
    byte _Count = 0;

    /// <summary>���̃R���{�̎萔</summary>
    byte _Step = 0;


    public byte Count { get => _Count; set => _Count = value; }
    public override CommandKind Kind => _Kind;
    protected override AttackPowerColumn[] AttackPowerTable => _AttackPowerTable;


    /// <summary>�R���X�g���N�^</summary>
    public CommandCombo()
    {
        _Kind = CommandKind.Combo;
    }

    public override ushort LoadData()
    {
        CSVToMembers(CSVIO.LoadCSV(LOAD_CSV_PATH));
        return 0;
    }

    public override void Initialize(int layer)
    {

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
        Vector3 reticleNorm = Vector3.Normalize(reticleDirection);

        //�K�萔�R���{��ł������ŕ���
        //�R���{�r��
        if (_Step < _Count)
        {
            //���s��Ԃ��ŕ���
            if (param.State.Kind == MotionState.StateKind.Run)
            {
                animKind = AnimationKind.ComboGroundFowardFar;

                //�U�������֑O�i
                rb.AddForce(reticleNorm, ForceMode.Impulse);
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
                        animKind = AnimationKind.ComboGroundWide;

                        //�U�������֑O�i
                        rb.AddForce(reticleNorm, ForceMode.Impulse);
                    }
                    //����ꍇ
                    else
                    {
                        /*TODO
                        //�L�����N�^�[�̉��������ƏƏ������̈ʒu�֌W�ŕ���
                        //�����ɋ߂�
                        if (Vector3.Dot(param.transform.up, reticleNorm) > 0.75f)
                        {
                            //�Ə������킹�Ă��鑊��Ƃ̋����ŕ���
                            //�߂��ꍇ
                            if (reticleDirection.sqrMagnitude < param.Sub.ComboProximityRange * param.Sub.ComboProximityRange)
                            {
                                animKind = AnimationKind.ComboGroundFoward;

                                //�U�������֑O�i
                                rb.AddForce(reticleNorm, ForceMode.Impulse);
                            }
                            //�����ꍇ
                            else
                            {
                                animKind = AnimationKind.ComboGroundFowardFar;

                                //�U�������֑O�i
                                rb.AddForce(reticleNorm * 2f, ForceMode.Impulse);
                            }
                        }
                        //�����ɋ߂�
                        else
                        {
                            animKind = AnimationKind.ComboGroundFoward;

                            //�U�������֑O�i
                            rb.AddForce(reticleNorm, ForceMode.Impulse);
                        }
                        */

                        //�Ə������킹�Ă��鑊��Ƃ̋����ŕ���
                        //�߂��ꍇ
                        if (reticleDirection.sqrMagnitude < param.Sub.ComboProximityRange * param.Sub.ComboProximityRange)
                        {
                            animKind = AnimationKind.ComboGroundFoward;

                            //�U�������֑O�i
                            rb.AddForce(reticleNorm, ForceMode.Impulse);
                        }
                        //�����ꍇ
                        else
                        {
                            animKind = AnimationKind.ComboGroundFowardFar;

                            //�U�������֑O�i
                            rb.AddForce(reticleNorm * 2f, ForceMode.Impulse);
                        }

                        //�U�������֑O�i
                        rb.AddForce(reticleNorm, ForceMode.Impulse);
                    }
                }
                //����̏ꍇ
                else
                {
                    animKind = AnimationKind.ComboGroundWide;

                    //�U�������֑O�i
                    rb.AddForce(reticleNorm, ForceMode.Impulse);
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

            //�U�������֑O�i
            rb.AddForce(reticleNorm * 2f, ForceMode.Impulse);
        }

        param.State.Kind = MotionState.StateKind.ComboNormal;
        param.State.Process = MotionState.ProcessKind.Preparation;
    }

    /// <summary>�R���{�萔��0�Ƀ��Z�b�g����</summary>
    public void CountReset()
    {
        _Step = 1;
    }

    public List<string> MembersToCSV()
    {
        throw new System.NotImplementedException();
    }

    public void CSVToMembers(List<string[]> csv)
    {
        _AttackPowerTable = new AttackPowerColumn[csv.Count - 4];
        for (int i = 5; i < csv.Count; i++)
        {
            _AttackPowerTable[i - 5] = new AttackPowerColumn(short.Parse(csv[i][0]), short.Parse(csv[i][1]), short.Parse(csv[i][2]));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHealSpray : CommandBase, ICSVDataConverter
{
    /// <summary>���擾�Ώۂ�CSV�t�@�C���p�X</summary>
    const string LOAD_CSV_PATH = "CSV/Command/HealSpray";

    /// <summary>�X�v���[�̉��v���n�u�̃p�X</summary>
    const string LOAD_PREF_PATH = "Prefabs/Particles/HealSpraySmoke";

    /// <summary>�X�v���[�̉��̃v�[��</summary>
    GameObjectPool _HealSpraySmokes = null;

    /// <summary>�R�}���hID</summary>
    static ushort _Id = 0;

    /// <summary>�R�}���h��</summary>
    static string _Name = null;

    /// <summary>�R�}���h����</summary>
    static string _Explain = null;

    /// <summary>�R�}���h�̎��</summary>
    static CommandKind _Kind = CommandKind.ItemHeal;

    /// <summary>�񕜗ʃ��[�g</summary>
    static float _HealRatio = 0f;

    /// <summary>�ő及����</summary>
    static byte _MaxInventory = 0;

    /// <summary>���ݏ�����</summary>
    byte _CurrentInventory = 0;



    public override ushort Id => _Id;
    public override string Name => _Name;
    public override string Explain => _Explain;
    public override CommandKind Kind => _Kind;
    public override byte MaxInventory => _MaxInventory;
    public override byte CurrentInventory => _CurrentInventory;



    public CommandHealSpray()
    {
        _Name = "�񕜃X�v���[";
        _Kind = CommandKind.ItemHeal;
    }

    public override ushort LoadData()
    {
        CSVToMembers(CSVIO.LoadCSV(LOAD_CSV_PATH));
        return _Id;
    }

    public override void Initialize(int layer)
    {
        _CurrentInventory = _MaxInventory;
        _HealSpraySmokes = new GameObjectPool(LOAD_PREF_PATH, 2);
    }

    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        if(_CurrentInventory > 0)
        {
            animKind = AnimationKind.UseItemSpray;

            param.State.Kind = MotionState.StateKind.HealCommand;
            param.State.Process = MotionState.ProcessKind.Preparation;

            _CurrentInventory--;
        }
    }

    public override void Running(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        param.GaveHeal(_HealRatio);
    }

    public List<string> MembersToCSV()
    {
        throw new System.NotImplementedException();
    }

    public void CSVToMembers(List<string[]> csv)
    {
        _Id = ushort.Parse(csv[1][0]);
        _Name = csv[1][1];
        _Explain = csv[1][2];
        _CurrentInventory = byte.Parse(csv[1][4]);
        _HealRatio = float.Parse(csv[4][1]);
    }
}

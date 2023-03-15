using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDictionary : Singleton<CommandDictionary>
{
    /// <summary>�A�N�e�B�u�X�L���R�}���h�̕ۊǌ�</summary>
    Dictionary<ushort, CommandBase> _Commands = null;

    /// <summary>�R���{���̕ۊǌ�</summary>
    CommandCombo _Combo = null;

    /// <summary>�A�N�e�B�u�X�L���R�}���h�̕ۊǌ�</summary>
    public IReadOnlyDictionary<ushort, CommandBase> Commands => _Commands;

    /// <summary>�R���{�R�}���h����</summary>
    public CommandCombo CloneCombo => _Combo.Clone() as CommandCombo;

    protected override void Awake()
    {
        //�V�[�����܂����ŗ��p����
        IsDontDestroyOnLoad = true;
        base.Awake();

        _Commands = new Dictionary<ushort, CommandBase>();

        //���R�}���h�f�[�^��new���āAID����N���X�I�u�W�F�N�g��R�Â�����悤�ɂ���
        ushort id;
        CommandBase skillBase = new CommandBase();
        _Commands.Add(0, skillBase);

        CommandFireShot fireShot = new CommandFireShot();
        id = fireShot.LoadData();
        _Commands.Add(id, fireShot);

        CommandFreezeShot freezeShot = new CommandFreezeShot();
        id = freezeShot.LoadData();
        _Commands.Add(id, freezeShot);

        CommandHealSpray healSpray = new CommandHealSpray();
        id = healSpray.LoadData();
        _Commands.Add(id, healSpray);

        CommandJumpTurn jumpTurn = new CommandJumpTurn();
        id = jumpTurn.LoadData();
        _Commands.Add(id, jumpTurn);

        CommandBiteCombo biteCombo = new CommandBiteCombo();
        id = biteCombo.LoadData();
        _Commands.Add(id, biteCombo);

        CommandRoundSlash roundSlash = new CommandRoundSlash();
        id = roundSlash.LoadData();
        _Commands.Add(id, roundSlash);

        _Combo = new CommandCombo();
        _Combo.LoadData();
    }

    void Start()
    {
        
    }

    /// <summary>�R�}���h����</summary>
    /// <param name="id">�R�}���h��ID</param>
    /// <returns>������</returns>
    public CommandBase CloneCommand(ushort id)
    {
        CommandBase command = _Commands[id];
        return command.Clone();
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDictionary : Singleton<CommandDictionary>
{
    /// <summary>アクティブスキルコマンドの保管庫</summary>
    Dictionary<ushort, CommandBase> _Commands = null;

    /// <summary>コンボ情報の保管庫</summary>
    CommandCombo _Combo = null;

    /// <summary>アクティブスキルコマンドの保管庫</summary>
    public IReadOnlyDictionary<ushort, CommandBase> Commands => _Commands;

    /// <summary>コンボコマンド複製</summary>
    public CommandCombo CloneCombo => _Combo.Clone() as CommandCombo;

    protected override void Awake()
    {
        //シーンをまたいで利用する
        IsDontDestroyOnLoad = true;
        base.Awake();

        _Commands = new Dictionary<ushort, CommandBase>();

        //一つ一つコマンドデータをnewして、IDからクラスオブジェクトを紐づけられるようにする
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

    /// <summary>コマンド複製</summary>
    /// <param name="id">コマンドのID</param>
    /// <returns>複製物</returns>
    public CommandBase CloneCommand(ushort id)
    {
        CommandBase command = _Commands[id];
        return command.Clone();
    }
}



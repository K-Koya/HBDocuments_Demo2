using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>照準を合わせて特定条件を満たすと発動するコマンドのベース 調べるコマンド</summary>
public class CommandReactionBase : CommandActiveSkillBase
{
    public CommandReactionBase()
    {
        if (_Name.Length < 1) _Name = "調べる";
        if (_Explain.Length < 1) _Explain = "特に気になるものは無いみたいだ。";
    }


}



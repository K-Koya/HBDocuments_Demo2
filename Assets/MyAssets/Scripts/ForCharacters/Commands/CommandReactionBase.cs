using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>�Ə������킹�ē�������𖞂����Ɣ�������R�}���h�̃x�[�X ���ׂ�R�}���h</summary>
public class CommandReactionBase : CommandActiveSkillBase
{
    public CommandReactionBase()
    {
        if (_Name.Length < 1) _Name = "���ׂ�";
        if (_Explain.Length < 1) _Explain = "���ɋC�ɂȂ���͖̂����݂������B";
    }


}



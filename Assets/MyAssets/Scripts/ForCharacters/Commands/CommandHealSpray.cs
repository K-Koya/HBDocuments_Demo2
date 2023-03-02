using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHealSpray : CommandActiveSkillBase, ICSVDataConverter
{
    /// <summary>情報取得対象のCSVファイルパス</summary>
    const string LOAD_CSV_PATH = "CSV/Command/HealSpray";

    /// <summary>スプレーの煙プレハブのパス</summary>
    const string LOAD_PREF_PATH = "Prefabs/Particles/HealSpraySmoke";

    /// <summary>回復量レート</summary>
    float _HealRatio = 0f; 


    public CommandHealSpray()
    {
        _Name = "回復スプレー";
        _Kind = CommandKind.ItemHeal;
    }

    public override void Initialize(int layer)
    {
        CSVToMembers(CSVIO.LoadCSV(LOAD_CSV_PATH));
    }

    public override void DoRun(CharacterParameter param, Rigidbody rb, Vector3 gravityDirection, Vector3 reticleDirection, ref AnimationKind animKind)
    {
        if(_Count > 0)
        {
            animKind = AnimationKind.UseItemSpray;

            param.State.Kind = MotionState.StateKind.HealCommand;
            param.State.Process = MotionState.ProcessKind.Preparation;

            _Count--;
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
        _Name = csv[1][1];
        _Explain = csv[1][2];
        _Count = byte.Parse(csv[1][4]);
        _HealRatio = float.Parse(csv[4][1]);
    }
}

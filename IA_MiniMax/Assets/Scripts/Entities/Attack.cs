using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : ScriptableObject
{
    public AttackInfo AttackMade;
    public PlayerInfo Source;
    public PlayerInfo Target;

    public override string ToString()
    {
        return $"{Source.Name}=>{Target.Name}: {AttackMade.Name}";
    }
}
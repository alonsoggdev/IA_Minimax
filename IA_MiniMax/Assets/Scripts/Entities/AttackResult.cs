using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackResult : ScriptableObject
{
    public Attack Attack;
    public bool IsHit;
    public int Damage;
    public int Energy;

    public override string ToString()
    {
        return $"{Attack}: Result: {IsHit}, {Damage}, {Energy}";
    }
}

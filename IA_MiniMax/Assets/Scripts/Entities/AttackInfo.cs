using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/AttackInfo")]
public class AttackInfo : ScriptableObject, ISerializationCallbackReceiver
{
    public string Name;
    public float HitChance;
    public int MinDam;
    public int MaxDam;
    public int Energy;

    public AttackVisual Visual;
    
    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        
    }
}


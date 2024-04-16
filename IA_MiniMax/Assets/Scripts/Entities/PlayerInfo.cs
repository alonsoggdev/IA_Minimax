using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Variables/PlayerInfo")]
public class PlayerInfo: ScriptableObject, ISerializationCallbackReceiver
{
    public string Name;
    public int InitialHP;
    public int InitialEnergy;

    public PlayerVisual Visual;

    [NonSerialized]
    public float HP;
    [NonSerialized]
    public float Energy;

    public AttackInfo[] Attacks;

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        HP = InitialHP;
        Energy = InitialEnergy;
    }
}


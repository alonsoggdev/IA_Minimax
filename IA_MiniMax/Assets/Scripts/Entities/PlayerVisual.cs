using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/PlayerVisual")]
public class PlayerVisual : ScriptableObject, ISerializationCallbackReceiver
{
    public Color InitialColor;
    

    [NonSerialized]
    public Color Color;
   
    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        Color = InitialColor;
    }
}
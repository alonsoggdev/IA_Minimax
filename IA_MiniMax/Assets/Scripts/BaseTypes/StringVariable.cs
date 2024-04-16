using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Variables/String")]
public class StringVariable : ScriptableObject, ISerializationCallbackReceiver
{
    public string InitialValue;

    [NonSerialized]
    public string RuntimeValue;

    public void OnAfterDeserialize()
    {
        RuntimeValue = InitialValue;
    }

    public void OnBeforeSerialize() { }
}

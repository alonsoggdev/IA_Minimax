using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Attack Visual")]
public class AttackVisual : ScriptableObject, ISerializationCallbackReceiver
{
    public AudioClip Audio;
    public ParticleSystem Particles;

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {

    }
}

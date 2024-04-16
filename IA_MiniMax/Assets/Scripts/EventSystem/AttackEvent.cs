using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Attack")]
public class AttackEvent : ScriptableObject
{
    private List<AttackEventListener> listeners =
        new List<AttackEventListener>();

    public void Raise(Attack player)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(player);
    }

    public void RegisterListener(AttackEventListener listener)
    { listeners.Add(listener); }

    public void UnregisterListener(AttackEventListener listener)
    { listeners.Remove(listener); }


}
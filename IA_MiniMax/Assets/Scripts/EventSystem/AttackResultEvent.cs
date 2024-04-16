using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Attack Result")]
public class AttackResultEvent : ScriptableObject
{
    private List<AttackResultEventListener> listeners =
        new List<AttackResultEventListener>();

    public void Raise(AttackResult res)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(res);
    }

    public void RegisterListener(AttackResultEventListener listener)
    { listeners.Add(listener); }

    public void UnregisterListener(AttackResultEventListener listener)
    { listeners.Remove(listener); }


}
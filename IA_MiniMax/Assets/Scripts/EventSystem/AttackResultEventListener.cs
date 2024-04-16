using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackResultEventListener : MonoBehaviour
{
    public AttackResultEvent Event;
    public AttackResultUnityEvent Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(AttackResult res)
    { Response.Invoke(res); }
}

[Serializable]
public class AttackResultUnityEvent : UnityEvent<AttackResult>
{
}
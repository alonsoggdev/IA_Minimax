using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEventListener : MonoBehaviour
{
    public PlayerEvent Event;
    public PlayerUnityEvent Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(PlayerInfo player)
    { Response.Invoke((PlayerInfo)player); }
}

[Serializable]
public class PlayerUnityEvent : UnityEvent<PlayerInfo>
{
}
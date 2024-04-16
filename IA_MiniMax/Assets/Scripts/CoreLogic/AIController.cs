using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject Body;
    public PlayerInfo Player;

    public GameState GameState;

    private Attack _attackToDo;

    public AttackEvent AttackEvent;

    public void OnGameTurnChange(PlayerInfo currentTurn)
    {
        if (currentTurn != Player) return;
        Perceive();
        Think();
        Act();
        
    }

    private void Perceive()
    {

    }

    private void Think()
    {
        ExpectMiniMax();
    }

    private void ExpectMiniMax()
    {

    }
    private void Act()
    {
        AttackEvent.Raise(_attackToDo);
    }


}

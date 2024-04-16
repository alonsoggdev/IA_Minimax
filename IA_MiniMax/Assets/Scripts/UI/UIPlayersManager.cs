using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayersManager : MonoBehaviour
{
    public ActionButtonManager LeftPlayerPanel;

    public ActionButtonManager RightPlayerPanel;

    public TMP_Text EndGameMessage;

    public GameState GameState;
    public PlayerList PlayerList;

    
    public void OnGameTurnChange(PlayerInfo currentTurn)
    {

        Debug.Log($"Current Player: {currentTurn} : {GameState.CurrentPlayer}");
        if (GameState.CurrentPlayer == LeftPlayerPanel.Player)
        {
            LeftPlayerPanel.gameObject.SetActive(true);

            RightPlayerPanel.gameObject.SetActive(false);
            if (!GameState.LeftPlayerIsHuman)
            {
                DeactivateButtons(LeftPlayerPanel);
            }
        }
        if (GameState.CurrentPlayer == RightPlayerPanel.Player)
        {
            RightPlayerPanel.gameObject.SetActive(true);

            LeftPlayerPanel.gameObject.SetActive(false);
            if (!GameState.RightPlayerIsHuman)
            {
                DeactivateButtons(RightPlayerPanel);
            }
        }
    }

    public void OnEndGame()
    {
        DeactivateButtons(LeftPlayerPanel);
        DeactivateButtons(RightPlayerPanel);
        EndGameMessage.text = "ENDDDDDD!!!!";
    }

    private void DeactivateButtons(ActionButtonManager panel)
    {
        foreach (var b in panel.GetComponentsInChildren<Button>())
        {
            b.interactable = false;
        }
    }

    private void ActivateButtons(ActionButtonManager panel)
    {
        foreach (var b in panel.GetComponentsInChildren<Button>())
        {
            b.interactable = true;
        }
    }
    

}

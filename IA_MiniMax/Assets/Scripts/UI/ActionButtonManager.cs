using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonManager : MonoBehaviour
{
    public Text PlayerLabel;
    public PlayerList PlayerList;
    public PlayerInfo Player;

    public ActionButtonAttack ActionButtonPrefab;
    // Start is called before the first frame update
    void Start()
    {
        PlayerLabel.text = Player.Name;
        foreach (var playerAttack in Player.Attacks)
        {
            var btn = GameObject.Instantiate(ActionButtonPrefab,transform);
            btn.TheAttack = ScriptableObject.CreateInstance<Attack>();
            btn.TheAttack.Source = Player;
            btn.TheAttack.AttackMade = playerAttack;
            btn.TheAttack.Target = PlayerList.Players.Find(p => p != Player);
        

            btn.name = playerAttack.Name;
            btn.GetComponentInChildren<TMP_Text>().text = playerAttack.Name;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

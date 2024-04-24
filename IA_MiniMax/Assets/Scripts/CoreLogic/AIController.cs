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
    
    private double _valueH = 0;
    private double _valueP = 0;
    private double[] _valueAttacks = { 0 };

    public void OnGameTurnChange(PlayerInfo currentTurn)
    {
        if (currentTurn != Player) return;
        Perceive();
        Think();
        Act();        
    }

    private void Perceive()
    {
        // Información que debe saber:
        //Estado inicial:
        //Vida Actual
        //Energia Actual
        //Horizonte?

        _attackToDo = new Attack();

        _valueH = Player.Energy + Player.HP * 20;
        
        foreach (PlayerInfo player in GameState.ListOfPlayers.Players)
        {
            if (player != Player)
            {
                // Debug.Log("Perceive " + player.name);
                _attackToDo.Target = player;
                _valueP = player.Energy + player.HP * 20;
            }
        }

        // Debug.Log("_valueH <" + _valueH + "> _valueP <" + _valueP + ">");

        _valueAttacks = new double[Player.Attacks.Length];

        for (int i = 0; i < _valueAttacks.Length; i++)
        {
            _valueAttacks[i] = ((Player.Attacks[i].MinDam + Player.Attacks[i].MaxDam) / 2) * Player.Attacks[i].HitChance - Player.Attacks[i].Energy;
            // Debug.Log("name of attack " + Player.Attacks[i].name);
            // Debug.Log("Attack value number " + i + ": " + _valueAttacks[i].ToString());
        }
        
    }

    private void Think()
    {
        ExpectMiniMax();
    }

    private void ExpectMiniMax()
    {
        //SIN HORIZONTE
        //Construir el árbol de juego
        // Detectar el nodo en el que GameState.IsFinished = true;

        //CON HORIZONTE
        //Construir un árbol de juego con horizonte h
        //Calcular el valor de cada nodo terminal
        //Elegir el valor mayor
        //Colapsar el arbol desde ese nodo y ejecutar la accion

        //CALCULAR VALOR HEURISTICO
        //Variables: Daño, %impacto, energía, vida oponente, vida propia
        //Prioridades:
            //Mantenerse con vida
            //Matar al enemigo
                //Mantener energia
                //Hacerle daño
                //70-30 de posibilidades de un ataque u otro:
                    //Enemigo con <= 4 vida>
                        //Ataque ligero 70%
                    //Enemigo con >4 de vida
                        //Ataque pesado 70%
        

                
                    
        int randomAttack = Random.Range(0, Player.Attacks.Length - 1);
        _attackToDo.AttackMade = Player.Attacks[randomAttack];
        _attackToDo.Source = Player;        
                            
    }
    private void Act()
    {
        // Debug.Log("IA: " + _attackToDo.ToString());
        AttackEvent.Raise(_attackToDo);
    }


}

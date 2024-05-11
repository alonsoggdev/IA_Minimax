using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor.Experimental.GraphView;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject Body;
    public PlayerInfo Player;
    public PlayerInfo otherPlayer;

    public Attack bestAttack;

    public GameState GameState;

    private Attack _attackToDo;

    public AttackEvent AttackEvent;
    
    private double _valueH = 0;
    private double _valueP = 0;
    private List<double> _valueAttacks;
    private List<double> randomValues;
    private bool isMax = false;
    public int k = 3;

    public Node firstNode = new Node(-Mathf.Infinity, null, null);
    public Node openNode;
    public List<Node> expandNodes;

    public void OnGameTurnChange(PlayerInfo currentTurn)
    {
        if (currentTurn != Player) return;
        Perceive();
        //Think();
        //Act();        
    }

    private void Perceive()
    {
        // Información que debe saber:
        //Estado inicial:
        //Vida Actual
        //Energia Actual
        //Horizonte?
        openNode = firstNode;
        expandNodes = new List<Node>();

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

        _valueAttacks = new List<double>();
        randomValues = new List<double>();

        for (int i = 0; i < Player.Attacks.Length; i++)
        {
            int numAttacks = Player.Attacks[i].MaxDam - Player.Attacks[i].MinDam;
            double totalValueRandom = 0;
            if (numAttacks <= 0)
            {
                double temp_rest_value = - ((double) Player.Attacks[i].Energy / 100);
                _valueAttacks.Add(temp_rest_value);

                totalValueRandom += temp_rest_value;
                randomValues.Add(totalValueRandom);

                Node restNode = new Node(totalValueRandom, Player.Attacks[i], firstNode);
                expandNodes.Add(restNode);

                Debug.Log("name of attack " + Player.Attacks[i].name + " value of the attack miss: " + temp_rest_value.ToString());

                continue;
            }

            for (int j = 0; j <= numAttacks; j++)
            {
                double temp_value = Player.Attacks[i].MinDam + j - ((double) Player.Attacks[i].Energy / 100);
                _valueAttacks.Add(temp_value);

                totalValueRandom += temp_value * (Player.Attacks[i].HitChance / numAttacks);

                Debug.Log("name of attack " + Player.Attacks[i].name + " value of the attack: " + temp_value.ToString());
            }
            double temp_miss_value = - ((double)Player.Attacks[i].Energy / 100);
            _valueAttacks.Add(temp_miss_value);

            totalValueRandom += temp_miss_value * (1 - Player.Attacks[i].HitChance);
            randomValues.Add(totalValueRandom);

            Node calculatedNode = new Node(totalValueRandom, Player.Attacks[i], firstNode);
            expandNodes.Add(calculatedNode);

            Debug.Log("name of attack " + Player.Attacks[i].name + " value of the attack miss: " + temp_miss_value.ToString());

            //_valueAttacks[i] = ((Player.Attacks[i].MinDam + Player.Attacks[i].MaxDam) / 2) * Player.Attacks[i].HitChance - (Player.Attacks[i].Energy / 10);
            // Debug.Log("Attack value number " + i + ": " + _valueAttacks[i].ToString());
        }

        for (int i  = 0; i < expandNodes.Count; i++)
        {
            Debug.Log("Calculo de nodo random. valor: " + expandNodes[i].val + " Name: " + expandNodes[i].attack.Name);
        }

    }

    private void Think()
    {
        ExpectMiniMax();
    }

    public double GetValue(AttackInfo _attack, PlayerInfo thisPlayer, PlayerInfo otherPlayer)
    {
        double dmg = (_attack.MinDam + _attack.MaxDam) / 2;
        double val = (dmg * _attack.HitChance - (otherPlayer.HP - dmg / 10) + ((thisPlayer.Energy - _attack.Energy) / 100));

        //Si tenemos que usar el random
        switch (_attack.Name)
        {
            case "Dummy":

                break;
            case "Hard":
                break;
            case "Soft":
                break;
            case "Rest":
                break;
            default:
                Debug.Log("Wrong attack chosen");
                break;
        }

        return val;

    }

    private Node MinValue(Node node, int k)
    {
        return node;
    }

    private Node MaxValue(Node node, int k)
    {
        if (k < 4 && !GameState.IsFinished)
        {
            //Abrimos cuatro nodos por las cuatro acciones posibles
            for(int i = 0; i < 4; i++)
            {
                double val = GetValue(Player.Attacks[i], Player, otherPlayer);
                expandNodes[i] = new Node(val, Player.Attacks[i], openNode);
            }

            foreach(Node expandedNode in expandNodes)
            {
                
                RandomValue(expandedNode, true, k);
            }
        }

        return node;
    }

    private Node RandomValue(Node node, bool isMax, int k)
    {
        return node;
    }

    private void ExpectMiniMax()
    {

        MaxValue(openNode, k);


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

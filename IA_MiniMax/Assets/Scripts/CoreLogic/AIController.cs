using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
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
    
    private List<double> _valueAttacks;
    private List<double> randomValues;
    private bool isMax = false;
    public int k = 3;

    public Node MaxNode = new Node(-Mathf.Infinity, null, null);
    public Node MinNode = new Node(Mathf.Infinity, null, null);
    public Node openNode;
    public List<Node> expandNodes;

    public void OnGameTurnChange(PlayerInfo currentTurn)
    {
        if (currentTurn != Player) return;
        Perceive();
        Think();
        //Act();        
    }

    private void Perceive()
    {
        // Información que debe saber:
        //Estado inicial:
        //Vida Actual
        //Energia Actual
        //Horizonte?
        openNode = MaxNode;
        expandNodes = new List<Node>();

        _attackToDo = new Attack();
        
        foreach (PlayerInfo player in GameState.ListOfPlayers.Players)
        {
            if (player != Player)
            {
                _attackToDo.Target = player;
            }
        }

        _valueAttacks = new List<double>();
        randomValues = new List<double>();

        /*
            Dummy:
            Change: 0.15
            Min: 1
            Max: 2
            Energy: 10
            
            Soft:
            Chance: 0.55
            Min: 1
            Max: 4
            Energy: 15

            Hard:
            Chance: 0.25
            Min: 3
            Max: 6
            Energy: 25

            Rest:
            Chance: 1
            Min: 0
            Max:0
            Energy: -20

         
         */

        for (int i = 0; i < Player.Attacks.Length; i++)
        {
            AttackInfo attack = Player.Attacks[i];
            int numAttacks = attack.MaxDam - attack.MinDam;
            double totalValueRandom = 0;

            // Calculate expected value for non-rest attacks
            if (numAttacks > 0)
            {
                double expectedDamage = (attack.MinDam + attack.MaxDam) / 2.0;  // Average damage
                double temp_value = expectedDamage - _attackToDo.Target.HP - (Player.Attacks[i].Energy * 10);
                totalValueRandom = temp_value * attack.HitChance;  // Only consider hit chance
            }
            else
            {
                // Rest Attack - Prioritize Rest when no damage range (fixed damage)
                double temp_rest_value = -_attackToDo.Target.HP;  // Rest prioritizes health gain
                totalValueRandom = temp_rest_value;
            }

            Node calculatedNode = new Node(totalValueRandom, attack, MaxNode);
            expandNodes.Add(calculatedNode);
        }


        //for (int i = 0; i < Player.Attacks.Length; i++)
        //{
        //    int numAttacks = Player.Attacks[i].MaxDam - Player.Attacks[i].MinDam;
        //    double totalValueRandom = 0;
        //    if (numAttacks <= 0)
        //    {
        //        double temp_rest_value = - _attackToDo.Target.HP  + (double) (Player.Attacks[i].Energy - Player.Energy) / 10;

        //        totalValueRandom += temp_rest_value;

        //        Node restNode = new Node(totalValueRandom, Player.Attacks[i], firstNode);
        //        expandNodes.Add(restNode);

        //        continue;
        //    }

        //    for (int j = 0; j <= numAttacks; j++)
        //    {
        //        double temp_value = (- _attackToDo.Target.HP + Player.Attacks[i].MinDam + j) + (double) (Player.Attacks[i].Energy - Player.Energy) / 10;

        //        totalValueRandom += temp_value * (Player.Attacks[i].HitChance / (numAttacks + 1));
        //    }
        //    double temp_miss_value = ( - _attackToDo.Target.HP)  + (double) (Player.Attacks[i].Energy - Player.Energy) / 10;

        //    totalValueRandom += temp_miss_value * (1 - Player.Attacks[i].HitChance);

        //    Node calculatedNode = new Node(totalValueRandom, Player.Attacks[i], firstNode);
        //    expandNodes.Add(calculatedNode);
        //}

        List<Node> orderNodes = expandNodes.OrderBy(node => node.val).ToList();

        for (int i  = 0; i < orderNodes.Count; i++)
        {
            Debug.Log("Calculo de nodo random. valor: " + orderNodes[i].val + " Name: " + orderNodes[i].attack.Name);
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
        if (k < 4 && !GameState.IsFinished)
        {
            if (node.val < MinNode.val)
                MinNode = node;

            for (int i = 0; i < Player.Attacks.Length; i++)
            {
                double val = GetValue(Player.Attacks[i], Player, otherPlayer);
                expandNodes[i] = new Node(val, Player.Attacks[i], openNode);
                openNode = expandNodes[i];
            }

            foreach (Node expandedNode in expandNodes)
            {
                RandomValue(expandedNode, true, k);
            }

        }
        return node;
    }

    private Node MaxValue(Node node, int k)
    {
        if (k < 4 && !GameState.IsFinished)
        {
            if (node.val > MaxNode.val)
                MaxNode = node;

            //Abrimos cuatro nodos por las cuatro acciones posibles
            for(int i = 0; i < Player.Attacks.Length; i++)
            {
                double val = GetValue(Player.Attacks[i], Player, otherPlayer);
                expandNodes[i] = new Node(val, Player.Attacks[i], openNode);
                openNode = expandNodes[i];
            }

            foreach(Node expandedNode in expandNodes)
            {
                RandomValue(expandedNode, true, k);
            }
        }

        return node;
    }

    private void RandomValue(Node node, bool isMax, int k)
    {
        bool hits = false;
        if(k < 4 && !GameState.IsFinished)
        {

            if(node.attack.HitChance >= Dice.PercentageChance())
            {
                int valDmg = Dice.RangeRoll(node.attack.MinDam, node.attack.MaxDam);

                for(int i = node.attack.MinDam; i< node.attack.MaxDam; i++)
                {
                    if(i == valDmg)
                    {
                        double val = GetValue(node.attack, Player, otherPlayer);
                        expandNodes.Add(new Node(val, node.attack, node));
                        openNode = expandNodes.Last();
                        if (isMax == true)
                        {
                            MinValue(expandNodes.Last(), k);
                        }
                        else
                            MaxValue(expandNodes.Last(), k);
                    }
                }

            }
            else
            {
                double val = GetValue(node.attack, Player, otherPlayer);
                expandNodes.Add(new Node(val, node.attack, node));
                openNode = expandNodes.Last();
                if (isMax == true)
                {
                    MinValue(expandNodes.Last(), k);
                }
                else
                    MaxValue(expandNodes.Last(), k);
            }
            
        }

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

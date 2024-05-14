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
    //public int k = 0;
    int test = 0;

    public Node MaxNode = new Node(-Mathf.Infinity, null, null, 0); //Esto esta al reves? Min es menos infinito
    public Node MinNode = new Node(Mathf.Infinity, null, null, 0);
    public Node openNode;
    public List<Node> expandNodes;

    public struct State
    {
        public float lifePlayer;
        public float lifeAI;
        public float energyAI;
        public float energyPlayer;
    }

    State temp_state;

    public void OnGameTurnChange(PlayerInfo currentTurn)
    {
        if (currentTurn != Player) return;
        Perceive();
        Think();
        Act();        
    }

    private void Perceive()
    {
        _attackToDo = new Attack();
        foreach(PlayerInfo player in GameState.ListOfPlayers.Players)
        {
            if (player != Player)
            {
                _attackToDo.Target = player;
                break;
            }
        }

        temp_state = new State
        {
            lifePlayer = _attackToDo.Target.HP,
            lifeAI = Player.HP,
            energyAI = Player.Energy,
            energyPlayer = _attackToDo.Target.Energy,
        };

        openNode = new Node(0, null, null, -1);
    }   

    private void Think()
    {
        ExpectMiniMax();
    }

    private Node MinValue(Node node, int k)
    {
        if (k < 3 && GameState.IsFinished)
        {
            double bestValue = Mathf.Infinity;

            Node currentNode = null;

            for (int i = 0; i < Player.Attacks.Length; i++)
            {
                double valueNode = 0;
                double randomValue = 0;

                if (temp_state.energyAI > Player.Attacks[i].Energy)
                {
                    int numAttacks = Player.Attacks[i].MaxDam - Player.Attacks[i].MinDam;
                    if (numAttacks <= 0)
                    {
                        // Rest
                        valueNode = CalculateValue(temp_state.lifePlayer, temp_state.energyAI, 0, Player.Attacks[i].MinDam, Player.Attacks[i].Energy);
                        randomValue = valueNode;
                        currentNode = new Node(valueNode, Player.Attacks[i], node, k);
                    }
                    else
                    {
                        for (int j = 0; j <= numAttacks; j++)
                        {
                            valueNode = CalculateValue(temp_state.lifePlayer, temp_state.energyAI, j, Player.Attacks[i].MinDam, Player.Attacks[i].Energy);
                            randomValue += valueNode * (Player.Attacks[i].HitChance / (numAttacks + 1));
                        }
                        valueNode = CalculateValue(temp_state.lifePlayer, temp_state.energyAI, 0, 0, Player.Attacks[i].Energy);
                        randomValue += valueNode * (1 - Player.Attacks[i].HitChance);
                        currentNode = new Node(randomValue, Player.Attacks[i], node, k);
                    }
                }
                else
                {
                    valueNode = Mathf.Infinity;
                    randomValue = Mathf.Infinity;
                }

                if (randomValue < bestValue)
                {
                    bestValue = randomValue;
                    MinNode = currentNode;
                }
            }

            if (RandomValue(MinNode, false, k))
            {
                // ALGO
            }
            else
            {
                // OTRA COSA
            }
            //currentNode = RandomValue(MinNode, true, k);

            return MaxValue(MinNode, k + 1);

        }
        return node;
    }

    private Node MaxValue(Node node, int k)
    {
        //Debug.Log(GameState.IsFinished);
        if (k < 3 && GameState.IsFinished)
        {
            double bestValue = - Mathf.Infinity;

            Node currentNode = null;

            for (int i = 0; i < Player.Attacks.Length; i++)
            {
                double valueNode = 0;
                double randomValue = 0;

                if (temp_state.energyPlayer > Player.Attacks[i].Energy)
                {
                    int numAttacks = Player.Attacks[i].MaxDam - Player.Attacks[i].MinDam;
                    if (numAttacks <= 0)
                    {
                        // Rest
                        valueNode = CalculateValue(temp_state.lifePlayer, temp_state.energyAI, 0, Player.Attacks[i].MinDam, Player.Attacks[i].Energy);
                        randomValue = valueNode;
                        currentNode = new Node(valueNode, Player.Attacks[i], node, k + 1);
                    }
                    else
                    {
                        for (int j = 0; j <= numAttacks; j++)
                        {
                            valueNode = CalculateValue(temp_state.lifePlayer, temp_state.energyAI, j, Player.Attacks[i].MinDam, Player.Attacks[i].Energy);
                            randomValue += valueNode * (Player.Attacks[i].HitChance / (numAttacks + 1));
                        }
                        valueNode = CalculateValue(temp_state.lifePlayer, temp_state.energyAI, 0, 0, Player.Attacks[i].Energy);
                        randomValue += valueNode * (1 - Player.Attacks[i].HitChance);
                        currentNode = new Node(randomValue, Player.Attacks[i], node, k + 1);
                    }
                }
                else
                {
                    valueNode = - Mathf.Infinity;
                    randomValue = - Mathf.Infinity;
                }

                if (randomValue > bestValue)
                {
                    bestValue = randomValue;
                    MaxNode = currentNode;
                }
            }

            if (RandomValue(MaxNode, true, k))
            {
                // ALGO
            }
            else
            {
                // OTRA COSA
            }
            //currentNode = RandomValue(MinNode, true, k);

            return MinValue(MaxNode, k);
        }
        test++;
        return node;
    }

    private bool RandomValue(Node node, bool isMax, int k)
    {
        float roll = Dice.PercentageChance();
        if (node.attack.HitChance >= roll)
        {
            int numAttacks = node.attack.MaxDam - node.attack.MinDam;
            if (numAttacks <= 0)
            {
                if (isMax)
                {
                    temp_state.energyPlayer -= node.attack.Energy;
                }
                else
                {
                    temp_state.energyAI -= node.attack.Energy;
                }
            }
            else
            {
                for(int i = 0; i <= numAttacks; i++)
                {
                    double percentage = 0;
                    percentage += (node.attack.HitChance) / (numAttacks + 1);
                    if (percentage >= roll)
                    {
                        if (isMax)
                        {
                            temp_state.lifeAI -= node.attack.MinDam + i;
                        }
                        else
                        {
                            temp_state.lifePlayer -= node.attack.MinDam + i;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }

    private void ExpectMiniMax()
    {

        Node minNode = MinValue(openNode, 0);

        Debug.Log("MaxNode " + MaxNode.ToString() + " K: " + MaxNode.k);
        Debug.Log("MinNode value " + MaxNode.val);
        Debug.Log("temp_state " + temp_state.lifePlayer);
        Debug.Log("minNode" + minNode.ToString() + " K: " + minNode.k);

        Node parent = minNode.parentNode;
        while (parent.k > 0)
        {
            Debug.Log("Nodes race " + parent.ToString() + " k: " + parent.k);
            parent = parent.parentNode;
        }

        _attackToDo.AttackMade = parent.attack;
        _attackToDo.Source = Player;
        //for(int i = minNode.k; i >= 0; i--)
        //{
        //    if (parent == null)
        //    {
        //        parent = minNode.parentNode;
        //    }
        //    else
        //    {
        //        parent = parent.parentNode;
        //    }

        //    Debug.Log("Nodes race " + parent.ToString() + " k: " + parent.k);
        //}


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




        //int randomAttack = Random.Range(0, Player.Attacks.Length - 1);
        //_attackToDo.AttackMade = Player.Attacks[randomAttack];
        //_attackToDo.Source = Player;        
                            
    }

    private double CalculateValue(float _life, float _energy, float iteration, float _attack, float _energyCost)
    {
        double response = (_life - _attack + iteration) + (double) (_energy - _energyCost) / 10;

        return response;
    }

    private void Act()
    {
        Debug.Log("IA: " + _attackToDo.ToString());
        AttackEvent.Raise(_attackToDo);
    }


}

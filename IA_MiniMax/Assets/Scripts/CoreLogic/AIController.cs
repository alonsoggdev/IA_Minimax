using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class AIController : MonoBehaviour
{

    int attackRatio = 1;

    public GameObject Body;
    public PlayerInfo Player;
    public PlayerInfo otherPlayer;

    public Attack bestAttack;

    public GameState GameState;

    private Attack _attackToDo;

    public AttackEvent AttackEvent;



    public Node MaxNode = new Node(-Mathf.Infinity, null, null, 0);
    public Node MinNode = new Node(Mathf.Infinity, null, null, 0);
    public Node openNode;

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
        if (k < 4 && GameState.IsFinished)
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
                    randomValue = Mathf.Infinity;
                }

                if (randomValue < bestValue)
                {
                    bestValue = randomValue;
                    MinNode = currentNode;
                }
            }

            RandomValue(MinNode, false);

            return MaxValue(MinNode, k + 1);

        }
        return node;
    }

    private Node MaxValue(Node node, int k)
    {
        if (k < 4 && GameState.IsFinished)
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
                    randomValue = - Mathf.Infinity;
                }

                if (randomValue > bestValue)
                {
                    bestValue = randomValue;
                    MaxNode = currentNode;
                }
            }

            RandomValue(MaxNode, true);

            return MinValue(MaxNode, k);
        }
        return node;
    }

    private bool RandomValue(Node node, bool isMax)
    {
        if (isMax)
        {
            temp_state.energyPlayer -= node.attack.Energy;
        }
        else
        {
            temp_state.energyAI -= node.attack.Energy;
        }

        float roll = Dice.PercentageChance();
        if (node.attack.HitChance >= roll)
        {
            int numAttacks = node.attack.MaxDam - node.attack.MinDam;
            if (numAttacks > 0)
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

        Node parent = minNode.parentNode;
        while (parent.k > 0)
        {
            parent = parent.parentNode;
        }

        _attackToDo.AttackMade = parent.attack;
        _attackToDo.Source = Player;                     
    }

    private double CalculateValue(float _life, float _energy, float iteration, float _attack, float _energyCost)
    {
        double response = (_life - _attack - iteration) * attackRatio + (double) (_energy - _energyCost) / 10;

        return response;
    }

    private void Act()
    {
        Debug.Log("IA: " + _attackToDo.ToString());
        AttackEvent.Raise(_attackToDo);
    }


}

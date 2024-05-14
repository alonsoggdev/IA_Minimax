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

    public Node MaxNode = new Node(-Mathf.Infinity, null, null); //Esto esta al reves? Min es menos infinito
    public Node MinNode = new Node(Mathf.Infinity, null, null);
    public Node openNode;
    public List<Node> expandNodes;

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
        Debug.Log("IA: " + _attackToDo.ToString());
        AttackEvent.Raise(_attackToDo);
    }


}

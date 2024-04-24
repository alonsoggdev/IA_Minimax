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
        // Información que debe saber:
            //Estado inicial:
                //Vida Actual
                //Energia Actual
            //Horizonte?
        
        
        
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
                    
                            
    }
    private void Act()
    {
        AttackEvent.Raise(_attackToDo);
    }


}

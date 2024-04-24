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
            //Horizonte
            //Fragmentar los estados en estados más amplios
                //

        //CALCULAR VALOR HEURISTICO
        //Variables: Daño, %impacto, energía, vida oponente, vida propia
        //Prioridades:
            //Mantenerse con vida
            //Matar al enemigo
                //Mantener energia
                //Hacerle daño
                

        //? FORMULA HEURISTICA
        //* Vida = Vida * 20
        //* Energía = Energía

        
        //? if energía < 25
            //? REST
        /* if h > 250 {

        }
        
        */
        
        
    }

    private void Think()
    {
        ExpectMiniMax();
    }

    private void ExpectMiniMax()
    {
        //CON HORIZONTE
        //Construir un árbol de juego con horizonte h
        //Calcular el valor de cada nodo terminal
        //Elegir el valor mayor
        //Colapsar el arbol desde ese nodo y ejecutar la accion

       
                    
                            
    }
    private void Act()
    {
        AttackEvent.Raise(_attackToDo);
    }


}

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
        // La vida vale el doble en cuanto al valor heuristico
        //* Vida = Vida * 20
        //* Energía = Energía

        
        /*
            int bestH = 0;
            Node bestNode = null;
            int closerH = Math.infinite;
            Node closerNode = null;

            for  (int i = 0; i < nodeList.Count; i++){
                
                int thisH = this.h - nodeList[i].energy;
                int otherH = other.h - nodeList[i].health * 20;

                int calcH = thisH - otherH;
                if (calcH > bestH){
                    bestH = calcH;
                    bestNode = nodeList[i];
                }
                if (calcH < Math.infinite) {
                    closerH = calcH;
                    closerNode = nodeList[i];
                }
            }

            if (bestNode != null){
                attackChosen = bestNode;
            } else attackChosen = closerNode;
        
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

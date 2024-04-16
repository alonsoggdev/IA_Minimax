using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtonAttack : MonoBehaviour
{

    public Attack TheAttack;
    public AttackEvent TheAttackEvent;
    public void OnClick()
    {
        TheAttackEvent.Raise(TheAttack);
    }
}

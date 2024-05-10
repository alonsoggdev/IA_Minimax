using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class Node
{
    public double val;
    public AttackInfo attack;
    public Node parentNode; 

    public Node(double _val, AttackInfo _attack, Node _node)
    {
        this.val = _val;
        this.attack = _attack;
        this.parentNode = _node;
    }

}
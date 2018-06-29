using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNumber : MonoBehaviour {
    //Player,Mob共通
    [SerializeField, Header("P番号")]
    protected int playerNum;//0は一般人
    [SerializeField,Range(0,1), Header("0:男  1:女")]
    protected int seibetu;

    public int PlayerNum
    {
        get { return playerNum; }
        set { playerNum = value; }
    }
    public int Seibetu
    {
        get { return seibetu; }
        set { seibetu = value; }
    }
}

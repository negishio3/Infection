using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNumber : MonoBehaviour {
    //Player,Mob共通
    [SerializeField, Header("P番号")]
    protected int playerNum;//0は無所属
    public int PlayerNum
    {
        get { return playerNum; }
        set { playerNum = value; }
    }
}

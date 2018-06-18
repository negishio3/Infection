using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOfAccel : ItemBase {

    public override void Execution(GameObject obj)
    {
        int i=obj.GetComponent<PlayerNumber>().PlayerNum;
        if (EntrySystem.entryFlg[i-1])
        {
            StartCoroutine(obj.GetComponent<PlayerMove>().SpeedUp());
        }
        else
        {
            StartCoroutine(obj.GetComponent<AIPlayer>().SpeedUp());
        }
    }
}

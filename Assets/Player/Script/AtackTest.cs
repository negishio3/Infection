using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackTest : MonoBehaviour {
    private int parNum;

    public int ParNum
    {
        get { return parNum; }
        set { parNum = value; }
    }
	void Start () {
        Destroy(gameObject, 0.5f);
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Mob" && col.GetComponent<PlayerNumber>().PlayerNum == 0)
        {
            MobChangeSystem.MobChanger(col.transform.position, parNum);
            Destroy(col.gameObject);
        }
    }
}

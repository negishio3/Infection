using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bress : MonoBehaviour {

    private int parNum;

    public int ParNum
    {
        get { return parNum; }
        set { parNum = value; }
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnParticleCollision(GameObject obj)
    {
        if ((obj.tag == "Mob" && obj.GetComponent<PlayerNumber>().PlayerNum != parNum))
        {
            Quaternion qua = obj.transform.rotation;
            MobChangeSystem.MobChanger(obj.gameObject, parNum);
            Destroy(gameObject, 0.05f);
        }
    }
}

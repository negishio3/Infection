using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieInstant : MonoBehaviour {
    public GameObject[] instantPos;
    public GameObject zombiePre;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instans();
        }
	}
    void Instans()
    {
        for (int i = 0; i < instantPos.Length; i++) {
            
            Instantiate(zombiePre, instantPos[i].transform.position, Quaternion.identity);
        }
    }
    //public IEnumerator ScoreCount()
    //{

    //}
}

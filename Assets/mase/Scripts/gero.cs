using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gero : MonoBehaviour
{

    //public GameObject[] Gero;
    public GameObject Gero;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            Instantiate(Gero, this.transform.position, Quaternion.identity);
            Debug.Log("気持ち悪い");

        }

    }
}

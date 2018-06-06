using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlashingArrow : MonoBehaviour {

    private float alpha=0;
    private float minus = 0.3f;
    private bool flg=true;
    private MeshRenderer meshrender;
    void Start () {
        meshrender = GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (alpha < 0) { minus *= -1; }
        else if(alpha>1){ minus *= -1; }
        alpha += Time.deltaTime * minus;
        Debug.Log(alpha);
        meshrender.material.color = new Color(1, 1, 1, alpha);
    }
}

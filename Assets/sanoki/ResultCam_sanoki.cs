using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCam_sanoki : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator camMove(Vector3 movePos)
    {
        float time = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x,transform.position.y+movePos.y,transform.position.z);

        while (time < 1.0f)
        {
            time += Time.deltaTime/0.1f;
            transform.position = Vector3.Lerp(startPos, endPos, time);
            yield return null;
        }
    }
}

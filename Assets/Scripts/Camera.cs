using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    Vector3 camera_Rotate;
    [SerializeField]
    float rotation_Speed;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            camera_Rotate.y = -1;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            camera_Rotate.y = 1;
        }
        else if (Input.GetKeyUp(KeyCode.O) || Input.GetKeyUp(KeyCode.P))
        {
            camera_Rotate.y = 0;
        }
        transform.Rotate(camera_Rotate * Time.deltaTime * rotation_Speed);
    }
}

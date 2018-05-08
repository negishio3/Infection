using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    [SerializeField,Header("最大回転角")]
    private float maxAngle = 30;
    [SerializeField, Header("最低回転角")]
    private float minAngle = -30;

    private int playerNumber;
    private GameObject playerObj;
    private float rotspeed;
    private Vector3 roteuler;

    private RaycastHit rayhit;
    private bool isRendered;

    public int PlayerNumber
    {
        get { return playerNumber; }
        set { playerNumber = value; }
    }
    public GameObject PlayerObj
    {
        get { return playerObj; }
        set { playerObj = value; }
    }

    public  float RotSpeed
    {
        get { return rotspeed; }
        set { rotspeed=value; }
    }


	void Start () {
        roteuler = new Vector3(transform.localEulerAngles.x,transform.localEulerAngles.y,0f);
	}

    // Update is called once per frame
    void Update()
    {
        float vertical= Input.GetAxis("VerticalR" + playerNumber.ToString()) *rotspeed;
        roteuler = new Vector3(Mathf.Clamp(roteuler.x - vertical, minAngle, maxAngle),transform.localEulerAngles.y,0f);
        transform.localEulerAngles = roteuler;
    }
}

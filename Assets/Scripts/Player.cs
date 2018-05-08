using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    float MoveSpeed = 4;
    [SerializeField]
    float jumpHight;
    Vector3 playerPos;
    Rigidbody rd;

    void Start()
    {
        rd = GetComponent<Rigidbody>();
    }
	
	void Update ()
 {
        if(MoveSpeed >= 0)MoveSpeed += Input.GetAxis("Mouse ScrollWheel");
        if(MoveSpeed < 0) MoveSpeed = 0;
        if (Input.GetButtonDown("Jump"))
        {
            rd.AddForce(transform.up * jumpHight);
        }
        playerPos.x = Input.GetAxisRaw("Horizontal");
        playerPos.z = Input.GetAxisRaw("Vertical");
        transform.position += playerPos * MoveSpeed * Time.deltaTime;
    }

    public void ColorChange()
    {

    }
}

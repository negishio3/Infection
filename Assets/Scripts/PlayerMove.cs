using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : PlayerNumber
{
    public GameObject atk;
    public GameObject Createpos;
    [SerializeField, Header("速度")]
    private float movespeed;
    [SerializeField, Header("回転速")]
    private float rotspeed;
    //[SerializeField]
    //private GameObject raypos;//rayを飛ばす地点


    private CharacterController cCon;


    private Vector3 vecInput;
    private Vector3 velocity;
    private Vector3 vecRot;
    private Vector3 graVelocity;

    private int layerMask;

    void Start()
    {
        cCon = GetComponent<CharacterController>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire" + playerNum.ToString()))
        {
            GameObject obj;
            obj=(GameObject)Instantiate(atk,Createpos.transform.position,Quaternion.identity);
            obj.GetComponent<AtackTest>().ParNum = playerNum;
        }

        vecRot = new Vector3(0f, Input.GetAxis("HorizontalR"+ playerNum.ToString()), 0f);
        transform.Rotate(vecRot * rotspeed);

        vecInput = new Vector3(Input.GetAxis("HorizontalL" + playerNum.ToString()), 0, Input.GetAxis("VerticalL" + playerNum.ToString()));
        velocity = Quaternion.Euler(0f, transform.localEulerAngles.y, 0f) * vecInput;

        velocity *= movespeed;
        cCon.Move(velocity * Time.deltaTime);

    }

    void FixedUpdate()
    {
        graVelocity.y += Physics.gravity.y * Time.deltaTime;
        cCon.Move(graVelocity * Time.deltaTime);
    }

    //bool OnGroundCheck()//指定レイヤーに接地しているならtrue
    //{
    //    Ray ray;
    //    ray = new Ray(raypos.transform.position, Vector3.down);
    //    if (Physics.Raycast(ray, 0.1f, layerMask))
    //    { return true; }
    //    else
    //    { return false; }
    //}
}

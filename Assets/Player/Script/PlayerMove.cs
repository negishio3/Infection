using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerNumber
{
    [SerializeField, Header("速度")]
    private float movespeed;
    [SerializeField, Header("回転速")]
    private float rotspeed;
    [SerializeField,Header("跳躍力")]
    private float jumpPower;
    [SerializeField]
    private GameObject CameraObj;
    [SerializeField]
    private int terrainLayer;//地面レイヤー番号
    [SerializeField]
    private GameObject raypos;//rayを飛ばす地点

    private CharacterController cCon;
    private CameraMove CM;

    private Vector3 vecInput;
    private Vector3 velocity;
    private Vector3 vecRot;
    private Vector3 graVelocity;
    private Vector3 jumpVec;

    private int layerMask;

    void Start()
    {
        cCon = GetComponent<CharacterController>();
        CM = CameraObj.GetComponent<CameraMove>();
        CM.PlayerNumber = playerNum;
        CM.RotSpeed = rotspeed;
        CM.PlayerObj = gameObject;
        layerMask = 1 << terrainLayer;
    }

    // Update is called once per frame
    void Update()
    {
        vecRot = new Vector3(0f, Input.GetAxis("HorizontalR"+ playerNum.ToString()), 0f);
        transform.Rotate(vecRot * rotspeed);

        vecInput = new Vector3(Input.GetAxis("HorizontalL" + playerNum.ToString()), 0, Input.GetAxis("VerticalL" + playerNum.ToString()));
        velocity = Quaternion.Euler(0f, transform.localEulerAngles.y, 0f) * vecInput;
        if (OnGroundCheck())
        {
            graVelocity.y = 0;
            if (Input.GetButtonDown("Jump" + playerNum.ToString()))
            {
                jumpVec.y = jumpPower;
            }
        }
        else
        {
            jumpVec.y -= 0.8f;
        }
        velocity *= movespeed;
        velocity.y = jumpVec.y;
        cCon.Move(velocity * Time.deltaTime);

    }

    void FixedUpdate()
    {
        graVelocity.y += Physics.gravity.y * Time.deltaTime;
        cCon.Move(graVelocity * Time.deltaTime);
    }

    bool OnGroundCheck()//指定レイヤーに接地しているならtrue
    {
        Ray ray;
        ray = new Ray(raypos.transform.position, Vector3.down);
        if (Physics.Raycast(ray, 0.1f, layerMask))
        { return true; }
        else
        { return false; }
    }
}

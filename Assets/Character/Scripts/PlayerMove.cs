using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : PlayerNumber
{
    public GameObject atk;
    public GameObject createpos;
    [SerializeField, Header("速度")]
    private float movespeed;
    //[SerializeField, Header("回転速")]
    //private float rotspeed;
    //[SerializeField]
    //private GameObject Yajirushi;


    private CharacterController cCon;
    private Animator anim;
    private AnimatorStateInfo stateInfo;

    private Vector3 vecInput;
    private bool atkFlg = true;
    //private Vector3 velocity;
    //private Vector3 vecRot;
    //private Vector3 graVelocity;

    //private int layerMask;

    void Start()
    {
        cCon = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (stateInfo.normalizedTime < 0.1) { anim.SetBool("Bress", false); }

        if (Input.GetButtonDown("Fire" + playerNum.ToString())&&atkFlg)
        {
            anim.SetBool("Bress", true);
            StartCoroutine(AtkCor());
        }


        //vecRot = new Vector3(0f, Input.GetAxis("HorizontalR"+ playerNum.ToString()), 0f);
        //transform.Rotate(vecRot * rotspeed);

        //vecInput = new Vector3(Input.GetAxis("HorizontalL" + playerNum.ToString()), 0, Input.GetAxis("VerticalL" + playerNum.ToString()));
        //velocity = Quaternion.Euler(0f, transform.localEulerAngles.y, 0f) * vecInput;

        //velocity *= movespeed;
        //cCon.Move(velocity * Time.deltaTime);

        vecInput = new Vector3(Input.GetAxis("HorizontalL" + playerNum.ToString()), 0, Input.GetAxis("VerticalL" + playerNum.ToString()));
        vecInput *= movespeed;
        cCon.Move(vecInput * Time.deltaTime);
        if (Input.GetAxis("HorizontalL" + playerNum.ToString())!=0 || Input.GetAxis("VerticalL" + playerNum.ToString())!=0)
        {
            Debug.Log("a");
            transform.rotation = Quaternion.LookRotation(transform.position +
            (Vector3.right * Input.GetAxisRaw("HorizontalL" + playerNum.ToString())) +
            (Vector3.forward * Input.GetAxisRaw("VerticalL" + playerNum.ToString()))
            - transform.position);
        }

        //void FixedUpdate()
        //{
        //    graVelocity.y += Physics.gravity.y * Time.deltaTime;
        //    cCon.Move(graVelocity * Time.deltaTime);
        //}
    }
    IEnumerator AtkCor()
    {
        atkFlg = false;
        yield return new WaitForSeconds(0.6f);
        GameObject obj;
        obj = (GameObject)Instantiate(atk, createpos.transform.position, Quaternion.identity);
        obj.GetComponent<AtackTest>().ParNum = playerNum;
        obj.transform.parent = gameObject.transform;
        yield return new WaitForSeconds(1.8f);
        atkFlg = true;
    }
}
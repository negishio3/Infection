using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.AI;

public class MobChangeSystem : MonoBehaviour
{
    public Text[] tx =new Text[5];
    [SerializeField]
    private GameObject[] objs;//モブのprefab
    [SerializeField]
    private Material[] materials = new Material[4];//色変え用
    private static Material[] mat = new Material[4];//色変え用
    private static GameObject[] mobZombies=new GameObject[5];//モブのprefab
    public static int[] scoreCount=new int[4];
    private int[] NowZombiNum=new int[5];

    //0は市民,1～4がゾンビ
    
    
    
    void Start()
    {
        for(int i = 0; i < objs.Length; i++)
        {
            mobZombies[i] = objs[i];
        }
        for (int i = 0; i < materials.Length; i++)
        {
            mat[i] = materials[i];
        }
    }

    void Update()
    {
        if (tx.Length > 4)
        {
            for (int i = 0; i < 5; i++)
            {
                NowZombiNum[i] = MobCount(i);
                tx[i].text = NowZombiNum[i].ToString();
            }
        }
    }

    public static int MobCount(int num)//mobオブジェクトの番号がnumと一致するオブジェクトの数をcountに渡すはず
    {
        GameObject[] mobs;
        mobs = GameObject.FindGameObjectsWithTag("Mob").
        Where(e => e.GetComponent<PlayerNumber>().PlayerNum == num).
        ToArray();
        return mobs.Length;
    }

    public static void MobChanger(GameObject obj, int num)
    {

        //Instantiate(pars[num - 1], new Vector3(obj.transform.position.x,0.7f,obj.transform.position.z),Quaternion.Euler(-90,0,0));
        if (obj.GetComponent<PlayerNumber>().PlayerNum == 0)
        {
            if (obj.GetComponent<HumanMove>().Smoke == false)
            {
                GameObject zombi;
                zombi = (GameObject)Instantiate(mobZombies[num], obj.transform.position, obj.transform.rotation);
                zombi.GetComponent<NavMeshAgent>().enabled = true;
                Destroy(obj);
            }
            else
            {
                return;
            }
        }
        else
        {
            SkinnedMeshRenderer s = obj.GetComponentInChildren<SkinnedMeshRenderer>();
            s.material = mat[num - 1];
            obj.GetComponent<PlayerNumber>().PlayerNum = num;
        }
    }

    public static void HumanSpawn(Vector3 pos,Quaternion qu)
    {
        GameObject obj;
        obj = (GameObject)Instantiate(mobZombies[0], pos, qu);
        obj.GetComponent<NavMeshAgent>().enabled = true;
    }

    public static void MobDelete()
    {
        for(int i = 0; i < 4; i++)
        {
            scoreCount[i] += MobCount(i + 1);
        }
        foreach (GameObject mob in GameObject.FindGameObjectsWithTag("Mob"))
        {
            Destroy(mob);
        }
    }
}


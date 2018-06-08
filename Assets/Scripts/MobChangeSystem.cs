using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.AI;

public class MobChangeSystem : MonoBehaviour
{
    public Text a;
    [SerializeField]
    private GameObject[] objs;
    [SerializeField]
    private Material[] materials = new Material[4];
    private static Material[] mat = new Material[4];
    private static GameObject[] mobZombies=new GameObject[5];
    private static int[] scoreCount=new int[4];
    private static int[] NowZombiNum=new int[5];

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
        for(int i = 0; i < 5; i++)
        {
            NowZombiNum[i]=MobCount(i);
        }
        a.text = "1P:" + NowZombiNum[1]+ 
            "  2P:" + NowZombiNum[2] + 
            "  3P:" + NowZombiNum[3] + 
            "  4P:" + NowZombiNum[4]+
            "市民:"+NowZombiNum[0];
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
        if (obj.GetComponent<PlayerNumber>().PlayerNum == 0)
        {
            GameObject zombi;
            zombi = (GameObject)Instantiate(mobZombies[num], obj.transform.position, obj.transform.rotation);
            zombi.GetComponent<NavMeshAgent>().enabled = true;
            Destroy(obj);
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


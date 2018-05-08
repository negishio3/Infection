using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public abstract class AIBase : PlayerNumber {
    [SerializeField, Header("歩き移動速度")]
    protected float walkSpeed = 1.5f;
    [SerializeField, Header("走り移動速度")]
    protected float runSpeed = 3.7f;
    [SerializeField, Header("obj探索範囲")]
    protected float searchDistance;
    [SerializeField, Header("視野角")]
    protected float viewAngle=0.85f;


    protected NavMeshAgent navMeshAgent;
    protected GameObject[] targetMobs;          //対象候補
    protected GameObject target;                //現在の対象
    protected Vector3 Mypos;                    //自分の現在地
    protected Vector3 nextPos;                  //次の移動地点
    protected int num;                          //対象を選ぶ番号
    protected int loseSightTime = 5;            //見失うまでの時間
    protected float targetDistance;             //ターゲットと次の移動場所の距離
    protected float nextPosDistance;            //次の場所までの距離
    protected float stopTrackingTime;           //0になったら見失う
    protected bool TrackingFlg;                 //見失ったかどうか
    protected string targetTag;                 //対象のタグ



    protected virtual void Start ()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        stopTrackingTime = 0;
    }

	protected virtual void Update ()
    {
        if (stopTrackingTime <= 0)
        {
            target = null;
            TrackingFlg = false;
        }
        else
        {
            stopTrackingTime -= Time.deltaTime;
        }
        InSight();

	}

    protected abstract void OnTriggerEnter(Collider col);

    protected void InSight()//視界に入っているか確認
    {
        SearchObj(targetTag, out targetMobs);
        if (targetMobs.Any())
        {
            num = 0;
            while (targetMobs.Length > num)
            {
                if (RayCheck())
                {
                    target = targetMobs[num];
                    stopTrackingTime = loseSightTime;
                    TrackingFlg = true;
                    return;
                }
                else
                {
                    num++;
                }
            }
        }
    }

    bool RayCheck()//レイが通るならtrue
    {
        if (targetMobs.Any())//対象が存在するか
        {
            Vector3 dir = targetMobs[num].transform.position - transform.position;

            if (ViewingAngle(dir))//視界に入っているか
            {
                Ray ray = new Ray(transform.position, dir);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == targetTag)
                    {
                        Debug.Log("Insight");
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }
        else
        {

        }
        return false;
    }

    bool ViewingAngle(Vector3 dir)//正面にいるならtrue
    {
        dir.Normalize();
        float dot = Vector3.Dot(dir, transform.forward);
        float rad = Mathf.Acos(dot);
        if (rad < viewAngle)//視野角に入っているか
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SearchObj(string tag, out GameObject[] objs)//条件を満たしているobjを近い順に取得する
    {
        if (GameObject.FindGameObjectWithTag(tag))
        {
            objs = GameObject.FindGameObjectsWithTag(tag).
            Where(e => Vector3.Distance(transform.position, e.transform.position) < searchDistance).//範囲内か
            Where(e => e.GetComponent<PlayerNumber>().PlayerNum != playerNum).//陣営が異なるか
            OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).ToArray();//並び替え
        }
        else
        {
            objs = null;
        }
    }

    protected bool GetRandomPosition(Vector3 center, float range, out Vector3 result)
    //ランダムに移動場所を取得
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    protected abstract void MoveRandom(float range);//移動処理
}

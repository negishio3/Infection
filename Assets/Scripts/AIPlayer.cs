using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class AIPlayer : AIBase
{
    public enum MoveState
    {
        WAIT,       //待機
        MOVE,       //移動
        STALKING,   //追跡
        ATACK       //攻撃
    }
    [SerializeField]
    protected MoveState moveState=MoveState.MOVE;

    public GameObject createpos;
    [SerializeField]
    private GameObject atk;
    protected float randomPosRange=30;  //移動場所ランダム範囲
    protected GameObject atackedTarget; //直近の攻撃済みのtarget




    protected override void Start()
    {
        base.Start();
        targetTag = "Mob";
    }

    protected override void Update()
    {
        if (stopTrackingTime <= 0)//一定時間視界に入らなかったら見失う
        {
            target = null;
            TrackingFlg = false;
        }
        else
        {
            stopTrackingTime -= Time.deltaTime;
        }


        base.Update();


        switch (moveState)
        {
            case MoveState.WAIT:
                Wait();
                break;
            case MoveState.MOVE:
                Move();
                break;
            case MoveState.STALKING:
                Stalking();
                break;
            case MoveState.ATACK:
                Atack();
                break;
        }

        if (!recastFlg)//攻撃中なら対象のほうへ向く
        {
            AtackRotation();
        }

        Mypos = transform.position;
    }


    void Wait()
    {
        //待機処理  不要なら削除
        if (recastFlg)
        {
            moveState = MoveState.MOVE;
        }
    }

    void Move()
    {
        if (TrackingFlg)
        {
            moveState = MoveState.STALKING;
            return;
        }
        if (Vector3.Distance(nextPos, transform.position) < 4 || Mypos == nextPos || nextPos == Vector3.zero||Mypos==transform.position)
        {
            MoveRandom(randomPosRange);
        }
    }


    void Stalking()
    {
        if (!TrackingFlg||!target)//見失ったらMOVEに変更
        {
            MoveRandom(randomPosRange);
            moveState = MoveState.MOVE;
        }
        else if (target)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < 2)//接触していたらATACKに変更
            {
                moveState = MoveState.ATACK;
            }
            if (Vector3.Distance(nextPos, transform.position) < 4 || Mypos == nextPos || nextPos == Vector3.zero || Mypos == transform.position)
            {
                if (navMeshAgent.pathStatus != NavMeshPathStatus.PathInvalid)
                {
                    nextPos = target.transform.position;
                    navMeshAgent.SetDestination(nextPos);
                }
            }
        }
    }

    void Atack()
    {
        if (target&&recastFlg)
        {
            //攻撃処理
            GameObject obj;
            obj = (GameObject)Instantiate(atk, createpos.transform.position, Quaternion.identity);
            obj.GetComponent<AtackTest>().ParNum = playerNum;
            recastFlg = false;
            moveState = MoveState.WAIT;
            StartCoroutine(RecastTime(waitMoveTime));
            atackedTarget = target;
            //MobChangeSystem.MobChanger(atackedTarget.transform.position, playerNum);
            //Destroy(atackedTarget);
            target = null;

        }
        else
        {
            moveState = MoveState.MOVE;
        }
    }

    void AtackRotation()//攻撃するとき対象に向く処理
    {
        if (atackedTarget)//対象がいるか確認
        {
            float rotationSpeed = 5;
            Vector3 dir = new Vector3(atackedTarget.transform.position.x, transform.position.y, atackedTarget.transform.position.z) - transform.position;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, dir, rotationSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newdir);

        }
    }


    protected override void MoveRandom(float range)
    {
        if (GetRandomPosition(transform.position, range, out nextPos))
        {
            Vector3 nexdis = nextPos - transform.position;
            if (!ViewingAngle(nexdis,transform.forward, 1.5f))
            {
                MoveRandom(range);
            }
            else
            {
                navMeshAgent.SetDestination(nextPos);
            }
        }
    }

    protected override void SearchObj(string tag, out GameObject[] objs)
    {
        if (GameObject.FindGameObjectWithTag(tag))//tagのオブジェクトの存在確認
        {
            objs = GameObject.FindGameObjectsWithTag(tag).
            Where(e => Vector3.Distance(transform.position, e.transform.position) < searchDistance).//範囲内で
            Where(e => e.GetComponent<PlayerNumber>().PlayerNum != playerNum).                      //番号が異なるなら取得
            OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).ToArray();     //近い順に並び替え
        }
        else
        {
            objs = null;
        }
    }

    protected override void OnTriggerEnter(Collider col)
    {

    }


    void OnDestroy()
    {
        AreaSystem.AIPlayerList.Remove(gameObject.GetComponent<AIPlayer>());
    }
}
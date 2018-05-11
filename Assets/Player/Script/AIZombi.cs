using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombi : AIBase
{
    public enum MoveState
    {
        WAIT,
        MOVE,
        STALKING,
        ATACK
    }
    [SerializeField]
    protected MoveState moveState = MoveState.MOVE;

    protected float randomPosRange = 30;  //移動場所ランダム範囲
    protected GameObject atackedTarget; //直近の攻撃済みのtarget

    protected override void Start()
    {
        base.Start();
        targetTag = "Mob";
    }

    protected override void Update()
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

        if (!recastFlg)
        {
            AtackRotation();
        }

        Mypos = transform.position;
    }


    void Wait()
    {
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
            if (target)
            {
                nextPos = target.transform.position;
                navMeshAgent.SetDestination(nextPos);
            }
            return;
        }
        if (Vector3.Distance(nextPos, transform.position) < 4 || Mypos == nextPos || nextPos == Vector3.zero || Mypos == transform.position)
        {
            MoveRandom(randomPosRange);
        }
    }

    void Stalking()
    {
        if (target)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < 2)//接触していたらATACKに変更
            {
                moveState = MoveState.ATACK;
            }
        }
        else if (!TrackingFlg)//見失ったらMOVEに変更
        {
            MoveRandom(randomPosRange);
            moveState = MoveState.MOVE;
        }
        if (target)
        {
            if (Vector3.Distance(nextPos, transform.position) < 4 || Mypos == nextPos || nextPos == Vector3.zero || Mypos == transform.position)
            {
                nextPos = target.transform.position;
                navMeshAgent.SetDestination(nextPos);
            }
        }
    }

    void Atack()
    {
        if (target && recastFlg)
        {
            //攻撃処理
            MobTest mobTest = target.GetComponent<MobTest>();
            mobTest.GetCaughtFlg = true;
            mobTest.PlayerNum = playerNum;
            recastFlg = false;
            moveState = MoveState.WAIT;
            StartCoroutine(RecastTime(waitMoveTime));
            atackedTarget = target;
            target = null;
            //MobChangeSystem.MobChanger(atackedTarget.transform.position, playerNum);
            //Destroy(atackedTarget);
        }
        else
        {
            moveState = MoveState.MOVE;
        }
    }

    void AtackRotation()
    {
        if (atackedTarget)
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
            if (!ViewingAngle(nexdis, transform.forward, 1.5f))
            {
                MoveRandom(range);
            }
            else
            {
                navMeshAgent.SetDestination(nextPos);
            }
        }
    }

    protected override void OnTriggerEnter(Collider col)
    {
        throw new System.NotImplementedException();
    }

}
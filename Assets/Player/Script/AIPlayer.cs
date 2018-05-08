using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class AIPlayer : AIBase
{
    public enum MoveState
    {
        WAIT,
        MOVE,
        STALKING,
        ATACK
    }
    protected MoveState moveState=MoveState.MOVE;
    protected float randomPosRange=30;  //移動場所ランダム範囲

    protected override void Start()
    {
        base.Start();
        targetTag = "Mob";
    }

    protected override void Update()
    {
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
        Mypos = transform.position;
    }

    void Wait()
    {
        //待機処理  不要なら削除
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
        if (Vector3.Distance(nextPos, transform.position) < 4 || Mypos == nextPos || nextPos == Vector3.zero)
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
            if (Vector3.Distance(nextPos, transform.position) < 4 || Mypos == nextPos || nextPos == Vector3.zero)
            {
                nextPos = target.transform.position;
                navMeshAgent.SetDestination(nextPos);
            }
        }
    }

    void Atack()
    {
        if (target)
        {
            target.GetComponent<MobTest>().GetCaughtFlg=true;
            //攻撃処理
        }
        else
        {
            moveState = MoveState.MOVE;
        }
    }


    protected override void MoveRandom(float range)
    {
        if (GetRandomPosition(transform.position, range, out nextPos))
        {
            navMeshAgent.SetDestination(nextPos);
        }
    }

    protected override void OnTriggerEnter(Collider col)
    {
        throw new System.NotImplementedException();
    }


}
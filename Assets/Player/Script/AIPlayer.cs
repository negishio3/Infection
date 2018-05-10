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
    [SerializeField]
    protected MoveState moveState=MoveState.MOVE;

    [SerializeField]
    private Camera _camera;
    protected float randomPosRange=30;  //移動場所ランダム範囲
    protected GameObject atackedTarget; //直近の攻撃済みのtarget

    protected override void Start()
    {
        base.Start();
        targetTag = "Mob";
        CameraRect();
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
            if (target)
            {
                nextPos = target.transform.position;
                navMeshAgent.SetDestination(nextPos);
            }
            return;
        }
        if (Vector3.Distance(nextPos, transform.position) < 4 || Mypos == nextPos || nextPos == Vector3.zero||Mypos==transform.position)
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
        if (target&&recastFlg)
        {
            MobTest mobTest = target.GetComponent<MobTest>();
            mobTest.GetCaughtFlg=true;
            mobTest.PlayerNum = playerNum;
            recastFlg = false;
            moveState = MoveState.WAIT;
            StartCoroutine(RecastTime(waitMoveTime));
            atackedTarget = target;
            target = null;
            //攻撃処理
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

    protected override void OnTriggerEnter(Collider col)
    {
        throw new System.NotImplementedException();
    }

    void CameraRect()
    {
        switch (playerNum)
        {
            case 1:
                _camera.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                break;

            case 2:
                _camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                break;

            case 3:
                _camera.rect = new Rect(0, 0, 0.5f, 0.5f);
                break;

            case 4:
                _camera.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                break;
        }
    }
}
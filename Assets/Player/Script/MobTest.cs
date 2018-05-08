using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class MobTest : AIBase
{
    public enum MobPattern
    {
        WAIT,
        RUNDOMWAIK,
        RUN
    }
    private MobPattern mobPattern;

    [SerializeField]
    private int walkDistance;
    [SerializeField]
    private int runDistance;

    protected float RandomRunPosrange = 25;
    protected float RandomWalkPosRange = 15;

    private float dis;//プレイヤーが近ければ0それ以外５
    private int numberOfZombi;//変身するゾンビの番号
    private bool getCaughtFlg = false;

    public MobPattern _MobPattern
    {
        get { return mobPattern; }
        set { mobPattern = value; }
    }

    public bool GetCaughtFlg
    {
        get { return getCaughtFlg; }
        set { getCaughtFlg = value; }
    }

    protected override void Start()
    {
        base.Start();
        targetTag = "Player";
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!getCaughtFlg)
        {
            base.Update();
            switch (mobPattern)
            {
                case MobPattern.WAIT:
                    Wait();
                    break;
                case MobPattern.RUNDOMWAIK:
                    RundomWalk();
                    break;
                case MobPattern.RUN:
                    Run();
                    break;
            }

            if (Vector3.Distance(nextPos, transform.position) < 4 || Mypos == nextPos || nextPos == Vector3.zero)
            {
                switch (mobPattern)
                {
                    case MobPattern.WAIT:
                        break;
                    case MobPattern.RUNDOMWAIK:
                        MoveRandom(RandomWalkPosRange);
                        break;
                    case MobPattern.RUN:
                        MoveRandom(RandomRunPosrange);
                        break;
                }
            }

            if (targetMobs.Any())
            {
                if (Vector3.Distance(targetMobs[0].transform.position, transform.position) < 5)
                {
                    dis = 0;
                }
                else { dis = 5; }
            }
            Mypos = transform.position;
        }
    }

    protected override void OnTriggerEnter(Collider col)
    {
        throw new System.NotImplementedException();
    }


    void Wait()
    {
        if (targetMobs.Any())
        {
            if (Vector3.Distance(targetMobs[0].transform.position, transform.position) < walkDistance)
            {
                mobPattern = MobPattern.RUNDOMWAIK;
                MoveRandom(RandomWalkPosRange);
                navMeshAgent.speed = walkSpeed;
                return;
            }
        }
    }

    void RundomWalk()
    {
        if (targetMobs.Any())
        {
            if (target)
            {
                if (Vector3.Distance(target.transform.position, transform.position) < runDistance)
                {
                    mobPattern = MobPattern.RUN;
                    MoveRandom(RandomRunPosrange);
                    navMeshAgent.speed = runSpeed;
                    return;
                }
            }
            //視界に入ってなくても逃げ始める
            else if (Vector3.Distance(targetMobs[0].transform.position, transform.position) < runDistance / 2)
            {
                target = targetMobs[0];
                mobPattern = MobPattern.RUN;
                MoveRandom(RandomRunPosrange);
                navMeshAgent.speed = runSpeed;
                return;
            }
        }
    }

    void Run()
    {
        if (targetMobs.Any())
        {
            if ((Vector3.Distance(targetMobs[0].transform.position, transform.position) > runDistance))
            {
                target = null;
                mobPattern = MobPattern.RUNDOMWAIK;
                MoveRandom(RandomWalkPosRange);
                navMeshAgent.speed = walkSpeed;
                return;
            }
        }
        else
        {
            target = null;
            mobPattern = MobPattern.RUNDOMWAIK;
            MoveRandom(RandomWalkPosRange);
            navMeshAgent.speed = walkSpeed;
            return;
        }
    }
    protected override void MoveRandom(float range)
    {
        if (targetMobs.Any())
        {
            if (GetRandomPosition(transform.position, range, out nextPos))
            {
                targetDistance = Vector3.Distance(targetMobs[0].transform.position, nextPos);
                nextPosDistance = Vector3.Distance(transform.position, nextPos);

                //プレイヤーに近い位置ならやり直す
                if ((mobPattern == MobPattern.RUN && targetDistance - dis < nextPosDistance) ||
                (mobPattern == MobPattern.RUNDOMWAIK && targetDistance < runDistance + 2))
                {
                    MoveRandom(range);
                }
                else
                {
                    navMeshAgent.SetDestination(nextPos);
                }
            }
        }
    }
}


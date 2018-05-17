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
        AREAMOVE,
        STALKING,   //追跡
        ATACK       //攻撃
    }
    [SerializeField]
    protected MoveState moveState=MoveState.MOVE;

    [SerializeField]
    private Camera _camera;
    protected Vector3 areaPos;               //(仮)移動先エリア
    protected float randomPosRange=30;  //移動場所ランダム範囲
    protected GameObject atackedTarget; //直近の攻撃済みのtarget

    public Vector3 AreaPos
    {
        get { return areaPos; }
        set
        {
            areaPos = value;
            moveState = MoveState.AREAMOVE;
            navMeshAgent.SetDestination(areaPos);
        }
    }

    protected override void Start()
    {
        base.Start();
        targetTag = "Mob";
        CameraRect();
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
            case MoveState.AREAMOVE:
                AreaMove();
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

    void AreaMove()
    {
        Ray ray;RaycastHit hit;
        ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit,5f))
        {
            if(hit.collider.tag == "Area")
            {
                moveState = MoveState.MOVE;
            }
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
            //攻撃処理
            MobTest mobTest = target.GetComponent<MobTest>();
            mobTest.GetCaughtFlg=true;                          //Mobの動きを止める
            mobTest.PlayerNum = playerNum;                      //MobChangeSystemを使うようになったら消す
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
        throw new System.NotImplementedException();
    }

    void CameraRect()//カメラの表示位置
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

    void OnDestroy()
    {
        AreaSystem.AIPlayerList.Remove(gameObject.GetComponent<AIPlayer>());
    }
}
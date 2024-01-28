using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField]
    private float score;//分数

    public enum Status
    {
        idle, //空閑
        walk,//移動
        fear,//害怕

    }; //用于实现怪物AI
    public Status status;
    public float[] actionWeight = { 3000, 4000};//設置待機時各種動作的權重，順序依次爲空閑、移動
    public float actRestTme;//更換待機指令的間隔時間
    private float lastActTime;//最近一次指令時間
    private Vector3 initialPosition;//初始位置

    private float diatanceToInitial;//與初始位置的距離
    private Quaternion targetRotation;//怪物的目標朝向

    [Header("檢測半徑")]
    public float wanderRadius;//遊走半徑，移動狀態下，如果超出遊走半徑會返回出生位置

    public float walkSpeed;          //移動速度
    public float turnSpeed;         //轉身速度，建議0.1

    public bool isScan;//笑声

    [Header("Title")]
    public bool isUseInTitle;//勾选被砍是用来开始游戏的

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        //保存初始位置信息
        initialPosition = gameObject.GetComponent<Transform>().position;
        status = Status.idle;
        //隨機一個待機動作
        RandomAction();
    }

    // Update is called once per frame
    void Update()
    {
        switch (status)
        {
            //待機狀態，等待actRestTme後重新隨機指令
            case Status.idle:
                if (Time.time - lastActTime > actRestTme)
                {
                    RandomAction();         //隨機切換指令
                }
                //該狀態下的檢測指令

                break;


            //遊走，根據狀態隨機時生成的目標位置修改朝向，並向前移動
            case Status.walk:
                transform.Translate(Vector3.up * Time.deltaTime * walkSpeed);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);

                if (Time.time - lastActTime > actRestTme)
                {
                    RandomAction();         //隨機切換指令
                }
                WanderRadiusCheck();
                break;

            case Status.fear:
                if (Time.time - lastActTime > actRestTme)
                {
                    RandomAction();         //隨機切換指令
                }
                //該狀態下的檢測指令
                break;

        }


        //transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z);

        if (isScan)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            status = Status.fear;
            isScan = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

        }
    }

    void RandomAction()
    {
        //更新行動時間
        lastActTime = Time.time;
        //根據權重隨機
        float number = Random.Range(0, actionWeight[0] + actionWeight[1]);
        if (number <= actionWeight[0])
        {
            status = Status.idle;
            //thisAnimator.SetTrigger("Stand");
        }
        else if (actionWeight[0] < number && number <= actionWeight[0] + actionWeight[1])
        {
            status = Status.walk;
            //隨機一個朝向
            targetRotation = Quaternion.Euler(0, 0, Random.Range(1, 60) * 3);
            //thisAnimator.SetTrigger("Walk");

        }
    }

    /// <summary>
    /// 遊走狀態檢測，檢測遊走是否越界
    /// </summary>
    void WanderRadiusCheck()
    {
        diatanceToInitial = Vector2.Distance(transform.position, initialPosition);


        if (diatanceToInitial > wanderRadius)//超出游走范围
        {
            //朝向調整爲初始方向
            Vector2 dir = initialPosition - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            //targetRotation = Quaternion.LookRotation(initialPosition - transform.position, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerMelee")Die();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "laughSound")
        {
            status = Status.fear;
        }
        if (collision.gameObject.tag == "Building")
        {
            //status = Status.idle;
            //朝向調整爲初始方向
            Vector2 dir = initialPosition - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        }
    }

    public void Die()
    {
        if (isUseInTitle)
        {
            GameController.instance.StartGame();
            Destroy(gameObject);
        }
        else
        {
            PlayerControllerScript.PCS.WidenSoundwave(score);
            //TODO 加分和效果 += score;
            GameController.instance.AddScore();
            Destroy(gameObject);
        }
    }

}

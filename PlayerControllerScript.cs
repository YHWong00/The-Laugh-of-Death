using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerScript : MonoBehaviour
{
    public static PlayerControllerScript PCS;
    Rigidbody2D rb;
    public Animator anim;
    [Header("移動")]
    public float speed;
    public float oSpeed;
    Vector2 movement;
    private Quaternion targetRotation;//朝向
    public AudioSource playerAS;
    public AudioClip playerLaugh;
    bool walkingSFXPlaying;

    [Header("攻击")]

    //鼠標點擊
    //public bool isClicked;
    public GameObject melee;
    public GameObject weapon;
    public AudioSource killAS;
    public AudioClip kill;


    [Header("声波")]
    public List<GameObject> soundWave;
    private LayerMask LNpcs, LWall;
    float angle;
    public float soundDis;//探测距离
    //public float colorA;
    //public Collider2D crl;
    public float laughTime;
    float olaughTime;
    public bool laughing;
    public bool shouldBreathe;
    public float breathTime;
    float obreathTime;

    public Image Bar;

    public bool isInBuilding;


    // Start is called before the first frame update
    void Start()
    {
        PCS = this;
        rb = GetComponent<Rigidbody2D>();

        walkingSFXPlaying = false;
        LNpcs = 1 << 6;
        LWall = 1 << 7;
        angle = 360f / soundWave.Count;
        //colorA = 0;
        for (int i = 0; i < soundWave.Count; i++)
        {
            soundWave[i].gameObject.SetActive(false);
        }
        isInBuilding = false;

        if (GameController.instance.player == null)
        {
            GameController.instance.player = this.gameObject;
        }
        laughing = false;
        olaughTime = laughTime;
        shouldBreathe = false;
        obreathTime = breathTime;

        oSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.x >= 0.1f || movement.y >= 0.1f)
        {
            if (!walkingSFXPlaying)
            {
                //walkingSFX.Play();
                walkingSFXPlaying = true;
            }
        }
        else
        {
            //walkingSFX.Stop();
            walkingSFXPlaying = false;
        }

        SwitchAnim();

        //attack
        if (Input.GetMouseButtonDown(0))//當按下鼠標左鍵
        {
            melee.SetActive(true);
            melee.GetComponent<Animator>().Play("MeleeAnimation");
            weapon.GetComponent<Animator>().Play("baguetteAnimation");//武器动画
        }

        if (Input.GetMouseButton(1) && !shouldBreathe)
        {
            if (!playerAS.isPlaying)
            {
                playerAS.PlayOneShot(playerLaugh);
            }
            
            laughing = true;
            //colorA = 1;
            for (int i = 0; i < soundWave.Count; i++)
            {
                
                soundWave[i].gameObject.SetActive(true);
                float x1 = soundDis * Mathf.Sin((angle * i) * (Mathf.PI / 180f));
                float y1 = soundDis * Mathf.Cos((angle * i) * (Mathf.PI / 180f));

                bool istouched = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(x1,y1), soundDis, LNpcs | LWall);
                RaycastHit2D hit2D = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(x1, y1), soundDis, LNpcs | LWall);

                if (istouched)
                {
                    soundWave[i].transform.position = hit2D.point;
                    if (hit2D.transform.gameObject.GetComponent<Enemies>() != null)
                    {
                        hit2D.transform.gameObject.GetComponent<Enemies>().isScan = true;
                    }
                }
                else
                {
                    soundWave[i].transform.position = new Vector2(transform.position.x + x1, transform.position.y + y1);
                }
            }
        }

        if (Input.GetMouseButtonUp(1) || shouldBreathe)
        {
            playerAS.Stop();
            laughing = false;
            for (int i = 0; i < soundWave.Count; i++)
            {
                soundWave[i].gameObject.SetActive(false);
            }
        }

        if (!shouldBreathe && laughing && laughTime >= 0)
        {
            laughTime -= Time.deltaTime;
        }
        if(laughTime <= 0)
        {
            shouldBreathe = true;
        }

        if (shouldBreathe && breathTime>0)
        {
            breathTime -= Time.deltaTime;
        }

        if(breathTime <= 0)
        {
            shouldBreathe = false;
            breathTime = obreathTime;
        }

        if(breathTime<= obreathTime && !shouldBreathe)
        {
            breathTime += Time.deltaTime;
        }


        if (!laughing && !shouldBreathe && laughTime < olaughTime)
        {
            laughTime += Time.deltaTime;
        }

        if(laughTime >= olaughTime)
        {
            laughTime = olaughTime;
        }

        if(Bar != null)
        {
            Bar.rectTransform.sizeDelta = new Vector2(680 * laughTime / olaughTime, 7.8f);

        }

        /*if(colorA > 0)
        {
            colorA -= 0.02f;
        }

        for (int i = 0; i < soundWave.Count; i++)
        {
            soundWave[i].gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, colorA);
        }*/
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.3f);
    }

    void SwitchAnim()
    {
        anim.SetFloat("speed", movement.magnitude);
    }

    public void WidenSoundwave(float score)
    {
        killAS.PlayOneShot(kill);
        if (soundDis < 2f)
        {
            soundDis += score * 0.01f;
        }
        else
        {
            soundDis = 2f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Building")
        {
            isInBuilding = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            isInBuilding = false;

        }

    }
}

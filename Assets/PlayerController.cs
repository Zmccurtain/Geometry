using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float MaxHP;
    public float HP;
    [Header("Movement")]
    public float speed;
    public float timeToMax;
    public Rigidbody2D rb;
    [Header("Shooting")]
    public Transform shootPos;
    public GameObject Bullet;
    public GameObject Flash;
    public float bulletSpeed;
    public float bulletTime;
    public float bulletCD;
    public float bulletDamage;
    public float flashTime;
    private float bulletCDTimer = 0;
    public float knockback;

    [Header("Dashing")]
    public float dashSpeed;
    public float dashCD;
    public float dashTime;
    private float dashTimer=0;
    private float dashCDTimer=0;
    private Vector2 dir = new Vector2(0, 0);
    // Start is called before the first frame update
    void Start()
    {
        MaxHP = GlobalControl.Instance.MaxHP;
        HP = GlobalControl.Instance.HP;
        speed = GlobalControl.Instance.speed;
        bulletCD = GlobalControl.Instance.bulletCD;
        bulletDamage = GlobalControl.Instance.bulletDamage;
        bulletSpeed = GlobalControl.Instance.bulletSpeed;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletCDTimer <= 0)
        {
            bulletCDTimer = bulletCD;
            if (Input.GetMouseButton(0))
            {
                Bullet2D bullet = (Instantiate(Bullet) as GameObject).GetComponent<Bullet2D>();
                bullet.transform.position = shootPos.position;
                bullet.transform.rotation = transform.rotation;
                bullet.bulletSpeed = bulletSpeed;
                bullet.bulletTime = bulletTime;
                bullet.bulletDamage = bulletDamage;

                /*
                var angle = (transform.eulerAngles.z+90) * Mathf.Deg2Rad;
                rb.velocity = (dashSpeed * -1 * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
                */

                Flash flash = (Instantiate(Flash) as GameObject).GetComponent<Flash>();
                flash.transform.position = shootPos.position;
                flash.transform.rotation = transform.rotation;
                flash.flashTime = flashTime;

            }
        }
        else
        {
            bulletCDTimer -= Time.deltaTime;
        }
        Dashing();
    }
    private void FixedUpdate()
    {
        
        float targetH = 0;
        float targetV = 0;
        float rate = 1 / timeToMax;
        if (Input.GetKey(KeyCode.W)) targetV = 1;
        if (Input.GetKey(KeyCode.S)) targetV = -1;
        if (Input.GetKey(KeyCode.D)) targetH = 1;
        if (Input.GetKey(KeyCode.A)) targetH = -1;
        float x = Mathf.MoveTowards(rb.velocity.x, targetH*speed,  rate);
        float y = Mathf.MoveTowards(rb.velocity.y, targetV*speed, rate);

        rb.velocity = new Vector2(x, y);
        var mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 Direction = new Vector2(mousePosition.x - transform.position.x, 
                                        mousePosition.y - transform.position.y);

        transform.up = Direction;
        
    }
    private void savePlayer()
    {
        GlobalControl.Instance.MaxHP = MaxHP;
        GlobalControl.Instance.HP = HP;
        GlobalControl.Instance.speed = speed;
        GlobalControl.Instance.bulletCD = bulletCD;
        GlobalControl.Instance.bulletDamage = bulletDamage;
        GlobalControl.Instance.bulletSpeed = bulletSpeed;
    }

    void Dashing()
    {

        if (dashTimer <= 0)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)
                || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                dir = Vector2.zero;
                if (Input.GetKey(KeyCode.W)) dir.y = 1;
                if (Input.GetKey(KeyCode.S)) dir.y = -1;
                if (Input.GetKey(KeyCode.D)) dir.x = 1;
                if (Input.GetKey(KeyCode.A)) dir.x = -1;
            }

            if (dashCDTimer <= 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    dashTimer = dashTime;
                    dashCDTimer = dashCD;
                }
            }
            else dashCDTimer -= Time.deltaTime;
        }
        else
        {
            dashTimer -= Time.deltaTime;
            var angle = Mathf.Atan2(dir.y, dir.x);
            rb.velocity = dashSpeed * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
            
        }

    public void takeDamage(float damage)
    {
        
        HP -= damage;
        if(HP <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
    }

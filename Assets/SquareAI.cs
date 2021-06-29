using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SquareAI : MonoBehaviour
{

    private AstarPath aStar;
    private PlayerController target;

    public float speed = 7;
    public float nextWaypointDist = 3;
    public float range = 15;

    [Header("Shooting")]
    public Transform shootPos;
    public float bulletSpeed=10;
    public float bulletTime = 2;
    public float bulletCD = 1f;
    public float bulletDamage = 20;
    private float bulletCDTimer = 0;

    Path path;

    int currentWaypoint = 0;
    bool reachedEnd = false;
    private Vector2 startingPos;

    Seeker seeker;
    Rigidbody2D rb;

    public float aggroRange;
    public float HP;
    public float MaxHP = 100;
    public float damage = 20;
    public float XpWorth;
    // Start is called before the first frame update

    void Start()
    {
        aStar = GameObject.Find("A*").GetComponent<AstarPath>();
        HP = MaxHP;
        startingPos = transform.position;
        seeker = GetComponent<Seeker>();
        target = GlobalControl.player;
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0, 0.25f);

    }
    
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (transform.parent.GetComponent<Room>().aggro)
            {
                var position = (Vector2)target.transform.position + target.rb.velocity / 3;
                seeker.StartPath(rb.position, position, OnPathComplete);
            }
            else
            {
                seeker.StartPath(rb.position, startingPos, OnPathComplete);
            }
        }
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {


        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEnd = true;
            return;
        }
        else reachedEnd = false;
        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        float angle = 0;
        if (Vector2.Distance(transform.position, target.transform.position) < range)
        {
            rb.velocity = Vector2.zero;
            angle = Mathf.Atan2(transform.position.y - target.transform.position.y,
            transform.position.x - target.transform.position.x) * Mathf.Rad2Deg - 180;
            Shoot();
        }
        else
        {
            

            Vector2 force = dir * speed;

            rb.AddForce(force);
            angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        }
        float dist = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (dist < nextWaypointDist)
        {
            currentWaypoint++;
        }
        float delta = -1 * Mathf.Sqrt(2 * dist) + 10;
        
        angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle-90, 50);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Player"))
        {
            collision.collider.GetComponent<PlayerController>().takeDamage(damage);
        }
    }
    public void takeDamage(float damage)
    {
        if (HP - damage <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            HP -= damage;
            Debug.Log(HP);
        }
    }

    void Shoot()
    {
        Bullet2D bullet = (Instantiate(Resources.Load("Bullet") as GameObject) as GameObject).GetComponent<Bullet2D>();
        bullet.transform.position = shootPos.position;
        bullet.transform.rotation = transform.rotation;
        bullet.bulletSpeed = bulletSpeed;
        bullet.bulletTime = bulletTime;
        bullet.bulletDamage = bulletDamage;
    }

    private void OnDestroy()
    {
        GlobalControl.gainXP(XpWorth);
    }
}

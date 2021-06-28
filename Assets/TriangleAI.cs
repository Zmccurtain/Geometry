using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TriangleAI : MonoBehaviour
{

    private AstarPath aStar;
    private PlayerController target;

    public float speed = 20;
    public float nextWaypointDist = 3;

    Path path;

    int currentWaypoint=0;
    bool reachedEnd = false;
    private Vector2 startingPos;

    Seeker seeker;
    Rigidbody2D rb;

    public float aggroRange;
    private CircleCollider2D aggroCircle;
    public float HP;
    public float MaxHP = 100;
    public float damage = 20;
    public float XpWorth;
    // Start is called before the first frame update

    void Start()
    {
        aStar = GameObject.Find("A*").GetComponent<AstarPath>();
        HP = MaxHP;
        aggroCircle = GetComponent<CircleCollider2D>();
        aggroCircle.radius = aggroRange;
        startingPos = transform.position;
        seeker = GetComponent<Seeker>();
        target = GameObject.Find("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0, 0.25f);
        Invoke("Scan", 7);
       
    }
    void Scan()
    {
        aStar.Scan();
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
        if(path == null)
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
        
        Vector2 force = dir * speed;

        rb.AddForce(force);

        float dist = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(dist < nextWaypointDist)
        {
            currentWaypoint++;
        }
        float delta = -1 * Mathf.Sqrt(2 * dist) + 10;
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle - 90, 50);
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

    private void OnDestroy()
    {
        GlobalControl.gainXP(XpWorth);
    }
}

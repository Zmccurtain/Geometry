using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PlayerController player;
    public float aggroRange;
    public float speed;
    public float timeToMax;
    private CircleCollider2D aggroCircle;
    private bool aggro;
    public float HP = 100;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        aggroCircle = GetComponent<CircleCollider2D>();
        aggroCircle.radius = aggroRange;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (aggro)
        {
            Vector3 dir = transform.position - player.transform.position;
            float distance = Vector2.Distance(transform.position, player.transform.position);
            float delta = -1*Mathf.Sqrt(2*distance) + 10;
            float angle = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
            angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z-90, angle, delta);
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            
            float rate = 1 / timeToMax;
            float x = Mathf.MoveTowards(rb.velocity.x, transform.up.x * speed, rate);
            float y = Mathf.MoveTowards(rb.velocity.y, transform.up.y * speed, rate);

            rb.velocity = new Vector2(x, y);
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
        }
    }

    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2D : MonoBehaviour
{

    public float bulletSpeed;
    public float bulletTime;
    public float bulletDamage;
    private float bulletTimer;
    private Vector2 velocity;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        var angle = (transform.eulerAngles.z +90 )* Mathf.Deg2Rad;
        velocity.x = Mathf.Cos(angle) * bulletSpeed;
        velocity.y = Mathf.Sin(angle) * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(bulletTimer < bulletTime)
        {
            rb.velocity = velocity;
            bulletTimer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            if (collision.transform.tag == "Enemy")
            {
                collision.GetComponent<TriangleAI>().takeDamage(bulletDamage);
                Destroy(gameObject);
            }

            
            if (collision.gameObject.layer==6)
            {
                Destroy(gameObject);
            }
        }
    }
    
        
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{

    public float friction = 10;
    public float hitImpulse = 1;
    public float recoilImpulse = 1;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Worm")
        {
            Rigidbody2D bodyRB = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 dir = collision.gameObject.transform.position - transform.position;
            dir.Normalize();
            bodyRB.AddForce(dir * hitImpulse * bodyRB.mass, ForceMode2D.Impulse);
            rb.AddForce(-dir * recoilImpulse * rb.mass, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<Part>().health--;
        }
    }

    void Update()
    {
        rb.AddForce(new Vector2(-friction * rb.mass * rb.velocity.x, -friction * rb.mass * rb.velocity.y), ForceMode2D.Force);
    }
}

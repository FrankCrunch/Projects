using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    //Класс описывает поведение мяча в игре.

    public float maxSpeed = 100;
    public float smellTime = 10;

    [HideInInspector]
    public int smell = 0;

    Rigidbody2D rb;
    SpriteRenderer sprite;



    void Start () {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Draw()
    {
        if (smell == 0) sprite.color = Color.white;
        if (smell == 3) sprite.color = Color.red;
    }
	
    void CutSpeed()
    {
        Vector2 vel = rb.velocity;

        if (vel.SqrMagnitude() > Mathf.Pow(maxSpeed, 2))
        {
            vel.Normalize();
            rb.velocity = new Vector2(vel.x * maxSpeed, vel.y * maxSpeed);
        }
    }

    public void SetDangerous()
    {
        smell = 3;
        StartCoroutine("WaitAndRemoveSmell");
    }

    IEnumerator WaitAndRemoveSmell()
    {
        yield return new WaitForSeconds(smellTime);
        smell = 0;
    }

    void Update () {
        CutSpeed();

        Draw();
    }
}

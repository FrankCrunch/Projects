using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gate : MonoBehaviour {

    public int owner = 0;
    public float protectionTime = 1F;

    int count = 0;
    bool isProtected = false;
    SpriteRenderer sprite;
    Text textField; 

    private void Start()
    {
        textField = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        gameObject.tag = "Gate" + owner;
        sprite = GetComponent<SpriteRenderer>();
    }

    void Draw()
    {
        textField.text = "" + count;
        if (isProtected) sprite.color = Color.blue;
        else sprite.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball" && !isProtected)
        {
            Destroy(collision.gameObject);
            count++;
            LevelManager.instance.NewBall();
        }
    }

    public void Protect()
    {
        isProtected = true;
        StartCoroutine("WaitAndRemoveProtection");
    }

    IEnumerator WaitAndRemoveProtection()
    {
        yield return new WaitForSeconds(protectionTime);
        isProtected = false;
    }

    private void Update()
    {
        Draw();
    }
}

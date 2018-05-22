using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance = null;
    public GameObject ball;
    public GameObject[] foodTypes;
    public float foodWaitTime = 2;

    void Start () {
        instance = this;
        StartCoroutine("WaitAndPlaceFood");
    }

    IEnumerator WaitAndPlaceFood()
    {
        yield return new WaitForSeconds(foodWaitTime);
        Instantiate(foodTypes[Random.Range(0, foodTypes.Length)], new Vector3(Random.Range(-5F, 5F), Random.Range(-3F, 3F), 0), Quaternion.identity);
        StartCoroutine("WaitAndPlaceFood");
    }

    public void NewBall()
    {
        Instantiate(ball, new Vector3(0, 0, 0), Quaternion.identity);
    }
}

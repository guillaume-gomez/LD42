using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{

    private GameObject center;
    private float up = 1f;

    // Use this for initialization
    void Start()
    {
        center = GameObject.FindGameObjectsWithTag("Gravity")[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.instance.doingSetup)
            transform.position += (Time.deltaTime / 5f) * up * (center.transform.position - transform.position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        up *= -1f;
    }
}
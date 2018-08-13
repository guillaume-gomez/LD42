using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float movementSpeed = 800f;
    private GameObject center;
    protected Rigidbody2D rb2D;
    protected BoxCollider2D bodyCollider;

	// Use this for initialization
	void Start () {
        center = GameObject.FindGameObjectsWithTag("Gravity")[0];
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        bodyCollider = gameObject.GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void FixedUpdate () {
        if(!GameManager.instance.doingSetup) {
          //Vector3 forceDirection = transform.position - center.transform.position;
          rb2D.velocity = rb2D.velocity / 1.5f;

          //Vector2 vec = new Vector2(movementSpeed * 5.0f, 0.0f);
          float moveX = movementSpeed * Time.deltaTime;
          Vector3 addX = transform.right * moveX;
            rb2D.AddForce(addX);
          //rb2D.AddForce(forceDirection.normalized * 1f * Time.fixedDeltaTime);
        }
	}

   void OnCollisionEnter2D(Collision2D col)
   {
        Collider2D[] contacts = new Collider2D[1];
        if (bodyCollider.GetContacts(contacts) > 0) {
            rb2D.velocity = new Vector2(0f, 0f);
            movementSpeed = -movementSpeed;
        }
   }
}

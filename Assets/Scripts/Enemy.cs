using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

  public float movementSpeed = 20000f;
  private GameObject center;
  protected Rigidbody2D rb2D;

	// Use this for initialization
	void Start () {
    center = GameObject.FindGameObjectsWithTag("Gravity")[0];
    rb2D = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
    if(!GameManager.instance.doingSetup) {
      Vector3 forceDirection = transform.position - center.transform.position;
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
     if(col.gameObject.tag != "Floor")
     {
        movementSpeed = -movementSpeed;
     }
   }
}

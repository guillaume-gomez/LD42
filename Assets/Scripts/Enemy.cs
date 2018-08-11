using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

  public float movementSpeed = 1;
  protected Rigidbody2D rb2D;


	// Use this for initialization
	void Start () {
    rb2D = gameObject.GetComponent<Rigidbody2D>();
	}
	// Update is called once per frame
	void Update () {
    Vector2 vec = new Vector2(movementSpeed * 5.0f, 0.0f);
    rb2D.AddForce(vec);
	}

   void OnCollisionEnter2D(Collision2D col)
   {
     if(col.gameObject.tag != "Floor")
     {
        movementSpeed = -movementSpeed;
     }
   }
}

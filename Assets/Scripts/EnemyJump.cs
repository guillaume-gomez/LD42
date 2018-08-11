using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour {

  public float maxJump = 50.0f;
  protected Rigidbody2D rb2D;
  protected bool grounded;


  // Use this for initialization
  void Start () {
    rb2D = gameObject.GetComponent<Rigidbody2D>();
    grounded = false;
  }

  // Update is called once per frame
  void Update () {
    if(grounded) {
      Vector2 vec = new Vector2(0.0f, maxJump * 5.0f);
      rb2D.AddForce(vec);
      grounded = false;
    }
  }

  void OnCollisionEnter2D(Collision2D col)
   {
     if(col.gameObject.tag == "Floor")
     {
        grounded = true;
     }
  }
}

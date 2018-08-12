using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour {

  public float maxJump = 50.0f;
  protected Rigidbody2D rb2D;
  protected bool grounded;
  private float jumpTimer = 0f;
  private GameObject center;


  // Use this for initialization
  void Start () {
    center = GameObject.FindGameObjectsWithTag("Gravity")[0];
    rb2D = gameObject.GetComponent<Rigidbody2D>();
    grounded = false;
  }

  // Update is called once per frame
  void Update () {
    if(grounded) {
      Vector3 forceDirection = transform.position - center.transform.position;
      rb2D.AddForce(forceDirection.normalized * maxJump);
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

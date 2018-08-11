using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

  protected float speed = 1.0f;
  public float topSpeed = 7f;
  public float jumpTakeOffSpeed = 7f;
  bool facingRight = true;
  bool grounded = false;


  protected Animator animator;
  protected SpriteRenderer spriteRenderer;
  protected Rigidbody2D rb2D;

  protected virtual void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
    rb2D = gameObject.GetComponent<Rigidbody2D>();
  }

	// Use this for initialization
	void Start () {

	}

  void FixedUpdate()
  {
    float move = Input.GetAxis("Horizontal");
    Vector2 newVel = new Vector2(move * topSpeed, GetComponent<Rigidbody2D>().velocity.y);

    //bool startJump = false;
    if(Input.GetButtonDown("Jump") && grounded) {
      //SoundManager.instance.PlaySingle(jumpSound);
      newVel.y = jumpTakeOffSpeed;
      //startJump = true;
    }
    GetComponent<Rigidbody2D>().velocity = newVel;

    if(move > 0 && !facingRight || move < 0 && facingRight) {
      Flip();
    }
    //animator.SetBool("startJump", startJump);
    animator.SetBool("grounded", grounded);
    animator.SetFloat ("velocityX", Mathf.Abs (move));
  }

  void Flip()
  {
    facingRight = !facingRight;
    spriteRenderer.flipX = !spriteRenderer.flipX;
  }

  void OnCollisionEnter2D(Collision2D other) {
    if(other.gameObject.name == "ground")
    {
      grounded = true;
    }
    if(other.gameObject.tag == "Enemy") {
      animator.SetBool("hurt", true);
    }
  }

  void OnCollisionExit2D(Collision2D other) {
    if(other.gameObject.name == "ground")
    {
      grounded = false;
    }
  }

  void Update () {
 /*   Vector2 move = Vector2.zero;
    move.x = Input.GetAxis("Horizontal");

    bool flipSprite = spriteRenderer.flipX ? (move.x > 0.00f) : (move.x < 0.00f);
    if (flipSprite) {
      spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    //animator.SetBool("startJump", startJump);
    //animator.SetBool("grounded", grounded);
    animator.SetFloat ("velocityX", Mathf.Abs (move.x));*/
    //this.transform.Rotate(0,0, Input.GetAxis("Horizontal") * speed);
	}
}

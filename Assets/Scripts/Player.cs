using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    bool facingRight = true;
    bool grounded = false;

    private GameObject center;
    public const float jumpBaseTimer = 0.5f;
    private float jumpTimer = 0f;
    public float moveSpeed = 20000f;
    public float jumpSpeed = 1f;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb2D;

    protected virtual void Awake() {
      spriteRenderer = GetComponent<SpriteRenderer>();
      animator = GetComponent<Animator>();
      rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

  void Start () {
        center = GameObject.FindGameObjectsWithTag("Gravity")[0];
    }

    void FixedUpdate() {
      if(!GameManager.instance.doingSetup) {
        float move = Input.GetAxis("Horizontal");
        Vector3 forceDirection = transform.position - center.transform.position;

        rb2D.velocity = rb2D.velocity / 1.5f;

        if (Input.GetAxis("Horizontal") != 0f)
        {
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            Vector3 addX = transform.right * moveX;
            rb2D.AddForce(addX);
        }
        if (Input.GetButtonDown("Jump") && jumpTimer <= 0f) {
            //animator.SetBool("startJump", startJump);
            jumpTimer = jumpBaseTimer;
        }

        rb2D.AddForce(forceDirection.normalized * 1f * Time.fixedDeltaTime);
        if (jumpTimer > 0f) {
            jumpTimer -= Time.deltaTime;
            if (Input.GetButton("Jump")) {
                rb2D.AddForce(forceDirection.normalized * (jumpSpeed * (jumpTimer / jumpBaseTimer)) * Time.fixedDeltaTime);
            }
        }

        if (move > 0 && !facingRight || move < 0 && facingRight) {
            Flip();
        }
        animator.SetBool("grounded", grounded);
        animator.SetFloat ("velocityX", Mathf.Abs (move));
      }
    }

    void GroundCollision(Collision2D other)
    {

    }

  void Flip()
  {
    facingRight = !facingRight;
    spriteRenderer.flipX = !spriteRenderer.flipX;
  }

  void OnCollisionEnter2D(Collision2D collision) {

    if (rb2D.OverlapPoint(collision.GetContact(0).point)) {
        Debug.Log("RESET");
        jumpTimer = -1f;
        grounded = true;
    }

    if(collision.gameObject.tag == "Enemy") {
      GameManager.instance.GameOver("player_die");
      //animation hurt
      //Destroy(gameObject);
    }

    //if(other.gameObject.tag == "Enemy") {
    //  animator.SetBool("hurt", true);
    //}
    }

  void OnCollisionExit2D(Collision2D other) {
    if(other.gameObject.name == "ground")
    {
      grounded = false;
    }
  }

  void Update () {
  }

}
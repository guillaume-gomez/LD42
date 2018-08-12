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
    protected CircleCollider2D footCollider;
    protected CapsuleCollider2D bodyColliderVer;
    protected CapsuleCollider2D bodyColliderHor;
    protected int animeState = 0; //IDLE /RUN /JUMP /Slide
    protected float distToCenter = 0;

    protected virtual void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        footCollider = this.GetComponent<CircleCollider2D>();
        CapsuleCollider2D[] colliders = this.GetComponents<CapsuleCollider2D>();
        foreach(CapsuleCollider2D cap in colliders) {
            if (cap.direction.Equals(CapsuleDirection2D.Horizontal))
                bodyColliderHor = cap;
            else
                bodyColliderVer = cap;
        }
    }

    void Start () {
          center = GameObject.FindGameObjectsWithTag("Gravity")[0];
    }

    void FixedUpdate() {
        //if (!GameManager.instance.doingSetup) {
        //    return;
        //}

        float newDist = (Mathf.Round(Vector3.Distance(center.transform.position, transform.position) * 10)) / 10f;
        float move = Input.GetAxis("Horizontal");
        Vector3 forceDirection = transform.position - center.transform.position;
        Debug.Log(newDist);
        if (distToCenter == newDist && !Input.GetButtonDown("Jump")) {
            grounded = true;
            jumpTimer = -1f;
            if (Input.GetAxisRaw("Vertical") < 0f) {
                SwitchAnimeState(3);
            }
            else if (Input.GetAxisRaw("Horizontal") == 0f) {
                SwitchAnimeState(0);
            }
            else {
                SwitchAnimeState(1);
            }
        }
        distToCenter = newDist;

        rb2D.velocity = rb2D.velocity / 1.5f;

        if (Input.GetAxis("Horizontal") != 0f && Input.GetAxis("Vertical") >= -0.8f)
        {
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            Vector3 addX = transform.right * moveX;
            rb2D.AddForce(addX);
        }

        if (Input.GetButtonDown("Jump") && jumpTimer <= 0f) {
            grounded = false;
            jumpTimer = jumpBaseTimer;
            SwitchAnimeState(2);
        }

        rb2D.AddForce(forceDirection.normalized * 1f * Time.fixedDeltaTime);
        if (jumpTimer > 0f) {
            jumpTimer -= Time.deltaTime;
            if (Input.GetButton("Jump")) {
                grounded = false;
                rb2D.AddForce(forceDirection.normalized * (jumpSpeed * (jumpTimer / jumpBaseTimer)) * Time.fixedDeltaTime);
            }
        }

        if (move > 0 && !facingRight || move < 0 && facingRight) {
            Flip();
        }
        animator.SetBool("grounded", grounded);
        animator.SetFloat ("velocityX", Mathf.Abs (move));
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
            //Debug.Log("RESET");
            jumpTimer = -1f;
            grounded = true;
        }

        if(collision.gameObject.tag == "Enemy"  || collision.gameObject.tag == "Laser") {
          GameManager.instance.GameOver("player_die");
          //animation hurt
          //Destroy(gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
    }

    void OnCollisionExit2D(Collision2D other) {
    }

    void Update () {
    }

    void SwitchAnimeState(int change) {
        if (animeState != change) {
            animeState = change;
            switch(animeState) {
                case 0:
                    animator.Play("IdlePlayer");
                    bodyColliderVer.enabled = true;
                    bodyColliderHor.enabled = false;

                    bodyColliderVer.offset.Set(0f, 0f);
                    bodyColliderVer.size.Set(0.1f, 0.5f);
                    footCollider.offset.Set(0f, -0.3213656f);
                    break;

                case 1:
                    animator.Play("RunPlayer");
                    bodyColliderVer.enabled = true;
                    bodyColliderHor.enabled = false;

                    bodyColliderVer.offset.Set(0.1f, 0f);
                    bodyColliderVer.size.Set(0.35f, 0.5f);
                    footCollider.offset.Set(0f, -0.3213656f);
                    break;

                case 2:
                    animator.Play("JumpPlayer");
                    bodyColliderVer.enabled = true;
                    bodyColliderHor.enabled = false;

                    bodyColliderVer.offset.Set(0.08f, 0.05f);
                    bodyColliderVer.size.Set(0.2f, 0.3f);
                    footCollider.offset.Set(0.08f, 0.01f);
                    break;

                case 3:
                    animator.Play("SlidePlayer");
                    bodyColliderVer.enabled = false;
                    bodyColliderHor.enabled = true;

                    bodyColliderHor.offset.Set(0f, -0.3f);
                    bodyColliderHor.size.Set(0.6f, 0.1f);
                    footCollider.offset.Set(0f, -0.3213656f);
                    break;

                default:
                    animator.Play("IdlePlayer");
                    bodyColliderVer.enabled = true;
                    bodyColliderHor.enabled = false;

                    bodyColliderVer.offset.Set(0f, 0f);
                    bodyColliderVer.size.Set(0.1f, 0.5f);
                    footCollider.offset.Set(0f, -0.3213656f);
                    break;
            }
        }

    }
}
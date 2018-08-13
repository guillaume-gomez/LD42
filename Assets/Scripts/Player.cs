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

    public AudioClip[] jumpSounds;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb2D;
    protected CircleCollider2D footCollider;
    protected CapsuleCollider2D bodyColliderVer;
    protected CapsuleCollider2D bodyColliderHor;
    public int animeState = 0; //IDLE /RUN /JUMP /SLIDE /PUNCH
    protected float distToCenter = 0;
    protected float atkTimer = 0f;

    private bool stopAnimations = false;
    private bool teleporting = false;

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
          center = GameObject.FindGameObjectWithTag("Gravity");
    }

    void FixedUpdate() {
        if (GameManager.instance.doingSetup || teleporting) {
            return;
        }
        float newDist = (Mathf.Round(Vector3.Distance(center.transform.position, transform.position) * 10)) / 10f;

        float move = Input.GetAxis("Horizontal");
        if(GameManager.instance.hasInvertedInput) {
            move = -move;
        }


        Vector3 forceDirection = transform.position - center.transform.position;
        if (distToCenter == newDist && jumpTimer <= 0) {
            grounded = true;
            jumpTimer = -1f;
            if (atkTimer > 0) {
                SwitchAnimeState(4);
            }
            else if (Input.GetAxisRaw("Vertical") < 0f && grounded)
            {
                SwitchAnimeState(3);
            }
            else if (Input.GetAxisRaw("Horizontal") == 0f)
            {
                SwitchAnimeState(0);
            }
            else
            {
                SwitchAnimeState(1);
            }
        }
        distToCenter = newDist;

        rb2D.velocity = rb2D.velocity / 1.5f;

        if (move != 0f && Input.GetAxis("Vertical") >= -0.8f)
        {
            float moveX = move * moveSpeed * Time.deltaTime;
            Vector3 addX = transform.right * moveX;
            rb2D.AddForce(addX);
        }

        if (Input.GetAxisRaw("Jump") != 0 && jumpTimer <= 0f) {
            grounded = false;
            jumpTimer = jumpBaseTimer;
            SwitchAnimeState(2);
            JumpSound();
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

        if (Input.GetButtonDown("Fire3") && atkTimer <= 0f) {
            atkTimer = 0.3f;
        }

        if (atkTimer >= 0f)
            atkTimer -= Time.deltaTime;

        animator.SetBool("grounded", grounded);
        animator.SetFloat ("velocityX", Mathf.Abs (move));
    }

    void GroundCollision(Collision2D other)
    {

    }

    void JumpSound() {
        SoundManager.instance.RandomizeSfx(jumpSounds);
    }

    void Flip()
    {
      facingRight = !facingRight;
      spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Collider2D[] colliders = new Collider2D[1];
        if (footCollider.GetContacts(colliders) > 0) {
            jumpTimer = -1f;
            grounded = true;
        }

        if (collision.gameObject.tag == "Enemy"  || collision.gameObject.tag == "Laser") {
            if (collision.gameObject.tag == "Laser"
                && !collision.gameObject.GetComponent<Laser>().isActive())
                return;

            onDeath();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
    }

    void OnCollisionExit2D(Collision2D other) {
    }

    void Update () {
    }

    public void callbackGameManagerOnDeath()
    {
        GameManager.instance.GameOver("player_die");
    }

    public void onDeath() {
        animator.Play("PlayerDeath1");
        stopAnimations = true;
        Invoke("callbackGameManagerOnDeath", 0.6f);
    }

    public void onPortalEnd()
    {
        animator.Play("IdlePlayer");
        teleporting = false;
        stopAnimations = false;
    }
    public void onPortal()
    {
        animator.Play("TeleportationIn");
        teleporting = true;
        stopAnimations = true;
        Invoke("onPortalEnd", 0.6f);
    }

    void SwitchAnimeState(int change) {
        if (!stopAnimations && animeState != change) {
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

                case 4:
                    if (Random.Range(0, 2) % 2 == 0)
                    {
                        animator.Play("PunchPlayer");
                        bodyColliderVer.enabled = false;
                        bodyColliderHor.enabled = true;

                        bodyColliderHor.offset.Set(0.1f, -0.1f);
                        bodyColliderHor.size.Set(0.6f, 0.3f);
                        footCollider.offset.Set(0f, -0.3213656f);

                    }
                    else {
                        animator.Play("PunchPlayer1");
                        bodyColliderVer.enabled = true;
                        bodyColliderHor.enabled = false;

                        bodyColliderVer.offset.Set(0.05f, 0f);
                        bodyColliderVer.size.Set(0.3f, 0.5f);
                        footCollider.offset.Set(0f, -0.3213656f);
                    }
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
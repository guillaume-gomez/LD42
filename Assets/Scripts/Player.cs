﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MoveableObject {

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
    public CapsuleCollider2D bodyColliderVer;
    public CapsuleCollider2D bodyColliderHor;
    public int animeState = 0; //IDLE /RUN /JUMP /SLIDE /PUNCH
    protected float distToCenter = 0;
    protected float atkTimer = 0f;

    private bool stopAnimations = false;
    private bool teleporting = false;
    // for android build
    private int directionButton = 0;
    private bool slideButton = false;
    private bool jumpButton = false;
    private bool punchButton = false;
    private float jumpVelocity = 0.0f;

    protected virtual void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        footCollider = this.GetComponent<CircleCollider2D>();
    }

    void Start () {
          center = GameObject.FindGameObjectWithTag("Gravity");
    }

    void FixedUpdate() {
        float newDist = (Mathf.Round(Vector3.Distance(center.transform.position, transform.position) * 10)) / 10f;
        distToCenter = newDist;
        if (!CanMove()) {
            return;
        }

        if(teleporting) {
            //to keep colission during the teleportation with the new layer
            rb2D.velocity = rb2D.velocity / 1.5f;
            return;
        }

        float move = 0;
        #if UNITY_STANDALONE || UNITY_WEBGL
            move = Input.GetAxis("Horizontal");
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            move = (float) directionButton;
        #endif
        if(GameManager.instance.hasInvertedInput) {
            move = -move;
        }

        Vector3 forceDirection = transform.position - center.transform.position;
        rb2D.velocity = rb2D.velocity / 1.5f;

        #if UNITY_STANDALONE || UNITY_WEBGL
            if (move != 0f && Input.GetAxis("Vertical") >= -0.8f)
         #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            if(move != 0f ) //&& slideButton)
         #endif
        {
            float moveX = move * moveSpeed * Time.deltaTime;
            Vector3 addX = transform.right * moveX;
            rb2D.AddForce(addX);
        }
        #if UNITY_STANDALONE || UNITY_WEBGL
           if (Input.GetAxisRaw("Jump") != 0 && jumpTimer <= 0f)
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            if (jumpButton && jumpTimer <= 0f)
        #endif
        {
            grounded = false;
            jumpTimer = jumpBaseTimer;
            SwitchAnimeState(2);
            JumpSound();
        }

        rb2D.AddForce(forceDirection.normalized * 1f * Time.fixedDeltaTime);


        if (jumpTimer > 0f) {
            jumpTimer -= Time.deltaTime;
            #if UNITY_STANDALONE || UNITY_WEBGL
               if (Input.GetButton("Jump"))
            #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
                if(jumpButton)
            #endif
            {
                grounded = false;
                rb2D.AddForce(forceDirection.normalized * (jumpSpeed * (jumpTimer / jumpBaseTimer)) * Time.fixedDeltaTime);
            }
        }


        if (move > 0 && !facingRight || move < 0 && facingRight) {
            Flip();
        }

        #if UNITY_STANDALONE || UNITY_WEBGL
           if (Input.GetButtonDown("Fire3") && atkTimer <= 0f) {
               atkTimer = 0.3f;
           }
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            if (punchButton && atkTimer <= 0f) {
                atkTimer = 0.3f;
            }
        #endif

        if (atkTimer >= 0f)
            atkTimer -= Time.deltaTime;

        if (atkTimer > 0)
            SwitchAnimeState(4);
        else if (jumpTimer > 0)
            SwitchAnimeState(2);
        #if UNITY_STANDALONE || UNITY_WEBGL
        else if (Input.GetAxisRaw("Vertical") < 0f && grounded)
            SwitchAnimeState(3);
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        else if(slideButton && grounded)
            SwitchAnimeState(3);
        #endif
        #if UNITY_STANDALONE || UNITY_WEBGL
        else if (Input.GetAxisRaw("Horizontal") == 0f)
           SwitchAnimeState(0);
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        else if (directionButton == 0)
            SwitchAnimeState(0);
        #endif
        else
            SwitchAnimeState(1);

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

        if (collision.gameObject.tag == "Enemy"  || collision.gameObject.tag == "Laser" || collision.gameObject.tag == "CustomEnemy") {
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
        teleporting = true;
        rb2D.velocity = new Vector2(0f, 0f);
        Invoke("callbackGameManagerOnDeath", 0.6f);
    }

    public void onPortalEnd()
    {
        //Debug.Break();
        animator.Play("IdlePlayer");
        teleporting = false;
        stopAnimations = false;
    }
    public void onPortal()
    {
        animator.Play("TeleportationIn");
        teleporting = true;
        stopAnimations = true;

        rb2D.velocity = new Vector2(0f, 0f);
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

                    bodyColliderVer.offset.Set(0.02f, 0.05f);
                    bodyColliderVer.size.Set(0.5f, 0.5f);
                    footCollider.offset.Set(0.08f, 0.01f);
                    break;

                case 3:
                    animator.Play("SlidePlayer");
                    bodyColliderVer.enabled = false;
                    bodyColliderHor.enabled = true;

                    bodyColliderHor.offset.Set(0f, -0.3f);
                    bodyColliderHor.size.Set(0.72f, 0.18f);
                    footCollider.offset.Set(0f, -0.3213656f);
                    break;

                case 4:
                    if (Random.Range(0, 2) % 2 == 0)
                    {
                        animator.Play("PunchPlayer");
                        bodyColliderVer.enabled = false;
                        bodyColliderHor.enabled = true;

                        bodyColliderHor.offset.Set(0.12f, -0.06f);
                        bodyColliderHor.size.Set(0.6f, 0.1f);
                        footCollider.offset.Set(0f, -0.3213656f);

                    }
                    else {
                        animator.Play("PunchPlayer1");
                        bodyColliderVer.enabled = true;
                        bodyColliderHor.enabled = false;

                        bodyColliderVer.offset.Set(0.06f, 0.05f);
                        bodyColliderVer.size.Set(0.4f, 0.1f);
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

    // for android
    public void LeftButton() {
        directionButton = -1;
    }

    public void RightButton() {
        directionButton = 1;
    }

    public void NoDirection() {
        directionButton = 0;
    }

    public void SlideButtonUp() {
        slideButton = false;
    }

    public void SlideButtonDown() {
        slideButton = true;
    }

    public void JumpButtonUp() {
        jumpButton = false;
        //if(jumpVelocity > 0.01f) {
            jumpVelocity-= 0.01f;
        //}
    }

    public void JumpButtonDown() {
        jumpButton = true;
        //if(jumpVelocity < 1.0f)
            jumpVelocity+= 0.01f;
    }

    public void PunchButtonUp() {
        punchButton = false;
    }

    public void PunchButtonDown() {
        punchButton = true;
    }
}
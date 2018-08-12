using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    BoxCollider2D collisionBox2D;
    Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collisionBox2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    void ResetAfterAnimation()
    {
        spriteRenderer.enabled = false;
        animator.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            collisionBox2D.enabled = false;
            animator.enabled = true;
            Invoke("ResetAfterAnimation", 0.4f);
        }
    }
}
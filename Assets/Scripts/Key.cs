using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    public Portal portalTarget;

    BoxCollider2D targetCollider;
    BoxCollider2D collision2D;
    SpriteRenderer targetRenderer;
    SpriteRenderer spriteRenderer;

    Animator animator;

    // Use this for initialization
    void Start () {
        targetCollider = portalTarget.GetComponent<BoxCollider2D>();
        targetRenderer = portalTarget.GetComponent<SpriteRenderer>();
        collision2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetCollider.enabled = false;
        targetRenderer.enabled = false;
        animator = GetComponent<Animator>();
    }

    public void Deactivate()
    {
        spriteRenderer.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            collision2D.enabled = false;
            targetCollider.enabled = true;
            targetRenderer.enabled = true;
            animator.SetBool("Taken", true);
            Invoke("Deactivate", 0.6f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    public Portal portalTarget;

    BoxCollider2D targetCollider;
    BoxCollider2D collision2D;
    SpriteRenderer targetRenderer;
    SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start () {
        targetCollider = portalTarget.GetComponent<BoxCollider2D>();
        targetRenderer = portalTarget.GetComponent<SpriteRenderer>();
        collision2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetCollider.enabled = false;
        targetRenderer.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            collision2D.enabled = false;
            spriteRenderer.enabled = false;
            targetCollider.enabled = true;
            targetRenderer.enabled = true;
        }
    }
}

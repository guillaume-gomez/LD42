using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Malus : MonoBehaviour {
    GameObject cameraAffected;
    SpriteRenderer spriteRenderer;
    BoxCollider2D collisionBox2D;
    Animator animator;
    public PostProcessingProfile profile_active;
    public Layer layer;

    PostProcessingProfile base_profile;

    private void Start()
    {
        cameraAffected = GameObject.FindGameObjectWithTag("MainCamera");
        base_profile = cameraAffected.gameObject.GetComponent<PostProcessingBehaviour>().profile;
        spriteRenderer = GetComponent<SpriteRenderer>();
        collisionBox2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    void ResetAfterAnimation() {
        spriteRenderer.enabled = false;
        animator.enabled = false;
    }
    void ResetAfterGlitch() {
        cameraAffected.gameObject.GetComponent<PostProcessingBehaviour>().profile = base_profile;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            layer.onMalus();
            collisionBox2D.enabled = false;
            animator.enabled = true;
            Invoke("ResetAfterAnimation", 0.6f);
            Invoke("ResetAfterGlitch", 1.5f);
            cameraAffected.gameObject.GetComponent<PostProcessingBehaviour>().profile = profile_active;
        }
    }
}

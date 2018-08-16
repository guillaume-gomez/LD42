using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class MalusInput : Malus {
    public AudioClip [] glitches;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.InvertedInput(5f);
            layer.onMalus();
            collisionBox2D.enabled = false;
            animator.enabled = true;
            GameManager.instance.ReduceTime();
            Invoke("ResetAfterAnimation", 0.6f);
            Invoke("ResetAfterGlitch", 5f);
            cameraAffected.gameObject.GetComponent<PostProcessingBehaviour>().profile = profile_active;
        }
    }
}

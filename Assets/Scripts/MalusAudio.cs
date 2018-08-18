using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class MalusAudio : Malus {
    public AudioClip [] glitches;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            SoundManager.instance.RandomizeSfx(glitches);
            layer.onMalus();
            collisionBox2D.enabled = false;
            animator.enabled = true;
            GameManager.instance.ReduceTime();
            Invoke("ResetAfterAnimation", 0.6f);
            Invoke("ResetAfterGlitch", 1.5f);
            cameraAffected.gameObject.GetComponent<PostProcessingBehaviour>().profile = profile_active;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyableBase : IAbstractPlatform
{
    public AudioClip destroyedSound;
    protected Animator animator;
    private bool transitionning = false;

    public override void Start() {
      base.Start();
      animator = GetComponent<Animator>();
      animator.SetBool("destroyed", false);
    }

    public override void Activate() {
        base.active = true;
        base.spriteRenderer.enabled = true;

        foreach (Collider2D collider in base.colliders)
            collider.enabled = true;
    }
    public override void Deactivate() {
      base.active = false;
      base.spriteRenderer.enabled = false;

      animator.SetBool("destroyed", false);
        animator.Play("Idle");
        transitionning = false;
      foreach (Collider2D collider in base.colliders)
          collider.enabled = false;
    }

    void PerformDestroyAnim() {
        transitionning = true;
      SoundManager.instance.PlaySingle(destroyedSound);
      animator.SetBool("destroyed", true);
      Invoke("Deactivate", 0.6f);
      foreach (Collider2D collider in base.colliders)
          collider.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
       if(!transitionning && col.gameObject.tag == "Player"
            && col.gameObject.GetComponent<Player>().animeState >= 3)
       {
         PerformDestroyAnim();
       }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyableBase : IAbstractPlatform
{
    protected Animator animator;

    public override void Start() {
      base.Start();
      animator = GetComponent<Animator>();
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

      foreach (Collider2D collider in base.colliders)
          collider.enabled = false;
    }

    void PerformDestroyAnim() {
      animator.SetBool("destroyed", true);
      Invoke("Deactivate", 0.6f);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
       if(col.gameObject.tag == "Player")
       {
         PerformDestroyAnim();
       }
    }
}

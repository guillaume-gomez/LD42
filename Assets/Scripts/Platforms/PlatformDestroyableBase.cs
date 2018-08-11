using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyableBase : IAbstractPlatform
{
    public AudioClip destroyedSound;
    protected Animator animator;

    public override void Start() {
      base.Start();
      Invoke("PerformDestroyAnim", 3f);
      animator = GetComponent<Animator>();
    }

    public override void Activate() {
        base.active = true;
        base.spriteRenderer.enabled = true;
    }
    public override void Deactivate() {
      base.active = false;
      base.spriteRenderer.enabled = false;
    }

    void PerformDestroyAnim() {
      SoundManager.instance.PlaySingle(destroyedSound);
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

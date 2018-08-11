using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAudio : IAbstractPlatform
{
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
}

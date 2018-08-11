using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformVideo : IAbstractPlatform
{
    public override void Activate() {
        base.active = true;
        base.spriteRenderer.enabled = true;
    }
    public override void Deactivate() {
        base.active = false;
        base.spriteRenderer.enabled = false;
    }
}

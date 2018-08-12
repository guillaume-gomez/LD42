using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractPlatform : MonoBehaviour {
    protected SpriteRenderer spriteRenderer;
    protected Collider2D[] colliders;
    public bool active;
    public bool breakable;

	// Use this for initialization
	public virtual void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();

        spriteRenderer.enabled = active;

        foreach (Collider2D collider in colliders)
            collider.enabled = active;
    }

	// Update is called once per frame
	void Update () {
        //Debug.Log("Updating plateform");
    }

    abstract public void Activate();
    abstract public void Deactivate();

}

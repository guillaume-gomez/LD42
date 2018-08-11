using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractPlatform : MonoBehaviour {
    protected SpriteRenderer spriteRenderer;
    public bool active;
	// Use this for initialization
	public virtual void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = active;
	}

	// Update is called once per frame
	void Update () {
        //Debug.Log("Updating plateform");
    }

    abstract public void Activate();
    abstract public void Deactivate();

}

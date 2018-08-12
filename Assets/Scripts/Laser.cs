using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    BoxCollider2D   collider;
    SpriteRenderer  renderer;

    public float activationTime;
    public float deactivationTime;
    public bool active;
    float switchIn;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();

        renderer.enabled = active;
    }

    float getActiveTimer()
    {
        return active ? activationTime : deactivationTime;
    }

	// Update is called once per frame
	void Update () {
        switchIn += Time.deltaTime;

        if (switchIn >= getActiveTimer())
        {
            active = !active;
            renderer.enabled = active;
            switchIn = 0f;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (active && other.gameObject.tag == "Player")
        {
            // kill player
        }
    }
}

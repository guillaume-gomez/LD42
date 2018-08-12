using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    public Portal portalTarget;
    public uint layer;
    public PlatformManager platformManager;
    public float resetTimer = 3f;
    public float lastReset = 3f;

    private void Update()
    {
        lastReset += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (lastReset >= resetTimer && other.gameObject.tag == "Player") {
            lastReset = 0f;
            portalTarget.lastReset = 0f;
            other.transform.position = portalTarget.transform.position;
            platformManager.SetPlayerLayer(portalTarget.layer);
        }
    }
}

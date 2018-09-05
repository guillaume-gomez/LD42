using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    Animator        animator;
    BoxCollider2D   collisions;

    public float    activationTime;
    public float    deactivationTime;
    bool            active = false;
    bool            switched = false;
    float           switchIn;
    float           activeIn;

    // 0 - Off / 1- OffToOn / 2- On / 3- OnToOff
    int             state = 0;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        collisions = GetComponent<BoxCollider2D>();
        collisions.enabled = false;
    }

    public bool isActive() { return active; }
    float getActiveTimer()
    {
        return active ? activationTime : deactivationTime;
    }

	// Update is called once per frame
	void Update () {
        switchIn += Time.deltaTime;
        activeIn = switchIn;

        if (switchIn >= getActiveTimer() - 0.7f)
        {
            if (state == 0)
            {
                animator.SetTrigger("OffToOn");
                animator.ResetTrigger("OffToOn");
                collisions.enabled = false;
                state = 1;
            }
            else if (state == 2)
            {
                animator.SetTrigger("OnToOff");
                animator.ResetTrigger("OnToOff");
                collisions.enabled = false;
                state = 3;
            }
            if (activeIn >= getActiveTimer() - 0.2f && !switched)
            {
                switched = true;
                active = !active;
            }

        }
        if (switchIn >= getActiveTimer())
        {
            if (state == 1)
            {
                animator.SetTrigger("OffToOn");
                collisions.enabled = true;
                state = 2;
            }
            else if (state == 3)
            {
                animator.SetTrigger("OnToOff");
                collisions.enabled = false;
                state = 0;
            }
            switchIn = 0f;
            activeIn = 0f;
            switched = false;
        }
    }
}

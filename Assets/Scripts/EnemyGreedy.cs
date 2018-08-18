using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGreedy : MoveableObject {

    public float slowAnimationBy = 3f;
    public float maxScale = 1.5f;
    private float scaleUp = 1f;
    // Use this for initialization
    void Start () {
    }

    void Update () {
        if(CanMove()) {
            if (transform.localScale.x >= maxScale)
                scaleUp = -1f;
            else if (transform.localScale.x <= 0.3f)
                scaleUp = 1f;
            float tmp = (Time.deltaTime / slowAnimationBy) * scaleUp;
            transform.localScale = new Vector3(transform.localScale.x + tmp, transform.localScale.y + tmp, transform.localScale.z + tmp); 
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
  public float speed;

	// Use this for initialization
	void Start () {

	}
	// Update is called once per frame
	void Update () {
    this.transform.Rotate(0,0, Input.GetAxis("Horizontal") * speed);
	}
}

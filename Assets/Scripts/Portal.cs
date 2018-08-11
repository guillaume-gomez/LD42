using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

  public GameObject portalTarget;

  void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.tag == "Player") {
      other.transform.position = portalTarget.transform.position;
    }
  }
}

using UnityEngine;
using System.Collections;

public class MoveableObject : MonoBehaviour {

  // Use this for initialization
  void Start () {
  }

  void Update () {
  }

  public virtual bool CanMove() {
    return !GameManager.instance.doingSetup;
  }

}
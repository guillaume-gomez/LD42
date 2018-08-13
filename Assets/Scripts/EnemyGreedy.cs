using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGreedy : MonoBehaviour {

  public float lengthAnimation = 2.0f;
  public float maxScale = 1.5f;
  private float originalScale = 0.7f;
  private float currentScale;
  private int direction = 1;
  private float step;
  protected Rigidbody2D rb2D;


  // Use this for initialization
  void Start () {
    rb2D = gameObject.GetComponent<Rigidbody2D>();
    currentScale = originalScale;
    step = (maxScale - originalScale) / (lengthAnimation * 100);
  }

  // TO DO Taking acount of timer and Time.deltaTime
  void Update () {
    if(!GameManager.instance.doingSetup) {
      currentScale += direction * step;
      if(currentScale > maxScale || currentScale < originalScale) {
        direction = - direction;
      }
      transform.localScale = new Vector3(currentScale, currentScale, transform.localScale.z);
    }
  }

}
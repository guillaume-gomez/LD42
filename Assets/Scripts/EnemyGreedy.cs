using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGreedy : MonoBehaviour {

  public float lengthAnimation = 1.0f;
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
  }

  void Update () {
    if(!GameManager.instance.doingSetup) {
      currentScale += direction * (Time.deltaTime * (maxScale - originalScale))/ lengthAnimation;
      if(currentScale > maxScale || currentScale < originalScale) {
        direction = - direction;
      }
      Debug.Log(currentScale);
      transform.localScale = new Vector3(currentScale, currentScale, transform.localScale.z);
    }
  }

}
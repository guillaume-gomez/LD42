using UnityEngine;
using System.Collections;

public class CameraUnzoom : MonoBehaviour {

  public float lengthAnimation = 1.0f;
  public float maxSize = 7f;
  public bool moveToTarget = false;
  public Vector3 target = new Vector3(0.0f, 0.0f, 0.0f);
  private float originalSize;
  private float currentSize;
  private float distanceTarget;
  private Camera mainCamera;


  // Use this for initialization
  void Start () {
    mainCamera = GetComponent<Camera>();
    originalSize = mainCamera.orthographicSize;
    currentSize = originalSize;
    distanceTarget = Vector3.Distance(transform.position, target);
  }

  void Update () {
    if(GameManager.instance.isTransiting) {
      currentSize += (Time.deltaTime * (maxSize - originalSize))/ lengthAnimation;
      if(currentSize > maxSize) {
        return;
      }
      mainCamera.orthographicSize = currentSize;

      //
      if(moveToTarget) {
        float step = Time.deltaTime * (distanceTarget / lengthAnimation);
        transform.position = Vector3.MoveTowards(transform.position, target, step);
      }
    }
  }

}
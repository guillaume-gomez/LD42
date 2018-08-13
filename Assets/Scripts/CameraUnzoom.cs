using UnityEngine;
using System.Collections;

public class CameraUnzoom : MonoBehaviour {

  public float lengthAnimation = 1.0f;
  public float maxSize = 7f;
  private float originalSize;
  private float currentSize;
  private Camera camera;


  // Use this for initialization
  void Start () {
    camera = GetComponent<Camera>();
    originalSize = camera.orthographicSize;
    currentSize = originalSize;
  }

  void Update () {
    if(GameManager.instance.isTransiting) {
      currentSize += (Time.deltaTime * (maxSize - originalSize))/ lengthAnimation;
      if(currentSize > maxSize) {
        return;
      }
      camera.orthographicSize = currentSize;
    }
  }

}
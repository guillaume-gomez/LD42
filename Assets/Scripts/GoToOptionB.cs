using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GoToOptionB : MonoBehaviour {

  // Use this for initialization
  void Start () {
    Invoke("Play", 10f);
  }

  // Update is called once per frame
  void Update () {
     if(Input.GetButtonDown("Submit")) {
       Play();
     }
  }

  void Play() {
    SceneManager.LoadScene(2);
  }
}

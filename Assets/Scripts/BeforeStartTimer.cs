using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class BeforeStartTimer : MonoBehaviour {

  public int timeLeft = 25;
  public Text countdown;

  void Start () {
    timeLeft = (int) GameManager.instance.levelStartDelay;
    StartCoroutine("LoseTime");
    Time.timeScale = 1; //Just making sure that the timeScale is right
  }

  void Update () {
    countdown.text = ("" + timeLeft); //Showing the Score on the Canvas
  }

  //Simple Coroutine
  IEnumerator LoseTime()
  {
    while (true) {
      yield return new WaitForSeconds (1);
      timeLeft--;
    }
  }
}
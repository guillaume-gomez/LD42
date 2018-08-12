using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : Timer {

  public float timeLeft = 10.0f;


  // Update is called once per frame
  public override void Update () {
    if(started) {
      float t = timeLeft - (Time.time - startTimer);

      string minutes = (((int) t) / 60).ToString();
      float secondsFloat = (t % 60);
      string seconds = secondsFloat.ToString("f2");

      if(secondsFloat < 0.0f) {
        timerText.text = minutes + ":  0.00";
        GameManager.instance.GameOver("timer_over");
        StopTimer();
        return;
      }
      timerText.text = minutes + ": " + seconds;
    }
  }

  public void AddTime(float seconds) {
    timeLeft += seconds;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour {

  private Text text;
  public float interval = 0.5f;
  public float maxAlpha = 1.0f;
  public float minAlpha = 0.0f;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
    //StartBlinking();
	}

  IEnumerator Blink() {
    while(true)
    {
      if(minAlpha.ToString() == text.color.a.ToString()) {
        text.color = new Color(text.color.r, text.color.g, text.color.b, maxAlpha);
        yield return new WaitForSeconds(interval);

      } else if (maxAlpha.ToString() == text.color.a.ToString()) {
        text.color = new Color(text.color.r, text.color.g, text.color.b, minAlpha);
        yield return new WaitForSeconds(interval);
      }
    }
  }

  public void StartBlinking() {
    StopCoroutine ("Blink");
    StartCoroutine("Blink");
  }

	// Update is called once per frame
	public void StopBlinking () {
		StopCoroutine("Blink");
	}
}

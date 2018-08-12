using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class MusicControler : MonoBehaviour {

  public AudioMixerSnapshot firstLayer;
  public AudioMixerSnapshot secondLayer;

  public AudioClip[] stings;
  public AudioSource stingSounce;
  public float bpm = 128;

  private float transitionIn;
  private float transitionOut;
  private float quarterNote;
	// Use this for initialization
	void Start () {
		 quarterNote = 60 / bpm;
     // the transition speed is here quick 'In' and fast in 'Out'
     transitionIn = quarterNote;
     transitionOut= quarterNote * 32;

     Invoke("SwitchTo", 20f);
	}

  void SwitchTo() {
    secondLayer.TransitionTo(transitionIn);
  }

  void SwitchBack() {
    firstLayer.TransitionTo(transitionOut);
  }
}

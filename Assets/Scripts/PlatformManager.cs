﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {
    // Specify the layer on wich the player actually is
    private uint playerLayer = 0;
    private Layer[] layers;

	// Use this for initialization
	void Start () {
        layers = GetComponentsInChildren<Layer>();
	}

    public void SetPlayerLayer(uint layerIndex) {
        playerLayer = layerIndex;
        if(layers != null) {
            // what a wonderful patch don't you think ? :)
            if(layerIndex < layers.Length) {
                GameManager.instance.SetPlayerLayer(layers[playerLayer].rules.layerType , layerIndex);
            }

            for (uint i = 0; i < layers.Length; i++) {
                if (layers[i].index == playerLayer)
                    layers[i].Activate();
                else
                    layers[i].Deactivate();
            }
        }
    }
}

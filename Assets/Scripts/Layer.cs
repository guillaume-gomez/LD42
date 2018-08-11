using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LayerRulesAndState
{
    // Layers plateforms respawn rules
    public float timeToRespawn;
    public float minRespawnSpeed;
    public float timeToDecreaseSpawnTime;
    public float decreaseStep;

    // Layer Start State
    float lastRespawn = 0.0f;
    float lastUpdate = 0.0f;

    public void UpdateState()
    {
        lastUpdate += Time.deltaTime;
        lastRespawn += Time.deltaTime;

        if (lastUpdate >= timeToDecreaseSpawnTime)
        {
            timeToRespawn -= decreaseStep;
            if (minRespawnSpeed >= timeToRespawn)
                timeToRespawn = minRespawnSpeed;
            lastUpdate = 0.0f;
        }
    }

    public bool ShouldRespawn()
    {
        return lastRespawn >= timeToRespawn;
    }

    public void AsRespawn()
    {
        lastRespawn = 0.0f;
    }
}

public class Layer : MonoBehaviour {

    // The Plateforms of the layer
    public IAbstractPlatform[] plateforms;
    // Rules
    public LayerRulesAndState rules;
    public bool active;

    // Use this for initialization
    void Start () {
        plateforms = GetComponentsInChildren<IAbstractPlatform>();
	}
	
	// Update is called once per frame
	void Update () {
        rules.UpdateState();

        if (active && rules.ShouldRespawn()) {
            int toBeActivated = Mathf.RoundToInt(Random.Range(0, GetInactivePlateformsNumber()));

            plateforms[toBeActivated].Activate();
            rules.AsRespawn();
        }
	}

    uint GetInactivePlateformsNumber() {
        uint ret = 0;

        foreach (IAbstractPlatform plat in plateforms)
            if (!plat.active)
                ++ret;
        return ret;
    }
    public void Activate() { active = true; }
    public void Deactivate() { active = false; }
}

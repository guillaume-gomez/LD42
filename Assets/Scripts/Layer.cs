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

    // On malus/bonus on layer
    public uint plateformNbOnMalus;

    // Layer Start State
    float lastRespawn = 0.0f;
    float lastUpdate = 0.0f;

    public LayerTypeEnum layerType;

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
    public uint index;
    public bool active;
    public float rotationSpeed;

    // Use this for initialization
    void Start () {
        plateforms = GetComponentsInChildren<IAbstractPlatform>();
	}

	// Update is called once per frame
	void Update () {
        transform.Rotate(0,0,rotationSpeed);

        if (active)
        {
            rules.UpdateState();

            if (rules.ShouldRespawn())
            {
                ArrayList plateformIndexes = GetInactivePlateformsIndexes();

                if (plateformIndexes.Count > 0)
                {
                    int toBeActivated = Mathf.RoundToInt(Random.Range(0, plateformIndexes.Count));

                    plateforms[(uint)plateformIndexes[toBeActivated]].Activate();
                    rules.AsRespawn();
                }
            }
        }
	}

    public void onMalus()
    {
        uint triggered = 0;

        foreach (IAbstractPlatform plat in plateforms)
        {
            if (triggered < rules.plateformNbOnMalus &&
                !plat.active)
            {
                plat.Activate();
                ++triggered;
            }
        }
    }

    ArrayList GetInactivePlateformsIndexes() {
        uint index = 0;
        ArrayList ret = new ArrayList();

        foreach (IAbstractPlatform plat in plateforms) {
            if (!plat.active)
                ret.Add(index);
            ++index;
        }
        return ret;
    }
    public void Activate() { active = true; }
    public void Deactivate() { active = false; }
}

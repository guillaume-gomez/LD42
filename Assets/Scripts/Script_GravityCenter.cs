using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_GravityCenter : MonoBehaviour {

    private List<GameObject> elems = new List<GameObject>();
    private float pullForce = 100000f;

    // Use this for initialization
    void Start () {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Player");
        int i = 0;
        while (i < items.Length)
        {
            elems.Add(items[i].gameObject);
            ++i;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        foreach (GameObject elem in elems)
        {
            Vector3 forceDirection = transform.position - elem.transform.position;
            elem.GetComponent<Rigidbody2D>().AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
            Vector3 forceDirectionN = forceDirection.normalized;
            if (forceDirectionN.x > 0)
                elem.transform.eulerAngles = new Vector3(0,0, Mathf.Rad2Deg * Mathf.Acos(-forceDirectionN.y / (Mathf.Sqrt(Mathf.Pow(forceDirectionN.x, 2) + Mathf.Pow(forceDirectionN.y, 2)))));
            else
                elem.transform.eulerAngles = new Vector3(0, 0, -Mathf.Rad2Deg * Mathf.Acos(-forceDirectionN.y / (Mathf.Sqrt(Mathf.Pow(forceDirectionN.x, 2) + Mathf.Pow(forceDirectionN.y, 2)))));
        } 

    }
}

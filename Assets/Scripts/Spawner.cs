using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public float spawnTimePeriod = 5.0f;
    public GameObject prototype;
    public GameObject boundObj;

    float lastSpawnTime = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time > (lastSpawnTime + spawnTimePeriod))
        {
            spawnAcorn();
            lastSpawnTime = Time.time;
        }
	}

    void spawnAcorn()
    {
        ((GameObject)Instantiate(prototype, getRandomPos(), Quaternion.identity)).SetActive(true); 
    }

    private Vector3 getRandomPos()
    {
        Vector3 max  = boundObj.GetComponent<Renderer>().bounds.max;
        return new Vector3(Random.Range(-max.x, max.x), Random.Range(-max.y, max.y), 0);
    }
}

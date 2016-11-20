using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

    public float startTextTime = 3.0f;
    public GameObject startText;
    UnityEngine.UI.Text UIText;

    float startTime;
    // Use this for initialization
    void Start () {
        startTime = Time.time;
        UIText = startText.GetComponent<UnityEngine.UI.Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if(Time.time > (startTime + startTextTime))
        {
            UIText.text = "";
        }

        if(GameObject.FindGameObjectsWithTag("Squirrel").Length == 0)
        {
            UIText.text = "You are winner weener chicken dinner!";
        }
	}
}

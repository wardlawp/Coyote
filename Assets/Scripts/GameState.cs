using UnityEngine;

using System.Collections;

public class GameState : MonoBehaviour {

    public float startTextTime = 3.0f;
    public GameObject startText;
    UnityEngine.UI.Text UIText;

    bool ended;
    float startTime;
    float startNextLevel;
    // Use this for initialization
    void Start () {
        ended = false;
        startNextLevel = 0.0f;
        startTime = Time.time;
        UIText = startText.GetComponent<UnityEngine.UI.Text>();
    }
	
	// Update is called once per frame
    void Update () {
        if(!ended  && (Time.time > (startTime + startTextTime)))
        {
            UIText.text = "";
        }

        if(GameObject.FindGameObjectsWithTag("Squirrel").Length == 0 && !ended)
        {
            ended = true;
            UIText.text = "You are have complete! Very done." + System.Environment.NewLine + System.Environment.NewLine + "Make self for next level...";
            startNextLevel = Time.time + 8.0f;
        }

        if((startNextLevel != 0.0f) && (Time.time > startNextLevel))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("UBC2");
        }
    }
}

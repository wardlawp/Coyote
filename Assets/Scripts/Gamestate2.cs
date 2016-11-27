using UnityEngine;

using System.Collections;

public class Gamestate2 : MonoBehaviour
{

    public GameObject text;
    bool ended;
    UnityEngine.UI.Text UIText;

    void Start()
    {
        ended = false;
  
        UIText = text.GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
       

        if (GameObject.FindGameObjectsWithTag("Magpie").Length == 0 && !ended)
        {
            ended = true;
            UIText.text = "You accomplish!" + System.Environment.NewLine + System.Environment.NewLine + "You may now die happy.";

        }
    }
}

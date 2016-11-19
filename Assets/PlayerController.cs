using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //Modifiable settings
    //Standard
    public float translatePerSec = 3.0f;
    public float rotatePerSec = 270.0f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        float speed = 0.0f;
        float turn = 0.0f;

        if (Input.GetKey(KeyCode.W))
            speed += translatePerSec;

        if (Input.GetKey(KeyCode.A))
            turn += rotatePerSec;

        if (Input.GetKey(KeyCode.S))
            speed -= translatePerSec;

        if (Input.GetKey(KeyCode.D))
            turn -= rotatePerSec;

        transform.Translate(new Vector3(0, speed * Time.deltaTime));
        transform.Rotate(Vector3.forward * (turn * Time.deltaTime)); 
    }

    /// <summary>
    /// On collision with a Squirrel eat it
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Squirrel")
        {
            Destroy(coll.gameObject);
        }
    }
}

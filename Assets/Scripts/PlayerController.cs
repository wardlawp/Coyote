using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //Modifiable settings
    //Standard
    public float translatePerSec = 3.0f;
    public float rotatePerSec = 270.0f;
    public float biteTime = 0.5f;

    private float remainingBiteTime;

    GameObject noms;

    Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        remainingBiteTime = 0.0f;
        noms = null;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (remainingBiteTime > 0.0f)
        {
            handleBiting();
            return;
        }

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

        anim.SetBool("running", ((speed != 0) || (turn != 0)));

        transform.Translate(new Vector3(0, speed * Time.deltaTime));
        transform.Rotate(Vector3.forward * (turn * Time.deltaTime)); 
    }

    void handleBiting()
    {
        remainingBiteTime -= Time.deltaTime;

        if(remainingBiteTime < 0.0f)
        {
            anim.SetBool("biting", false);
            Destroy(noms);
            noms = null;
        }
    }


    /// <summary>
    /// On collision with a Squirrel eat it
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        if((coll.gameObject.tag == "Squirrel" || coll.gameObject.tag == "Magpie" ) && noms == null )
        {
            noms = coll.gameObject;
            anim.SetBool("biting", true);
            remainingBiteTime = biteTime;
            noms.SendMessage("bitten");
        }
    }
}

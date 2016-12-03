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

    AudioSource sound;
    Animator anim;
    // Use this for initialization
    void Start () {
        sound = GetComponent<AudioSource>();
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger)
            return;

        if((other.gameObject.tag == "Squirrel" || other.gameObject.tag == "Magpie" ) && noms == null )
        {
            noms = other.gameObject;
            sound.Play();
            anim.SetBool("biting", true);
            remainingBiteTime = biteTime;
            noms.SendMessage("bitten");
        }
    }


}

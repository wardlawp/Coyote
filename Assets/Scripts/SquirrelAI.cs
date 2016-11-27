using UnityEngine;
using System.Collections;

public class SquirrelAI : PrayAI {

    //Public attributes
    public float predatorThreshold = 2.0f;
   

    enum state { Chilling, GettingNut, Fleeing, Dieing };
    state currentState;

    //GettingNut State
    GameObject targetNut;

    bool ateNut;


    #region Public

    new void Start()
    {
        base.Start();
        currentState = state.Fleeing;
        
        ateNut = false;
    }

    public void bitten()
    {
        currentState = state.Dieing;
        anim.SetBool("dieing", true);

        if(homeTree != null)
        {
            homeTree.GetComponent<Tree>().free();
        }
    }

    void Update()
    {
        if (currentState == state.Chilling) handleChilling();
        else if (currentState == state.GettingNut) handleGettingNut();
        else if (currentState == state.Fleeing) handleFleeing();
        else if (currentState == state.Dieing) handleDieing();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        string tag = other.gameObject.tag;
        if (tag == "Nut")
        {
            ateNut = true;
            Destroy(other.gameObject);
        }
    }

    #endregion

    #region state machine

    void handleDieing()
    {
        transform.Rotate(Vector3.forward * 50 * Time.deltaTime);
    }

    /// <summary>
    /// Handle actions in the chilling state. When Chilling the Squirrel is looking for nuts
    /// </summary>
    private void handleChilling()
    {
        var nut = findNut();
        bool dog2Close = (doggoDistance() < predatorThreshold);

        if ((nut != null) && ! dog2Close)
        {
            transitionToGettingNut(nut);
        }
    }

    private void transitionToGettingNut(GameObject nut)
    {
        homeTree.GetComponent<Tree>().free();
        anim.SetBool("running", true);
        ateNut = false;
        targetNut = nut;
        currentState = state.GettingNut;
    }

    private void handleGettingNut()
    {
    
        if (doggoDistance() < predatorThreshold) {
            transitionToFleeing();
            return;
        }
        else if (!ateNut && targetNut != null)
        {
            gotoObj(targetNut);
        }
        else
        {
            goToHomeTree();
        }
        
    }

    private void transitionToFleeing()
    {
        homeTree = findHomeTree();
        currentState = state.Fleeing;
    }

    private void handleFleeing()
    {
        //Go back to tree
        goToHomeTree();
    }

    #endregion

    #region private helpers

    

    private void goToHomeTree()
    {
        if (atTree(homeTree))
        {
            anim.SetBool("running", false);
            currentState = state.Chilling;
        }
        else gotoObj(homeTree);
    }

    

    private GameObject findNut()
    {
        return findClosestObj("Nut");
    }
    #endregion
}

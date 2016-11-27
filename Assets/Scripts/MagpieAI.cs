using UnityEngine;
using System.Collections;

public class MagpieAI : PrayAI
{

    //Public attributes
    public float leaveTreeThreshold = 5.0f;
    public float turnSpeedMax = 120.0f;

    //References to Two colliders
    BoxCollider2D bodyCollider;
    PolygonCollider2D viewCollider;

    public enum state { Chilling, GoingToGarbage, Munching, Fleeing, Dieing };
    public state currentState;

    float startFlyDelay;
    float chillInTreeUntil;
    float nextTurnTime;
    bool atGarbage;
    GameObject targetGarbage;

    float currentTurnSpeed;


    //Fleeing
    bool seeDoggo;

    #region Public

    new void Start()
    {
        base.Start();

        currentState = state.Fleeing;
        anim.SetBool("flying", true);

        seeDoggo = false;
        atGarbage = false;

        nextTurnTime = calculateNextTurnTime();
        currentTurnSpeed = 0.0f;

        bodyCollider = GetComponent<BoxCollider2D>();
        viewCollider = GetComponent<PolygonCollider2D>();
    }

    public void ahhhhh()
    {
        if(currentState != state.Dieing)
            transitionToFleeing();
    }

    public void bitten()
    {
        currentState = state.Dieing;
        anim.SetBool("dieing", true);

        GameObject[] otherMagPies = GameObject.FindGameObjectsWithTag("Magpie");

        foreach(GameObject magPie in otherMagPies)
        {
            magPie.SendMessage("ahhhhh");
        }

        if (homeTree != null)
        {
            homeTree.GetComponent<Tree>().free();
        }

    }
    void Update()
    {
        if (currentState == state.Chilling) handleChilling();
        else if (currentState == state.GoingToGarbage) handleGoingToGarbage();
        else if (currentState == state.Munching) handleMunching();
        else if (currentState == state.Fleeing) handleFleeing();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
        atGarbage = (bodyCollider.bounds.Intersects(other.bounds) && other.gameObject.tag == "Garbage");
        seeDoggo = (viewCollider.bounds.Intersects(other.bounds) && other.gameObject.tag == "Player");
    }

    #endregion

    #region state machine

    /// <summary>
    /// Handle actions in the chilling state. When Chilling the Squirrel is looking for nuts
    /// </summary>

    private void handleChilling()
    {
        //Look for garbage to eat
        var garbage = findGarbage();
        //If found then transition to going to garbage

        bool dog2Close = (doggoDistance() < leaveTreeThreshold);

        if ((garbage != null) && !dog2Close  && chillInTreeUntil < Time.time)
        {
            startFlyDelay = Time.time + 0.3f;
            transitionToGoingToGarbage(garbage);
        }
    }

    private void transitionToGoingToGarbage(GameObject garbage)
    {
        startFlyDelay = Time.time + 0.3f;
        anim.SetBool("flying", true);
        homeTree.GetComponent<Tree>().free();
        targetGarbage = garbage;
        currentState = state.GoingToGarbage;
    }

    private void handleGoingToGarbage()
    {
        if (seeDoggo)
        {
            transitionToFleeing();
            return;
        }
        else if(atGarbage || (Vector2.Distance(transform.position, targetGarbage.transform.position) < 1.0f) )
        {
            transitionToMunching();
        }
        else if (Time.time > startFlyDelay)
        {
            gotoObj(targetGarbage);
        }
    }

    private void transitionToMunching()
    {
        anim.SetBool("flying", false);
        currentState = state.Munching;
    }

    private void handleMunching()
    {
        if(seeDoggo)
        {
            startFlyDelay = Time.time + 0.3f;
            transitionToFleeing();
            return;
        }
        if(Time.time > nextTurnTime)
        {
            if(currentTurnSpeed == 0.0f)
            {
                currentTurnSpeed = Random.Range(-turnSpeedMax, turnSpeedMax);
            }
            else
            {
                currentTurnSpeed = 0.0f;
            }
           
            nextTurnTime = calculateNextTurnTime();
        }

        transform.Rotate(Vector3.forward * (currentTurnSpeed * Time.deltaTime));
    }

    private void transitionToFleeing()
    {
        anim.SetBool("flying", true);
        seeDoggo = false;
        currentState = state.Fleeing;

        homeTree = findHomeTree();

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
            chillInTreeUntil = calculateNextTurnTime();
            anim.SetBool("flying", false);
            currentState = state.Chilling;
        }
        else if(Time.time > startFlyDelay) gotoObj(homeTree);
    }

    private GameObject findGarbage()
    {
        return findClosestObj("Garbage");
    }

    private float calculateNextTurnTime()
    {
        return Time.time + Random.Range(1.0f, 6.0f);
    }
    #endregion
}

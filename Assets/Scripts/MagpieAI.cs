using UnityEngine;
using System.Collections;

public class MagpieAI : MonoBehaviour
{

    //Public attributes
    public float speed = 9.0f;
    public float leaveTreeThreshold = 5.0f;
    public float turnSpeedMax = 120.0f;

    //References to Two colliders
    BoxCollider2D bodyCollider;
    PolygonCollider2D viewCollider;

    enum state { Chilling, GoingToGarbage, Munching, Fleeing };
    state currentState;

    float nextTurnTime;
    bool atGarbage;
    GameObject target;
    GameObject homeTree;
    float turnSpeed;
    private GameObject doggo;

    //Fleeing
    GameObject targetTree;
    bool seeDoggo;

    #region Public

    void Start()
    {
        currentState = state.Fleeing;
        seeDoggo = false;
        atGarbage = false;
        targetTree = findHomeTree();
        nextTurnTime = calculateNextTurnTime();
        doggo = GameObject.FindGameObjectWithTag("Player");
        bodyCollider = GetComponent<BoxCollider2D>();
        viewCollider = GetComponent<PolygonCollider2D>();
        homeTree = findHomeTree();
        turnSpeed = 0.0f;
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

        if ((garbage != null) && !dog2Close)
        {
            transitionToGoingToGarbage(garbage);
        }
    }

    private void transitionToGoingToGarbage(GameObject garbage)
    {
        target = garbage;
        currentState = state.GoingToGarbage;
    }

    private void handleGoingToGarbage()
    {
        if (seeDoggo)
        {
            transitionToFleeing();
            return;
        }
        else if(atGarbage)
        {
            transitionToMunching();
        }
        else
        {
            gotoObj(target);
        }
    }

    private void transitionToMunching()
    {
        currentState = state.Munching;
    }

    private void handleMunching()
    {
        if(seeDoggo)
        {
            transitionToFleeing();
            return;
        }
        if(Time.time > nextTurnTime)
        {
            if(turnSpeed == 0.0f)
            {
                turnSpeed = Random.Range(-turnSpeedMax, turnSpeedMax);
            }
            else
            {
                turnSpeed = 0.0f;
            }
           
            nextTurnTime = calculateNextTurnTime();
        }

        transform.Rotate(Vector3.forward * (turnSpeed * Time.deltaTime));
    }

    private void transitionToFleeing()
    {
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

    private float angleBetweenVector2(Vector2 from, Vector2 to)
    {
        Vector2 diference = to - from;
        float sign = (to.y < from.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign - 90;
    }

    private void goToHomeTree()
    {
        if (atTree()) currentState = state.Chilling;
        else gotoObj(homeTree);
    }

    private bool atTree()
    {
        return Vector2.Distance(homeTree.transform.position, transform.position) < 0.1;
    }

    private void gotoObj(GameObject obj)
    {
        transform.eulerAngles = new Vector3(0, 0, angleBetweenVector2(transform.position, obj.transform.position));
        transform.Translate(new Vector3(0, speed * Time.deltaTime));
    }

    private GameObject findHomeTree()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");

        if (trees.Length == 0)
        {
            return null;
        }
        else
        {
            return findClosest(trees);
        }
    }

    private GameObject findClosest(GameObject[] objs)
    {
        GameObject closestobj = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject obj in objs)
        {
            float distance = Vector2.Distance(obj.transform.position, transform.position);

            if (distance < closestDistance)
            {
                closestobj = obj;
                closestDistance = distance;
            }
        }

        return closestobj;
    }

    private GameObject findGarbage()
    {
        GameObject[] nuts = GameObject.FindGameObjectsWithTag("Garbage");

        if (nuts.Length == 0)
        {
            return null;
        }
        else
        {
            return findClosest(nuts);
        }
    }

    private float doggoDistance()
    {
        return Vector2.Distance(transform.position, doggo.transform.position);
    }


    private float calculateNextTurnTime()
    {
        return Time.time + Random.Range(1.0f, 6.0f);
    }
    #endregion
}

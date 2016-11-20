﻿using UnityEngine;
using System.Collections;

public class SquirrelAI : MonoBehaviour {

    //Public attributes
    public float predatorThreshold = 2.0f;
    public float speed = 3.5f;

    enum state { Chilling, GettingNut, Fleeing, Dieing };
    state currentState;

    //GettingNut State
    GameObject target;
    bool ateNut;

    //Fleeing
    GameObject homeTree;
    private GameObject doggo;

    Animator anim;

    #region Public

    void Start()
    {
        anim = GetComponent<Animator>();
        currentState = state.Fleeing;
        doggo = GameObject.FindGameObjectWithTag("Player");
        ateNut = false;
        homeTree = findHomeTree();
    }

    public void bitten()
    {
        currentState = state.Dieing;
        anim.SetBool("dieing", true);
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
        transform.Rotate(Vector3.forward * 120*Time.deltaTime);
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
        anim.SetBool("running", true);
        ateNut = false;
        target = nut;
        currentState = state.GettingNut;
    }

    private void handleGettingNut()
    {
    
        if (doggoDistance() < predatorThreshold) {
            transitionToFleeing();
            return;
        }
        else if (!ateNut && target != null)
        {
            gotoObj(target);
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

    private float angleBetweenVector2(Vector2 from, Vector2 to)
    {
        Vector2 diference = to - from;
        float sign = (to.y < from.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign - 90;
    }

    private void goToHomeTree()
    {
        if (atTree())
        {
            anim.SetBool("running", false);
            currentState = state.Chilling;
        }
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

    private GameObject findNut()
    {
        GameObject[] nuts = GameObject.FindGameObjectsWithTag("Nut");

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

    #endregion
}

using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour {

    bool occupied = false;

	public bool available()
    {
        return !occupied;
    }

    public bool book()
    {
        if (!occupied)
        {
            occupied = true;
            return true;
        }

        return false;
    }

    public bool free()
    {
        occupied = false;
        return true;
    }

}

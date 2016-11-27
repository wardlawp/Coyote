using UnityEngine;
using System.Collections;

abstract public class PrayAI : MonoBehaviour
{

    public float speed = 3.5f;

    protected Animator anim;
    protected GameObject doggo;
    protected GameObject homeTree;

    protected void Start()
    {
        doggo = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        homeTree = findHomeTree();
    }

    /// <summary>
    /// Find the closet object with a certain tag
    /// </summary>
    /// <param name="tag"></param>
    /// <returns>GameObject</returns>
    protected GameObject findClosestObj(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);

        if (objs.Length == 0)
        {
            return null;
        }
        else
        {
            return filterClosest(objs)[0];
        }
    }

    /// <summary>
    /// Filter a list of gameobjects using select sort so that they are sorted by closest to furthest away
    /// </summary>
    /// <param name="objs"></param>
    /// <returns></returns>
    protected GameObject[] filterClosest(GameObject[] objs)
    {
        /* a[0] to a[n-1] is the array to sort */
        int i, j;
        int n = objs.Length;

        /* advance the position through the entire array */
        /*   (could do j < n-1 because single element is also min element) */
        for (j = 0; j < n - 1; j++)
        {
            /* find the min element in the unsorted a[j .. n-1] */

            /* assume the min is the first element */
            int iMin = j;
            /* test against elements after j to find the smallest */
            for (i = j + 1; i < n; i++)
            {
                float distanceI = Vector2.Distance(objs[i].transform.position, transform.position);
                float distanceImin = Vector2.Distance(objs[iMin].transform.position, transform.position);
                /* if this element is less, then it is the new minimum */
                if (distanceI < distanceImin)
                {
                    /* found new minimum; remember its index */
                    iMin = i;
                }
            }

            if (iMin != j)
            {
                swap(ref objs[j], ref objs[iMin]);
            }
        }

        return objs;
    }

    static void swap(ref GameObject a, ref GameObject b)
    {
        GameObject temp = a;
        a = b;
        b = temp;
    }

    /// <summary>
    /// Determine how close the player is
    /// </summary>
    /// <returns></returns>
    protected float doggoDistance()
    {
        return Vector2.Distance(transform.position, doggo.transform.position);
    }

    protected void gotoObj(GameObject obj)
    {
        transform.eulerAngles = new Vector3(0, 0, angleBetweenVector2(transform.position, obj.transform.position));
        transform.Translate(new Vector3(0, speed * Time.deltaTime));
    }

    private float angleBetweenVector2(Vector2 from, Vector2 to)
    {
        Vector2 diference = to - from;
        float sign = (to.y < from.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign - 90;
    }

    protected bool atTree(GameObject tree)
    {
        return Vector2.Distance(tree.transform.position, transform.position) < 0.1;
    }

    protected GameObject findHomeTree()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Tree");
       
        if (objs.Length != 0)
        {

            foreach (GameObject treeObj in filterClosest(objs))
            {
                
                Tree tree = treeObj.GetComponent<Tree>();
                
                if (tree.available())
                {
                    tree.book();
                    return treeObj;
                }
            }
        }

        return null;
    }

}

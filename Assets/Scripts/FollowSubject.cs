using UnityEngine;
using System.Collections;

public class FollowSubject : MonoBehaviour {

    public GameObject subject;

	
// Update is called once per frame
    void Update () {
        Vector3 subjectPos = subject.transform.position;
        transform.position = new Vector3(subjectPos.x, subjectPos.y, -10);
    }
}

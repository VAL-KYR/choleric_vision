using UnityEngine;
using System.Collections;

public class aimAt : MonoBehaviour {

    public GameObject target;

	// Update is called once per frame
	void LateUpdate () {
        if (target != null)
            transform.LookAt(2 * transform.position - target.transform.position);
	}
}

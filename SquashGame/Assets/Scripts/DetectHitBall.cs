using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHitBall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ball")) {
            //other.gameObject.GetComponent<Rigidbody>().AddForce(this.GetComponent<Rigidbody>().transform.position);
            other.gameObject.GetComponent<Rigidbody>().velocity = (other.gameObject.GetComponent<Rigidbody>().velocity+this.GetComponent<Rigidbody>().velocity)*10;
        }
    }
}

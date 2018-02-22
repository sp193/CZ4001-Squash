using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour {
    private Vector3 oldvel;
    private Rigidbody myRigidbody;
    private Vector3 prevSpd;
	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        oldvel = myRigidbody.velocity;
	}
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint cp = collision.contacts[0];

        myRigidbody.velocity = Vector3.Reflect(oldvel, cp.normal);
        prevSpd = myRigidbody.velocity;
        myRigidbody.velocity += cp.normal * 10.0f;

    }
}

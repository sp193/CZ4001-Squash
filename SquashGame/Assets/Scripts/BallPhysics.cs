using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour {
    private Vector3 oldvel;
    private Rigidbody myRigidbody;
    private Vector3 prevSpd;
    private Vector3 startPos;
    private int bouncesOffGround;

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody>();
        startPos = myRigidbody.position;
        bouncesOffGround = 0;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 position = myRigidbody.position;
        oldvel = myRigidbody.velocity;

        //Check if out of bounds.
        if((position.x < -50 || position.x >= 50)
            || (position.y < -50 || position.y >= 50)
            || (position.z < -50 || position.z >= 50))
        {
            FindObjectOfType<GameController>().GameOver();
        }
	}

    //Handle collisions.
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint cp = collision.contacts[0];
        GameObject other = cp.otherCollider.gameObject;

        myRigidbody.velocity = Vector3.Reflect(oldvel, cp.normal);
        prevSpd = myRigidbody.velocity;
        myRigidbody.velocity += cp.normal * 10.0f;

        if (other.CompareTag("Wall"))
        {
            FindObjectOfType<GameController>().AddScore(1);
        }
 
        if (other.CompareTag("Ground"))
        {
            bouncesOffGround++;

            if (bouncesOffGround >= 2)
            {
                FindObjectOfType<GameController>().GameOver();
            }
        }
        else
            bouncesOffGround = 0;
    }

    public void Reset()
    {
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
        myRigidbody.MovePosition(startPos);
        bouncesOffGround = 0;

        Debug.Log("BC Reset");
    }
}

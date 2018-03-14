using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;
    private Rigidbody rb;
    private Vector3 startPos;
    private bool isFrozen;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        startPos = rb.position;
        isFrozen = false;
    }

    public void Reset()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.MovePosition(startPos);
        isFrozen = false;
        Debug.Log("PC Reset");
    }

    public void SetFreeze(bool freeze)
    {
        isFrozen = freeze;
    }

    // Update is called once per frame
    void Update () {
		
	}

    //FixedUpdate() runs off its own timer, independent of the refresh rate. It is called more often than Update()
    void FixedUpdate()
    {
        float moveHorizontal;

        if (!isFrozen)
        {
            moveHorizontal = Input.GetAxis("Horizontal");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
            rb.AddForce(movement * speed);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketController : MonoBehaviour {

    private bool swinging, swingingBack;
    private int rotationTickCount;
    private Rigidbody rb;
    public int rotationSpeed;
    private int rotationTicks;

    // Use this for initialization
    void Start () {
        swinging = false;
        swingingBack = false;
        rb = GetComponent<Rigidbody>();
        rotationTicks = 90 / rotationSpeed;
        Debug.Log("rotationTicks: " + rotationTicks);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Fire1"))
        {   //Change control set.

            if (!swinging && !swingingBack)
            {
                swinging = true;
                swingingBack = false;
                rotationTickCount = 0;
            }
        }

        if(swinging)
        {
            if (rotationTickCount >= rotationTicks)
            {
                swinging = false;
                swingingBack = true;
            }
            else
            {
                transform.Rotate(new Vector3(rotationSpeed, 0, 0));
                rotationTickCount++;
            }
        }
        if (swingingBack)
        {
            if (rotationTickCount <= 0)
                swingingBack = false;
            else
            {
                transform.Rotate(new Vector3(-rotationSpeed, 0, 0));
                rotationTickCount--;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketController : MonoBehaviour {

    private bool swinging, swingingBack;
    private int rotationTickCount;
    private Rigidbody rb;
    public int rotationSpeed;
    private int rotationTicks;
    private bool isFrozen;
    public GameObject ball;
    private GameObject ballInstance;

    public void SetFreeze(bool freeze)
    {
        isFrozen = freeze;
    }

    public void Reset()
    {
        isFrozen = false;
        Debug.Log("RC Reset");
    }

    public void GameOver()
    {
        //Destroy the ball
        Destroy(ballInstance);
        ballInstance = null;
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        rotationTicks = 90 / rotationSpeed;
        isFrozen = false;
        swinging = false;
        swingingBack = false;
        ballInstance = null;
    }

    private void SpawnBall(Vector3 velocity)
    {
        Vector3 pos = rb.position;
        pos.z += 5;
        ballInstance = Instantiate(ball, pos, Quaternion.identity);
        ballInstance.GetComponent<Rigidbody>().velocity = velocity;
    }

    // Update is called once per frame
    void Update () {

        if (isFrozen)
            return;

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

                if(ballInstance == null)
                {
                    SpawnBall(new Vector3(0, 8.0f, 5.0f*8.0f));
                }
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

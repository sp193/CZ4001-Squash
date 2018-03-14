using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketController : MonoBehaviour {

    private SteamVR_TrackedObject trackedObject;    //Controller
    public GameObject racket;
    private BoxCollider bc;
    public GameObject ball;
    private GameObject ballInstance;

    private SteamVR_Controller.Device Controller
    {   //Return controller's input via the trackedObject's index.
        get { return SteamVR_Controller.Input((int)trackedObject.index); }
    }

    public void Reset()
    {
        Debug.Log("RC Reset");
    }

    public void GameOver()
    {
        //Destroy the ball
        Destroy(ballInstance);
        ballInstance = null;
    }

    //Called before start(). Used like a constructor.
    void Awake()
    {   //Upon load, obtain a reference to the SteamVR_TrackedObject component that's attached to the controllers.
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    // Use this for initialization
    void Start () {
        bc = racket.GetComponentInChildren<BoxCollider>();
        racket.transform.SetParent(gameObject.transform, false);
        ballInstance = null;
    }

    private void SpawnBall(Vector3 velocity)
    {
        Vector3 pos = bc.transform.position;
        //pos.y += 20;
        pos.z += 10;
        ballInstance = Instantiate(ball, pos, Quaternion.identity);
        ballInstance.GetComponent<Rigidbody>().velocity = velocity;
    }

    // Update is called once per frame
    void Update () {
        if(IsControllerSwung())
        {
                if(ballInstance == null)
                {
                    Vector3 pos = Controller.transform.pos;
                    pos.z *= BallPhysics.FORCE_MULTIPLIER;
                    SpawnBall(pos);
                }

            Debug.Log("Controller swung");
        }
    }

    public bool IsControllerSwung()
    {
        return (Controller.velocity.z > 0.1f) ;
    }
}

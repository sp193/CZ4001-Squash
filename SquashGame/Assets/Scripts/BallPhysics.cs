﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour {
    private Vector3 oldvel;
    private Rigidbody myRigidbody;
    private Vector3 prevSpd;
    private Vector3 startPos;
    private int bouncesOffGround;
    // sound
	public AudioSource[] surfaceBounce;
    public AudioSource netBounce;
	// Particle For Ground
	public ParticleSystem dustEffect;
	//Particle For Target
	public ParticleSystem sparkEffect = null;
	//public ParticleSystem smokeEffect = null;

    public const float FORWARD_FORCE_MULTIPLIER = 28.0f;
    public const float DIRECTIONAL_FORCE_MULTIPLIER = 14.0f;
    public const float UPWARD_FORCE_MULTIPLIER = 16.0f;
    public const float REFLECT_FORCE_MULTIPLIER = 16.0f;

    // Use this for initialization
    void Start () {
		//init audio array
		surfaceBounce = new AudioSource[5];
	
        myRigidbody = GetComponent<Rigidbody>();
        startPos = myRigidbody.position;
        bouncesOffGround = 0;
		// get all audio source , 0 = hitwall 1 = racket 2+ = bounce
        var allAudio= GetComponents<AudioSource>();
		for(int i = 1; i < 6; i++) 
			surfaceBounce[i-1] = allAudio[i];
		
        netBounce = allAudio[0];

		//particle init
		var groundEffect =  GameObject.FindGameObjectWithTag("Dust");
		dustEffect = groundEffect.GetComponent<ParticleSystem> ();
	    var allEffect = GameObject.FindGameObjectsWithTag ("Explo");
		//assigned invisible particlesystem
		sparkEffect = allEffect [1].GetComponent<ParticleSystem>();

    }
	
	// Update is called once per frame
	void Update () {
        Vector3 position = myRigidbody.position;
        oldvel = myRigidbody.velocity;

        //Check if out of bounds.
        if((position.x < -110 || position.x >= 110)
            || (position.y < -110 || position.y >= 110)
            || (position.z < -110 || position.z >= 110))
        {
            FindObjectOfType<GameController>().InitGameOver();
        }
	}

    //Handle collisions.
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint cp;
        int i;
        for (i = 0; i < collision.contacts.Length; i++) {
            cp = collision.contacts[i];
            GameObject other = cp.otherCollider.gameObject;

            if (other.CompareTag("Net"))
            {
				if(!netBounce.isPlaying)
                	netBounce.Play();
                myRigidbody.velocity = Vector3.Reflect(oldvel, cp.normal);
                prevSpd = myRigidbody.velocity;
                myRigidbody.velocity += cp.normal * BallPhysics.REFLECT_FORCE_MULTIPLIER;
            }
            if (other.CompareTag("Wall"))
            {
				if (!surfaceBounce [0].isPlaying)
					surfaceBounce [0].Play();
                FindObjectOfType<GameController>().AddScore(1);
            }
            if (other.CompareTag("Destructible"))
            {
				
				PlayTargetEffect (collision.contacts[0]);
                
                FindObjectOfType<GameController>().AddScore(4);
                FindObjectOfType<RandomSpawn>().Despawn(other);
            }

            if (other.CompareTag("Ground"))
            {
				PlayGroundEffect(collision.contacts [0]);
                bouncesOffGround++;
                if (bouncesOffGround >= 2)
                {
                    FindObjectOfType<GameController>().InitGameOver();
                }
            }
            else
                bouncesOffGround = 0;

            if (Vector3.Magnitude(myRigidbody.velocity) < 4.0f)  //Ball has stopped moving on the ground.
                FindObjectOfType<GameController>().InitGameOver();
        }
    }

    public void Reset()
    {
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
        myRigidbody.MovePosition(startPos);
        bouncesOffGround = 0;

        Debug.Log("BC Reset");
    }

	public void PlayTargetEffect(ContactPoint target)
	{
		// play effect based on contact point position( to play it at previous position)
		Vector3 pos = target.point;
		sparkEffect.transform.position = pos;
		//particle play
		if(!sparkEffect.isPlaying)
			sparkEffect.Play();
		//audio play
		if(!surfaceBounce[4].isPlaying)
			surfaceBounce[4].Play();

	}

	public void PlayGroundEffect(ContactPoint target)
	{
		// play effect based on contact point position( to play it at previous position)
		Vector3 pos = target.point;
		pos.y += 0.1f;
		dustEffect.transform.position = pos;
		//particle emit
		if(!dustEffect.isEmitting)
			dustEffect.Emit (200);
		//audio
		surfaceBounce[Random.Range(0,3)].Play();

	}

}

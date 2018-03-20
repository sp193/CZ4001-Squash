using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour {
    private Vector3 oldvel;
    private Rigidbody myRigidbody;
    private Vector3 prevSpd;
    private Vector3 startPos;
    private int bouncesOffGround;
    // sound
    public AudioSource surfaceBounce;
    public AudioSource netBounce;
	// Particle For Ground
	public ParticleSystem dustEffect;
	//Particle For Target
	public ParticleSystem sparkEffect = null;
	public ParticleSystem smokeEffect = null;

    public const float FORCE_MULTIPLIER = 30.0f;
    public const float UPWARD_FORCE_MULTIPLIER = 24.0f;
    public const float REFLECT_FORCE_MULTIPLIER = 10.0f;

    // Use this for initialization
    void Start () {
        myRigidbody = GetComponent<Rigidbody>();
        startPos = myRigidbody.position;
        bouncesOffGround = 0;

        var allAudio= GetComponents<AudioSource>();
        surfaceBounce = allAudio[0];
        netBounce = allAudio[1];
		dustEffect =  GetComponents<ParticleSystem>()[0];
		var allEffect = GameObject.FindGameObjectsWithTag ("Explo");
		//assigned invisible particlesystem
		sparkEffect = allEffect [1].GetComponent<ParticleSystem>();
		smokeEffect = allEffect [0].GetComponent<ParticleSystem>();

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
                netBounce.Play();
                myRigidbody.velocity = Vector3.Reflect(oldvel, cp.normal);
                prevSpd = myRigidbody.velocity;
                myRigidbody.velocity += cp.normal * BallPhysics.REFLECT_FORCE_MULTIPLIER;
            }
            if (other.CompareTag("Wall"))
            {
                surfaceBounce.Play();
                FindObjectOfType<GameController>().AddScore(1);
            }
            if (other.CompareTag("Destructible"))
            {
				
				PlayEffect (collision.contacts[0]);
                surfaceBounce.Play();
                FindObjectOfType<GameController>().AddScore(2);
                FindObjectOfType<RandomSpawn>().Despawn(other);
            }

            if (other.CompareTag("Ground"))
            {
                surfaceBounce.Play();
                bouncesOffGround++;
				dustEffect.Emit (100);

                if (bouncesOffGround >= 2)
                {
                    FindObjectOfType<GameController>().InitGameOver();
                }

                if (Vector3.Magnitude(myRigidbody.velocity) < 2.0f)  //Ball has stopped moving on the ground.
                    FindObjectOfType<GameController>().InitGameOver();
            }
            else
                bouncesOffGround = 0;
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

	public void PlayEffect(ContactPoint target)
	{
		// play effect based on contact point position( to play it at previous position)
		Vector3 pos = target.point;


		smokeEffect.transform.position = pos;
		sparkEffect.transform.position = pos;

		smokeEffect.Play ();
		sparkEffect.Play ();


	}

}

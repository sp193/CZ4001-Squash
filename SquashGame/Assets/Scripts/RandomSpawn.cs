﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{

    private static int maxNumTarget = 3;
    // public static int numTarget = 0;
    public GameObject target;
    private Vector3 tSize = new Vector3(2.0f, 2.0f, 2.0f);
    private Vector3 tRotation = new Vector3(0, 90, -90);
    protected static List<GameObject> allTarget;


    // Use this for initialization
    void Start()
    {
        allTarget = new List<GameObject>();
       TestTarget();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = allTarget.Count; i < maxNumTarget; i++)
        {
            SpawnTarget();
        }
    }
    public void TestTarget()
    {
        Vector3 pos = transform.position;
        Vector3 size = transform.localScale;


        pos.z = pos.z - size.z - 3.0f;
    
        pos.y = pos.y - 2.5f;
        allTarget.Add(Instantiate(target, pos, target.transform.rotation));
        
    }
    public void SpawnTarget()
    {

        Vector3 pos = transform.position;
        Vector3 size = transform.localScale;


        pos.z = pos.z - size.z - 3.0f;
        pos.x = pos.x + Random.Range((-size.x + tSize.x) / 2, (size.x - tSize.x) / 2);
        pos.y = pos.y + Random.Range((-size.y + tSize.y) / 2, (size.y - tSize.y) / 2);

       
        allTarget.Add(Instantiate(target, pos, target.transform.rotation));
      
    }

    public void Despawn(GameObject other)
    {

        allTarget.Remove(other);
        GameObject.Destroy(other);
        
    }
   
}

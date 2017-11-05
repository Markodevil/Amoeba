﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBullet : MonoBehaviour
{

    [SerializeField]
    private GameObject BulletSplat;

    bool isPickupAble = false;

    Vector3 newRandomPos;

    [SerializeField]
    private GameObject Puddle;

    float myMass;
    float hitMass;


    public void SetMyMass(float mass)
    {
        myMass = mass;
    }

    public void SetHitMass(float mass)
    {
        hitMass = mass;
    }

    private void CreatePuddles()
    {
        if (hitMass > 0 && myMass > 0)
        {
            GameObject puddle;
            puddle = Instantiate(Puddle, transform.position + transform.up, Quaternion.identity);
            puddle.GetComponent<SlimePuddle>().SetMass(myMass);
            puddle = Instantiate(Puddle, transform.position + transform.up, Quaternion.identity);
            puddle.GetComponent<SlimePuddle>().SetMass(hitMass);
            Debug.Log("puddles Spawned");
        }
        else
        {
            Debug.Log("0 mass to Throw");
            Debug.Log(myMass);
            Debug.Log(hitMass);
        }
    }

    void OnCollisionStay(Collision x)
    {
        if (x.transform.tag == "Slime")
        {
            Debug.Log("trowing mass");
            CreatePuddles();
            Destroy(gameObject);
        }
    }

    // Collision Code
    // Different particle effect depending on which If Statement is triggered
    void OnCollisionEnter(Collision col)
    {

            Instantiate(BulletSplat, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
    }

}

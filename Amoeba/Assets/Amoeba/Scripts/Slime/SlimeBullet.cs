﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBullet : MonoBehaviour
{

    [SerializeField]
    private GameObject BulletSplat;

    [SerializeField]
    [Tooltip("When the child providing the model has been disabled, the child providing the trail/particles will last for this amount of seconds. Note: This variation roots from the SlimeBullet.cs file.")]
    private float TrailDestroyTime;

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

    private void CreatePuddles(Collision x)
    {
        if (x != null)
        {
            Vector3 randomRotDir = Vector3.zero;

            if (myMass > 0)
            {
                int randomPos = Random.Range(1, 4);

                if (randomPos == 1)
                {
                    randomRotDir = ((transform.position + transform.up) + transform.right * 2 + transform.forward * 2);
                }
                else if (randomPos == 2)
                {
                    randomRotDir = ((transform.position + transform.up) + -transform.right * 2 + transform.forward * 2);
                }
                else if (randomPos == 3)
                {
                    randomRotDir = ((transform.position + transform.up) + transform.right * 2 + -transform.forward * 2);
                }
                else if (randomPos == 4)
                {
                    randomRotDir = ((transform.position + transform.up) + -transform.right * 2 + -transform.forward * 2);
                }

                GameObject puddle;
                puddle = Instantiate(Puddle, randomRotDir, Quaternion.identity);
                puddle.GetComponent<SlimePuddle>().ShootOut = true;
                Physics.IgnoreCollision(puddle.GetComponent<Collider>(), x.transform.GetComponent<CharacterController>(), true);
                puddle.GetComponent<SlimePuddle>().SetMass(myMass);
            }
            else
            {
                Debug.Log("0 mass to Throw");
                Debug.Log(myMass);
                Debug.Log(hitMass);
            }
        }
    }


    // Collision Code
    // Different particle effect depending on which If Statement is triggered
    void OnCollisionEnter(Collision x)
    {
        if(x.transform.tag == "Slime")
        {
            CreatePuddles(x);
        }
        Instantiate(BulletSplat, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);

        // The bullet object is separated into a gameObject with 3 children,
        // the first for the Particle System, the second for the Model and the third for the Trail Effect.
        // This will disable the Model child...
        gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
        // And this will destroy the whole game object after the Trail Timer has run out.
        Destroy(gameObject, TrailDestroyTime);
    }
}
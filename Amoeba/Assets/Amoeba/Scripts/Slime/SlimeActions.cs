﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeActions : MonoBehaviour
{
    // GameObject-type variable for the projectile
    [SerializeField]
    private GameObject projectileShot;

    // The speed of the projectile
    [SerializeField]
    private float projectileShotSpeed;

    // The time that, when reached, will cause the projectile to be destroyed
    [SerializeField]
    private float bulletDestroyTimer;

    [SerializeField]
    [Tooltip("When the child providing the model has been disabled, the child providing the trail/particles will last for this amount of seconds. Note: This variation roots from the SlimeAction.cs file.")]
    private float TrailDestroyTime;

    [SerializeField]
    private GameObject slime;

    private SlimeMovement slimeMovement;

    private PlayerController playerController;

    [SerializeField]
    private GameObject ShootSplat;

    [SerializeField]
    private float massPercentLossed;

    // Use this for initialization
    void Start()
    {
        slimeMovement = gameObject.GetComponent<SlimeMovement>();
    }

    public void Shoot(Vector3 rot , float mass)
    {
    
        //Debug.Log(rot);

        if (rot != Vector3.zero)
        {
            // Create a Bullet object
            GameObject Bullet;

            Bullet = Instantiate(projectileShot, transform.position + (rot * 2), Quaternion.identity);

            if(playerController == null)
            {
                playerController = slimeMovement.player.GetComponent<PlayerController>();
            }

            playerController.mass -= playerController.mass * (massPercentLossed / 100);

            Bullet.GetComponent<SlimeBullet>().SetMyMass(mass * (massPercentLossed / 100));

            // Get the object (Bullet) and add the force to it
            Bullet.GetComponent<Rigidbody>().AddForce(rot * projectileShotSpeed, ForceMode.Impulse);

            // Play shooting sound
            AudioManager.PlaySound("ShootSound");

            Instantiate(ShootSplat, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);

            // The bullet object is separated into a gameObject with 3 children,
            // the first for the Particle System, the second for the Model and the third for the Trail Effect.
            // This will disable the Model child...
            gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            // And this will destroy the whole game object after the Trail Timer has run out.
            Destroy(gameObject, TrailDestroyTime);
        }
    }


        //public void Split(float amount)
        //{



        //    if (playerController == null)
        //    {
        //        if (slimeMovement.player.GetComponent<PlayerController>() != null)
        //        {
        //            playerController = slimeMovement.player.GetComponent<PlayerController>();
        //        }
        //    }
        //    playerController.slimes.Remove(gameObject);


        //    if (amount == 0)
        //    {
        //        playerController.speed = 15.5f;

        //        newSlime(0.75f, 1);
        //        newSlime(0.75f, 1);
        //    }
        //    else if (amount == 1)
        //    {

        //        playerController.speed = 15.7f;

        //        newSlime(.6f, 2);
        //        newSlime(.6f, 2);
        //        newSlime(.6f, 2);
        //    }

        //    else if (amount == 2)
        //    {
        //        playerController.speed = 16;

        //        newSlime(0.4f, 3);
        //        newSlime(0.4f, 3);
        //        newSlime(0.4f, 3);
        //        newSlime(0.4f, 3);
        //    }
        //    else if (amount == 3)
        //    {
        //        slimeMovement.currentSlimeState = SlimeMovement.SlimeState.flying;
        //    }
        //    playerController.newKingSlime();

        //}


        //void newSlime(float size, float amount)
        //{
        //    GameObject tempSlime = Instantiate(slime, transform.position, transform.rotation);
        //    playerController.slimes.Add(tempSlime);
        //    tempSlime.GetComponent<SlimeMovement>().parent = slimeMovement.parent;
        //    tempSlime.GetComponent<SlimeHealth>().amountOfSplits = amount;
        //    tempSlime.transform.localScale = new Vector3(size, size, size);
        //}

        void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "ProtectiveShield")
        {
            Debug.Log("shield");
            slimeMovement.player.gameObject.GetComponent<PlayerPowerUpController>().Shield();
            Destroy(hit.gameObject);
        }
    }
}

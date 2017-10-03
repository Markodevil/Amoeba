﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerController : MonoBehaviour {

    //declaring variables

    [SerializeField] [Tooltip("The slime that will be spawned by this player")]
    private GameObject slimePrefab;

    private InputDevice controller;

    [SerializeField][Tooltip("The speed that the player will move")]
    float speed;


    private GameStateManager gameManager;
    CharacterController cc;
    Vector3 startPos;

    List<GameObject> slimes = new List<GameObject>();

    [HideInInspector]
    public int playerNumber = 1;


    [SerializeField] [Tooltip("The amount of force applied when dodging")]
    float dodgeForce;

    [SerializeField]
    private float BufferTime;

    private float ShootTimer;

    private GameObject controllerRetical;

    void Start ()
    {
        //defining the Character Controller
        cc = GetComponent<CharacterController>();

        //defining the GameManager
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();

        ShootTimer = BufferTime;

        //getting the amount of players in the game
        playerNumber = gameManager.playerCount;

        //making the first slime
        GameObject tempSlime = Instantiate(slimePrefab, new Vector3(transform.position.x, 45.6f, transform.position.z), transform.rotation);
        tempSlime.GetComponent<SlimeMovement>().parent = gameObject.tag;
        slimes.Add(tempSlime);

        //if this player number is less then or equal to the amount of controllers
        if (playerNumber <= InputManager.Devices.Count)
        {
            Debug.Log("Added a controller to player number" + playerNumber);
            
            //give this player a controller
            controller = InputManager.Devices[playerNumber - 1];

        }

        controllerRetical = transform.GetChild(0).gameObject;

    }


    void Update ()
    {
        ShootTimer += Time.deltaTime;

        //if there is no controller
        if (controller == null)
        {

            //using keybored controls


            //add movment to the player if the user adds input
            cc.Move(transform.right * Input.GetAxis("HorizontalKeys") * speed * Time.deltaTime);
            cc.Move(transform.forward * Input.GetAxis("VerticalKeys") * speed * Time.deltaTime);
        



            //if q button is pressed
            if (Input.GetKeyDown("q"))
            {
                //find the center of all of the slimes
                Vector3 centerPoint = new Vector3();
                foreach (GameObject x in slimes)
                {
                    centerPoint += x.transform.position;
                }

                //average all the slimes positions
                centerPoint /= slimes.Count;

                
                foreach (GameObject x in slimes)
                {
                    //call dodge on each slime passing though the centerpoint and the amount of force 
                    x.GetComponent<SlimeMovement>().Dodge(centerPoint, dodgeForce);
                }


            }


            //Shooting on keybored

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            Vector3 vec3 = new Vector3();
            if (Physics.Raycast(ray, out rayHit))
            {


                vec3 = new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z) - transform.position;

                Debug.DrawLine(rayHit.point, transform.position);
            }

            //if mousebutton 0 is pressed
            if (Input.GetMouseButtonDown(0) && ShootTimer > BufferTime)
                {

                ShootTimer = 0.0f;

                foreach (GameObject i in slimes)
                    {
                    i.GetComponent<SlimeActions>().Shoot(vec3.normalized);
                    }

                }
                
                //find the normilised vector Between the the player(this gameobject) and the mouse 

                // foreach slime in the list slimes

                 // call shoot on current slime

        }
       
        
        //controller
        else
        {


            //add movment to the player if the user adds input 
            cc.Move(transform.right * controller.LeftStick.X * speed * Time.deltaTime);
            cc.Move(transform.forward * controller.LeftStick.Y * speed * Time.deltaTime);

            controllerRetical.transform.position = transform.position + new Vector3(controller.RightStick.X, 0, controller.RightStick.Y);


            //if the a button is pressed on xbox or the x button is pressed on controller (this will probs change)
            if (controller.LeftTrigger.WasPressed)
            {
                //find the center of all of the slimes
                Vector3 centerPoint = new Vector3();
                foreach (GameObject x in slimes)
                {
                    centerPoint += x.transform.position;
                }

                //average all the slimes positions
                centerPoint /= slimes.Count;

                foreach (GameObject x in slimes)
                {
                    //call dodge on each slime passing though the centerpoint and the amount of force 
                    x.GetComponent<SlimeMovement>().Dodge(centerPoint, dodgeForce);
                }
            }


            //shooting on controller


            if (controller.RightTrigger.WasPressed && ShootTimer > BufferTime)
            {

                ShootTimer = 0.0f;
                Vector3 vecBetween = new Vector3();
                vecBetween = controllerRetical.transform.position - transform.position;

                foreach(GameObject i in slimes)
                {
                    i.GetComponent<SlimeActions>().Shoot(vecBetween.normalized);
                }

            }


        }


        //if debug mode is active
        if (gameManager.debugMode == true)
        {
            DebugMode();
        }



    }

    void DebugMode()
    {
        //if enter is pressed
        if(Input.GetKeyDown(KeyCode.Return))
        {
            //make a new slime
            GameObject tempSlime;
            tempSlime = Instantiate(slimePrefab, new Vector3(transform.position.x, 45.6f, transform.position.z) + transform.right, transform.rotation);
            tempSlime.GetComponent<SlimeMovement>().parent = gameObject.tag;
            slimes.Add(tempSlime);
        }
    }

}

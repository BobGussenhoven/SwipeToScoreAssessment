using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : MonoBehaviour {

    public enum PawnState
    {
        AwaitingInput = 0,
        ActiveInput = 1,
        PostInput = 2
    }

    public PawnState pawnState { get; protected set; }
    [SerializeField]
    protected Camera playerCamera;
    [SerializeField]
    protected Ball playerBall;

    protected ControllerState lastControllerState;
    protected float inputStartTime;
    protected Vector3 inputStartPosition;

    // Use this for initialization
    void Start () {
        pawnState = PawnState.AwaitingInput;
        
        if (playerCamera == null)
            Debug.LogError("PlayerPawn got no Camera", this);
    }
	
	// Update is called once per frame
	void Update () {
        HandleGivenInput();
    }

    /// <summary>
    /// Function that handles the physical parts of the player based upon given input
    /// </summary>
    protected void HandleGivenInput()
    {
        //Todo, these might be cut up in more sub methods for better overview later on
        switch (pawnState)
        {
            //What happens when the player can shoot the ball
            case PawnState.AwaitingInput:
                //Happens when player controller makes an interaction
                if (lastControllerState.hasInput)
                {
                    //We create a Raycast from our camera to see if we hit our ball
                    Ray ray = playerCamera.ScreenPointToRay(lastControllerState.position);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo, 100))
                    {
                        if (hitInfo.transform.GetComponent<Ball>() == playerBall)
                        {
                            //if we've hit our ball. We start tracking the players swipe
                            inputStartTime = Time.time;
                            inputStartPosition = hitInfo.point;
                            pawnState = PawnState.ActiveInput;
                        }
                    }
                }
                break;
                //What happens when the player is actively swiping to shoot the ball
            case PawnState.ActiveInput:
                //Player is releasing the shot
                if (!lastControllerState.hasInput)
                {
                    //We create another ray to get a better understanding of where the player is aiming
                    Ray ray = playerCamera.ScreenPointToRay(lastControllerState.position);
                    RaycastHit hitInfo;
                    Vector3 swipeEndPosition = new Vector3();
                    if (Physics.Raycast(ray, out hitInfo, 100))
                    {
                        swipeEndPosition = hitInfo.point;
                    }
                    else
                    {
                        //When player is aiming at the sky. We pick a point in the air 30 units from where he is looking. This number came from playtesting
                        swipeEndPosition = ray.origin + ray.direction * 30;
                    }

                    
                    float swipeDuration = Time.time - inputStartTime;
                    Vector3 swipeDirection = swipeEndPosition - inputStartPosition;

                    //we check to see if the shot was not just a quick accidental click
                    if (swipeDirection.magnitude > 1)
                    {
                        //Based upon the swiping start and end point, as well as it's quickness we calculate the forces we want to put on the Ball object
                        swipeDirection += Vector3.up * swipeDirection.magnitude / 10;
                        float force = swipeDirection.magnitude * 7 / swipeDuration;

                        playerBall.LaunchBall(swipeDirection, force);
                        pawnState = PawnState.PostInput;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Function for input communication between Controller and Pawn.
    /// </summary>
    /// <param name="pawnState"></param>
    public void UpdateInput(ControllerState pawnState)
    {
        lastControllerState = pawnState;
    }
}

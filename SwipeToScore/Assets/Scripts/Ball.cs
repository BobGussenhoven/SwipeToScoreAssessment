using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Class of Ball object. Used for launching the ball and keeping track of it's life events
/// </summary>
/// TODO have the ball shoot a RayCast forward on FixedUpdate to double check Collisions. This will prevent the ball from going through thin objects when at high speed;
public class Ball : MonoBehaviour {

    [SerializeField]
    protected float maxForce;
    [SerializeField]
    protected float maxTravelTime = 7;
    [SerializeField]
    protected float minVelocity = 1;

    protected Rigidbody rigidBody;
    protected bool isLaunched = false;
    protected float launchTime;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
            Debug.LogError("Ball doesn't have RigidBody attached", this);
	}
	
	// Update is called once per frame
	void Update () {
        CheckBallFailure();
	}

    /// <summary>
    /// Function for launching ball, based upon information calculated by the PlayerPawn
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    public void LaunchBall(Vector3 direction, float force)
    {
        force = Mathf.Min(force, maxForce);
        direction.Normalize();
        rigidBody.AddForce(direction * force);
        isLaunched = true;
        launchTime = Time.time;
    }

    /// <summary>
    /// Check to see if ball is going nowhere
    /// </summary>
    protected void CheckBallFailure()
    {
        if (!isLaunched)
            return;

        float travelTime = Time.time - launchTime;
        if (travelTime <= 1)
            return;

        if (travelTime > maxTravelTime || rigidBody.velocity.magnitude < minVelocity)
        {
            HandleResult(false);
        }
    }

    /// <summary>
    /// Function used to tell GameMode whether or not this ball was successful at reaching the goal.
    /// </summary>
    /// <param name="successful"></param>
    protected void HandleResult(bool successful)
    {
        if (GameModeBase.activeGameMode == null)
            return;

        GameModeBase.activeGameMode.BallResult(successful);
        this.enabled = false;
    }

    /// <summary>
    /// Function called by BallTriggers to tell the ball whether is hit the goal or a death zone
    /// </summary>
    /// <param name="trigger"></param>
    public void HitTrigger(BallTrigger trigger)
    {
        if (!isActiveAndEnabled)
            return;

        HandleResult(trigger.IsGoal);
    }
}

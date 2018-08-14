using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Timed GameMode where player needs to score the most times within a specific set time. Now always two minutes
/// </summary>
public class TimerMode : GameModeBase {
    
    //TODO: Make game duration customizable
    protected float gameDuration = 2 * 60;

    public float remainingTime { get; protected set; }
	
	// Update is called once per frame
	protected override void Update () {
        if (gameState == GameState.GameActive)
        {
            remainingTime = gameDuration - Time.timeSinceLevelLoad;
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                EndGame();
            }
        }
	}

    /// <summary>
    /// Function that notifies the GameMode of a BallObject either successfully or unsuccessfully hitting the target
    /// </summary>
    /// <param name="successful"></param>
    public override void BallResult(bool successful)
    {
        base.BallResult(successful);

        StartCoroutine(RespawnPawnAfterDelay(this.playerController, defaultRespawnDelay));
    }

    /// <summary>
    /// Function for loading GameMode specific HUD interfaces
    /// </summary>
    protected override void LoadGamemodeHUD()
    {
        UnityEngine.Object timedHUDPrefab = Resources.Load("UI/HUDs/TimedHUD");
        if(timedHUDPrefab != null)
            Instantiate(timedHUDPrefab);
    }
}

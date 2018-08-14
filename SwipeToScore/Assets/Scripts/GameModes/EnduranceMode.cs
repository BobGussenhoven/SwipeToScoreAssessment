using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Endurance GameMode. Where player keeps shooting until he misses.
/// </summary>
public class EnduranceMode : GameModeBase {

    /// <summary>
    /// Function that notifies the GameMode of a BallObject either successfully or unsuccessfully hitting the target
    /// </summary>
    /// <param name="successful"></param>
    public override void BallResult(bool successful)
    {
        base.BallResult(successful);

        if (!successful)
            EndGame();
        else
            StartCoroutine(RespawnPawnAfterDelay(this.playerController, defaultRespawnDelay));
    }

    /// <summary>
    /// Function for loading GameMode specific HUD interfaces
    /// </summary>
    protected override void LoadGamemodeHUD()
    {
        UnityEngine.Object enduranceHUDPrefab = Resources.Load("UI/HUDs/EnduranceHUD");
        if (enduranceHUDPrefab != null)
            Instantiate(enduranceHUDPrefab);
    }
}

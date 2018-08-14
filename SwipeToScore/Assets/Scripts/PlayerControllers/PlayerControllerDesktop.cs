using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player controller that is being used for Desktop player input
/// </summary>
public class PlayerControllerDesktop : PlayerControllerBase {
    
    protected override void HandleInput()
    {
        currentControllerState.hasInput = Input.GetMouseButton(0);
        currentControllerState.position = Input.mousePosition;

        if (playerPawn != null)
        {
            playerPawn.UpdateInput(currentControllerState);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player controller that is being used for Mobile player input
/// </summary>
public class PlayerControllerMobile : PlayerControllerBase
{
    protected override void HandleInput()
    {
        if (Input.touchCount == 0)
        {
            currentControllerState.hasInput = false;
            currentControllerState.position = Vector2.zero;
        }
        else
        {
            bool stillTouching = !(Input.touches[0].phase == TouchPhase.Canceled || Input.touches[0].phase == TouchPhase.Ended);
            currentControllerState.hasInput = stillTouching;
            currentControllerState.position = Input.touches[0].position;
        }

        if (playerPawn != null)
            playerPawn.UpdateInput(currentControllerState);
    }
}

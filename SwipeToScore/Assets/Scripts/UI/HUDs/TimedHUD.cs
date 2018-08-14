using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HUD class for the Timed GameMode
/// </summary>
public class TimedHUD : HUDBase {

    [SerializeField]
    protected Text timerTextfield;

    /// <summary>
    /// Method that updates all the elements in the HUD
    /// </summary>
    protected override void UpdateHUD()
    {
        base.UpdateHUD();

        if (GameModeBase.activeGameMode == null)
            return;

        TimerMode timerMode = GameModeBase.activeGameMode as TimerMode;
        if (timerMode == null)
        {
            Debug.LogError("Got timedHUD, but no timed GameMode", this);
            return;
        }

        //Here we convert the timer to a readable M:SS structure
        //Todo, put this conversion in a Utility script
        float timer = timerMode.remainingTime;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerTextfield.text = formattedTime;
    }
}

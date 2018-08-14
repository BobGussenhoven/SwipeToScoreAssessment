using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HUDBase : MonoBehaviour {

    //Most likely, every form of Ingame HUD will at least have a basic score field
    [SerializeField]
    protected Text scoreTextfield;

	// Use this for initialization
	void Start () {
        if (scoreTextfield == null)
            Debug.LogError("HUD is missing textField for score", this);
	}
	
	// TODO: GameMode should fire an event when score changes, so this does not check every frame
	void Update () {
        if (GameModeBase.activeGameMode != null)
            UpdateHUD();
	}

    /// <summary>
    /// Method that updates all the elements in the HUD
    /// </summary>
    protected virtual void UpdateHUD()
    {
        scoreTextfield.text = "Score: " + GameModeBase.activeGameMode.playerScore.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Struct that hold all gameplay specific input. 
/// </summary>
public struct ControllerState
{
    public bool hasInput;
    public Vector2 position;
}

/// <summary>
/// Base class for PlayerControllers. They Handle the input for different platforms. Might also be extended for AI player behaviour.
/// </summary>
public abstract class PlayerControllerBase : MonoBehaviour {

    protected PlayerPawn playerPawn;
    protected ControllerState currentControllerState;
	
	// Update is called once per frame
	void Update () {
        if(GameModeBase.activeGameMode != null && GameModeBase.activeGameMode.gameState == GameModeBase.GameState.GameActive)
            HandleInput();
	}

    /// <summary>
    /// Function for asssigning a PlayerPawn (Physical part of player in world) to PlayerController
    /// </summary>
    /// <param name="playerPawn"></param>
    public void SetPawn(PlayerPawn playerPawn)
    {
        this.playerPawn = playerPawn;
    }
    
    /// <summary>
    /// Function to Destroy Pawn. Called on respawning and at game end
    /// </summary>
    public void DestroyPawn()
    {
        if(playerPawn != null)
            Destroy(playerPawn.gameObject);
    }

    /// <summary>
    /// Function to handle ending of game. 
    /// </summary>
    public void OnGameEnded()
    {
        DestroyPawn();
    }

    /// <summary>
    /// Function to Handle platform specific (or AI driven) input, and notify the PlayerPawn of this new input state
    /// </summary>
    protected abstract void HandleInput();
}

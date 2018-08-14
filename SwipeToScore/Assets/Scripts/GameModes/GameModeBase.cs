using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class managing the State of the game, the spawning of player, and keeping track of the score
/// </summary>
public abstract class GameModeBase : MonoBehaviour {
    /// <summary>
    /// Enum for tracking whether the game is loading, running or ended
    /// </summary>
    public enum GameState
    {
        GameLoading,
        GameActive,
        GameEnded
    }
    public GameState gameState { get; protected set; }

    public static GameModeBase activeGameMode;

    [SerializeField]
    protected float defaultRespawnDelay = 2;

    protected GameObject playerPawnPrefab;
    protected GameObject resultScreenPrefab;


    //This might one day be modified to create a multiplayer experience
    protected PlayerControllerBase playerController;
    protected SpawnPoint[] spawnPoints;
    

    //Todo for multiplayer: Store this and other playerinfo like name etc in a PlayerState class
    public int playerScore { get; protected set; }

    // Use this for initialization
    protected virtual void Awake()
    {
        activeGameMode = this;
        gameState = GameState.GameLoading;
        DontDestroyOnLoad(this.gameObject);

        //TODO: Make next two lines less hard coded
        playerPawnPrefab = Resources.Load("PlayerPawnPrefab") as GameObject;
        resultScreenPrefab = Resources.Load("UI/ResultScreen") as GameObject;

        InitPlayerController();
    }

    /// <summary>
    /// Function that actually starts game after loading of game scene is complete
    /// </summary>
    protected virtual void StartGame()
    {
        //Might want to change this, so that the SpawnPoints enlist themselves to the GameMode on Awake
        spawnPoints = FindObjectsOfType<SpawnPoint>();
        LoadGamemodeHUD();
        gameState = GameState.GameActive;

        SpawnPlayerPlawn(playerController);
    }

    /// <summary>
    /// Function for cleaning up
    /// </summary>
    protected void OnDestroy()
    {
        activeGameMode = null;
    }

    // Update is called once per frame
    protected virtual void Update () {
		
	}

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    /// <summary>
    /// Function to let GameMode know it can start the game
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        StartGame();
    }

    /// <summary>
    /// Function for loading GameMode specific HUD interfaces
    /// </summary>
    protected abstract void LoadGamemodeHUD();

    /// <summary>
    /// Function for initializing PlayerController. Might one day support multiplayer or AI players
    /// </summary>
    protected virtual void InitPlayerController()
    {
        GameObject playerControllerObject = new GameObject("PlayerController");
        playerControllerObject.transform.parent = this.transform;
        //This might some day be optimized to support more platforms
        if (Application.isMobilePlatform)
            playerController = playerControllerObject.AddComponent<PlayerControllerMobile>();
        else
            playerController = playerControllerObject.AddComponent<PlayerControllerDesktop>();
    }

    /// <summary>
    /// Function that ends the game. 
    /// </summary>
    protected void EndGame()
    {
        if (gameState != GameState.GameActive)
            return;

        playerController.OnGameEnded();
        Instantiate(resultScreenPrefab);
        gameState = GameState.GameEnded;
    }

    /// <summary>
    /// Function to go back to Main menu after game finished
    /// </summary>
    public void ExitToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Function to spawn PlayerPawn. Based on predefined Spawnpoints in the Scene
    /// </summary>
    /// <param name="playerController"></param>
    void SpawnPlayerPlawn(PlayerControllerBase playerController)
    {
        if (gameState == GameState.GameEnded)
            return;

        List<SpawnPoint> availableSpawnpoints = new List<SpawnPoint>();
        foreach(SpawnPoint spawnPoint in spawnPoints)
        {
            if (!spawnPoint.isOccupied)
                availableSpawnpoints.Add(spawnPoint);
        }

        if(availableSpawnpoints.Count == 0)
        {
            Debug.LogError("No available spawnpoint found, make sure each level has more spawnpoints than max player count");
            return;
        }

        SpawnPoint randomSpawnpoint = availableSpawnpoints[Random.Range(0, availableSpawnpoints.Count)];

        PlayerPawn spawnedPawn = randomSpawnpoint.SpawnPawn(playerPawnPrefab);
        playerController.SetPawn(spawnedPawn);
    }

    /// <summary>
    /// Function that notifies the GameMode of a BallObject either successfully or unsuccessfully hitting the target
    /// </summary>
    /// <param name="successful"></param>
    public virtual void BallResult(bool successful)
    {
        if (successful)
            playerScore++;
    }

    /// <summary>
    /// Function to wait a few seconds. Destroy old PlayerPawn and spawn new one. 
    /// </summary>
    /// <param name="playerController"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    protected IEnumerator RespawnPawnAfterDelay(PlayerControllerBase playerController, float delay)
    {
        yield return new WaitForSeconds(delay);
        playerController.DestroyPawn();
        
        yield return null;

        SpawnPlayerPlawn(playerController);
    }
}

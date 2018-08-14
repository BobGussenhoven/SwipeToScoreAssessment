using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimerModeButton : MonoBehaviour {
    
    public void OnClicked()
    {
        GameObject gameModeObject = new GameObject("GameMode");
        gameModeObject.AddComponent<TimerMode>();
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}

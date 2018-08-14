using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnduranceModeButton : MonoBehaviour {

    public void OnClicked()
    {
        GameObject gameModeObject = new GameObject("GameMode");
        gameModeObject.AddComponent<EnduranceMode>();
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}

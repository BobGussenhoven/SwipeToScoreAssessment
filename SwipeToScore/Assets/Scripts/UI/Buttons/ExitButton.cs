using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour {

	public void OnClicked()
    {
        if (GameModeBase.activeGameMode != null)
            GameModeBase.activeGameMode.ExitToMenu();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour {

    [SerializeField]
    protected Text scoreText;
    
	void Start () {
        //Todo localize texts
        int score = 0;
        if (GameModeBase.activeGameMode != null)
            score = GameModeBase.activeGameMode.playerScore;
        scoreText.text = "Your score was: " + score.ToString();
	}
}

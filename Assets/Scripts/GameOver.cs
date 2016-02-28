using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {
	public Text gameOverText;
	public Text continueText;
	public Text scoreText;
	public Text scoreTextGameOver;

	public void ShowGameOver()
	{
		gameOverText.text = ("GAME OVER");
		continueText.text = ("Continue");
		if (scoreText.text == "") {
			scoreTextGameOver.text = "Score 0";
		} else {
			scoreTextGameOver.text = "Score " + scoreText.text;
		}
	}
}

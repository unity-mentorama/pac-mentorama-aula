using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
	public TextMeshProUGUI CurrentScoreText;

	public TextMeshProUGUI HighScoreText;

	private void Start()
	{
		var scoreManager = FindObjectOfType<ScoreManager>();
		scoreManager.OnScoreChanged += ScoreManager_OnScoreChanged;
		scoreManager.OnHighScoreChanged += ScoreManager_OnHighScoreChanged;

		HighScoreText.text = $"{scoreManager.HighScore:00}";
	}

	private void ScoreManager_OnHighScoreChanged(int highScore)
	{
		HighScoreText.text = $"{highScore:00}";
	}

	private void ScoreManager_OnScoreChanged(int score)
	{
		CurrentScoreText.text = $"{score:00}";
	}
}

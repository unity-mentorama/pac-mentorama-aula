using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private enum GameState
	{
		Starting,
		Playing,
		LifeLost,
		GameOver,
		Victory
	}

	public float StartupTime;

	public float LifeLostTimer;

	private GhostAI[] _allGhosts;
	private CharacterMotor _pacmanMotor;

	private GameState _gameState;
	private int _victoryCount;

	private float _lifeLostTimer;
	private bool _isGameOver;

	public event Action OnGameStarted;
	public event Action OnVictory;
	public event Action OnGameOver;

	// Start is called before the first frame update
	void Start()
	{
		var allColletibles = FindObjectsOfType<Collectible>();

		_victoryCount = 0;
		foreach (var collectible in allColletibles)
		{
			_victoryCount++;
			collectible.OnCollected += Collectible_OnCollected;
		}

		var pacman = GameObject.FindWithTag("Player");
		_pacmanMotor = pacman.GetComponent<CharacterMotor>();
		_allGhosts = FindObjectsOfType<GhostAI>();
		StopAllCharacters();

		pacman.GetComponent<Life>().OnLifeRemoved += Pacman_OnLifeRemoved;

		_gameState = GameState.Starting;
	}

	private void Pacman_OnLifeRemoved(int remainingLives)
	{
		StopAllCharacters();

		_lifeLostTimer = LifeLostTimer;
		_gameState = GameState.LifeLost;

		_isGameOver = remainingLives <= 0;
	}

	private void Collectible_OnCollected(int _, Collectible collectible)
	{
		_victoryCount--;

		if (_victoryCount <= 0)
		{
			_gameState = GameState.Victory;
			StopAllCharacters();
			OnVictory?.Invoke();
		}

		collectible.OnCollected -= Collectible_OnCollected;
	}

	// Update is called once per frame
	private void Update()
	{
		switch (_gameState)
		{
			case GameState.Starting:
				StartupTime -= Time.deltaTime;

				if (StartupTime <= 0)
				{
					_gameState = GameState.Playing;
					StartAllCharacters();

					OnGameStarted?.Invoke();
				}

				break;

			case GameState.LifeLost:
				_lifeLostTimer -= Time.deltaTime;

				if (_lifeLostTimer <= 0)
				{
					if (_isGameOver)
					{
						_gameState = GameState.GameOver;
						OnGameOver?.Invoke();
					}
					else
					{
						ResetAllCharacters();
						_gameState = GameState.Playing;
					}
				}

				break;

			case GameState.GameOver:
			case GameState.Victory:

				if (Input.anyKey)
				{
					SceneManager.LoadScene(0);
				}

				break;
		}
	}

	private void ResetAllCharacters()
	{
		_pacmanMotor.ResetPosition();

		foreach (var ghost in _allGhosts)
		{
			ghost.Reset();
		}

		StartAllCharacters();
	}

	private void StartAllCharacters()
	{
		_pacmanMotor.enabled = true;

		foreach (var ghost in _allGhosts)
		{
			ghost.StartMoving();
		}
	}

	private void StopAllCharacters()
	{
		_pacmanMotor.enabled = false;

		foreach (var ghost in _allGhosts)
		{
			ghost.StopMoving();
		}
	}
}

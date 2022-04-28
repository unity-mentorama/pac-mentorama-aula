using System;
using System.Collections.Generic;
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

	private List<IMovableCharacter> _allMovableCharacters;

	private GhostHouse _ghostHouse;

	private GameState _gameState;
	private int _victoryCount;

	private float _lifeLostTimer;
	private bool _isGameOver;

	public event Action OnGameStarted;
	public event Action OnVictory;
	public event Action OnGameOver;

	private void Start()
	{
		var allColletibles = FindObjectsOfType<Collectible>();

		_victoryCount = 0;
		foreach (var collectible in allColletibles)
		{
			_victoryCount++;
			collectible.OnCollected += Collectible_OnCollected;
		}

		_allMovableCharacters = new List<IMovableCharacter>();

		var pacman = GameObject.FindWithTag("Player");
		_allMovableCharacters.Add(pacman.GetComponent<PacmanInput>());

		var allGhosts = FindObjectsOfType<GhostAI>();
		_allMovableCharacters.AddRange(allGhosts);

		StopAllCharacters();

		_ghostHouse = FindObjectOfType<GhostHouse>();
		_ghostHouse.enabled = false;

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
					_ghostHouse.enabled = true;

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
						ResetAllCharactersPositions();
						StartAllCharacters();
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

	private void ResetAllCharactersPositions()
	{
		foreach (var character in _allMovableCharacters)
		{
			character.ResetPosition();
		}
	}

	private void StartAllCharacters()
	{
		foreach (var character in _allMovableCharacters)
		{
			character.StartMoving();
		}
	}

	private void StopAllCharacters()
	{
		foreach (var character in _allMovableCharacters)
		{
			character.StopMoving();
		}
	}
}

using System;
using Assets.Scripts.GameLogic.Game;
using Assets.Scripts.GameLogic.Plane;
using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

	#region Constants

	private const string HighscorePrefs = "Highscore";

	private int _highScore;

	public event Action<int> OnHighScoreChange;

	#endregion

	#region Variables

	public static GameController Instance { get; private set; }

	private GameModel _model;

	public GameModel.GameState State
	{
		get { return _model.State; }
	}

	public Transform FightersParent;

	public Transform PlayerStartPosition;

	#endregion

	#region MonoBehaviours Actions

	private void Awake()
	{
		_model = GetComponent<GameModel>();

		if (_model == null)
		{
			throw new MissingComponentException("GameController.Awake - can't find GaeModel component");
		}

		Instance = this;

		_model.State = GameModel.GameState.BeforePlaying;

		if (PlayerPrefs.HasKey(HighscorePrefs))
		{
			_highScore = PlayerPrefs.GetInt(HighscorePrefs);
		}
		else
		{
			_highScore = 0;
		}

		UpdateHighScore();

		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PLayersFighter"), LayerMask.NameToLayer("PlayerBullets"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyFighter"), LayerMask.NameToLayer("EnemyBullets"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerBullets"), LayerMask.NameToLayer("PlayerBullets"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullets"), LayerMask.NameToLayer("EnemyBullets"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerBullets"), LayerMask.NameToLayer("EnemyBullets"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyFighter"), LayerMask.NameToLayer("EnemyFighter"));
	}

	// Use this for initialization
	private void Start()
	{

	}

	// Update is called once per frame
	private void Update()
	{

	}

	#endregion

	#region Actions

	public void StartGame()
	{
		_model.State = GameModel.GameState.Playing;

		InitPlayerPlane();
		UpdateHighScore();
	}

	public void PausedGame()
	{
		_model.State = GameModel.GameState.Paused;
	}

	public void UnPausedGame()
	{
		_model.State = GameModel.GameState.Playing;
	}

	public void GameOver(bool showThanksText = false)
	{
		_model.State = GameModel.GameState.GameOver;
		SaveHighScore();
		GameOverMenuController gameOverMenuController = FindObjectOfType<GameOverMenuController>();
		if (gameOverMenuController != null)
		{
			gameOverMenuController.Init(_model.LevelControler.Score);
			gameOverMenuController.Show();

			if (showThanksText)
			{
				gameOverMenuController.ShowThanksText();
			}
			else
			{
				gameOverMenuController.HideThanksText();
			}
		}
	}

	public void ToMainMenu()
	{
		_model.State = GameModel.GameState.BeforePlaying;
		OnMainMenu();

		AiGenerator aiGenerator = FindObjectOfType<AiGenerator>();
		if (aiGenerator != null)
		{
			aiGenerator.Reset();
		}

		WorldMovement[] worldMovements = FindObjectsOfType<WorldMovement>();
		for (int i = 0; i < worldMovements.Length; i++)
		{
			worldMovements[i].Reset();
		}
	}

	public void GameEnd()
	{
		_model.State = GameModel.GameState.GameEnd;
		GameOver(true);
		SaveHighScore();
	}

	private void InitPlayerPlane()
	{
		PlaneControlling plane = ResourceController.GetPlaneFromPool(PlaneControlling.PlayerPlanePrefabsPath, FightersParent);

		plane.Plane.Reset(true);

		plane.transform.position = PlayerStartPosition.position;

		Level level = Level.Create(plane);

		GameMenuController gameMenuController = FindObjectOfType<GameMenuController>();

		if (gameMenuController != null)
		{
			_model.LevelControler = LevelController.Create(level, gameMenuController);
			_model.LevelControler.Init();
			plane.Plane.OnPlaneDeath += gameMenuController.OnPlayerDeath;
			gameMenuController.InitPlayerData(plane.Plane);

			plane.Plane.SetPlaneDeathListener(_model.LevelControler);

			OnHighScoreChange += gameMenuController.OnHighScoreUdate;
		}

		if (_model.LevelControler != null)
		{
			_model.LevelControler.OnScoreChange += OnScoreChange;
		}

		plane.Plane.OnPlayerGameOver += PlayerGameOver;
	}

	private void PlayerGameOver(PlaneModel plane)
	{
		Debug.Log("GameController.PlayerGameOver - OK , current score:" + _model.LevelControler.Score);
		GameOver();
	}

	private void OnMainMenu()
	{
		_model.LevelControler.OnScoreChange -= OnScoreChange;
		_model.LevelControler.PlayerPlane.Plane.OnPlayerGameOver -= PlayerGameOver;

		_model.LevelControler.PlayerPlane.ResetPlaneData();

		GameMenuController gameMenuController = FindObjectOfType<GameMenuController>();

		if (gameMenuController != null)
		{
			_model.LevelControler.PlayerPlane.Plane.OnPlaneDeath -= gameMenuController.OnPlayerDeath;
		}

		_model.LevelControler.OnDestroy();

	}

	private void OnScoreChange(int score)
	{
		if (score < _highScore)
			return;
		_highScore = score;

		UpdateHighScore();
	}

	private void UpdateHighScore()
	{
		if (OnHighScoreChange != null)
		{
			OnHighScoreChange(_highScore);
		}
	}

	private void SaveHighScore()
	{
		PlayerPrefs.SetInt(HighscorePrefs, _highScore);
	}

	public void UpdatePlayerLives(PlaneModel plane)
	{
		GameMenuController gameMenuController = FindObjectOfType<GameMenuController>();

		if (gameMenuController != null)
		{
			gameMenuController.InitPlayerData(plane);
		}
	}

	public IPlaneDeathListener GetPlaneDeathLestener()
	{
		return _model.LevelControler;
	}


	#region Weapons Behaviour

	public void AddBullet(WeaponsBehaviour bullet)
	{
		_model.LevelControler.AddBullet(bullet);
	}

	public void RemoveBullet(WeaponsBehaviour bullet)
	{
		_model.LevelControler.RemoveBullet(bullet);
	}

	#endregion

	#region Plane Behaviour

	public PlaneControlling CreatePlaneByType(PlaneType type)
	{
		PlaneControlling plane = default(PlaneControlling);

		switch (type)
		{
			case PlaneType.JoyKikka:
				plane = ResourceController.GetPlaneFromPool(PlaneControlling.JoyKikkaFightersPrefabsPath, FightersParent);
				break;

			case PlaneType.Ki30Nagoya:
				plane = ResourceController.GetPlaneFromPool(PlaneControlling.Ki30NagoyaFightersPrefabsPath, FightersParent);
				break;

			case PlaneType.Ki57:
				plane = ResourceController.GetPlaneFromPool(PlaneControlling.Ki57FightersPrefabsPath, FightersParent);
				break;
		}

		if (plane != null)
		{
			plane.Plane.SetPlaneDeathListener(_model.LevelControler);
		}
		return plane;
	}

	public void AddPlane(PlaneControlling plane)
	{
		_model.LevelControler.AddAiPlayer(plane);
	}

	public void RemovePlane(PlaneControlling plane)
	{
		_model.LevelControler.RemovePlane(plane);
	}

	#endregion

	#endregion

}

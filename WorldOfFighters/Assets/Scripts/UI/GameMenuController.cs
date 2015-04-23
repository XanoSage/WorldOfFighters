using Assets.Scripts.GameLogic.Game;
using Assets.Scripts.GameLogic.Plane;
using Assets.Scripts.UI;
using UnityEngine;
using System.Collections;



public class GameMenuController : MonoBehaviour, IShowable, IScoreListener 
{

	#region Variables

	private GameMenuModel _model;
	
	#endregion

	#region MonoBehaviour Actions

	// Use this for initialization
	void Start ()
	{
		_model = GetComponent<GameMenuModel>();

		if (_model == null)
		{
			throw new MissingComponentException("GameMenuController.Start - can't find GameMenuModel component");
		}

		SubscribeEvents();
		//UpdatePlayerLives(2);

		Hide();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy()
	{
		UnSubscribeEvents();
	}

	#endregion

	#region Actions

	private void SubscribeEvents()
	{
		_model.PauseButton.onClick.AddListener(OnPauseButtonClick);
	}

	private void UnSubscribeEvents()
	{
		_model.PauseButton.onClick.RemoveAllListeners();
	}

	private void OnPauseButtonClick()
	{
		Debug.Log("GameMenuController.OnPauseButtonClick - OK");

		PauseMenuController pauseMenuController = FindObjectOfType<PauseMenuController>();
		if (pauseMenuController != null && !pauseMenuController.Visible)
		{
			if (!pauseMenuController.Visible)
			{
				GameController.Instance.PausedGame();
				pauseMenuController.Show();
			}
			else
			{
				GameController.Instance.UnPausedGame();
				pauseMenuController.Hide();
			}
		}
	}

	private void UpdatePlayerLives(int lives)
	{
		if (lives < 0 || lives >= _model.PlayerLives.Count)
			return;

		for (int i = 0; i < lives; i++)
		{
			_model.PlayerLives[i].gameObject.SetActive(true);
		}

		for (int i = lives; i < _model.PlayerLives.Count; i++)
		{
			_model.PlayerLives[i].gameObject.SetActive(false);
		}
	}

	public void OnHighScoreUdate(int highScore)
	{
		_model.HighScoreText.text = highScore.ToString();
	}

	public void InitPlayerData(PlaneModel plane)
	{
		UpdatePlayerLives(plane.Lives);
	}

	public void OnPlayerDeath(PlaneModel plane)
	{
		UpdatePlayerLives(plane.Lives);
	}

	#endregion

	#region IShowable implementations

	public void Show()
	{
		_model.GameMenuPanel.gameObject.SetActive(true);
		Visible = true;
	}

	public void Hide()
	{
		_model.GameMenuPanel.gameObject.SetActive(false);
		Visible = false;
	}

	public bool Visible { get; set; }

	#endregion

	#region IScoreListener implementation

	public void OnScoreChange(int score)
	{
		_model.ScoreText.text = score.ToString();
	}

	#endregion
}

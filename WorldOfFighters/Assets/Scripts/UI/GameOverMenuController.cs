﻿using Assets.Scripts.UI;
using UnityEngine;
using System.Collections;

public class GameOverMenuController : MonoBehaviour, IShowable
{

	#region Variables

	private GameOverMenuModel _model;

	#endregion

	#region MonoBehaviour Actions

	// Use this for initialization
	private void Start()
	{
		_model = GetComponent<GameOverMenuModel>();

		if (_model == null)
		{
			throw new MissingComponentException("GameOverMenuController.Start - can't find GameIverMenuController component");
		}

		SubscribeEvents();
		HideThanksText();
		Hide();
	}

	// Update is called once per frame
	private void Update()
	{

	}

	private void OnDestroy()
	{
		UnSubscribeEvents();
	}

	#endregion

	#region Actions

	private void SubscribeEvents()
	{
		_model.PlayAgainButton.onClick.AddListener(OnPlayAgainButtonClick);
		_model.MainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
	}

	private void UnSubscribeEvents()
	{
		_model.PlayAgainButton.onClick.RemoveAllListeners();
		_model.PlayAgainButton.onClick.RemoveAllListeners();
	}

	private void OnPlayAgainButtonClick()
	{
		Debug.Log("GameOverMenuController.OnPlayAgainButtonClick - OK");
		Hide();
		GameController.Instance.ToMainMenu();
		GameController.Instance.StartGame();
	}

	private void OnMainMenuButtonClick()
	{
		Debug.Log("GameOverMenuController.OnMainMenuButtonClick - OK");
		Hide();
		GameController.Instance.ToMainMenu();

		GameMenuController gameMenuController = FindObjectOfType<GameMenuController>();
		if (gameMenuController != null)
		{
			gameMenuController.Hide();
		}

		MainMenuController mainMenuController = FindObjectOfType<MainMenuController>();
		if (mainMenuController != null)
		{
			mainMenuController.Show();
		}
	}

	public void Init(int score)
	{
		_model.ScoreText.text = score.ToString();
	}

	public void ShowThanksText()
	{
		_model.ScoreText.gameObject.SetActive(true);
	}

	public void HideThanksText()
	{
		_model.ScoreText.gameObject.SetActive(false);
	}

	#endregion

	public void Show()
	{
		_model.GameOverPanel.gameObject.SetActive(true);
		Visible = true;
	}

	public void Hide()
	{
		_model.GameOverPanel.gameObject.SetActive(false);
		HideThanksText();
		Visible = false;
	}

	public bool Visible { get; set; }
}

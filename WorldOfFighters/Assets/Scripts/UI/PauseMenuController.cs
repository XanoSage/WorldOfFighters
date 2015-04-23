using Assets.Scripts.UI;
using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour, IShowable
{

	#region Variables

	private PauseMenuModel _model;

	#endregion

	#region MonoBehaviour Actions

	// Use this for initialization
	private void Start()
	{
		_model = GetComponent<PauseMenuModel>();

		if (_model == null)
		{
			throw new MissingComponentException("PauseMenuController.Start - can't find PauseMenuModel component");
		}

		SubscribeEvents();
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
		_model.ContinueButton.onClick.AddListener(OnContinueButtonClick);
		_model.MainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
	}

	private void UnSubscribeEvents()
	{
		_model.ContinueButton.onClick.RemoveAllListeners();
		_model.ContinueButton.onClick.RemoveAllListeners();
	}

	private void OnContinueButtonClick()
	{
		Debug.Log("PauseMenuController.OnResumeButtonClick - OK");
		Hide();
		GameController.Instance.UnPausedGame();
	}

	private void OnMainMenuButtonClick()
	{
		Debug.Log("PauseMenuController.OnMainMenuClick - OK");
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

	#endregion

	#region IShowable Implementation

	public void Show()
	{
		_model.PauseMenuPanel.gameObject.SetActive(true);
		Visible = true;
	}

	public void Hide()
	{
		_model.PauseMenuPanel.gameObject.SetActive(false);
		Visible = false;
	}

	public bool Visible { get; set; }

	#endregion
}

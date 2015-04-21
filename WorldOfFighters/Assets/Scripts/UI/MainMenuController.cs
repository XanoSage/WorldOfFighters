using Assets.Scripts.UI;
using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour, IShowable
{

	#region Variables

	private MainMenuModel _model;

	#endregion
	
	#region MonoBehaviour Actions

	// Use this for initialization
	void Start ()
	{

		_model = GetComponent<MainMenuModel>();

		if (_model == null)
		{
			throw new MissingComponentException("MainMenuController.Start - can't find MainMenuModel component");
		}

		SubscribeEvents();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnDestroy()
	{
		UnsubscribeEvents();
	}

	#endregion

	#region Actions

	private void SubscribeEvents()
	{
		_model.PlayButton.onClick.AddListener(OnPlayButtonClick);
	}
	
	private void UnsubscribeEvents()
	{
		_model.PlayButton.onClick.RemoveAllListeners();
	}

	private void OnPlayButtonClick()
	{
		Debug.Log("MainMenuController.OnPlayButtonClick - OK");
		Hide();

		GameController.Instance.StartGame();

		GameMenuController gameMenuController = FindObjectOfType<GameMenuController>();

		if (gameMenuController != null)
		{
			gameMenuController.Show();
		}
	}

	#endregion

	#region IShowable implementations

	public void Show()
	{
		_model.MainMenuPanel.gameObject.SetActive(true);
		Visible = true;
	}

	public void Hide()
	{
		_model.MainMenuPanel.gameObject.SetActive(false);
		Visible = false;
	}

	public bool Visible { get; set; }

	#endregion
}

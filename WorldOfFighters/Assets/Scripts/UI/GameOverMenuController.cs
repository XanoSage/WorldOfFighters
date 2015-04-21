using Assets.Scripts.UI;
using UnityEngine;
using System.Collections;

public class GameOverMenuController : MonoBehaviour, IShowable 
{

	#region Variables

	private GameOverMenuModel _model;

	#endregion

	#region MonoBehaviour Actions

	// Use this for initialization
	void Start ()
	{
		_model = GetComponent<GameOverMenuModel>();

		if (_model == null)
		{
			throw new MissingComponentException("GameOverMenuController.Start - can't find GameIverMenuController component");
		}

		SubscribeEvents();

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
	}

	private void OnMainMenuButtonClick()
	{
		Debug.Log("GameOverMenuController.OnMainMenuButtonClick - OK");
	}

	public void Init(int score)
	{
		_model.ScoreText.text = score.ToString();
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
		Visible = false;
	}

	public bool Visible { get; set; }
}

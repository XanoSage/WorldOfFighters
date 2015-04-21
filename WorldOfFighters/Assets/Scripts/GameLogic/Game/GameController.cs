using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

	#region Variables

	public static GameController Instance { get; private set; }

	private GameModel _model;

	public GameModel.GameState State
	{
		get { return _model.State; }
	}

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
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	#endregion

	#region Actions

	public void StartGame()
	{
		_model.State = GameModel.GameState.Playing;
		//Достать самолёт из ресурсов, создать левел, создать левел контроллер сделать инициализацию
	}

	public void PausedGame()
	{
		_model.State = GameModel.GameState.Paused;
	}

	public void UnPausedGame()
	{
		_model.State = GameModel.GameState.Playing;
	}

	public void GameOver()
	{
		_model.State = GameModel.GameState.GameOver;
	}

	public void ToMainMenu()
	{
		_model.State = GameModel.GameState.BeforePlaying;
	}

	public void GameEnd()
	{
		_model.State = GameModel.GameState.GameEnd;
	}

	#endregion

}

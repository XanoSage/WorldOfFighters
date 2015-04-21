using Assets.Scripts.GameLogic.Game;
using UnityEngine;
using System.Collections;

public class GameModel : MonoBehaviour
{
	public enum GameState
	{
		BeforePlaying,
		Playing,
		Paused,
		GameOver,
		GameEnd
	}

	#region Variables

	[HideInInspector] public GameState State;
	public LevelController LevelControler;

	#endregion
}

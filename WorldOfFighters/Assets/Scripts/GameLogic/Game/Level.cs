using System;
using System.Collections.Generic;
using Assets.Scripts.GameLogic.Plane;
using Assets.Scripts.GameLogic.Weapons;

namespace Assets.Scripts.GameLogic.Game
{
	public interface IPlaneDeathListener
	{
		void OnPlaneDeath(PlaneModel planeModel);
	}

	public class Level
	{
		#region Variables

		private int _playerScore;

		public int PlayerScore
		{
			get { return _playerScore; }
		}

		private PlaneControlling _player;

		private List<PlaneControlling> _aiPlayers;

		public List<PlaneControlling> AiPlaneList
		{
			get { return _aiPlayers; }
		}
		
		#endregion

		#region Constructor

		public Level()
		{
			_aiPlayers = new List<PlaneControlling>();
			_player = null;
			_playerScore = 0;
		}

		public Level(PlaneControlling player)
		{
			_player = player;
			_aiPlayers = new List<PlaneControlling>();
			_playerScore = 0;
		}

		#endregion

		#region Actions

		public void AddAiPlayer(PlaneControlling aiPlane)
		{
			_aiPlayers.Add(aiPlane);
		}

		public void AddAiPlayers(List<PlaneControlling> aiPlanes)
		{
			_aiPlayers.AddRange(aiPlanes);
		}

		public void RemovePlayers()
		{
			_aiPlayers.Clear();
		}

		public void RemovePlayer(PlaneControlling aiPlane)
		{
			_aiPlayers.Remove(aiPlane);
		}

		public void AddPlayerLive()
		{
			_player.Plane.AddLive();
		}

		public void AddScore(int score)
		{
			_playerScore += score;

			//ScoreUpdate event or listener
		}

		public void RemovePlane(PlaneModel plane)
		{
			_aiPlayers.RemoveAll(aiPlane=> aiPlane.Plane == plane);
		}

		#endregion

	}

	public class LevelController: IPlaneDeathListener
	{
		#region Variables

		private Level _level;


		public event Action OnPlayerDeathAction;

		public event Action OnPlayerGameOver;

		#endregion

		#region Constructor

		public LevelController()
		{

		}

		public LevelController(Level level)
		{
			_level = level;
		}

		public static LevelController Create(Level level)
		{
			return new LevelController(level);
		}
		#endregion

		#region Actions

		public void AddAiPlayer(PlaneControlling aiPlayer)
		{
			_level.AddAiPlayer(aiPlayer);
		}

		public void AddAiPlayers(List<PlaneControlling> aiPlayers)
		{
			_level.AddAiPlayers(aiPlayers);
		}

		public void RemovePlayers()
		{
			_level.RemovePlayers();
		}

		public void RemovePlayer(PlaneControlling aiPlayer)
		{
			_level.RemovePlayer(aiPlayer);
		}

		public void OnPlayerGameOverActions()
		{
			if (OnPlayerGameOver != null)
			{
				OnPlayerGameOver();
			}
		}

		#endregion

		#region IPlaneDeathListener implementation
		
		public void OnPlaneDeath(PlaneModel planeModel)
		{
			if (planeModel.Owner == OwnerInfo.Player)
			{
				if (OnPlayerDeathAction != null)
				{
					OnPlayerDeathAction();
				}
				planeModel.SubstractionLives();
			}
			else
			{
				_level.AddScore(planeModel.BonusPoint);
			}
		}

		#endregion
	}


}

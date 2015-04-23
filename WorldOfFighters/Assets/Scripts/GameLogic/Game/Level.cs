using System;
using System.Collections.Generic;
using Assets.Scripts.GameLogic.Plane;
using Assets.Scripts.GameLogic.Weapons;
using Object = UnityEngine.Object;

namespace Assets.Scripts.GameLogic.Game
{
	public interface IPlaneDeathListener
	{
		void OnPlaneDeath(PlaneModel planeModel);
	}

	public interface IScoreListener
	{
		void OnScoreChange(int score);
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

		public PlaneControlling PlayerPlane
		{
			get { return _player; }
		}

		private List<PlaneControlling> _aiPlayers;

		public List<PlaneControlling> AiPlaneList
		{
			get { return _aiPlayers; }
		}

		private List<WeaponsBehaviour> _weaponsBehaviours;
		
		public List<WeaponsBehaviour> Bullets
		{
			get { return _weaponsBehaviours; }
		}

		#endregion

		#region Constructor

		public Level()
		{
			_aiPlayers = new List<PlaneControlling>();
			_player = null;
			_playerScore = 0;
			_weaponsBehaviours = new List<WeaponsBehaviour>();
		}

		public Level(PlaneControlling player)
		{
			_player = player;
			_aiPlayers = new List<PlaneControlling>();
			_weaponsBehaviours = new List<WeaponsBehaviour>();
			_playerScore = 0;
		}

		public static Level Create()
		{
			return new Level();
		}

		public static Level Create(PlaneControlling playerPlane)
		{
			return new Level(playerPlane);
		}
		#endregion

		#region Actions

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

		public void OnDestroy()
		{
			PlaneControlling plane = _player;
			//Object.Destroy(plane.gameObject);
			plane.PlaneDestroy();
			_player = null;

			while (_aiPlayers.Count > 0)
			{
				PlaneControlling aiPlane = _aiPlayers[0];
				//Object.Destroy(aiPlane.gameObject);
				aiPlane.PlaneDestroy();
				_aiPlayers.Remove(aiPlane);
			}

			while (_weaponsBehaviours.Count > 0)
			{
				WeaponsBehaviour bullet = _weaponsBehaviours[0];
				bullet.BulletDestroy();
				_weaponsBehaviours.Remove(bullet);
			}
		}

		#endregion

	}

	public class LevelController: IPlaneDeathListener
	{
		#region Variables

		private Level _level;


		public event Action OnPlayerDeathAction;

		public event Action OnPlayerGameOver;

		public event Action<int> OnScoreChange;

		private IScoreListener _scoreListener;

		public PlaneControlling PlayerPlane
		{
			get { return _level.PlayerPlane; }
		}

		#endregion

		#region Constructor

		public LevelController()
		{

		}

		public LevelController(Level level, IScoreListener scoreListener)
		{
			_level = level;
			_scoreListener = scoreListener;
		}

		public static LevelController Create(Level level, IScoreListener scoreListener)
		{
			return new LevelController(level, scoreListener);
		}
		#endregion

		#region Actions

		public void AddAiPlayer(PlaneControlling aiPlayer)
		{
			_level.AiPlaneList.Add(aiPlayer);
		}

		public void AddAiPlayers(List<PlaneControlling> aiPlayers)
		{
			_level.AiPlaneList.AddRange(aiPlayers);
		}

		public void RemovePlanes()
		{
			_level.RemovePlayers();
		}

		public void RemovePlane(PlaneControlling aiPlayer)
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

		public void AddBullet(WeaponsBehaviour bullet)
		{
			_level.Bullets.Add(bullet);
		}

		public void AddBullets(List<WeaponsBehaviour> bullets)
		{
			_level.Bullets.AddRange(bullets);
		}

		public void RemoveBullet(WeaponsBehaviour bullet)
		{
			_level.Bullets.Remove(bullet);
		}

		public void Init()
		{
			_scoreListener.OnScoreChange(_level.PlayerScore);
			if (OnScoreChange != null)
			{
				OnScoreChange(_level.PlayerScore);
			}
		}

		public void OnDestroy()
		{
			_scoreListener = null;
			_level.OnDestroy();
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
				//planeModel.SubstractionLives();
			}
			else
			{
				_level.AddScore(planeModel.BonusPoint);
				_scoreListener.OnScoreChange(_level.PlayerScore);
				if (OnScoreChange != null)
				{
					OnScoreChange(_level.PlayerScore);
				}
			}
		}

		#endregion
	}


}

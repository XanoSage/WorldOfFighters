using System;
using System.Collections.Generic;
using Assets.Scripts.GameLogic.Game;
using UnityEngine;
using System.Collections;

public class AiGenerator : MonoBehaviour
{

	#region Variables

	public enum GeneratorAction
	{
		Start,
		Pause,
		CreateKi30Nagoya, // white, enemy
		CreateJoyKikka, // black
		CreateKi57, // gray
		Over
	}

	[Serializable]
	public class StageDirector
	{
		[SerializeField] public float Duration;
		[SerializeField] public GeneratorAction StageAction;
	}

	[SerializeField] private List<StageDirector> _actions;

	private StageDirector _currentStage;
	private int _index = 0;

	private float _durationCounter = 0;

	#endregion

	#region MonoBehaviourActions

	// Use this for initialization
	private void Start()
	{
		_index--;
		PrepareNextStage();
	}

	// Update is called once per frame
	private void Update()
	{

		if (GameController.Instance != null && GameController.Instance.State != GameModel.GameState.Playing)
			return;

		UpdateStageDirector();

	}

	#endregion

	#region Action

	private void UpdateStageDirector()
	{
		if (_currentStage != null)
		{
			if (_durationCounter < _currentStage.Duration)
			{
				_durationCounter += Time.deltaTime;
			}

			if (_durationCounter >= _currentStage.Duration)
			{
				PrepareNextStage();
				DoAction(_currentStage.StageAction);
			}
		}
	}

	private void DoAction(GeneratorAction action)
	{
		switch (action)
		{
			case GeneratorAction.CreateJoyKikka:
				CreateJoyKikka();
				break;
			case GeneratorAction.CreateKi30Nagoya:
				CreateKi30Nagoya();
				break;
			case GeneratorAction.CreateKi57:
				CreateKi57();
				break;
			case GeneratorAction.Start:
				break;
			case GeneratorAction.Pause:
				break;
			case GeneratorAction.Over:
				GameController.Instance.GameEnd();
				Reset();
				break;
		}
	}

	private void CreateKi30Nagoya()
	{
		PlaneControlling plane = ResourceController.GetPlaneFromPool(PlaneControlling.Ki30NagoyaFightersPrefabsPath,
		                                                             GameController.Instance.FightersParent);
		if (plane != null)
		{
			plane.Plane.SetPlaneDeathListener(GameController.Instance.GetPlaneDeathLestener());
			GameController.Instance.AddPlane(plane);
		}
	}

	private void CreateJoyKikka()
	{
		PlaneControlling plane = ResourceController.GetPlaneFromPool(PlaneControlling.JoyKikkaFightersPrefabsPath,
		                                                             GameController.Instance.FightersParent);
		if (plane != null)
		{
			plane.Plane.SetPlaneDeathListener(GameController.Instance.GetPlaneDeathLestener());
			GameController.Instance.AddPlane(plane);
		}
	}

	private void CreateKi57()
	{
		PlaneControlling plane = ResourceController.GetPlaneFromPool(PlaneControlling.Ki57FightersPrefabsPath,
		                                                             GameController.Instance.FightersParent);
		if (plane != null)
		{
			plane.Plane.SetPlaneDeathListener(GameController.Instance.GetPlaneDeathLestener());
			GameController.Instance.AddPlane(plane);

		}
	}

	private void PrepareNextStage()
	{
		_index++;
		if (_index >= _actions.Count)
		{
			_currentStage = null;
			return;
		}
		_currentStage = _actions[_index];
		_durationCounter = 0;
	}

	public void Reset()
	{
		_index = -1;
		_durationCounter = 0;
		PrepareNextStage();
	}

	#endregion
}

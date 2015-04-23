using System.Collections.Generic;
using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlaneControlling))]
public class AiMoveByPoint : MonoBehaviour
{

	#region Variables

	private PlaneControlling _planeControlling;

	private List<Transform> _variousPointPosition;

	private Transform _currentTargetPoint;

	private int _indexOfPos = 0;

	private Vector3 _direction;

	private float _distance = 1f;

	private AiPathHelper _aiPathHelper;
	#endregion

	#region MonoBehavoiur Actions

	void Awake()
	{
		_planeControlling = GetComponent<PlaneControlling>();
		_aiPathHelper = FindObjectOfType<AiPathHelper>();
		if (_aiPathHelper == null)
		{
			throw new MissingComponentException("AiMoveByPoint.Start - OK");
		}
	}

	// Use this for initialization
	void Start ()
	{
		_variousPointPosition = _aiPathHelper.GetAiPath();

		transform.position = _variousPointPosition[0].position;

		Init();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.Instance != null && GameController.Instance.State != GameModel.GameState.Playing)
			return;

		UpdatePosition();
	}

	void OnEnable()
	{
		Init();
	}

	#endregion

	#region Actions

	private void AutoDestroy()
	{
		_planeControlling.ResetPlaneData();
		_planeControlling.PlaneDestroy();
	}

	private void PowerOn()
	{
		_planeControlling.SetIsPowerOn(true);
	}

	private void PowerOff()
	{
		_planeControlling.SetIsPowerOn(false);
	}

	private void SetDirection()
	{
		_planeControlling.SetDirection(_direction);
	}

	private void Init()
	{
		_indexOfPos = 0;
		_variousPointPosition = _aiPathHelper.GetAiPath();
		_currentTargetPoint = _variousPointPosition[_indexOfPos];
		_direction = (_currentTargetPoint.position - transform.position).normalized;
		SetDirection();
		PowerOn();
	}

	private void UpdatePosition()
	{
		float distance = Vector3.Distance(_currentTargetPoint.position, transform.position);
		if (distance < _distance)
		{
			PowerOff();
			_indexOfPos++;

			if (_indexOfPos < _variousPointPosition.Count)
			{
				_currentTargetPoint = _variousPointPosition[_indexOfPos];
				_direction = (_currentTargetPoint.position - transform.position).normalized;
				SetDirection();
				PowerOn();
			}

			else
			{
				Reset();
				AutoDestroy();
			}
		}
	}

	private void Reset()
	{
		_indexOfPos = 0;
		PowerOff();

		_variousPointPosition = _aiPathHelper.GetAiPath();
		transform.position =_variousPointPosition[0].position;
	}
	#endregion
}

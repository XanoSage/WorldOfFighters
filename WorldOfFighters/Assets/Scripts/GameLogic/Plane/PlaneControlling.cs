using Assets.Scripts.GameLogic.Plane;
using Assets.Scripts.GameLogic.Weapons;
using UnityEngine;
using System.Collections;

public class PlaneControlling : PoolItem
{
	#region Constants

	private const float MinPlaneSpeed = 0;

	public const string PlayerPlanePrefabsPath = "Prefabs/Fighters/PlayerFighters";
	public const string JoyKikkaFightersPrefabsPath = "Prefabs/Fighters/EnemiesFighters/JoyKikkaFighters";
	public const string Ki30NagoyaFightersPrefabsPath = "Prefabs/Fighters/EnemiesFighters/Ki30NagoyaFighters";
	public const string Ki57FightersPrefabsPath = "Prefabs/Fighters/EnemiesFighters/Ki57Fighters";

	#endregion

	#region Variables

	[SerializeField] private Transform _planeTransform;

	[SerializeField] private PlaneSimple _planeSimple;

	private PlaneModel _planeModel;

	private Vector2 _direction;

	private Rigidbody2D _rigidbody;

	private float _currentVelocity = 0f;
	private float _acceleration = 0f;

	private Vector2 _vectorVelocity;

	private bool _isPowerOn = false;
	private bool _isMoving = false;

	[HideInInspector]
	public OwnerInfo Owner
	{
		get { return _planeModel != null ? _planeModel.Owner : OwnerInfo.AI; }
	}


	public PlaneModel Plane
	{
		get { return _planeModel; }
	}

	public PlaneType Type
	{
		get { return _planeModel != null ? _planeModel.Type : _planeSimple != null ? _planeSimple.Type : PlaneType.G4M1Betty; }
	}

	#endregion

	#region MonoBehaviours Actions

	// Use this for initialization
	private void Start()
	{

	}

	private void Awake()
	{
		_planeModel = PlaneModel.Create(_planeSimple);
		_rigidbody = GetComponent<Rigidbody2D>();

		_direction = Vector2.zero;

		_vectorVelocity = Vector2.zero;

		if (_rigidbody == null)
		{
			throw new MissingComponentException("PlaneControlling.Start - can't find RigidBody2d components");
		}
	}

	// Update is called once per frame
	private void Update()
	{

	}

	private void FixedUpdate()
	{
		if (GameController.Instance != null && GameController.Instance.State != GameModel.GameState.Playing)
			return;

		if (Plane.State == PlaneState.Death)
			return;

		UpdateDirections();

		UpdateMovement();

		UpdateRigidBodyMovement();
		UpdateAcceleration();
	}

	#endregion

	#region Actions

	#region Movements

	private void UpdateDirections()
	{
		if (_planeModel == null)
			return;

		if (_planeModel.Owner == OwnerInfo.AI)
			return;

		Vector2 direction = Vector2.zero;

		_isPowerOn = false;
		_isMoving = false;

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			direction += new Vector2(-1, 0);
			_isMoving = true;
			_isPowerOn = true;
		}
		else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			direction += Vector2.right;
			_isMoving = true;
			_isPowerOn = true;
		}

		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			direction += Vector2.up;
			_isMoving = true;
			_isPowerOn = true;
		}
		else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			direction += new Vector2(0, -1);
			_isMoving = true;
			_isPowerOn = true;
		}

		if (Input.GetKey(KeyCode.G))
		{
			_isPowerOn = true;
		}

		if (direction != Vector2.zero)
		{
			_direction = direction;
		}

	}

	private void UpdateMovement()
	{
		if (_planeModel == null)
			return;

		_vectorVelocity = _direction*
		                  (_currentVelocity*Time.deltaTime + _acceleration*0.5f*Time.deltaTime*Time.deltaTime);


		_currentVelocity += _acceleration*Time.deltaTime;

		_currentVelocity = Mathf.Clamp(_currentVelocity, MinPlaneSpeed, _planeModel.Speed);
	}

	private void UpdateRigidBodyMovement()
	{
		if (_planeModel == null)
			return;

		_rigidbody.velocity = _vectorVelocity;
	}

	private void UpdateAcceleration()
	{
		if (_planeModel == null)
			return;

		if (!_isPowerOn)
		{
			_acceleration = _currentVelocity > MinPlaneSpeed ? _planeModel.AccelerationDown : 0;
		}

		else if (_isPowerOn)
		{
			_acceleration = _currentVelocity >= _planeModel.Speed ? 0f : _planeModel.Acceleration;
		}
	}

	public void ResetPlaneData()
	{
		_currentVelocity = 0;
		_acceleration = 0;
		_isPowerOn = false;
		_isMoving = false;
	}

	public void SetIsPowerOn(bool powerOn)
	{
		_isPowerOn = powerOn;
	}

	public void SetDirection(Vector3 direction)
	{
		_direction = direction;
	}

	#endregion

	#endregion

	#region PoolItem implementation

	public override bool EqualsTo(PoolItem item)
	{
		if (!(item is PlaneControlling))
			return false;

		PlaneControlling plane = item as PlaneControlling;

		if (plane.Type != Type)
		{
			return false;
		}

		return true;
	}

	public override void Activate()
	{
		base.Activate();
		gameObject.SetActive(true);
	}

	public override void Deactivate()
	{
		base.Deactivate();
		gameObject.SetActive(false);
	}

	public void PlaneDestroy()
	{
		GameController.Instance.RemovePlane(this);
		_planeModel.Reset(true);

		Pool.Push(this);
		Deactivate();
	}

	#endregion
}

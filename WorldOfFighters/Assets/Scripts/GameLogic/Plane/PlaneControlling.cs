using Assets.Scripts.GameLogic.Plane;
using Assets.Scripts.GameLogic.Weapons;
using UnityEngine;
using System.Collections;

public class PlaneControlling : PoolItem
{
	#region Constants

	private const float MinPlaneSpeed = 0;

	#endregion

	#region Variables

	[SerializeField]
	private Transform _planeTransform;

	[SerializeField] private PlaneType _planeType;
	[SerializeField] private int _lives;
	[SerializeField] private int _helathPoint;
	[SerializeField] private float _speed;
	[SerializeField] private float _accelerationPlane;
	[SerializeField] private float _accelerationDown;
	[SerializeField] private OwnerInfo _owner;

	private PlaneModel _planeModel;

	private Vector2 _direction;

	private Rigidbody2D _rigidbody;

	private float _currentVelocity = 0f;
	private float _acceleration = 0f;

	private Vector2 _vectorVelocity;

	private bool _isPowerOn = false;
	private bool _isMoving = false;

	#endregion

	#region MonoBehaviours Actions

	// Use this for initialization
	void Start ()
	{
		_planeModel = PlaneModel.Create(_planeType, 0, _lives, _helathPoint, _speed, _accelerationPlane, _accelerationDown, _owner);
		_rigidbody = GetComponent<Rigidbody2D>();

		_direction = Vector2.zero;

		_vectorVelocity = Vector2.zero;

		if (_rigidbody == null)
		{
			throw new MissingComponentException("PlaneControlling.Start - can't find RigidBody2d components");
		}
	}
	
	// Update is called once per frame
	void Update () {
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

		Vector2 direction = Vector2.zero;

		_isPowerOn = false;
		_isMoving = false;

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			direction +=new Vector2(-1, 0);
			_isMoving = true;
		}
		else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			direction += Vector2.right;
			_isMoving = true;
		}

		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			direction += Vector2.up;
			_isMoving = true;
		}
		else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			direction += new Vector2(0, -1);
			_isMoving = true;
		}

		if (Input.GetKey(KeyCode.G))
		{
			_isPowerOn = true;
		}

		if (direction != Vector2.zero)
		{
			//Debug.Log("Directions :" + direction);
			_isPowerOn = true;

			//if (!_isMoving)
				_direction = direction;
		}

	}

	private void UpdateMovement()
	{
		if (_planeModel == null)
			return;

		_vectorVelocity =  _direction *
									 (_currentVelocity * Time.deltaTime + _acceleration * 0.5f * Time.deltaTime * Time.deltaTime);

		
		_currentVelocity += _acceleration * Time.deltaTime;
		
		_currentVelocity = Mathf.Clamp(_currentVelocity, MinPlaneSpeed, _planeModel.Speed);

		//Debug.Log(string.Format("velocity: {0}, currentVelocity:{1}, acceleration: {2}, forward: {3}", _vectorVelocity, _currentVelocity, _acceleration));
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

	#endregion

	#endregion

	#region PoolItem implementation

	public override bool EqualsTo(PoolItem item)
	{
		throw new System.NotImplementedException();
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

	#endregion
}

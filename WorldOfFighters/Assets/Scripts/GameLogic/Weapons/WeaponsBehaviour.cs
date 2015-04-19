using Assets.Scripts.GameLogic.Weapons;
using Assets.Scripts.GameLogic.Helper;
using UnityEngine;
using System.Collections;

public class WeaponsBehaviour : PoolItem, ICooldownTime
{

	#region Variables

	[SerializeField] private WeaponsType _type;

	[SerializeField] private float _speed;
	[SerializeField] private float _blowUpTime;
	[SerializeField] private int _damage;
	[SerializeField] private float _cooldownTimer;

	private Weapons _weaponModel;

	private Rigidbody2D _rigidbody;

	private float _blowUpCounter = 0;

	private Vector3 _direction = Vector3.zero;

	private bool _isInit;

	public WeaponsType Type
	{
		get { return _type; }
	}

	#endregion

	#region MonoBehaviour Action

	void Awake()
	{
		_weaponModel = Weapons.Create(_type, _speed, _cooldownTimer, _blowUpTime, _damage);
	}
	
	// Use this for initialization
	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody2D>();

		if (_rigidbody == null)
		{
			throw new MissingComponentException("WeaponsBehaviour.Start - can't find RigidBody component");
		}

		_type = WeaponsType.Missile;

		_isInit = false;

	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate()
	{
		UpdatePosition();
		UpdateCounter();
	}
	#endregion

	#region Actions

	private void TimetoBlowUp()
	{
		//Pool.Push(this);
		transform.parent = null;
		Destroy(gameObject);
	}

	private void UpdatePosition()
	{
		if (_rigidbody == null)
			return;

		if (_weaponModel == null)
			return;

		_rigidbody.transform.Translate(_direction * _weaponModel.Speed);
	}

	private void UpdateCounter()
	{
		if (_weaponModel == null)
			return;

		if (_blowUpCounter < _weaponModel.TimeToBlowUp)
		{
			_blowUpCounter += Time.deltaTime;
		}

		if (_blowUpCounter >= _weaponModel.TimeToBlowUp)
		{
			_blowUpCounter = 0;
			TimetoBlowUp();
		}
	}

	public void Init(Transform parent, Vector3 startPosition,  Quaternion startRotation, Vector3 direction)
	{
		transform.parent = parent;
		transform.position = startPosition;
		transform.rotation = startRotation;
		_direction = direction;

		_blowUpCounter = 0;

		_isInit = true;
	}

	#endregion

	#region PoolItem Implementation

	public override bool EqualsTo(PoolItem item)
	{
		if (!(item is WeaponsBehaviour))
			return false;

		WeaponsBehaviour weapons = item as WeaponsBehaviour;

		if (weapons.Type != Type)
			return false;

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

	#endregion

	#region ICooldown implementation

	public float CooldownTime
	{
		get { return _weaponModel == null ? _cooldownTimer : _weaponModel.CooldownTime; }
		set { }
	}

	#endregion
}

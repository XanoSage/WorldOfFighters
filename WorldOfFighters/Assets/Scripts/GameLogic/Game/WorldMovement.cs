using UnityEngine;
using System.Collections;

public class WorldMovement : MonoBehaviour
{
	#region Variables

	[SerializeField] private float _speed;

	[SerializeField]
	private float _minBorder;
	[SerializeField]
	private float _maxBorder;
	private float _spriteWidth;
	private float _spriteHeight;

	[SerializeField] private bool _isNeedReplay = false;

	private Vector3 _startPosition;

	#endregion

	#region MonoBehaviour Actions

	// Use this for initialization
	void Start ()
	{
		_startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.Instance != null && GameController.Instance.State != GameModel.GameState.Playing)
		return;

		UpdatePosition();
	}

	#endregion

	#region Actions

	private void UpdatePosition()
	{
		Vector3 newPos = transform.position + Vector3.down*_speed*Time.deltaTime;
		transform.position = Vector3.Lerp(transform.position, newPos, 0.33f);
		CheckPosition();
	}

	private void CheckPosition()
	{
		if ((transform.position.y < _minBorder))
		{
			if (_isNeedReplay)
				transform.position = new Vector3(transform.position.x, _maxBorder, transform.position.z);
			else
			{
				Innactive(false);
			}
		}
	}

	private void Innactive(bool acctive)
	{
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		if (sprite != null)
		{
			sprite.enabled = acctive;
		}
	}

	public void Reset()
	{
		transform.position = _startPosition;
		Innactive(true);
	}
	#endregion
}

using UnityEngine;

[RequireComponent(typeof(GhostMove))]
public class GhostAI : MonoBehaviour
{
	private GhostMove _ghostMove;

	private Transform _pacman;

	public void Reset()
	{
		_ghostMove.CharacterMotor.ResetPosition();
	}

	public void StartMoving()
	{
		_ghostMove.CharacterMotor.enabled = true;
	}

	public void StopMoving()
	{
		_ghostMove.CharacterMotor.enabled = false;
	}

	private void Awake()
	{
		_ghostMove = GetComponent<GhostMove>();
		_ghostMove.OnUpdateMoveTarget += GhostMove_OnUpdateMoveTarget;

		_pacman = GameObject.FindWithTag("Player").transform;
	}

	private void GhostMove_OnUpdateMoveTarget()
	{
		_ghostMove.SetTargetMoveLocation(_pacman.position);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<Life>().RemoveLife();
		}
	}
}

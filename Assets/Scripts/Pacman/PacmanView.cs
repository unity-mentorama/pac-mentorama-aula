using UnityEngine;

public class PacmanView : BaseView
{
	public CharacterMotor CharacterMotor;

	public Life CharacterLife;

	public Animator Animator;

	public AudioSource AudioSource;

	public AudioClip LifeLostSound;

	private void Start()
	{
		CharacterMotor.OnDirectionChanged += CharacterMotor_OnDirectionChanged;
		CharacterMotor.OnResetPosition += CharacterMotor_OnResetPosition;
		CharacterMotor.OnDisabled += CharacterMotor_OnDisabled;
		CharacterLife.OnLifeRemoved += CharacterLife_OnLifeRemoved;

		Animator.SetBool("Moving", false);
		Animator.SetBool("Dead", false);
	}

	private void CharacterMotor_OnDisabled()
	{
		Animator.speed = 0;
	}

	private void CharacterMotor_OnResetPosition()
	{
		Animator.SetBool("Moving", false);
		Animator.SetBool("Dead", false);
	}

	private void CharacterLife_OnLifeRemoved(int _)
	{
		transform.Rotate(0, 0, -90);
		AudioSource.PlayOneShot(LifeLostSound);
		Animator.speed = 1;
		Animator.SetBool("Moving", false);
		Animator.SetBool("Dead", true);
	}

	private void CharacterMotor_OnDirectionChanged(Direction direction)
	{
		switch (direction)
		{
			case Direction.None:
				Animator.SetBool("Moving", false);
				break;

			case Direction.Up:
				transform.rotation = Quaternion.Euler(0, 0, 90);
				Animator.SetBool("Moving", true);
				break;

			case Direction.Left:
				transform.rotation = Quaternion.Euler(0, 0, 180);
				Animator.SetBool("Moving", true);
				break;

			case Direction.Down:
				transform.rotation = Quaternion.Euler(0, 0, 270);
				Animator.SetBool("Moving", true);
				break;

			case Direction.Right:
				transform.rotation = Quaternion.Euler(0, 0, 0);
				Animator.SetBool("Moving", true);
				break;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : Interactive {

	private bool _moving = false;

	public bool moving {
		get {
			return _moving;
		}
		private set {
			_moving = value;

			if (animator == null) {
				animator = GetComponent<Animator>();
			}

			animator.SetBool("moving", moving);
		}
	}

	private Animator animator;
	private float startY;

	public float moveSpeed = 5.0f;
	public float upperY;
	public float lowerY;

	public override void Awake() {
		base.Awake();

		startY = transform.position.y;
		upperY += startY;
		lowerY += startY;
	}

	override public void Interact() {
		StartCoroutine(Move());
	}

	IEnumerator Move() {
		if (moving) {
			yield break;
		}

		bool up = transform.position.y != upperY;

		moving = true;

		yield return new WaitForSeconds(1f);

		if (up) {
			while (transform.position.y != upperY) {
				Vector3 newPos = transform.position;
				newPos.y += moveSpeed * Time.deltaTime;
				if (newPos.y > upperY) {
					newPos.y = upperY;
				}
				transform.position = newPos;

				yield return null;
			}
		} else {
			while (transform.position.y != lowerY) {
				Vector3 newPos = transform.position;
				newPos.y -= moveSpeed * Time.deltaTime;
				if (newPos.y < lowerY) {
					newPos.y = lowerY;
				}
				transform.position = newPos;

				yield return null;
			}
		}

		moving = false;

		yield break;
	}
}
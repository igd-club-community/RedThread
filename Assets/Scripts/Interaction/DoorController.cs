 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Interactive {

	private bool _opened;
	public bool opened {
		get {
			return _opened;
		}
		private set {
			_opened = value;

			if (animator == null) {
				animator = GetComponent<Animator>();
			}

			animator.SetBool("opened", opened);
		}
	}

	private Animator animator;

	public float openingSpeed = 5.0f;

	override public void Interact() {
		opened = !opened;
	}
}
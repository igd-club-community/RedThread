using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive: MonoBehaviour {
	public bool isInteractiveDirectly = true;
	public virtual void Awake() {
		if (isInteractiveDirectly) {
			gameObject.layer = LayerMask.NameToLayer("Interactive");
		}
	}
	public abstract void Interact();
}

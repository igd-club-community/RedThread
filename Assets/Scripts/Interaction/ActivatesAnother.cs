using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatesAnother : Interactive {

	[SerializeField]
	private GameObject activationObject;
	public bool activated = true;

	override public void Awake() {
		base.Awake();
		if (activationObject != null) {
			activationObject.gameObject.SetActive(activated);
		}
	}

	override public void Interact() {
		activated = !activated;
		if (activationObject != null) {
			activationObject.SetActive(activated);
		}
	}
}

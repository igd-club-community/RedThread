using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractsAnother : Interactive {
	[SerializeField]
	private Interactive interactionObject;

	override public void Interact() {
		if (interactionObject != null) {
			interactionObject.gameObject.SendMessage("Interact");
		}
	}
}

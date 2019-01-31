using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIntreaction : Interactive {

	private DoorController mainDoorObject;
	public override void Awake() {
		base.Awake();

		mainDoorObject = GetComponentInParent<DoorController>();
	}
	override public void Interact() {
		mainDoorObject.Interact();
	}
}

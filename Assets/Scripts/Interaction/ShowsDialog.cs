using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowsDialog : Interactive {

	public string dialogText;
	public float dialogTime = 2f;

	override public void Interact() {
		DialogRenderer dialogRenderer =  gameObject.GetComponent<DialogRenderer>();
		if (dialogRenderer != null) {
			dialogRenderer.ShowDialog(dialogText, dialogTime);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {
	public float maxRaycastRange = 100f;
	private int _layerMask;

	void Awake() {
		_layerMask = LayerMask.GetMask("Interactive");
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Interactive interactionObject = GetInteractionObject();
			if (interactionObject != null) {
				Debug.Log("Interaction with " + interactionObject.gameObject.name + ".");
				interactionObject.SendMessage("Interact");
			}		
		}
	}

	private Interactive GetInteractionObject() {
		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, maxRaycastRange, _layerMask)) {
			if (hit.collider != null) {
				if (hit.collider.gameObject.GetComponent<Interactive>() != null) {
					return hit.collider.gameObject.GetComponent<Interactive>();
				}
			}
		}
		return null;
	}
}

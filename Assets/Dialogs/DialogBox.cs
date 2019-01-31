using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DialogBox : MonoBehaviour {

	public Transform owner;
	public float moveSpeed = 2f;

	private bool initialized;
	private Vector3 offset;
	
	void Update () {
		if (owner == null) {
			Destroy(gameObject);
			return;
		}

		if (initialized) {
			transform.position = Vector3.Lerp(transform.position, owner.position - offset, moveSpeed);
		}
	}

	public void Initialize(Transform owner, string text) {
		this.owner = owner;
		offset = owner.position - transform.position;
		initialized = true;
		GetComponentInChildren<Text>().text = text;
	}
}

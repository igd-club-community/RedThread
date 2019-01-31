using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogRenderer : MonoBehaviour {

	[SerializeField]
	private DialogBox dialogPrefab;
	[SerializeField]
	private Canvas dialogCanvas;

	private DialogBox currentDialog;
	private bool dialogIsShowing = false;
	void Start () {
		currentDialog = null;
	}

	public void ShowDialog(string text, float dialogTime) {
		StartCoroutine(CreateDialogBox(text, dialogTime));
	}

	private IEnumerator CreateDialogBox(string text, float dialogTime) {
		while (dialogIsShowing) {
			yield return null;
		}

		yield return null;

		dialogIsShowing = true;
		currentDialog = Instantiate(dialogPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
		currentDialog.transform.SetParent(dialogCanvas.transform);
		currentDialog.Initialize(this.transform, text);

		yield return new WaitForSeconds(dialogTime);

		if (currentDialog != null) {
			Destroy(currentDialog.gameObject);
		}
		dialogIsShowing = false;

		yield break;
	}

	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z_MonoBase : MonoBehaviour {
	[Header ("MonoBase Parameter")]
	public bool DebugMode = true;

	protected void ShowMessage (string s) {
		if (DebugMode)
			Debug.Log (s);
	}
	protected void ShowError (string s) {
		if (DebugMode)
			Debug.LogError (s);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexEditor : MonoBehaviour {

	Hex hex;
	private Color startcolor;

	void Start() {
		this.hex = GetComponent<Hex>();
	}

	void OnMouseEnter() {
		startcolor = GetComponent<Renderer>().material.color;
		GetComponent<Renderer>().material.color = Color.yellow;
	}

	void OnMouseOver() {
		if (Input.GetMouseButton (0)) {
			this.GetComponentInParent<GridField> ().ChangeHex (hex.x, hex.z);
		}
	}

	void OnMouseExit() {
		GetComponent<Renderer>().material.color = startcolor;
	}


}

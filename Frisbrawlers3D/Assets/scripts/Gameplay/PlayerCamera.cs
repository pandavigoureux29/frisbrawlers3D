using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	public Transform Player;
	public int Size = 8;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Player != null) {
			//this.transform.position = new Vector3 (Player.position.x, Player.position.y, - 1);
		}
	}

	public void Reverse(){
		transform.Rotate (0, 0, 180);
	}
}

using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {
	float speed;

	void Start () {
		speed = 10.0f;
	}
	void Update () {
		if (Input.GetMouseButton (0)) { //left click
			transform.position += new Vector3 (Input.GetAxisRaw ("Mouse X") * Time.deltaTime * speed,
				Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * speed, 0.0f);
		}
		if (Input.GetMouseButton (1)) { //right click

			transform.position += new Vector3 (0.0f, 
				0.0f, Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * speed);
		}
	}
}

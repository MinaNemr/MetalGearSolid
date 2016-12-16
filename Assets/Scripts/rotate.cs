using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {
	float rotationsPerMinute;
	// Use this for initialization
	void Start () {
	 rotationsPerMinute = 10.0f;
	}
		
	// Update is called once per frame
	void Update () {
		if(transform.tag!="key")
		transform.Rotate(0.0f,6.0f*rotationsPerMinute*Time.deltaTime,0.0f);
		else
			transform.Rotate(0.0f,0.0f,6.0f*rotationsPerMinute*Time.deltaTime);
	}
}

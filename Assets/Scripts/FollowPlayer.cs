using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newCamPos = player.transform.position;
		newCamPos.z = transform.position.z;
		transform.position = newCamPos;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this simple script just ramdomizes the speed of an animation.

public class INF_AnimSpeedRandomizer : MonoBehaviour {


	public float speed=1;     // a speed of 1 and a rnd of 0.2 means acutal speed will be between 0.8 and 1.2
	public float speed_rnd=0;
	private Animator myAnimator;

	// Use this for initialization
	void Start () {
		myAnimator = gameObject.GetComponent<Animator> ();
		myAnimator.speed = Random.Range (speed-speed_rnd, speed+speed_rnd);
	}
	

}

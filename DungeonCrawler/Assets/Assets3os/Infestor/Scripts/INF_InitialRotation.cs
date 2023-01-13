using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//rotates the gameobject by some random amount, mostly used to randomize the orientation of generated objects

public class INF_InitialRotation : MonoBehaviour {


	public float X;   	//values start from this set value
	public float Xrnd;  // the RND value is the maximum deviation, so for example	
						// on X=90 and Xrnd=20 the actual rotation will be between 70 and 110

	public float Y;
	public float Yrnd;

	public float Z;
	public float Zrnd;

	public bool local;  //use world coordinates or the local one

	private float X_Actual;   //for nicer readibility these will be used
	private float Y_Actual;
	private float Z_Actual;


	// Use this for initialization
	void Start () {

	


		X_Actual=Random.Range(X-Xrnd, X+Xrnd);  //it is easier to read if you calculate these here
		Y_Actual=Random.Range(Y-Yrnd, Y+Yrnd);
		Z_Actual=Random.Range(Z-Zrnd, Z+Zrnd);


		if (local==false)
			transform.Rotate(X_Actual, Y_Actual, Z_Actual, Space.World);
		else	
			transform.Rotate(X_Actual, Y_Actual, Z_Actual);



	}
	

}

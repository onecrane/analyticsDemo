using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TurnABitJustOnce : MonoBehaviour {

	// Use this for initialization
	void Start () {

        transform.Rotate(Vector3.up, Random.Range(0f, 360f));
        DestroyImmediate(this);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestruct : MonoBehaviour {

    public float aliveTime;
	
	// Update is called once per frame
	void Update () {
        Destroy(this.gameObject, aliveTime);
	}
}

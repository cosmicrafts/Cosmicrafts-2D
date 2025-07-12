using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_FloatShips : MonoBehaviour {

    float deltamove = 0.5f;
    int dir = -1; 
	
	// Update is called once per frame
	void Update () {

        transform.Translate(new Vector3(0f, deltamove* dir, 0f) * Time.deltaTime);

        deltamove -= Time.deltaTime*1.75f;
        if (deltamove <= 0)
        {
            dir *= -1;
            deltamove = 1f;
        }

    }
}

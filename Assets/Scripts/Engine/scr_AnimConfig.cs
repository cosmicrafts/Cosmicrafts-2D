using UnityEngine;

public class scr_AnimConfig : MonoBehaviour {

    public float AnimSpeed = 1f;

	// Use this for initialization
	void Start () {
        GetComponent<Animator>().speed = AnimSpeed;

    }
	
}

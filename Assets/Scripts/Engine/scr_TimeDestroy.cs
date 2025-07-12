using UnityEngine;

public class scr_TimeDestroy : MonoBehaviour {

    public float TimeD = 0.1f;

	// Use this for initialization
	void Start () {
        if (TimeD>=0f)
        Destroy(gameObject, TimeD);
	}

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}

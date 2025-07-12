using UnityEngine;

public class src_fps : MonoBehaviour {

	// Use this for initialization
	void Start () {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
	}
	
	
}

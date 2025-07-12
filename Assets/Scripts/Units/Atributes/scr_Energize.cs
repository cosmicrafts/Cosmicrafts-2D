using UnityEngine;

public class scr_Energize : MonoBehaviour {

    public scr_BaseStats MyBS;

    // Update is called once per frame
    void Update () {
         scr_MNGame.GM.AddResources(MyBS.NS.Energize * Time.deltaTime);
	}
}

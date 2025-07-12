using UnityEngine;

public class scr_Fxs : MonoBehaviour {

    public int i_type = 0;
    public int i_Class = 0;

    void Start()
    {
        if (scr_StatsPlayer.Op_SoundFxs && GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();

        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().SetInteger("Type", i_type);
            GetComponent<Animator>().SetInteger("Class", i_Class);
        }

        Destroy(gameObject, 3f);
    }

	public void DestroySelf()
    {
        Destroy(gameObject);
    }

}

using UnityEngine;

public class scr_SoundFxsControl : MonoBehaviour {

    public bool checkConstant = false;

    AudioSource MyAudio;

	// Use this for initialization
	void Start () {

        MyAudio = GetComponent<AudioSource>();

        if (!MyAudio.mute)
        {
            if (!scr_StatsPlayer.Op_SoundFxs)
                MyAudio.mute = true;
        }

    }

    void Update()
    {
        if (!checkConstant)
            return;

        if (scr_StatsPlayer.Op_SoundFxs)
        {
            MyAudio.mute = false;
        } else
        {
            MyAudio.mute = true;
        }
    }

}

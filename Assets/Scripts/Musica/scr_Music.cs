using UnityEngine;

public class scr_Music : MonoBehaviour {

    public static bool AudioBegin = false;
    public static AudioSource as_audio;

    void Awake()
    {
        if (as_audio!=null)
        {
            Destroy(gameObject);
            return;
        }
        as_audio = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

}

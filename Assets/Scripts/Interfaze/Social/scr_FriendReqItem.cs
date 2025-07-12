using UnityEngine;

public class scr_FriendReqItem : MonoBehaviour {

    scr_FriendRequest UIP;
    [HideInInspector]
    public string Friend;

	// Use this for initialization
	void Start () {
        UIP = FindObjectOfType<scr_FriendRequest>();
    }

    public void AF()
    {
        if (scr_BDUpdate.IsCHDataF)
            return;

        UIP.AcceptFriend(Friend);
        Destroy(gameObject);
    }

    public void RF()
    {
        UIP.AcceptFriend(Friend);
        Destroy(gameObject);
    }
}

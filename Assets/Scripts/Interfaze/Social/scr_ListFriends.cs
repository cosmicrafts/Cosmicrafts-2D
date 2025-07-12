using System.Collections;
using UnityEngine;

public class scr_ListFriends : Photon.MonoBehaviour {

    public GameObject Container;
    public GameObject IconPlayer;
    public ChatNewGui Chat;
    public Sprite[] sp_avatars = new Sprite[7];

    public bool IsLoadingDataFriends = false;

    WaitForFixedUpdate DelayUpdate = new WaitForFixedUpdate();

    // Use this for initialization
    void Start()
    {
        StartCoroutine(LoadFriends());
    }

    // Use this for initialization
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator LoadFriends()
    {
        IsLoadingDataFriends = true;
        while (scr_StatsPlayer.Friends.Count > scr_StatsPlayer.FriendsData.Count)
        {
            if (!scr_BDUpdate.IsCHDataF)
                scr_BDUpdate.f_GetInfoUser(scr_StatsPlayer.Friends[scr_StatsPlayer.FriendsData.Count], true);
            yield return DelayUpdate;
        }

        int childs = Container.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            Destroy(Container.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < scr_StatsPlayer.Friends.Count; i++)
        {
            if (scr_StatsPlayer.Friends[i] == "")
                continue;
            scr_UIFriend f = Instantiate(IconPlayer, Container.transform).GetComponent<scr_UIFriend>();
            f.NameFriend.text = scr_StatsPlayer.Friends[i];
            f.Avatar.sprite = sp_avatars[scr_StatsPlayer.FriendsData[i].IDAvatar];
            f.Level.text = scr_StatsPlayer.FriendsData[i].Level.ToString();

            Chat.friendListItemLUT[scr_StatsPlayer.Friends[i]] = f;
            if (!ChatNewGui.TextFriends.ContainsKey(scr_StatsPlayer.Friends[i]))
                ChatNewGui.TextFriends[scr_StatsPlayer.Friends[i]] = "";
        }
        IsLoadingDataFriends = false;
    }

    public void DeleteFriend(string name)
    {
        if (PhotonNetwork.offlineMode)
            return;

        if (ChatNewGui.TextFriends.ContainsKey(name))
        {
            ChatNewGui.TextFriends.Remove(name);
        }
        int idfriend = scr_StatsPlayer.Friends.IndexOf(name);
        scr_StatsPlayer.Friends.Remove(name);
        scr_StatsPlayer.FriendsData.RemoveAt(idfriend);
        scr_StatsPlayer.IndexFriend--;
        scr_BDUpdate.f_UpdatePlayerFriends();
        Destroy(Container.transform.GetChild(idfriend).gameObject);
    }

    public void AddNewFriend(string friend)
    {
        StartCoroutine(IEAddNewFriend(friend));
    }

    IEnumerator IEAddNewFriend(string _friend)
    {
        IsLoadingDataFriends = true;
        yield return new WaitUntil(() => (!scr_BDUpdate.IsCHDataF));
        scr_BDUser _nfd = null;
        for (int i= scr_StatsPlayer.FriendsData.Count-1; i>=0; i--)
        {
            if (scr_StatsPlayer.FriendsData[i].Name == _friend)
            {
                _nfd = scr_StatsPlayer.FriendsData[i];
                break;
            }
        }
        if (_nfd != null)
        {
            scr_UIFriend f = Instantiate(IconPlayer, Container.transform).GetComponent<scr_UIFriend>();
            f.NameFriend.text = _friend;
            f.Avatar.sprite = sp_avatars[_nfd.IDAvatar];
            f.Level.text = _nfd.Level.ToString();
            Chat.friendListItemLUT[_friend] = f;
            if (!ChatNewGui.TextFriends.ContainsKey(_friend))
                ChatNewGui.TextFriends[_friend] = "";
        }
        IsLoadingDataFriends = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_FriendRequest : Photon.MonoBehaviour {

    public GameObject ItemFriend;
    public GameObject Content;
    public GameObject Empty;
    public ChatNewGui Chat;
    public GameObject IconNewRequest;
    public Text NumberRequest;

    public GameObject Panel;
    public scr_ListFriends ListFriends;
    List<string> NewForAdd = new List<string>();

    WaitForSeconds Delay2s = new WaitForSeconds(2.5f);

    private void OnEnable()
    {
        StartCoroutine(CheckFriendRequest());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void LoadRequests()
    {
        if (!Panel.activeSelf)
            return;

        int childs = Content.transform.childCount;

        for (int i = childs - 1; i >= 0; i--)
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }

        if (scr_StatsPlayer.NewFriends.Count > 0)
        {
            Empty.SetActive(false);
            for (int i = 0; i < scr_StatsPlayer.NewFriends.Count; i++)
            {
                GameObject item = Instantiate(ItemFriend, Content.transform);
                item.transform.GetChild(0).GetComponent<Text>().text = scr_StatsPlayer.NewFriends[i];
                item.GetComponent<scr_FriendReqItem>().Friend = scr_StatsPlayer.NewFriends[i];
            }
        }
        else
        {
            Empty.SetActive(true);
        }


    }

    IEnumerator CheckFriendRequest()
    {
        while (true)
        {
            yield return Delay2s;
            
            // Skip all friend request processing in dev mode
            if (PhotonNetwork.offlineMode || Scr_Database.DEV_BYPASS)
                continue;

            if (scr_StatsPlayer.CurrentState != 5 && scr_StatsPlayer.CurrentState != 6)
            {
                Chat.ChangeStatePlayer(scr_StatsPlayer.LastState);
            }

            if (scr_BDUpdate.NewRequest.Length>0)
            {
                string[] str_friends = scr_BDUpdate.NewRequest.Split('?');
                Debug.Log(str_friends.Length + "/" + scr_StatsPlayer.NewFriends.Count);
                if (str_friends.Length > scr_StatsPlayer.NewFriends.Count)
                {
                    scr_StatsPlayer.NewFriends.Clear();
                    for (int i = 0; i < str_friends.Length; i++)
                    {
                        if (!scr_StatsPlayer.NewFriends.Contains(str_friends[i]) && !scr_StatsPlayer.Friends.Contains(str_friends[i]))
                            scr_StatsPlayer.NewFriends.Add(str_friends[i]);
                    }
                    LoadRequests();
                }
            }
            //Update Icon
            if (scr_StatsPlayer.NewFriends.Count > 0)
            {
                IconNewRequest.SetActive(true);
                NumberRequest.text = scr_StatsPlayer.NewFriends.Count.ToString();
            }
            else
                IconNewRequest.SetActive(false);
            //New Friends Acepted
            if (Panel.activeSelf)
            {
                string[] str_all_friends = scr_BDUpdate.NewAcepted.Split('?');

                if (str_all_friends.Length > (scr_StatsPlayer.Friends.Count + NewForAdd.Count))
                {
                    for (int i = scr_StatsPlayer.Friends.Count; i < str_all_friends.Length; i++)
                    {
                        if (!NewForAdd.Contains(str_all_friends[i]) && !scr_StatsPlayer.Friends.Contains(str_all_friends[i]))
                        {
                            NewForAdd.Add(str_all_friends[i]);
                        }
                    }
                }

                if (NewForAdd.Count > 0)
                {
                    scr_StatsPlayer.Friends.Add(NewForAdd[0]);
                    scr_BDUpdate.f_GetInfoUser(NewForAdd[0], true);
                    ListFriends.AddNewFriend(NewForAdd[0]);
                    NewForAdd.RemoveAt(0);
                }
            }

            //Update DB
            scr_BDUpdate.f_GetDataFriends(scr_StatsPlayer.Name);
            yield return Delay2s;
                
        }
    }

    public void AcceptFriend(string friend)
    {
        if (PhotonNetwork.offlineMode || scr_BDUpdate.IsCHFriend)
            return;

        if (!scr_StatsPlayer.Friends.Contains(friend))
        {
            scr_BDUpdate.f_DeleteFriendR(scr_StatsPlayer.Name, friend, true);
            scr_BDUpdate.f_GetInfoUser(friend, true);
            if (ListFriends.gameObject.activeSelf)
                ListFriends.AddNewFriend(friend);
        }
            
    }

    public void RefuseFriend(string friend)
    {
        if (PhotonNetwork.offlineMode || scr_BDUpdate.IsCHFriend)
            return;

        scr_BDUpdate.f_DeleteFriendR(scr_StatsPlayer.Name, friend, false);
    }
}

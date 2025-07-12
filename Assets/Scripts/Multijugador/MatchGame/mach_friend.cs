using UnityEngine;
using UnityEngine.UI;

public class mach_friend : Photon.MonoBehaviour {

    public Text StatusMatch;

    bool IsJoin = false;

    public Button btn_Accept;

    public scr_UIPlayerEditor PE;
	
	// Update is called once per frame
	void Update () {
        if (PhotonNetwork.inRoom)
        {
            //cambiar para modo dev a 1
            //
            if (PhotonNetwork.room.PlayerCount >= 2)
            {
                StatusMatch.text = scr_Lang.GetText("txt_mn_info55");
                PhotonNetwork.room.IsOpen = false;
                PhotonNetwork.room.IsVisible = false;
                Debug.Log("Cargo Mapa_base_");
                PhotonNetwork.LoadLevel("Mapa_Base_");
            }
        }
    }

    public bool FriendJoinMatch(string friend)
    {
        if (scr_StatsPlayer.Offline)
        {
            PE.ShowWarningNoConnection();
            return false;
        }

        return PhotonNetwork.JoinRoom(friend);
    }

    public void CreateMatch()
    {
        if (scr_StatsPlayer.Offline)
        {
            PE.ShowWarningNoConnection();
            return;
        }

        if (PhotonNetwork.inRoom)
            return;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(scr_StatsPlayer.Name, roomOptions, TypedLobby.Default);
    }

    public void CancelMatch()
    {
        if (!PhotonNetwork.inRoom)
            return;

        PhotonNetwork.LeaveRoom();
        PE.WaithForFriend.SetActive(false);
    }

    public void JoinMatch()
    {
        if (PE.MRequests.Count>0) 
        {
            StatusMatch.text = scr_Lang.GetText("txt_mn_info55");
            btn_Accept.interactable = false;
            FriendJoinMatch(PE.MRequests[0]);
            PE.MRequests.RemoveAt(0);
        } else
        {
            PE.MatchsRequests.SetActive(false);
        }
    }

    public void RefuseMatch()
    {
        if (PE.MRequests.Count > 0)
        {
            ChatNewGui chat = FindObjectOfType<ChatNewGui>();
            if (chat != null)
            {
                chat.chatClient.SendPrivateMessage(PE.MRequests[0], "/RefInvitation");
            }
            PE.MRequests.RemoveAt(0);
        }
        if (IsJoin)
        {
            CancelMatch();
        } else
        {
            PE.MatchsRequests.SetActive(false);
        }
    }
}

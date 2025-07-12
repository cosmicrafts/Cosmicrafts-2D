using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_UIProfile : Photon.MonoBehaviour {

    public Text Pr_Lvl;
    public Text Pr_Name;
    public Text TotalWins;
    public Text League;
    public Text IA;
    public Text XpBuff;
    public Text Clan;
    public Text Title;
    public Text RP;
    public Text Range;
    public Image AvatarPr;
    public Text Xp;
    public Text StatusFriend;
    public Image StatusColor;
    public Image AvatarRef;
    public GameObject favorite_deck;
    public GameObject Loading;
    public GameObject WaithForFriend;

    public Slider XPBar;

    public scr_BDUser User = null;

    public Button btn_Addfriend;
    public Button btn_Deletefriend;
    public Button btn_Emotes;
    public Button btn_MatchHistory;
    public Button btn_InviteMatch;
    public Button btn_Titles;
    public Button btn_Achiv;

    public Dropdown dw_SetStatus;
    public GameObject SetStatus;

    public mach_friend mf;
    [HideInInspector]
    public scr_ListFriends lf;

    public scr_UIPlayerEditor PE;

    WaitForSeconds Delay1s = new WaitForSeconds(1f);

    bool FromChat = false;

    int IdFriendStatus = 2;

    public string[] StatusIds = new string[7] { "txt_mn_info7_player", "txt_mn_info7_player", "txt_mn_info6_player", "txt_mn_info8_player", "txt_mn_info9_player", "txt_mn_info10_player", "txt_mn_info11_player" };

    

    private void Start()
    {
        lf = FindObjectOfType<scr_ListFriends>();

        if (lf != null)
        {
            if (lf.gameObject.activeSelf)
            {
                lf.gameObject.SetActive(false);
                FromChat = true;
            }
        }

        List<string> _status = new List<string>();
        _status.Add(scr_Lang.GetText("txt_mn_info6_player"));
        _status.Add(scr_Lang.GetText("txt_mn_info8_player"));
        _status.Add(scr_Lang.GetText("txt_mn_info9_player"));
        _status.Add(scr_Lang.GetText("txt_mn_info7_player"));
        dw_SetStatus.ClearOptions();
        dw_SetStatus.AddOptions(_status);

        switch (scr_StatsPlayer.CurrentState)
        {
            case 1:
                {
                    dw_SetStatus.value = 3;
                }break;
            case 2:
                {
                    dw_SetStatus.value = 0;
                }
                break;
            case 3:
                {
                    dw_SetStatus.value = 1;
                }
                break;
            case 4:
                {
                    dw_SetStatus.value = 2;
                }
                break;
        }
    }

    void Update()
    {
        if (PhotonNetwork.offlineMode)
            gameObject.SetActive(false);
    }

    IEnumerator SetFavoriteDeck()
    {
        yield return Delay1s;
        if (User.FavoriteDeck.Length>0)
        {
            string[] deck = User.FavoriteDeck.Split('-');
            favorite_deck.SetActive(true);
            Loading.SetActive(false);
            for (int i = 0; i < 8; i++)
            {
                favorite_deck.transform.GetChild(i).GetComponent<Image>().sprite = scr_StatsPlayer.GetIconUnit(deck[i]);
            }
            btn_MatchHistory.gameObject.SetActive(true);
            StopCoroutine(SetFavoriteDeck());
        } else
        {
            StartCoroutine(SetFavoriteDeck());
        }
    }

    public void SetUser(string _name, int Status)
    {
        User = scr_StatsPlayer.FriendsData[scr_StatsPlayer.Friends.IndexOf(_name)];
        IdFriendStatus = Status;
    }

    public void UpdateProfileUIInfo()
    {
        if (User==null)
        {
            btn_MatchHistory.gameObject.SetActive(true);
            string[] deck = scr_StatsPlayer.FavoriteDeck.Split('-');
            favorite_deck.SetActive(true);
            Loading.SetActive(false);
            for (int i=0; i< deck.Length; i++)
            {
                favorite_deck.transform.GetChild(i).GetComponent<Image>().sprite = scr_StatsPlayer.GetIconUnit(deck[i]);
            }
            StatusColor.color = scr_StatsPlayer.GetColorStatusUser(scr_StatsPlayer.CurrentState);
            SetStatus.SetActive(true);
            btn_Addfriend.gameObject.SetActive(true);
            btn_Emotes.gameObject.SetActive(true);
            btn_Deletefriend.gameObject.SetActive(false);
            btn_InviteMatch.gameObject.SetActive(false);
            btn_Titles.gameObject.SetActive(true);
            btn_Achiv.gameObject.SetActive(true);
            StatusFriend.gameObject.SetActive(false);
            XPBar.maxValue = scr_StatsPlayer.GetXPforNextLevel(scr_StatsPlayer.Level);
            XPBar.value = scr_StatsPlayer.Xp;
            Pr_Lvl.text = scr_Lang.GetText("txt_mn_info2_unit") + " " + scr_StatsPlayer.Level.ToString();
            Pr_Name.text = scr_StatsPlayer.Name;
            Clan.text = scr_StatsPlayer.Clan;
            Title.text = scr_Lang.GetTitleName(scr_StatsPlayer.IdCurrentTitle);
            Xp.text = scr_StatsPlayer.Xp.ToString() + " / " + (scr_StatsPlayer.GetXPforNextLevel(scr_StatsPlayer.Level)).ToString() + " " + scr_Lang.GetText("txt_mn_info2_player");
            AvatarPr.sprite = AvatarRef.sprite;
            TotalWins.text = scr_StatsPlayer.VLeague.ToString();
            League.text = (scr_StatsPlayer.VLeague + scr_StatsPlayer.LLeague + scr_StatsPlayer.DLeague).ToString();
            IA.text = (scr_StatsPlayer.VIa + scr_StatsPlayer.LIa + scr_StatsPlayer.DIa).ToString();
            RP.text = scr_StatsPlayer.RankPoints.ToString();
            if (scr_StatsPlayer.Range==0)
            {
                Range.text = scr_Lang.GetText("txt_league_" + scr_StatsPlayer.Range.ToString());
            } else
            {
                Range.text = scr_Lang.GetText("txt_league_" + scr_StatsPlayer.Range.ToString()) + " " + scr_StatsPlayer.LRange.ToString();
            }
            
            Range.color = PE.LeaguesColors[scr_StatsPlayer.Range];
            if (scr_StatsPlayer.XpBuff>1f)
            {
                System.DateTime _bex = scr_StatsPlayer.XpBuffExpire;
                XpBuff.color = Color.yellow;
                XpBuff.text = "%"+((int)(scr_StatsPlayer.XpBuff*100f)).ToString()+" ("+ _bex.Day.ToString()+ "/"+ _bex.Month.ToString()+"/"+ _bex.Year.ToString()+")";
            } else
            {
                XpBuff.color = Color.white;
                XpBuff.text = "%0";
            }
            
        } else
        {
            SetStatus.SetActive(false);
            btn_Addfriend.gameObject.SetActive(false);
            btn_Emotes.gameObject.SetActive(false);
            btn_Deletefriend.gameObject.SetActive(true);
            btn_InviteMatch.gameObject.SetActive(true);
            btn_Titles.gameObject.SetActive(false);
            StatusFriend.gameObject.SetActive(true);
            btn_Achiv.gameObject.SetActive(false);
            StatusColor.color = scr_StatsPlayer.GetColorStatusUser(IdFriendStatus);
            StatusFriend.text = scr_Lang.GetText(StatusIds[IdFriendStatus]); ;
            XPBar.maxValue = scr_StatsPlayer.GetXPforNextLevel(User.Level);
            XPBar.value = User.Xp;
            Pr_Lvl.text = scr_Lang.GetText("txt_mn_info2_unit") + " " + User.Level.ToString();
            Pr_Name.text = User.Name;
            Clan.text = User.Clan;
            Title.text = scr_Lang.GetTitleName(User.IdCurrentTitle);
            Xp.text = User.Xp.ToString() + " / " + (scr_StatsPlayer.GetXPforNextLevel(User.Level)).ToString() + " " + scr_Lang.GetText("txt_mn_info2_player");
            AvatarPr.sprite = transform.root.GetComponent<scr_UIPlayerEditor>().sp_avatars[User.IDAvatar];
            TotalWins.text = User.VLeague.ToString();
            League.text = (User.VLeague + User.LLeague + User.DLeague).ToString();
            IA.text = (User.VIa + User.LIa + User.DIa).ToString();
            RP.text = User.RankPoints.ToString();
            if (User.Range==0)
            {
                Range.text = scr_Lang.GetText("txt_league_" + User.Range.ToString());
            } else
            {
                Range.text = scr_Lang.GetText("txt_league_" + User.Range.ToString()) + " " + User.LRange.ToString();
            }
            Range.color = PE.LeaguesColors[User.Range];
            scr_BDUpdate.f_GetHistoryMatchs(User.idAccount, scr_StatsPlayer.FriendsData.IndexOf(User));
            StartCoroutine(SetFavoriteDeck());
        }

    }

    public void InvitePlay()
    {
        if (User!=null && !WaithForFriend.activeSelf)
        {
            ChatNewGui chat = FindObjectOfType<ChatNewGui>();
            if (chat!=null)
            {
                mf.CreateMatch();
                chat.chatClient.SendPrivateMessage(User.Name, "/InviteToPlay");
                WaithForFriend.SetActive(true);
                gameObject.SetActive(false);
                if (lf != null)
                    lf.gameObject.SetActive(false);
            }
        }
    }

    public void DeleteFriend()
    {
        if (User != null)
        {
            scr_ListFriends friends = FindObjectOfType<scr_ListFriends>();
            if (friends != null)
            {
                friends.DeleteFriend(User.Name);
            }
        }
    }

    public void ChangeStatus()
    {
        switch(dw_SetStatus.value)
        {
            case 0:
                {
                    scr_StatsPlayer.LastState = 2;
                    PE.ChangeStatePlayer(2);
                }
                break;
            case 1:
                {
                    scr_StatsPlayer.LastState = 3;
                    PE.ChangeStatePlayer(3);
                }
                break;
            case 2:
                {
                    scr_StatsPlayer.LastState = 4;
                    PE.ChangeStatePlayer(4);
                }
                break;
            case 3:
                {
                    scr_StatsPlayer.LastState = 1;
                    PE.ChangeStatePlayer(1);
                }
                break;
        }
        StatusColor.color = scr_StatsPlayer.GetColorStatusUser(scr_StatsPlayer.CurrentState);
    }

    void OnDisable()
    {
        if (FromChat && lf!=null)
        {
            FromChat = false;
            lf.gameObject.SetActive(true);
        }

        if (User != null)
        {
            User.HistoryMatchs.Clear();
            StopCoroutine("SetFavoriteDeck");
            User = null;
            favorite_deck.SetActive(false);
            Loading.SetActive(true);
            btn_MatchHistory.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (lf != null)
        {
            if (lf.gameObject.activeSelf)
            {
                lf.gameObject.SetActive(false);
                FromChat = true;
            }
        }
    }



}

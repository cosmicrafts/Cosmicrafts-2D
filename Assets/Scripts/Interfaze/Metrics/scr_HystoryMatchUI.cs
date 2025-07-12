using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class scr_HystoryMatchUI : MonoBehaviour {

    public Transform Content;
    public GameObject BoxUI;
    public GameObject Empty;

    int index = 0;
    scr_BDUser User = null;

    WaitForFixedUpdate DelayUpdate = new WaitForFixedUpdate();

    public void SetUser(Text txtname)
    {
        if (txtname.text == scr_StatsPlayer.Name)
            User = null;
        else
            User = scr_StatsPlayer.FriendsData[scr_StatsPlayer.Friends.IndexOf(txtname.text)];
    }

    void OnEnable()
    {
        index = 0;

        int childs = Content.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }

        if (User!=null)
        {
            if (User.HistoryMatchs.Count>0)
            {
                StartCoroutine(LoopHistoryUser());
                Empty.SetActive(false);
            }
        } else if (scr_StatsPlayer.HistoryMatchs.Count>0)
        {
            StartCoroutine(LoopHistory());
            Empty.SetActive(false);
        } 
    }

    IEnumerator LoopHistory()
    {
        yield return DelayUpdate;
        int i = index;
        
        scr_MatchUI match = Instantiate(BoxUI, Content).GetComponent<scr_MatchUI>();
        match.Player1.text = scr_StatsPlayer.HistoryMatchs[i].Name1;
        match.Player2.text = scr_StatsPlayer.HistoryMatchs[i].Name2;
        match.Score1.text = scr_StatsPlayer.HistoryMatchs[i].Xp1.ToString() + " " + scr_Lang.GetText("txt_mn_info2_player");
        match.Score2.text = scr_StatsPlayer.HistoryMatchs[i].Xp2.ToString() + " " + scr_Lang.GetText("txt_mn_info2_player");
        match.Winer.text = scr_Lang.GetText("txt_game_info13");
        match.Date.text = scr_StatsPlayer.HistoryMatchs[i].Date;
        if (scr_StatsPlayer.HistoryMatchs[i].WinerName == scr_StatsPlayer.Name)
        {
            match.Winer.text = scr_Lang.GetText("txt_game_info14");
            match.Winer.color = Color.green;
        }

        string[] deck1 = scr_StatsPlayer.HistoryMatchs[i].Deck1.Split('-');
        string[] deck2 = scr_StatsPlayer.HistoryMatchs[i].Deck2.Split('-');
        for (int j = 0; j < 8; j++)
        {
            match.Deck1.transform.GetChild(j).GetComponent<Image>().sprite = scr_StatsPlayer.GetIconUnit(deck1[j]);
            match.Deck2.transform.GetChild(j).GetComponent<Image>().sprite = scr_StatsPlayer.GetIconUnit(deck2[j]);
        }

        index++;

        if (index< scr_StatsPlayer.HistoryMatchs.Count)
            StartCoroutine(LoopHistory());
    }

    IEnumerator LoopHistoryUser()
    {
        yield return DelayUpdate;
        int i = index;

        scr_MatchUI match = Instantiate(BoxUI, Content).GetComponent<scr_MatchUI>();
        match.Player1.text = User.HistoryMatchs[i].Name1;
        match.Player2.text = User.HistoryMatchs[i].Name2;
        match.Score1.text = User.HistoryMatchs[i].Xp1.ToString() + " " + scr_Lang.GetText("txt_mn_info2_player");
        match.Score2.text = User.HistoryMatchs[i].Xp2.ToString() + " " + scr_Lang.GetText("txt_mn_info2_player");
        match.Winer.text = scr_Lang.GetText("txt_game_info13");
        match.Date.text = User.HistoryMatchs[i].Date;
        if (User.HistoryMatchs[i].WinerName == User.Name)
        {
            match.Winer.text = scr_Lang.GetText("txt_game_info14");
            match.Winer.color = Color.green;
        }

        string[] deck1 = User.HistoryMatchs[i].Deck1.Split('-');
        string[] deck2 = User.HistoryMatchs[i].Deck2.Split('-');
        for (int j = 0; j < 8; j++)
        {
            match.Deck1.transform.GetChild(j).GetComponent<Image>().sprite = scr_StatsPlayer.GetIconUnit(deck1[j]);
            match.Deck2.transform.GetChild(j).GetComponent<Image>().sprite = scr_StatsPlayer.GetIconUnit(deck2[j]);
        }

        index++;

        if (index < scr_StatsPlayer.HistoryMatchs.Count)
            StartCoroutine(LoopHistoryUser());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

}

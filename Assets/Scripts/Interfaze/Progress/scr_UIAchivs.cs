using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_UIAchivs : MonoBehaviour {

    public scr_UIAchivItem BaseAchivItem;
    public scr_UIAchivItem LastItemSelected;

    public Text Description;
    public Text ProgressLevels;
    public Text ProgressLevel;
    public Text Rewards;

    public Image BarLevels;
    public Image BarProgress;

    public Transform ContentItems;

    public GameObject AchivComplete;

    bool items_init = false;

    public scr_UIPlayerEditor PE;

    List<scr_UIAchivItem> ListUIAchivs = new List<scr_UIAchivItem>();

	// Use this for initialization
	void Start () {
        InitAchivs();
    }
	
    void InitAchivs()
    {
        if (items_init)
            return;

        for (int i = 0; i < scr_StatsPlayer.MyAchiv.Count; i++)
        {
            bool sel = false;
            bool completed = false;
            if (scr_StatsPlayer.MyAchiv[i].Level >= scr_StatsPlayer.MyAchiv[i].MaxLevel)
                completed = true;

            if (i == 0)
            {
                sel = true;
                Description.text = scr_Lang.GetTitleDescription(i);
                ProgressLevels.text = scr_StatsPlayer.MyAchiv[i].Level.ToString() + "/" + scr_StatsPlayer.MyAchiv[i].MaxLevel.ToString();
                BarLevels.fillAmount = scr_StatsPlayer.MyAchiv[i].Level / scr_StatsPlayer.MyAchiv[i].MaxLevel;
                int lvl = scr_StatsPlayer.MyAchiv[i].Level;
                if (lvl >= scr_StatsPlayer.MyAchiv[i].Levels.Length)
                    lvl = scr_StatsPlayer.MyAchiv[i].Levels.Length - 1;
                ProgressLevel.text = scr_StatsPlayer.MyAchiv[i].Progress.ToString() + "/" + scr_StatsPlayer.MyAchiv[i].Levels[lvl];
                BarProgress.fillAmount = scr_StatsPlayer.MyAchiv[i].Progress / scr_StatsPlayer.MyAchiv[i].Levels[lvl];
                AchivComplete.SetActive(true);
            }

            scr_UIAchivItem ai = Instantiate(BaseAchivItem.gameObject, ContentItems).GetComponent<scr_UIAchivItem>();
            ai.InitItem(i, scr_Lang.GetTitleName(i), sel, completed);
            ListUIAchivs.Add(ai);
            if (sel)
                LastItemSelected = ai;
        }
        items_init = true;
        Rewards.text = scr_Lang.GetText("txt_mn_info95"); ;
        Destroy(BaseAchivItem.gameObject);
    }

    public void ShowAchivInfo(scr_UIAchivItem _item)
    {
        AchivComplete.SetActive(false);
        LastItemSelected.DeSelectItem();
        LastItemSelected = _item;
        _item.SelectItem();

        int _id = _item.ID;
        Description.text = scr_Lang.GetTitleDescription(_id);
        ProgressLevels.text = scr_StatsPlayer.MyAchiv[_id].Level.ToString() + "/" + scr_StatsPlayer.MyAchiv[_id].MaxLevel.ToString();
        BarLevels.fillAmount = (float)scr_StatsPlayer.MyAchiv[_id].Level / (float)scr_StatsPlayer.MyAchiv[_id].MaxLevel;
        int lvl = scr_StatsPlayer.MyAchiv[_id].Level;
        if (lvl >= scr_StatsPlayer.MyAchiv[_id].Levels.Length)
        {
            AchivComplete.SetActive(true);
            lvl = scr_StatsPlayer.MyAchiv[_id].Levels.Length - 1;
        }
        ProgressLevel.text = scr_StatsPlayer.MyAchiv[_id].Progress.ToString() + "/" + scr_StatsPlayer.MyAchiv[_id].Levels[lvl];
        BarProgress.fillAmount = (float)scr_StatsPlayer.MyAchiv[_id].Progress / (float)scr_StatsPlayer.MyAchiv[_id].Levels[lvl];
        Rewards.text = scr_StatsPlayer.MyAchiv[_id].GetStringOfCurrentRewards();
    }

    public void ShowAchivInfoFromNotif()
    {
        if (PE.CurrentAchivNotif == -1)
            return;

        if (!items_init)
            InitAchivs();

        scr_UIAchivItem achiv = ListUIAchivs.Find(a => a.ID == PE.CurrentAchivNotif);

        if (achiv == null)
            return;

        ShowAchivInfo(achiv);
    }

    private void OnEnable()
    {
        if (!items_init)
            return;

        for (int i = 0; i < scr_StatsPlayer.MyAchiv.Count; i++)
        {
            if (scr_StatsPlayer.MyAchiv[i].Level >= scr_StatsPlayer.MyAchiv[i].MaxLevel)
                ContentItems.GetChild(i).GetComponent<scr_UIAchivItem>().Completed.SetActive(true);
        }
    }
}

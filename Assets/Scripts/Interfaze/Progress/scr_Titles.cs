using UnityEngine;
using UnityEngine.UI;

public class scr_Titles : MonoBehaviour {

    public Sprite S_Lock;
    public Sprite S_Free;
    public Sprite S_Selected;

    public Transform Content;

    public scr_TitleItem TitleItem;

    public Text TitleProfile;

    public Text TitleName;
    public Text TitleInfo;
    public Text Progress;
    public Image BarProgress;
    public GameObject TitleGOInfo;

    bool OkDef = false;

    // Use this for initialization
    void Start () {
		for (int i=0; i<scr_StatsPlayer.MyTitles.Length; i++)
        {
            scr_TitleItem ti = Instantiate(TitleItem.gameObject, Content).GetComponent<scr_TitleItem>();
            ti.Name.text = scr_Lang.GetTitleName(i);
            ti.id = i;
            if (i==scr_StatsPlayer.IdCurrentTitle)
            {
                ti.Status.sprite = S_Selected;
            } else if (scr_StatsPlayer.MyTitles[i])
            {
                ti.Status.sprite = S_Free;
            } else
            {
                ti.Status.sprite = S_Lock;
            }
        }
        Destroy(TitleItem.gameObject);
        OkDef = true;
    }

    private void OnEnable()
    {
        if (!OkDef)
            return;

        for (int i = 0; i < Content.childCount; i++)
        {
            scr_TitleItem ti = Content.GetChild(i).GetComponent<scr_TitleItem>();
            if (i == scr_StatsPlayer.IdCurrentTitle)
            {
                ti.Status.sprite = S_Selected;
            }
            else if (scr_StatsPlayer.MyTitles[i])
            {
                ti.Status.sprite = S_Free;
            }
            else
            {
                ti.Status.sprite = S_Lock;
            }
        }
    }

    public void SelectTitle(scr_TitleItem title)
    {
        if (scr_StatsPlayer.MyTitles[title.id] && title.id != scr_StatsPlayer.IdCurrentTitle)
        {
            Content.GetChild(scr_StatsPlayer.IdCurrentTitle).GetComponent<scr_TitleItem>().Status.sprite = S_Free;
            title.Status.sprite = S_Selected;
            scr_StatsPlayer.IdCurrentTitle = title.id;
            TitleProfile.text = title.Name.text;
            scr_BDUpdate.f_SetTitle(scr_StatsPlayer.id, scr_StatsPlayer.IdCurrentTitle);
        }
    }

    public void ShowInfoTitle(scr_TitleItem title)
    {
        TitleGOInfo.SetActive(true);
        TitleName.text = title.Name.text;
        TitleInfo.text = scr_Lang.GetTitleDescription(title.id);
        scr_Achievements ach = scr_StatsPlayer.MyAchiv[title.id];
        Progress.text = ach.Progress.ToString() + " / " + ach.Levels[ach.Levels.Length - 1].ToString();
        BarProgress.fillAmount = ach.Progress / ach.Levels[ach.Levels.Length - 1];
    }

}

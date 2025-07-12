using UnityEngine;
using UnityEngine.UI;

public class scr_UIMissions : MonoBehaviour
{
    public Text[] Text_Progress = new Text[10];
    public Image[] Bar_Progress = new Image[10];
    public GameObject[] Complete = new GameObject[10];

    public GameObject Daily_Claim;
    public GameObject Weekly_Claim;
    public GameObject Daily_Complete;
    public GameObject Weekly_Complete;

    public scr_UIPlayerEditor PE;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < scr_Missions.MD_Progress.Length; i++)
        {
            Complete[i].SetActive(scr_Missions.MD_Progress[i] >= scr_Missions.MD_Goal[i]);
            Text_Progress[i].text = scr_Missions.MD_Progress[i].ToString() + "/" + scr_Missions.MD_Goal[i].ToString();
            Bar_Progress[i].fillAmount = (float)scr_Missions.MD_Progress[i] / (float)scr_Missions.MD_Goal[i];
        }

        for (int i = 0; i < scr_Missions.MW_Progress.Length; i++)
        {
            Complete[5 + i].SetActive(scr_Missions.MW_Progress[i] >= scr_Missions.MW_Goal[i]);
            Text_Progress[5 + i].text = scr_Missions.MW_Progress[i].ToString() + "/" + scr_Missions.MW_Goal[i].ToString();
            Bar_Progress[5 + i].fillAmount = (float)scr_Missions.MW_Progress[i] / (float)scr_Missions.MW_Goal[i];
        }

        scr_Missions.CheckProgressComplete();

        //Missions
        if (scr_Missions.Claim_Day)
        {
            Daily_Complete.SetActive(true);
        } else if (scr_Missions.Day_Complete)
        {
            //Success Day
            Daily_Claim.SetActive(true);
        }
        if (scr_Missions.Claim_Week)
        {
            Weekly_Complete.SetActive(true);
        } else if (scr_Missions.Week_Complete)
        {
            //Success Week
            Weekly_Claim.SetActive(true);
        }
    }

    public void ClaimDay()
    {
        scr_Missions.Claim_Day = true;
        scr_StatsPlayer.Orbes[0]++;
        scr_BDUpdate.f_SetOrbes(scr_StatsPlayer.id);
        PE.UpdateCoins();
        Daily_Complete.SetActive(true);
        Daily_Claim.SetActive(false);
    }

    public void ClaimWeek()
    {
        scr_Missions.Claim_Week = true;
        scr_StatsPlayer.Orbes[1]++;
        scr_BDUpdate.f_SetOrbes(scr_StatsPlayer.id);
        PE.UpdateCoins();
        Weekly_Complete.SetActive(true);
        Weekly_Claim.SetActive(false);
    }

}

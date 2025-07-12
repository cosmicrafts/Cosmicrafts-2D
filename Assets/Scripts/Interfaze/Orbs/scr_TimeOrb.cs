using UnityEngine;
using UnityEngine.UI;

public class scr_TimeOrb : MonoBehaviour {

    [HideInInspector]
    public scr_DataOrb MyData;

    public GameObject[] Icons = new GameObject[6];
    public Text Time;
    public bool ResetAtOpen = false;

    [HideInInspector]
    public int CostToOpen = 0;

    public System.TimeSpan TimeToOpen;
    System.TimeSpan Remains;

    [HideInInspector]
    public bool OkInit = false;

    [HideInInspector]
    public bool RedyToOpen = false;

    /*Update
    if (!OkInit || RedyToOpen)
        return;

    System.TimeSpan current_past = (System.DateTime.Now - MyData.Created);

    if (TimeToOpen > current_past)
    {
        Remains = TimeToOpen - current_past;

        Time.text = Remains.Hours.ToString() + ":" + Remains.Minutes.ToString() + ":" + Remains.Seconds.ToString();
    } else
    {
        RedyToOpen = true;
        Time.text = scr_Lang.GetText("txt_mn_info68");
    }
    */

    public int GetCostToOpen()
    {
        return ((int)TimeToOpen.TotalMinutes*CostToOpen)/(int)TimeToOpen.TotalMinutes;
    }

    public void InitOrbe(scr_DataOrb _data, System.TimeSpan _timetoopen)
    {
        MyData = _data;
        TimeToOpen = _timetoopen;

        switch (_data.Type)
        {
            case 0: //Silver
                { CostToOpen = 5; }
                break;
            case 1: //gold
                { CostToOpen = 10; }
                break;
            case 2: //platino
                { CostToOpen = 25; }
                break;
            case 3: //diamante
                { CostToOpen = 50; }
                break;
            case 4: //antimateria
                { CostToOpen = 100; }
                break;
            case 5: //Desconocido
                { CostToOpen = 250; }
                break;
            default: 
                { CostToOpen = 10; }
                break;
        }

        OkInit = true;
    }

    public void InitOrbe(scr_DataOrb _data)
    {
        int hours_to_open = 0;
        int minutes_to_open = 0;

        switch (_data.Type)
        {
            case 0: //Silver
                {minutes_to_open = 15;}
                break;
            case 1: //gold
                { minutes_to_open = 30;}
                break;
            case 2: //platino
                {hours_to_open = 1;}
                break;
            case 3: //diamante
                { hours_to_open = 2; }
                break;
            case 4: //antimateria
                { hours_to_open = 4; }
                break;
            case 5: //Desconocido
                { hours_to_open = 8; }
                break;
            default: //
                { hours_to_open = 4; }
                break;
        }

        if (!ResetAtOpen)
        {
            Icons[_data.Type].SetActive(true);
        }

        System.TimeSpan _TimeToOpen = new System.TimeSpan(hours_to_open, minutes_to_open, 0);
        InitOrbe(_data, _TimeToOpen);
    }

    public void RestartOrb()
    {
        /*
        RedyToOpen = false;
        MyData.Created = System.DateTime.Now;
        TimeToOpen = new System.TimeSpan(12 * (MyData.Type+1), 0, 0);
        scr_BDUpdate.f_SetTimeOrbs(scr_StatsPlayer.id, false);
        */
    }

    public void DestroyOrb()
    {
        /*
        MyData.Type = -1;
        scr_BDUpdate.f_SetTimeOrbs(scr_StatsPlayer.id, false);
        Destroy(gameObject);
        */
    }

}

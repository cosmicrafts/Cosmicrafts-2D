
using UnityEngine;

public class scr_Missions
{
    public static int[] MD_Progress;
    public static int[] MW_Progress;
    public static int[] MD_Goal = new int[5] { 100,50,35,25,3};
    public static int[] MW_Goal = new int[5] { 1000, 500, 350, 250, 30 };

    public static int DaysCounter = 0;

    public static bool Day_Complete = false;
    public static bool Week_Complete = false;

    public static bool Claim_Day = false;
    public static bool Claim_Week = false;

    public static bool New_Day = false;

    public static void InitMissions()
    {
        DaysCounter = 0;
        Day_Complete = false;
        Week_Complete = false;
        Claim_Day = false;
        Claim_Week = true;
        MD_Progress = new int[5] { 0, 0, 0, 0, 0 };
        MW_Progress = new int[5] { 0, 0, 0, 0, 0 };
    }

    public static void ResetDay()
    {
        Day_Complete = false;
        Claim_Day = false;
        MD_Progress = new int[5] { 0, 0, 0, 0, 0 };
    }

    public static void ResetWeek()
    {
        DaysCounter = 0;
        Week_Complete = false;
        Claim_Week = false;
        MW_Progress = new int[5] { 0, 0, 0, 0, 0 };
    }

    public static void AddProgress(int mission, int amount)
    {
        // Safety check: ensure arrays are initialized
        if (MD_Progress == null || MW_Progress == null)
        {
            Debug.LogWarning("Missions arrays not initialized, skipping progress update");
            return;
        }
        
        // Safety check: ensure mission index is valid
        if (mission < 0 || mission >= MD_Progress.Length || mission >= MW_Progress.Length)
        {
            Debug.LogWarning("Invalid mission index: " + mission + ", skipping progress update");
            return;
        }
        
        MD_Progress[mission] += amount;
        if (MD_Progress[mission] > MD_Goal[mission]) { MD_Progress[mission] = MD_Goal[mission]; }
        MW_Progress[mission] += amount;
        if (MW_Progress[mission] > MW_Goal[mission]) { MW_Progress[mission] = MW_Goal[mission]; }
    }

    public static void CheckProgressComplete()
    {
        // Safety check: ensure arrays are initialized
        if (MD_Progress == null || MW_Progress == null)
        {
            Debug.LogWarning("Missions arrays not initialized, skipping progress check");
            Day_Complete = false;
            Week_Complete = false;
            return;
        }
        
        Day_Complete = (MD_Progress[0]>= MD_Goal[0] && MD_Progress[1] >= MD_Goal[1] && MD_Progress[2] >= MD_Goal[2] && MD_Progress[3] >= MD_Goal[3] && MD_Progress[4] >= MD_Goal[4]);
        Week_Complete = (MW_Progress[0] >= MW_Goal[0] && MW_Progress[1] >= MW_Goal[1] && MW_Progress[2] >= MW_Goal[2] && MW_Progress[3] >= MW_Goal[3] && MW_Progress[4] >= MW_Goal[4]);
    }

    public static void CheckExpire()
    {
        System.DateTime NowTime = System.DateTime.Now;

        System.DateTime OldDate = new System.DateTime(scr_StatsPlayer.LastDateConection.Year, scr_StatsPlayer.LastDateConection.Month, scr_StatsPlayer.LastDateConection.Day);

        System.TimeSpan ts = NowTime - OldDate;

        if (ts.TotalDays >= 1 || NowTime.DayOfWeek != OldDate.DayOfWeek)
        {
            DaysCounter += (int)ts.TotalDays;
            ResetDay();
            New_Day = true;
        }

        if (DaysCounter >= 7)
            ResetWeek();

        scr_BDUpdate.f_SaveMissions(scr_StatsPlayer.id);
    }
}

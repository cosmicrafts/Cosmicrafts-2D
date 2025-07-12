using System.Collections;
using UnityEngine;

public class scr_GenUnit : MonoBehaviour {

    public float f_TimeSpawnUnit;

    public string s_UnitSpawn;

    [HideInInspector]
    public Vector2 PosSpawnOthers;

    public scr_Unit MyUS;

    WaitForSeconds DelaySpawn;

    Vector2[] MF;

    public bool is_Inhibitor = false;

    // Use this for initialization
    void Start() {

        if (MyUS.ImClone || scr_StatsPlayer.Tutorial)
            return;

        s_UnitSpawn = MyUS.NS.GenUnit;
        f_TimeSpawnUnit = MyUS.NS.TimeGen;

        PosSpawnOthers = transform.position;

        if (MyUS.i_Team == 0)
            PosSpawnOthers += new Vector2(1.5f, 0f);
        else
            PosSpawnOthers += new Vector2(-1.5f, 0f);

        if (f_TimeSpawnUnit>0)
        {
            DelaySpawn = new WaitForSeconds(f_TimeSpawnUnit);
            StartCoroutine(SpawnUnits());
        }

        is_Inhibitor = (MyUS.s_IdName == "Station_Inhibitor");

        if (is_Inhibitor)
        {
            MF = new Vector2[3];

            int dir = 1;
            if (MyUS.i_Team == 1)
                dir = -1;

            MF[0] = new Vector2(dir, 0f);
            MF[1] = new Vector2(0f, 0.5f);
            MF[2] = new Vector2(0f, -0.5f);
        }
    }

    IEnumerator SpawnUnits()
    {
        while(!scr_MNGame.GM.b_EndGame)
        {
            yield return DelaySpawn;

            if (is_Inhibitor)
            {
                GenerateMinions();
            }
            else
            {
                if (CompareTag("Station"))
                    MyUS.MyAnimator.speed = 1f;


                if (scr_MNGame.GM.b_InNetwork)
                {
                    if (s_UnitSpawn.Contains("Squad"))
                    {
                        scr_MNGame.GM.CreateSquad(s_UnitSpawn, PosSpawnOthers.x, PosSpawnOthers.y, MyUS.i_Team, MyUS.MySkin);
                    }
                    else
                    {
                        scr_MNGame.GM.CreateShip(s_UnitSpawn, PosSpawnOthers.x, PosSpawnOthers.y, MyUS.i_Team, -1f, 1f, MyUS.MySkin);
                    }

                }
                else
                {
                    scr_MNGame.GM.CreateUnitOff(s_UnitSpawn, PosSpawnOthers, MyUS.i_Team, MyUS.MySkin);
                }

                if (MyUS.IsMyTeam(scr_MNGame.GM.TeamInGame))
                {
                    scr_MNGame.BattleMetrics.Gen_Units++;
                }
            }
        }
    }

    public void GenerateMinions()
    {
        if (!scr_MNGame.GM.InivitorsActive)
            return;

        if (scr_MNGame.GM.b_InNetwork)
        {
            if (!MyUS.ImClone)
            {
                for (int i = 0; i < 3; i++)
                {
                    scr_MNGame.GM.CreateShip(s_UnitSpawn, PosSpawnOthers.x + MF[i].y, PosSpawnOthers.y + MF[i].y, MyUS.i_Team, -1, -1, MyUS.MySkin);
                }
            }
        }
        else
        {
            GenOfflineMinions();
        }
    }

    public void GenOfflineMinions()
    {
        for (int i = 0; i < 3; i++)
        {
            scr_MNGame.GM.CreateUnitOff(s_UnitSpawn, PosSpawnOthers + MF[i], MyUS.i_Team, MyUS.MySkin);
        }
    }
}

using UnityEngine;

public class scr_BaseStats : Photon.MonoBehaviour {

    public string s_IdName = "";

    [HideInInspector]
    public string s_MyMame = "";

    public int i_Team = 0;
    public float f_Duration = 0f;
    public float f_CastDelay = 1f;
    public int i_poblation = 0;

    public bool StatsFromInspec = false;

    [HideInInspector]
    public bool UnitRedy = false;

    [HideInInspector]
    public float f_MaxDuration = 0f;

    [HideInInspector]
    public int Level = 1;

    [HideInInspector]
    public int i_IdP = -1; //Id unit in Units list

    [HideInInspector]
    public bool OkStatsLoad = false;

    [HideInInspector]
    public bool IsEnable = true;

    [HideInInspector]
    public bool ImClone = false;

    [HideInInspector]
    public float MySkin = -1;

    public Transform Engine;

    public SpriteRenderer MySprite;

    public Animator MyAnimator;

    public scr_UiUnit MyUI;

    public scr_StatsUnit NS; //Numeric Stats

    public scr_Energize PS_Energize;
    public scr_SpawnArea PS_SpawnArea;
    public scr_GenUnit PS_GenUnits;

    protected void InitStatsData () {

        if (s_IdName != "" && scr_MNGame.GM.b_InNetwork && photonView.instantiationData != null)
        {
            i_Team = (int)photonView.instantiationData[0];
            s_IdName = photonView.instantiationData[1].ToString();
            MySkin = (float)photonView.instantiationData[2];
        }

        if (s_IdName.Contains("Station"))
            gameObject.tag = "Station";

        if (scr_MNGame.GM.b_InNetwork)
        {
            if (!photonView.isMine)
                ImClone = true;
        }

        NS = scr_MNGame.GM.GetStats(s_IdName);

        CheckPasives();

        if (!StatsFromInspec)
        {
            f_Duration = NS.Duration;
            f_MaxDuration = f_Duration;
            f_CastDelay = 1f / NS.CastDelay;
            i_poblation = NS.PoblationReq;
            MySkin = NS.Skin;
        } 
    }

    public void UpgradeStatsLevel()
    {
        i_IdP = scr_StatsPlayer.FindIdUnit(s_IdName);
        if (i_IdP != -1)
        {
            Level = scr_StatsPlayer.MyUnits[i_IdP].Level;
        }
    }

    protected void UpdateStats() {

        if (!UnitRedy)
            return;

        if (f_Duration > 0f)
        {
            f_Duration -= Time.deltaTime;
            if (MyUI)
                MyUI.SetDurationBar(f_Duration/f_MaxDuration);
            if (f_Duration <= 0 && !ImClone)
            {
                IsEnable = false;
                SendMessage("DestroyUnit");
                return;
            }
        }
    }

    public bool IsMyTeam(int team)
    {
        return team == i_Team;
    }

    public void SetTeam(int _team)
    {
        i_Team = _team;
        if (MyUI)
        {
            MyUI.SetColorTeam();
        }
        if (scr_MNGame.GM.b_InNetwork && !ImClone)
        {
            photonView.RPC("SetTeamRPC", PhotonTargets.OthersBuffered, _team);
        }
    }

    [PunRPC]
    public void SetTeamRPC(int _team)
    {
        i_Team = _team;
        if (MyUI)
        {
            MyUI.SetColorTeam();
        }
    }

    public void CheckPasives()
    {
        if (PS_GenUnits && NS.GenUnit != "none")
            PS_GenUnits.enabled = true;

        bool Enemy = !IsMyTeam(scr_MNGame.GM.TeamInGame);

        if (PS_SpawnArea)
        {
            if (NS.SpawnRange <= 0f || Enemy)
                Destroy(PS_SpawnArea.gameObject);
            else
                PS_SpawnArea.enabled = true;
        }

        if (Enemy)
            return;

        if (PS_Energize && NS.Energize > 0f)
            PS_Energize.enabled = true;

        scr_MNGame.GM.f_MaxResources += NS.Banck;
    }

}

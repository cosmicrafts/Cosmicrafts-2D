
public class scr_Achievements {

    //Data
    public int Id = 0;
    public int IdTitle = 0;
    public string Name = "";
    public string Description = "";
    public int Level;
    public int MaxLevel;
    public int[] Levels;
    public int Progress;
    public int ProgressBattle;

    //Rewards
    int[] R_Titles;
    int[] R_Neutrinos;
    int[,] R_Orbs;
    string[] R_Skins;

    //traducciones
    string[] Orbs_type = new string[6] { "txt_mn_info96", "txt_mn_info97", "txt_mn_info98", "txt_mn_info99", "txt_mn_info100", "txt_mn_info101" };

    public void InitAchiv(int id, int idtitle, string name, string description)
    {
        Id = id;
        IdTitle = idtitle;
        Name = name;
        Description = description;
        Level = 0;
    }

    public void SetLevels(int[] levels, int current_progress)
    {
        MaxLevel = levels.Length;
        Levels = new int[MaxLevel];
        Progress = current_progress;
        ProgressBattle = 0;
        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i] = levels[i];
            if (Progress >= Levels[i])
                Level++;
        }
    }

    public void AddProgressBattle(int _progress)
    {
        ProgressBattle += _progress; 
    }

    public int MoveProgressBattle()
    {
        if (Level == MaxLevel || ProgressBattle<=0)
            return -1;

        int newlevels = 0;
        Progress += ProgressBattle;
        for (int i = Level; i < Levels.Length; i++)
        {
            if (Progress >= Levels[i])
            {
                CheckRewardsAtLevel(i);
                newlevels++;
            }
        }
        Level += newlevels;

        ProgressBattle = 0;

        return newlevels;
    }

    public void CheckRewardsAtLevel(int level)
    {
        //Check Titles
        if (R_Titles != null)
        {
            if (level < R_Titles.Length)
            {
                if (R_Titles[level]!=-1)
                {
                    scr_StatsPlayer.MyTitles[R_Titles[level]] = true;
                    scr_BDUpdate.f_UpdateMyTitles(scr_StatsPlayer.id);
                }
            }
        }

        //Check Neutrinos
        if (R_Neutrinos != null)
        {
            if (level < R_Neutrinos.Length)
            {
                if (R_Neutrinos[level]!=-1)
                {
                    scr_StatsPlayer.Neutrinos += R_Neutrinos[level];
                    scr_BDUpdate.f_SetNeutrinos(scr_StatsPlayer.id, scr_StatsPlayer.Neutrinos);
                }
            }
        }

        //Check Orbs
        if (R_Orbs != null)
        {
            if (level < R_Orbs.Length)
            {
                if (R_Orbs[level,0]!=-1)
                {
                    scr_StatsPlayer.Orbes[R_Orbs[level, 1]] += R_Orbs[level, 0];
                    scr_BDUpdate.f_SetOrbes(scr_StatsPlayer.id);
                }
            }
        }

        //Check Skins
        if (R_Skins != null)
        {
            if (level < R_Skins.Length)
            {
                if (R_Skins[level]!="none")
                {
                    string[] data_skin = R_Skins[level].Split(':');
                    scr_StatsPlayer.MySkins.Add(R_Skins[level]);
                    scr_UnitProgress _unit = scr_StatsPlayer.MyUnits.Find(i => i.Name == data_skin[0]);
                    if (_unit != null)
                        scr_BDUpdate.f_AddSkins(_unit, data_skin[1]);
                }
            }
        }
    }

    public string GetStringOfCurrentRewards()
    {
        string Rewards = "";
        //Check Titles
        if (R_Titles != null)
        {
            if (Level < R_Titles.Length)
            {
                int id_title = R_Titles[Level];
                if (id_title>-1)
                    Rewards += "+ "+scr_Lang.GetText("txt_mn_info93")+" "+ scr_Lang.GetTitleName(id_title)+"\n";
            }
        }

        //Check Neutrinos
        if (R_Neutrinos != null)
        {
            if (Level < R_Neutrinos.Length)
            {
                if (R_Neutrinos[Level]>0)
                    Rewards += "+ "+R_Neutrinos[Level].ToString()+" "+ scr_Lang.GetText("txt_mn_info31") + "\n";
            }
        }

        //Check Orbs
        if (R_Orbs != null)
        {
            if (Level < R_Orbs.Length)
            {
                if (R_Orbs[Level,0]>0)
                    Rewards += "+ " + R_Orbs[Level,0].ToString() + " " + scr_Lang.GetText("txt_mn_info29") +" " + scr_Lang.GetText(Orbs_type[R_Orbs[Level, 1]]) + "\n"; 
            }
        }

        //Check Skins
        if (R_Skins != null)
        {
            if (Level < R_Skins.Length)
            {
                if (R_Skins[Level]!="none")
                {
                    string[] data_skin = R_Skins[Level].Split(':');
                    string name_unit = "";
                    name_unit = scr_GetStats.GetPropUnit(data_skin[0], "Name");
                    Rewards += "+ " + scr_Lang.GetText("txt_mn_info94") + " " + data_skin[1] + " (" + name_unit + ")\n";
                }
            }
        }

        if (Rewards == "")
            Rewards = scr_Lang.GetText("txt_mn_info95");

        return Rewards;
    }

    public void AddTitlesRewards(int[] _titles)
    {
        R_Titles = _titles;
    }

    public void AddNeutrinosRewards(int[] _neutrinos)
    {
        R_Neutrinos = _neutrinos;
    }

    public void AddOrbsRewards(int[,] _orbs)
    {
        R_Orbs = _orbs;
    }

    public void AddSkinsRewards(string[] _skins)
    {
        R_Skins = _skins;
    }

}

using UnityEngine;
using UnityEngine.UI;

public class scr_FinalResults : MonoBehaviour {

    public Text T_Time;
    public Text T_XP;
    public Text T_QUARKS;
    public Text T_QUANTUMS;
    public Text T_RANK;
    public Text T_GEN_UNITS;
    public Text T_GEN_SKILLS;
    public Text T_DMG;
    public Text T_DSHIPS;
    public Text T_DSTATIONS;
    public Text T_DMG_GET;
    public Text T_LOST_SHIPS;
    public Text T_LOST_STATIONS;

    public Text T_WIN;
    public Text T_PLAYER_1;
    public Text T_PLAYER_2;
    public Text T_RANK_battle;

    scr_BattleMetrics BM;

    string[] Ranges = new string[10] { "C", "C+", "B-", "B", "B+", "A-", "A", "A+","S","SS" };

    // Use this for initialization
    void Start () {
        BM = scr_MNGame.BattleMetrics;
        if (BM.I_Win)
        {
            T_WIN.text = scr_Lang.GetText("txt_game_info14");
            T_WIN.color = Color.green;
        } else
        {
            T_WIN.text = scr_Lang.GetText("txt_game_info13");
            T_WIN.color = Color.red;
        }

        T_PLAYER_1.text = scr_StatsPlayer.Name;
        T_PLAYER_2.text = BM.Other_Player;

        T_Time.text = BM.Time_Minutes.ToString() + "'" + BM.Time_Sec.ToString();
        T_XP.text = BM.XP_Win.ToString();
        T_QUARKS.text = BM.Quarks_Win.ToString();
        T_QUANTUMS.text = BM.Quantums.ToString();
        T_RANK.text = BM.Rank_Points.ToString();
        T_GEN_UNITS.text = (BM.Stations_Spawn+ BM.Ships_Spawn+ BM.Gen_Units).ToString();
        T_GEN_SKILLS.text = BM.Skills_Spawn.ToString();
        T_DMG.text = BM.all_dmg.ToString();
        T_DSHIPS.text = BM.Ships_Kills.ToString();
        T_DSTATIONS.text = BM.Stations_Kills.ToString();
        T_DMG_GET.text = BM.all_get_dmg.ToString();
        T_LOST_SHIPS.text = BM.Ships_Lost.ToString();
        T_LOST_STATIONS.text = BM.Stations_Lost.ToString();

        int Puntuation = 0;

        Puntuation += BM.XP_Win;
        Puntuation += BM.all_dmg;
        Puntuation += BM.Ships_Kills*50;
        Puntuation += BM.Stations_Kills*100;

        Puntuation -= BM.Time_Minutes*500;
        Puntuation -= BM.Gen_Units;
        Puntuation -= BM.Skills_Spawn;
        Puntuation -= BM.Stations_Spawn;
        Puntuation -= BM.Ships_Spawn;
        Puntuation -= BM.Ships_Lost;
        Puntuation -= BM.Stations_Lost;
        Puntuation -= BM.all_get_dmg;

        int battle_range = (int)((float)Puntuation * 0.00075f);

        if (battle_range < 0) { battle_range = 0; }
        if (battle_range >= Ranges.Length) { battle_range = Ranges.Length - 1; }

        T_RANK_battle.text = Ranges[battle_range];

	}
	
}

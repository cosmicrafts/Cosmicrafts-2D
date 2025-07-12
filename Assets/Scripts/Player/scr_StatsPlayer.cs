using System.Collections.Generic;
using UnityEngine;

public class scr_StatsPlayer : MonoBehaviour
{
    public static bool Op_DMGText = true;
    public static bool Op_UIHPBar = true;
    public static bool Op_RadialFill = true;
    public static float Op_CameraSen = 0.1f;
    public static float Op_CameraMov = 4f;
    public static float Op_ShakeCamera = 1f;
    public static bool Op_Fullscr = false;
    public static bool Op_Music = true;
    public static bool Op_SoundFxs = true;
    public static int Op_Leng = 0;
    public static int OP_Resolution = 0;
    public static int OP_Graphics = 2;

    public static bool MobileDevice = false;

    public static bool Offline = false;

    public static int IndexFriend = 0;

    public static List<string>[] PlayerDeck;
    public static List<string>[] PlayerAvUnits;
    public static List<string> UnitsNotAv;
    public static List<string> AllUnits;

    public static List<scr_DataMatch> HistoryMatchs = new List<scr_DataMatch>();
    public static List<string> Friends = new List<string>();
    public static List<string> NewFriends = new List<string>();
    public static List<scr_BDUser> FriendsData = new List<scr_BDUser>(); 
    public static List<scr_UnitProgress> MyUnits = new List<scr_UnitProgress>();
    public static List<string> MySkins = new List<string>();
    public static List<scr_Achievements> MyAchiv = new List<scr_Achievements>();

    static public int LimitCardsUnits = 8;
    static public int[] NcardsUnits = new int[] { 0, 0, 0 };
    static public string[] Name_deck = new string[] { "", "", "" };

    public static Sprite SAvatar;
    public static int id = 0;
    public static int iduser = 0;
    public static string Name = "";
    public static string Email = "";
    public static int Level = 0;
    public static int Range = 0;
    public static int LRange = 1;
    public static int WinsStreak = 0;
    public static int IDAvatar = 0;
    public static int IdCurrentTitle = 0;
    public static int Xp = 0;
    public static float XpBuff = 1f;
    public static System.DateTime XpBuffExpire;
    public static int RankPoints = 0;
    public static int IdClan = -1;
    public static string Clan = "<No Clan>";
    public static int idc = 0;
    public static int Neutrinos = 0;
    public static int[] Orbes;
    public static string Deck = "";
    public static string Region = "No def";
    public static int VLeague = 0;
    public static int LLeague = 0;
    public static int DLeague = 0;
    public static int VIa = 0;
    public static int LIa = 0;
    public static int DIa = 0;
    public static string[] Emotes = new string[4] { "Hello", "GG", "XD", "Nice Play" };
    public static string FavoriteDeck = "";
    public static int LastState = 2;
    public static int CurrentState = 2;
    public static bool[] MyTitles = new bool[11]; //Defines number of titles
    public static scr_DataOrb[] OpeningOrbs;

    public static System.DateTime LastDateConection;
    public static bool FirstConnection = false;
    public static bool FirstBattle = false;

    //Stats In Game
    public static bool Afterbattle = false;
    public static bool Promotion = false;
    public static bool Downgrade = false;
    public static bool Tutorial = false;
    public static bool Practice = false;
    public static int New_Levels = 0;

    //After Game
    public static bool b_LevelUp = false;

    public static bool AddXp(int _xp)
    {
        int NextLevel = GetXPforNextLevel(Level);
        Xp += _xp;
        bool result = false;
        while (Xp >= NextLevel)
        {
            New_Levels++;
            Level++;
            Xp -= NextLevel;
            Debug.Log("Level Up");
            result = true;
            b_LevelUp = true;
            NextLevel = GetXPforNextLevel(Level);
        }
        return result;
    }

    public static bool AddRangePoints(int _p)
    {
        bool result = false;

        RankPoints += _p;
        if (RankPoints>=GetPointsNextRange(Range,LRange) && Range<8)
        {
            result = true;
            LRange --;
            if (LRange==0)
            {
                LRange = 5;
                Range++;
            }
        }

        Promotion = result;

        return result;
    }

    public static bool RestRangePoints(int _p)
    {
        bool result = false;

        RankPoints -= _p;
        if (RankPoints < GetPointsPrevRange(Range, LRange))
        {
            if (Range>1)
            {
                result = true;
                LRange ++;
                if (LRange > 5)
                {
                    LRange = 1;
                    Range--;
                }
            } else if (Range==1 && LRange<5)
            {
                result = true;
                LRange++;
            }
        }

        Downgrade = result;

        if (RankPoints <= 0) { RankPoints = 0; }

        return result;
    }

    public static int GetPointsNextRange(int _current_range, int current_lrange)
    {
        if (_current_range==0)
        {
            return -1;
        }

        int real_current_range = ((_current_range-1) * 5) + (5-(current_lrange-1));
        return 200 * real_current_range;
    }

    public static int GetPointsPrevRange(int _current_range, int current_lrange)
    {
        if (_current_range == 0 || (_current_range==1 && current_lrange==5))
        {
            return -1;
        }

        int cr = _current_range;
        int clr = current_lrange;

        clr++;
        if (clr>5)
        {
            clr = 1;
            cr--;
        }

        int real_current_range = ((cr - 1) * 5) + (5 - (clr - 1));
        return 200 * real_current_range;
    }

    public static int GetRankPointsGame(int Other_Points, bool iwin)
    {
        int result = 0;

        int diference = Other_Points-RankPoints;

        int index_dif = 0;

        bool other_is_less = diference < 0;

        if (other_is_less)
        {
            diference = Mathf.Abs(index_dif / 100);
            index_dif = 8 - diference;
            if (index_dif < 0) { index_dif = 0; }
        } else
        {
            diference = Mathf.Abs(index_dif / 100);
            index_dif = 8 + diference;
            if (index_dif > 16) { index_dif = 16; }
        }

        float porcent_win = 0;
        if (other_is_less)
            porcent_win = 0.5f + (0.065f * ((float)index_dif));
        else
            porcent_win = 1f + (0.125f * (float)(8-index_dif));

        float points_game = 25f* porcent_win;

        if (WinsStreak>5)
        {
            if (WinsStreak == 10)
                points_game = 2f * points_game;
            else
                points_game = 1.5f * points_game;
        }

        if (!iwin)
        {
            float porcent_lose = 1.5f - (0.065f * ((float)index_dif));
            points_game = points_game * porcent_lose;
            points_game *= -1f;
        }

        result = (int)points_game;

        return result;
    }

    public static int GetXPforNextLevel(int current_level)
    {
        float _rxp = 1500f;

        if (current_level > 100) { current_level = 100; }

        for (int i = 0; i < current_level-1; i++)
            _rxp *= 1.1f;

        return (int)(_rxp);
    }

    public static int GetNextReinfLevel(int current_level, int rarity)
    {
        int _rr = 8;

        switch(rarity)
        {
            case 1:
                {
                    _rr = 4;
                }
                break;
            case 2:
                {
                    _rr = 2;
                }
                break;
            case 3:
                {
                    _rr = 1;
                }
                break;
        }

        if (current_level > 32) { current_level = 32; }

        if (current_level>2)
            _rr = (int)Mathf.Pow(_rr, current_level-2);

        return _rr;
    }

    public static int FindIdUnit(string id)
    {
        int result = -1;
        scr_UnitProgress _unit = MyUnits.Find(i => i.Name == id);
        if (_unit != null)
            result = MyUnits.IndexOf(_unit);

        return result;
    }

    public static string GetRandomUnitFromRarity(int rarity)
    {
        string result = "";
        string _sr = rarity.ToString();

        int start_index = Random.Range(0, AllUnits.Count);

        for (int i=0; i<AllUnits.Count; i++)
        {
            if (scr_GetStats.GetPropUnit(AllUnits[start_index], "Rarity")==_sr)
            {
                result = AllUnits[start_index];
                break;
            }
            start_index++;
            if (start_index >= AllUnits.Count)
                start_index = 0;
        }

        return result;
    }

    public static scr_UnitProgress FindUnitProgress(string id)
    {
        return MyUnits.Find(i => i.Name == id);
    }

    public static void MoveXpBattleToPlayer()
    {
        if (Tutorial)
            return;

        if (XpBuff>1f)
        {
            scr_MNGame.BattleMetrics.XP_Win = (int)(((float)scr_MNGame.BattleMetrics.XP_Win) * XpBuff);
        }
        AddXp(scr_MNGame.BattleMetrics.XP_Win);
        Afterbattle = true;
    }

    public static void AddProgressBattleAchiv(int id, int _p)
    {
        if (scr_MNGame.GM.DebugMode || Tutorial)
            return;

        MyAchiv[id].AddProgressBattle(_p);
    }

    public static void ClearAll()
    {

        ChatNewGui chat = FindObjectOfType<ChatNewGui>();
        if (chat != null)
        {
            Destroy(chat.gameObject);
        }

        FirstConnection = false;
        FirstBattle = false;

        Afterbattle = false;
        Promotion = false;
        Downgrade = false;
        Tutorial = false;
        New_Levels = 0;

        MyUnits.Clear();
        Friends.Clear();
        FriendsData.Clear();
        HistoryMatchs.Clear();
        UnitsNotAv.Clear();
        MySkins.Clear();
        MyAchiv.Clear();
        AllUnits.Clear();
        NewFriends.Clear();

        LimitCardsUnits = 8;
        NcardsUnits = new int[] { 0, 0, 0 };
        Name_deck = new string[] { "", "", "" };
        Emotes = new string[4] { "Hello", "GG", "XD", "Nice Play" };
        MyTitles = new bool[11];

        id = 0;
        iduser = 0;
        Name = "";
        Email = "";
        Level = Xp = 0;
        IdClan = -1;
        Range = LRange = 0;
        IDAvatar = 0;
        IdCurrentTitle = 0;
        Neutrinos = 0;
        WinsStreak = 0;
        Deck = "";
        idc = 0;
        Region = "No def";
        Clan = "<No Clan>";
        FavoriteDeck = "";
        XpBuff = 1f;
        VLeague = DLeague = LLeague = VIa = DIa = LIa = RankPoints = 0;
        LastState = CurrentState = 2;
    }

    static public void ChangeAvatar(int idavatar)
    {
        if (!scr_BDUpdate.IsCHAvatar)
        {
            IDAvatar = idavatar;
            scr_BDUpdate.f_SetAvatar(id, idavatar);
            PhotonNetwork.player.CustomProperties.Remove("idAvatar");
            PhotonNetwork.player.CustomProperties.Add("idAvatar", id);
        }
        
    }

    static public string DeckToString()
    {
        string str_deck="";

        bool start = true;
        for (int i = 0; i < 3; i++)
        {
            start = true;
            str_deck += Name_deck[i] + "-";
            if (PlayerDeck[i].Count != 8)
            {
                Debug.LogWarning("Warning: Lost units in deck to save");
                continue;
            }
                
            for (int j=0; j<PlayerDeck[i].Count; j++)
            {
                if (start)
                {
                    start = false;
                }
                else { str_deck += "-"; }
                str_deck += PlayerDeck[i][j];
            }
            if (i < 2)
            {
                str_deck += "?";
            }
        }

        return str_deck;
    }

    static public string SpecificDeckToString(int index)
    {
        string str_deck = "";
        bool start = true;
        for (int i=0; i<PlayerDeck[index].Count; i++)
        {
            if (start)
            {
                start = false;
            }
            else { str_deck += "-"; }
            str_deck += PlayerDeck[index][i];
        }
        return str_deck;
    }

    static public string[] ConvertDeckNames(string deck)
    {
        string[] names;

        string[] str_deckunits = deck.Split('-');
        names = new string[str_deckunits.Length];

        if (deck.Length > 1)
            for (int i = 0; i < str_deckunits.Length; i++)
            {
                    names[i]= scr_GetStats.GetPropUnit(str_deckunits[i],"Name");
            }

        return names;
    }

    static public Sprite GetIconUnit(string NameUnit)
    {
        return Resources.Load<Sprite>("Units/Iconos/" + scr_GetStats.GetPropUnit(NameUnit, "Icon")); ;
    }

    static public Color GetColorStatusUser(int status)
    {
        Color sc = new Color(0.5f, 0.5f, 0.5f);
        switch (status)
        {
            case 1:
                //_status = "Invisible";
                sc = Color.gray;
                break;
            case 2:
                //_status = "Online";
                sc = Color.green;
                break;
            case 3:
                //_status = "Esperando Invitacion";
                sc = Color.yellow;
                break;
            case 4:
                //_status = "Ausente/Ocupado";
                sc = Color.red;
                break;
            case 5:
                //_status = "Buscando Partida";
                sc = Color.cyan;
                break;
            case 6:
                //_status = "Jugando";
                sc = Color.blue;
                break;
            default:
                //_status = "Offline";
                {
                    sc = Color.gray;
                }
                break;
        }

        return sc;

    }

}
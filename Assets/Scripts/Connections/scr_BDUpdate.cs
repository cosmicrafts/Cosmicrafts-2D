using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class scr_BDUpdate : MonoBehaviour {

    //static int idplayertest = 1; //ID PLAYER PARA TESTEO
    public static bool IsCHAvatar = false;
    public static bool IsCHDeck = false;
    public static bool IsCHUlti = false;
    public static bool IsCHFriend = false;
    public static bool IsCHDataF = false;
    public static bool IsCHUnitProg = false;
    public static int ExistUser = 0;
    public static int ExistEmail = -1;
    public static string NewRequest = "";
    public static string NewAcepted = "";
    static scr_BDUpdate SDBUP;
    public scr_BDUser UserData;
    bool LogOutInProcess = false;
    string BaseUrl = "http://cosmicrafts.com/CCPHPS";

    void Awake()
    {
        SDBUP = this;
        //f_CheckEmailExist("frenzy00@hotmail.com");
        //f_SetMatch("pancho", "julio", 1, "2018-02-01 12:00:00");
        //Debug.Log("PUsa Q-W-E-R-T-Y-U-I-O-P-A-S Para hacer distintas peticiones a la base de datos");
    }

    void Update()
    {
        //Test
        //TestSetsBD();
    }

    void TestSetsBD()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            f_SetRange(1, 2,4,0);
        }
    }

    public void LogOut()
    {
        if (LogOutInProcess)
            return;

        LogOutInProcess = true;
        Debug.Log("Request for Log out Send");
        StartCoroutine(IELogout());
    }

    public static void f_CheckEmailExist(string email)
    {
        Debug.Log("Request for exist email");
        SDBUP.StartCoroutine(SDBUP.CheckEmailExist(email));
    }

    public static void f_GetInfoUser(string _nameuser, bool addfrienddata)
    {
        if (_nameuser.Length >= 4)
        {
            Debug.Log("Request for Get User Info Send");
            SDBUP.StartCoroutine(SDBUP.GetInfoUser(_nameuser, addfrienddata));
        }
        else
        {
            Debug.Log("Invalid name user");
        }
    }

    public static void f_GetHistoryMatchs(int idaccount, int idfriend)
    {
        SDBUP.StartCoroutine(SDBUP.GetHistoryMatchs(idaccount, idfriend));
        Debug.Log("Request for Get History Matchs");
    }

    public static void f_UpdateUnit(scr_UnitProgress up)
    {
        if (!IsCHUnitProg)
        {
            SDBUP.StartCoroutine(SDBUP.UpdateUnit(up.Id, up.Level, up.Reinf, up.Skins, up.CurrentSkin));
            Debug.Log("Request for Unit Progress Sent");
        }
    }

    public static void f_BuyRefUnit(scr_UnitProgress up, int NewMoney)
    {
        SDBUP.StartCoroutine(SDBUP.BuyRefUnit(up.Id, up.Reinf, NewMoney));
        Debug.Log("Request for Unit Progress Sent");
    }

    public static void f_AddNewUnit(int idaccount, string idunit, int refactions)
    {
        SDBUP.StartCoroutine(SDBUP.AddNewUnit(idaccount, idunit, refactions));
        Debug.Log("Request for New Unit Player Sent");
    }

    public static void f_BuyNewUnit(int idaccount, string idunit, int refactions, int NewMoney)
    {
        SDBUP.StartCoroutine(SDBUP.BuyNewUnit(idaccount, idunit, refactions, NewMoney));
        Debug.Log("Request for Buy Unit Player Sent");
    }

    public static void f_AddNewPackUnits(int idaccount, string units_list)
    {
        SDBUP.StartCoroutine(SDBUP.AddNewPackUnits(idaccount, units_list));
        Debug.Log("Request for New Player Units Pack Sent");
    }

    public static void f_BuyNewPackUnits(int idaccount, string units_list, int NewMoney)
    {
        SDBUP.StartCoroutine(SDBUP.BuyNewPackUnits(idaccount, units_list, NewMoney));
        Debug.Log("Request for New Player Units Pack Sent");
    }

    public static void f_AddSkins(scr_UnitProgress forunit, string skin)
    {
        SDBUP.StartCoroutine(SDBUP.UpdateSkins(forunit, skin));
        Debug.Log("Request for New Skin Unit Sent");
    }

    public static void f_BuySkin(scr_UnitProgress forunit, string skin, int NewMoney)
    {
        SDBUP.StartCoroutine(SDBUP.BuySkin(forunit, skin, NewMoney));
        Debug.Log("Request for New Skin Unit Sent");
    }

    public static void f_AddSkinsPack(scr_UnitProgress[] forunits, string[] skins)
    {
        SDBUP.StartCoroutine(SDBUP.UpdateSkinsPack(forunits, skins));
        Debug.Log("Request for New Skin Unit Sent");
    }

    public static void f_BuySkinsPack(scr_UnitProgress[] forunits, string[] skins, int NewMoney)
    {
        SDBUP.StartCoroutine(SDBUP.BuySkinsPack(forunits, skins, NewMoney));
        Debug.Log("Request for New Skin Unit Sent");
    }

    public static void f_UpdatePlayerXp(int idaccount, int Xp)
    {
        SDBUP.StartCoroutine(SDBUP.UpdatePlayerXp(idaccount, Xp));
        Debug.Log("Request for Player experience Sent");
    }

    public static void f_UpdateLastConnection(int idaccount, System.DateTime date)
    {
        SDBUP.StartCoroutine(SDBUP.UpdateLastConnection(idaccount, date));
        Debug.Log("Request for update last connection");

    }

    public static void f_UpdatePlayerFriends(int iduser)
    {
        SDBUP.StartCoroutine(SDBUP.UpdatePlayerFriends(iduser));
        Debug.Log("Request for Player friends Sent");

    }

    public static void f_SaveMissions(int idaccount)
    {
        SDBUP.StartCoroutine(SDBUP.SaveMissions(idaccount));
        Debug.Log("Request for Missions Save Sent");
    }

    public static void f_SetLevelunit(int idaccount, string name_ship, int Level)
    {
        SDBUP.StartCoroutine(SDBUP.SetLevelunit(idaccount, name_ship, Level));
        Debug.Log("Request for unit experience Sent");
    }

    public static void f_SetLevelPlayer(int idaccount, int level)
    {
        SDBUP.StartCoroutine(SDBUP.SetLevelPlayer(idaccount, level));
        Debug.Log("Request for player level Sent");
    }

    public static void f_SetLvlXpPlayer(int idaccount, int level, int Xp)
    {
        SDBUP.StartCoroutine(SDBUP.SetLvlXpPlayer(idaccount, level, Xp));
        Debug.Log("Request for player xp and lvl Sent");
    }

    public static void f_SetRegion(int idaccount, string _Region)
    {
        SDBUP.StartCoroutine(SDBUP.SetRegion(idaccount, _Region));
        Debug.Log("Request for player region Sent");
    }

    public static void f_SetTitle(int idaccount, int idTitle)
    {
        SDBUP.StartCoroutine(SDBUP.SetTitle(idaccount, idTitle));
        Debug.Log("Request for player title Sent");
    }

    public static void f_SetAvatar(int idaccount, int avatar)
    {
        if (!IsCHAvatar)
        {
            if (idaccount == scr_StatsPlayer.id)
            {
                PhotonNetwork.player.CustomProperties.Remove("idAvatar");
                PhotonNetwork.player.CustomProperties.Add("idAvatar", scr_StatsPlayer.IDAvatar);
            }
            IsCHAvatar = true;
            SDBUP.StartCoroutine(SDBUP.SetAvatar(idaccount, avatar));
            Debug.Log("Request for player avatar Sent");
        }
    }

    public static void f_SetClan(int idaccount, string Clan)
    {
        SDBUP.StartCoroutine(SDBUP.SetClan(idaccount, Clan));
        Debug.Log("Request for player clan Sent");
    }

    public static void f_UpdateAchievements(int idaccount)
    {
        SDBUP.StartCoroutine(SDBUP.UpdateAchievements(idaccount));
        Debug.Log("Request for player Achievements Sent");
    }

    public static void f_UpdateMyTitles(int idaccount)
    {
        SDBUP.StartCoroutine(SDBUP.UpdateMyTitles(idaccount));
        Debug.Log("Request for player MyTitles Sent");
    }

    public static void f_SetStatsGamesLeague(int idaccount, int victorys, int defeats, int draws)
    {
        SDBUP.StartCoroutine(SDBUP.SetStatsGamesLeague(idaccount, victorys, defeats, draws));
        Debug.Log("Request for Stats League");
    }

    public static void f_SetStatsGamesIA(int idaccount, int victorys, int defeats, int draws)
    {
        SDBUP.StartCoroutine(SDBUP.SetStatsGamesIA(idaccount, victorys, defeats, draws));
        Debug.Log("Request for Stats IA Games");
    }

    public static void f_SetOrbes(int idaccount)
    {
        SDBUP.StartCoroutine(SDBUP.SetOrbes(idaccount));
        Debug.Log("Request for Orbes Sent");
    }

    public static void f_BuyOrbes(int NewMoney)
    {
        SDBUP.StartCoroutine(SDBUP.BuyOrbes(NewMoney));
        Debug.Log("Request for Orbes Sent");
    }

    public static void f_SetStarsOrbs(int idaccount, bool WaitResponse)
    {
        SDBUP.StartCoroutine(SDBUP.SetStarsOrbs(idaccount, WaitResponse));
        Debug.Log("Request for Stars Orbes Sent");
    }

    public static void f_SetAllOrbs(int idaccount, bool WaitResponse)
    {
        SDBUP.StartCoroutine(SDBUP.SetAllOrbes(idaccount, WaitResponse));
        Debug.Log("Request for All Orbes Sent");
    }

    public static void f_SetXpBuff(int idaccount)
    {
        SDBUP.StartCoroutine(SDBUP.SetXpBuff(idaccount));
        Debug.Log("Request for Xp Buff Sent");
    }

    public static void f_BuyXpBuff(int NewMoney)
    {
        SDBUP.StartCoroutine(SDBUP.BuyXpBuff(NewMoney));
        Debug.Log("Request for buy Xp Buff Sent");
    }

    public static void f_SetNeutrinos(int idaccount, int value)
    {
        SDBUP.StartCoroutine(SDBUP.SetNeutrinos(idaccount, value));
        Debug.Log("Request for Neutrinos Sent");
    }

    public static void f_SetRange(int idaccount, int Rango, int Lvl, int points)
    {
        if (idaccount == scr_StatsPlayer.id)
        {
            PhotonNetwork.player.CustomProperties.Remove("Rpoints");
            PhotonNetwork.player.CustomProperties.Add("Rpoints", scr_StatsPlayer.RankPoints);
        }
        SDBUP.StartCoroutine(SDBUP.SetRange(idaccount, Rango, Lvl, points));
        Debug.Log("Request for player Range Sent");
    }

    public static void f_SetMatch(int id1, int id2, string team1, string team2, int Xp1, int Xp2, int winer, string date)
    {
        SDBUP.StartCoroutine(SDBUP.SetMatch(id1, id2, team1, team2, Xp1, Xp2, winer, date));
        Debug.Log("Request for send Match");
    }

    public static void f_CheckExistUser(string name)
    {
        SDBUP.StartCoroutine(SDBUP.CheckExistUser(name));
        Debug.Log("Request for check and add friend Sent");
    }

    public static void f_UpdateAllUnitsProgress()
    {
        SDBUP.StartCoroutine(SDBUP.UpdateAllUnitsProgress());
        Debug.Log("Request for update all units progress send");
    }

    public static void f_UpdateListUnitsProgress(List<scr_UnitProgress> _list_units)
    {
        SDBUP.StartCoroutine(SDBUP.UpdateListUnitsProgress(_list_units));
        Debug.Log("Request for update list of units send");
    }

    public static void f_AddFriend(string nameuser, string newfriend)
    {
        SDBUP.StartCoroutine(SDBUP.AddPlayerFriend(nameuser, newfriend));
        Debug.Log("Request for Add friend Sent");
    }

    public static void f_DeleteFriend(string nameuser, string friend)
    {
        SDBUP.StartCoroutine(SDBUP.DeletePlayerFriend(nameuser, friend));
        Debug.Log("Request for Delete friend Sent");
    }

    public static void f_AddFriendR(string nameuser, string newfriend)
    {
        SDBUP.StartCoroutine(SDBUP.AddPlayerFriendR(nameuser, newfriend));
        Debug.Log("Request for Add friend Sent");
    }

    public static void f_DeleteFriendR(string nameuser, string newfriend, bool accepted)
    {
        SDBUP.StartCoroutine(SDBUP.DeletePlayerFriendR(nameuser, newfriend, accepted));
        Debug.Log("Request for Delete friend Sent");
    }

    public static void f_UpdatePlayerFriends()
    {
        SDBUP.StartCoroutine(SDBUP.UpdatePlayerFriends(scr_StatsPlayer.iduser));
        Debug.Log("Request for player Range Sent");
    }

    public static void f_GetDataFriends(string nameuser)
    {
        SDBUP.StartCoroutine(SDBUP.GetDataFriends(nameuser));
        //Debug.Log("Request for Add friend Sent");
    }

    void AddDataPackUnits(string units_list, string StatsUnit)
    {
        string[] units = units_list.Split('|');
        string[] sids = StatsUnit.Split('|');

        Debug.Log("L DB:" + sids.Length);
        Debug.Log("L UN:" + units.Length);

        scr_UIPlayerEditor PE = FindObjectOfType<scr_UIPlayerEditor>();

        for (int i = 0; i < units.Length; i++)
        {
            int newidunit = -1;
            int.TryParse(sids[i + 1], out newidunit);
            if (newidunit != -1)
            {
                if (PE)
                {
                    PE.NewCardsToDeck.Add(units[i]);
                    if (PE.Store.gameObject.activeSelf)
                    {
                        PE.Store.AddUnitSkins(units[i]);
                    }
                    else
                    {
                        PE.NewSkinsToAdd.Add(units[i]);
                    }
                }

                scr_StatsPlayer.UnitsNotAv.Remove(units[i]);

                scr_UnitProgress NewUnit = new scr_UnitProgress();
                NewUnit.Skins = "Classic";
                NewUnit.Name = units[i];
                NewUnit.Id = newidunit;
                int rarity = 0;
                string rs = scr_GetStats.GetPropUnit(NewUnit.Name, "Rarity");
                int.TryParse(rs, out rarity);
                NewUnit.Rarity = rarity;

                scr_StatsPlayer.MyUnits.Add(NewUnit);

                for (int j = 0; j < scr_StatsPlayer.PlayerAvUnits.Length; j++)
                {
                    if (!scr_StatsPlayer.PlayerAvUnits[j].Contains(units[i]) && !scr_StatsPlayer.PlayerDeck[j].Contains(units[i]))
                        scr_StatsPlayer.PlayerAvUnits[j].Add(units[i]);
                }
            }
        }
    }

    void AddDataUnit(string idunit, string StatsUnit)
    {
        string sid = StatsUnit.Substring(3);
        int newidunit = -1;
        if (int.TryParse(sid, out newidunit))
        {
            scr_UIPlayerEditor PE = FindObjectOfType<scr_UIPlayerEditor>();

            if (PE)
            {
                PE.NewCardsToDeck.Add(idunit);
                if (PE.Store.gameObject.activeSelf)
                {
                    PE.Store.AddUnitSkins(idunit);
                }
                else
                {
                    PE.NewSkinsToAdd.Add(idunit);
                }
            }

            scr_StatsPlayer.UnitsNotAv.Remove(idunit);

            scr_UnitProgress NewUnit = new scr_UnitProgress();
            NewUnit.Skins = "Classic";
            NewUnit.Name = idunit;
            NewUnit.Id = newidunit;
            int rarity = 0;
            string rs = scr_GetStats.GetPropUnit(NewUnit.Name, "Rarity");
            int.TryParse(rs, out rarity);
            NewUnit.Rarity = rarity;

            scr_StatsPlayer.MyUnits.Add(NewUnit);

            for (int i = 0; i < scr_StatsPlayer.PlayerAvUnits.Length; i++)
            {
                if (!scr_StatsPlayer.PlayerAvUnits[i].Contains(idunit) && !scr_StatsPlayer.PlayerDeck[i].Contains(idunit))
                    scr_StatsPlayer.PlayerAvUnits[i].Add(idunit);
            }
        }
    }

    public static void f_UpdateDeck(int idaccount)
    {
        if (!IsCHDeck)
        {
            if (idaccount == scr_StatsPlayer.id)
            {

                PhotonNetwork.player.CustomProperties.Remove("Deck");
                PhotonNetwork.player.CustomProperties.Add("Deck", scr_StatsPlayer.SpecificDeckToString(scr_StatsPlayer.idc));
            }

            SDBUP.StartCoroutine(SDBUP.UpdateDeck(idaccount));
            Debug.Log("Request for player Deck Sent");
            IsCHDeck = true;
        }
    }

    IEnumerator UpdatePlayerXp(int idaccount, int Xp)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("PlayerXp", Xp);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlUpdatePlayerXp.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("UpdatePlayerXp");
        }

        string StatsXp = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsXp.Length>0)
        if (StatsXp.Substring(0, 2) == "OK")
        {
            Debug.Log("Player Xp updated");
        }
        else if (StatsXp.Length > 4)
        {
            if (StatsXp.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsXp.Substring(6));
                Application.Quit();
            }
        }
        StopCoroutine("UpdatePlayerXp");
    }

    IEnumerator UpdateAllUnitsProgress()
    {
        for(int i=0; i< scr_StatsPlayer.MyUnits.Count; i++)
        {
            WWWForm sendInfo = new WWWForm();
            sendInfo.AddField("dw", Scr_Database.db_dm);

            sendInfo.AddField("UnitId", scr_StatsPlayer.MyUnits[i].Id);
            sendInfo.AddField("UnitLevel", scr_StatsPlayer.MyUnits[i].Level);
            sendInfo.AddField("UnitRein", scr_StatsPlayer.MyUnits[i].Reinf);
            sendInfo.AddField("Skins", scr_StatsPlayer.MyUnits[i].Skins);
            sendInfo.AddField("CurrentSkin", scr_StatsPlayer.MyUnits[i].CurrentSkin);

            //Request
            UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlUpdateUnit.php", sendInfo);

            getData.downloadHandler = new DownloadHandlerBuffer();

            yield return getData.SendWebRequest(); //esperamos

            if (getData.isNetworkError || getData.isHttpError)
            {
                Debug.Log(getData.error);
                Debug.Log("Error update ship " + scr_StatsPlayer.MyUnits[i].Name + " ("+getData.downloadHandler.text+")");
            }

            Debug.Log("Status update unit " + scr_StatsPlayer.MyUnits[i].Name + " " + getData.downloadHandler.text);
        }
        StopCoroutine("UpdateAllDeckProgress");
    }

    IEnumerator UpdateListUnitsProgress(List<scr_UnitProgress> _lup)
    {
        for (int i = 0; i < _lup.Count; i++)
        {
            WWWForm sendInfo = new WWWForm();
            sendInfo.AddField("dw", Scr_Database.db_dm);

            sendInfo.AddField("UnitId", _lup[i].Id);
            sendInfo.AddField("UnitLevel", _lup[i].Level);
            sendInfo.AddField("UnitRein", _lup[i].Reinf);
            sendInfo.AddField("Skins", _lup[i].Skins);
            sendInfo.AddField("CurrentSkin", _lup[i].CurrentSkin);

            //Request
            UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlUpdateUnit.php", sendInfo);

            getData.downloadHandler = new DownloadHandlerBuffer();

            yield return getData.SendWebRequest(); //esperamos

            if (getData.isNetworkError || getData.isHttpError)
            {
                Debug.Log(getData.error);
                Debug.Log("Error update ship " + _lup[i].Name + " (" + getData.downloadHandler.text + ")");
            }

            Debug.Log("Status update unit " + _lup[i].Name + " " + getData.downloadHandler.text);

        }

    }
    /*
    IEnumerator AddNewRefactions(Dictionary<string,string> _units)
    {
        
        foreach (KeyValuePair<string, int> kvp in _units)
        {
            WWWForm sendInfo = new WWWForm();
            sendInfo.AddField("dw", Scr_Database.db_dm);

            sendInfo.AddField("UnitId", kvp.Key);
            sendInfo.AddField("UnitRein", kvp.Value);
        }
        
        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlUpdateUnit.php", _units);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            Debug.Log("Error add refactions");
        }

        Debug.Log("Status add refactions" + getData.downloadHandler.text);
    }
    */
    IEnumerator UpdateUnit(int idunit, int levelunit, int reifunit, string skins, string currentskin)
    {
        IsCHUnitProg = true;
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("UnitId", idunit);
        sendInfo.AddField("UnitLevel", levelunit);
        sendInfo.AddField("UnitRein", reifunit);
        sendInfo.AddField("Skins", skins);
        sendInfo.AddField("CurrentSkin", currentskin);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlUpdateUnit.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            IsCHUnitProg = false;
            StopCoroutine("UpdateUnit");
        }

        string StatsUnit = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsUnit.Length > 0)
            if (StatsUnit.Substring(0, 2) == "OK")
            {
                Debug.Log("Unit Progress updated");
            }
            else if (StatsUnit.Length > 4)
            {
                if (StatsUnit.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsUnit.Substring(6));
                    Application.Quit();
                }
            }
        IsCHUnitProg = false;
        StopCoroutine("UpdateUnit");
    }

    IEnumerator BuyRefUnit(int idunit, int reifunit, int NewMoney)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", scr_StatsPlayer.id);
        sendInfo.AddField("UnitId", idunit);
        sendInfo.AddField("UnitRein", reifunit);
        sendInfo.AddField("NewMoney", NewMoney);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlBuyRefUnit.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("BuyRefUnit");
        }

        string StatsUnit = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsUnit.Length > 0)
            if (StatsUnit.Substring(0, 2) == "OK")
            {
                scr_Store.WaitForDBResponse = true;
                Debug.Log("Unit Progress updated");
            }
            else if (StatsUnit.Length > 4)
            {
                if (StatsUnit.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsUnit.Substring(6));
                    Application.Quit();
                }
            }
    }

    IEnumerator SetLevelunit(int idaccount, string name_ship, int Level)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("UnitName", name_ship);
        sendInfo.AddField("Level", Level);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetLevelUnit.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetLevelunit");
        }

        string StatsLvl = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsLvl.Length>0)
        if (StatsLvl.Substring(0, 2) == "OK")
        {
            Debug.Log("Unit level updated");
        }
        else if (StatsLvl.Length > 4)
        {
            if (StatsLvl.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsLvl.Substring(6));
                Application.Quit();
            }
        }
        StopCoroutine("SetLevelunit");
    }

    IEnumerator SetLevelPlayer(int idaccount, int Level)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Level", Level);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetLevelPlayer.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetLevelPlayer");
        }

        string StatsLvl = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsLvl.Length>0)
        if (StatsLvl.Substring(0, 2) == "OK")
        {
            Debug.Log("Player Level updated");
        }
        else if (StatsLvl.Length > 4)
        {
            if (StatsLvl.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsLvl.Substring(6));
                Application.Quit();
            }
        }
        StopCoroutine("SetLevelPlayer");
    }

    IEnumerator SetLvlXpPlayer(int idaccount, int Level,int Xp)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Level", Level);
        sendInfo.AddField("Xp", Xp);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetLvlXpPlayer.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetLvlXpPlayer");
        }

        string StatsXpLvl = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsXpLvl.Length>0)
        if (StatsXpLvl.Substring(0, 2) == "OK")
        {
            Debug.Log("Player Level and xp updated");
        }
        else if (StatsXpLvl.Length > 4)
        {
            if (StatsXpLvl.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsXpLvl.Substring(6));
                Application.Quit();
            }
        }
        StopCoroutine("SetLvlXpPlayer");
    }

    IEnumerator SetRegion(int idaccount, string _Region)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Region", _Region);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetRegion.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetRegion");
        }

        string StatsRegion = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsRegion.Length > 0)
        if (StatsRegion.Substring(0, 2) == "OK")
        {
            Debug.Log("Player Region updated");
        }
        else if (StatsRegion.Length > 4)
        {
            if (StatsRegion.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsRegion.Substring(6));
                Application.Quit();
            }
        }
        StopCoroutine("SetRegion");
    }

    IEnumerator UpdateSkins(scr_UnitProgress forunit, string skin)
    {
        int indexunit = scr_StatsPlayer.MyUnits.IndexOf(forunit);
        string newskins = scr_StatsPlayer.MyUnits[indexunit].Skins + "|" + skin;
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdUnit", forunit.Id);
        sendInfo.AddField("Skins", newskins);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/UpdateSkins.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("UpdateSkins");
        }

        string StatsSkin = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsSkin.Length > 0)
            if (StatsSkin.Substring(0, 2) == "OK")
            {
                scr_StatsPlayer.MyUnits[indexunit].Skins += "|" + skin;
                scr_StatsPlayer.MySkins.Add(forunit.Name + ":" + skin);
                Debug.Log("New Skin Added");
            }
            else if (StatsSkin.Length > 4)
            {
                if (StatsSkin.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsSkin.Substring(6));
                    Application.Quit();
                }
            }
        StopCoroutine("UpdateSkins");
    }

    IEnumerator BuySkin(scr_UnitProgress forunit, string skin, int NewMoney)
    {
        int indexunit = scr_StatsPlayer.MyUnits.IndexOf(forunit);
        string newskins = scr_StatsPlayer.MyUnits[indexunit].Skins + "|" + skin;
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", scr_StatsPlayer.id);
        sendInfo.AddField("IdUnit", forunit.Id);
        sendInfo.AddField("Skins", newskins);
        sendInfo.AddField("NewMoney", NewMoney);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/BuySkin.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("BuySkins");
        }

        string StatsSkin = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsSkin.Length > 0)
            if (StatsSkin.Substring(0, 2) == "OK")
            {
                scr_StatsPlayer.MyUnits[indexunit].Skins += "|" + skin;
                scr_StatsPlayer.MySkins.Add(forunit.Name + ":" + skin);
                scr_Store.WaitForDBResponse = true;
                Debug.Log("New Skin Added");
            }
            else if (StatsSkin.Length > 4)
            {
                if (StatsSkin.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsSkin.Substring(6));
                    Application.Quit();
                }
            }
    }

    IEnumerator UpdateSkinsPack(scr_UnitProgress[] forunits, string[] skins)
    {
        string _skins = "";
        string _forunits = "";
        for (int i=0; i<skins.Length; i++)
        {
            if (skins[i] == "null")
                continue;
            if (_skins != "")
                _skins += "?";
            if (_forunits != "")
                _forunits += "|";
            _skins += forunits[i].Skins += "|" + skins[i];
            _forunits += forunits[i].Id;
        }

        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdsUnits", _forunits);
        sendInfo.AddField("AllSkins", _skins);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/UpdateSkinsPack.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("UpdateSkinsPack");
        }

        string StatsSkin = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsSkin.Length > 0)
        {
            if (StatsSkin.Substring(0, 2) == "OK")
            {
                for (int a = 0; a < forunits.Length; a++)
                {
                    if (skins[a] == "null")
                        continue;
                    int idx = scr_StatsPlayer.MyUnits.IndexOf(forunits[a]);
                    scr_StatsPlayer.MyUnits[idx].Skins += "|"+skins[a];  
                }
                Debug.Log("New Skins Pack Added");
            }
            else if (StatsSkin.Length > 4)
            {
                if (StatsSkin.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsSkin.Substring(6));
                    Application.Quit();
                }
            }
        }

        StopCoroutine("UpdateSkinsPack");
    }

    IEnumerator BuySkinsPack(scr_UnitProgress[] forunits, string[] skins, int NewMoney)
    {
        string _skins = "";
        string _forunits = "";
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i] == "null")
                continue;
            if (_skins != "")
                _skins += "?";
            if (_forunits != "")
                _forunits += "|";
            _skins += forunits[i].Skins += "|" + skins[i];
            _forunits += forunits[i].Id;
        }

        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", scr_StatsPlayer.id);
        sendInfo.AddField("IdsUnits", _forunits);
        sendInfo.AddField("AllSkins", _skins);
        sendInfo.AddField("NewMoney", NewMoney);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/BuySkinsPack.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("BuySkinsPack");
        }

        string StatsSkin = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsSkin.Length > 0)
        {
            if (StatsSkin.Substring(0, 2) == "OK")
            {
                for (int a = 0; a < forunits.Length; a++)
                {
                    if (skins[a] == "null")
                        continue;
                    int idx = scr_StatsPlayer.MyUnits.IndexOf(forunits[a]);
                    scr_StatsPlayer.MyUnits[idx].Skins += "|" + skins[a];
                }
                scr_Store.WaitForDBResponse = true;
                Debug.Log("New Skins Pack Added");
            }
            else if (StatsSkin.Length > 4)
            {
                if (StatsSkin.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsSkin.Substring(6));
                    Application.Quit();
                }
            }
        }
    }

    IEnumerator AddNewUnit(int idaccount, string idunit, int refactions)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("IdUnit", idunit);
        sendInfo.AddField("Reinforcements", refactions);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/AddNewUnit.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("AddNewUnit");
        }

        string StatsUnit = getData.downloadHandler.text;
        
        //Checamos si fue exitoso
        if (StatsUnit.Substring(0, 2) == "OK")
        {
            scr_Store.WaitForDBResponse = true;
            AddDataUnit(idunit, StatsUnit);
            Debug.Log("Player New Unit Added");
        }
        else if (StatsUnit.Length > 4)
        {
            if (StatsUnit.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsUnit.Substring(6));
                Application.Quit();
            }
            else { Debug.Log(StatsUnit); }
        }

        StopCoroutine("AddNewUnit");
    }

    IEnumerator BuyNewUnit(int idaccount, string idunit, int refactions, int NewMoney)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("IdUnit", idunit);
        sendInfo.AddField("Reinforcements", refactions);
        sendInfo.AddField("NewMoney", NewMoney);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/BuyNewUnit.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("BuyNewUnit");
        }

        string StatsUnit = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsUnit.Substring(0, 2) == "OK")
        {
            scr_Store.WaitForDBResponse = true;
            AddDataUnit(idunit, StatsUnit);
            Debug.Log("Player New Unit Added");
        }
        else if (StatsUnit.Length > 4)
        {
            if (StatsUnit.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsUnit.Substring(6));
                Application.Quit();
            }
            else { Debug.Log(StatsUnit); }
        }
    }

    IEnumerator AddNewPackUnits(int idaccount, string units_list)
    {
        //Formulario

        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("UnitsList", units_list);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/AddNewPackUnits.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("AddNewPackUnits");
        }

        string StatsUnit = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsUnit.Length > 0)
            if (StatsUnit.Substring(0, 2) == "OK")
            {
                AddDataPackUnits(units_list, StatsUnit);
                Debug.Log("Player New Units Pack Added");
            }
            else if (StatsUnit.Length > 4)
            {
                if (StatsUnit.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsUnit.Substring(6));
                    Application.Quit();
                }
            }
        StopCoroutine("AddNewPackUnits");
    }

    IEnumerator BuyNewPackUnits(int idaccount, string units_list, int NewMoney)
    {
        //Formulario

        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("UnitsList", units_list);
        sendInfo.AddField("NewMoney", NewMoney);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/BuyNewPackUnits.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("BuyNewPackUnits");
        }

        string StatsUnit = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsUnit.Length > 0)
            if (StatsUnit.Substring(0, 2) == "OK")
            {
                scr_Store.WaitForDBResponse = true;

                AddDataPackUnits(units_list, StatsUnit);
            }
            else if (StatsUnit.Length > 4)
            {
                if (StatsUnit.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsUnit.Substring(6));
                    Application.Quit();
                }
            }
        StopCoroutine("AddNewPackUnits");
    }

    IEnumerator UpdateLastConnection(int idaccount, System.DateTime _date)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("LDate", _date.ToString("yyyy-MM-dd HH:mm:ss"));

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlUpdateLastConn.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("UpdateLastConnection");
        }

        string StatsLC = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsLC.Length > 0)
            if (StatsLC.Substring(0, 2) == "OK")
            {
                Debug.Log("Player LastConnection updated");
            }
            else if (StatsLC.Length > 4)
            {
                if (StatsLC.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsLC.Substring(6));
                    Application.Quit();
                }
            }
        StopCoroutine("UpdateLastConnection");
    }

    IEnumerator UpdateAchievements(int idaccount)
    {
        string str_atch = "";

        for (int i = 0; i < scr_StatsPlayer.MyAchiv.Count; i++)
        {
            if (i>0)
                str_atch += "|";

            str_atch += scr_StatsPlayer.MyAchiv[i].Progress;
        }

        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Achievements", str_atch);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetAchievements.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("UpdateAchievements");
        }

        string StatsAtch = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsAtch.Length > 0)
            if (StatsAtch.Substring(0, 2) == "OK")
            {
                Debug.Log("Player Achievements updated");
            }
            else if (StatsAtch.Length > 4)
            {
                if (StatsAtch.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsAtch.Substring(6));
                    Application.Quit();
                }
            }
        StopCoroutine("UpdateAchievements");
    }

    IEnumerator UpdateMyTitles(int idaccount)
    {
        string str_titles = "";
        bool start = true;

        for (int i = 0; i < scr_StatsPlayer.MyTitles.Length; i++)
        {
            if (scr_StatsPlayer.MyTitles[i])
            {
                if (!start)
                    str_titles += "|";
                else
                    start = false;
                str_titles += i.ToString();
            }
        }

        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("MyTitles", str_titles);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetMyTitles.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("UpdateMyTitles");
        }

        string StatsMyTitles = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsMyTitles.Length > 0)
            if (StatsMyTitles.Substring(0, 2) == "OK")
            {
                Debug.Log("Player MyTitles updated");
            }
            else if (StatsMyTitles.Length > 4)
            {
                if (StatsMyTitles.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsMyTitles.Substring(6));
                    Application.Quit();
                }
            }
        StopCoroutine("UpdateMyTitles");
    }

    IEnumerator SetTitle(int idaccount, int idTitle)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Title", idTitle);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetTitle.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetTitle");
        }

        string StatsTitle = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsTitle.Substring(0,2)=="OK")
        {
            Debug.Log("Player title updated");
        }
        else
        {
            if (StatsTitle.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsTitle.Substring(6));
                Application.Quit();
            }
        }
        StopCoroutine("SetTitle");
    }

    IEnumerator SaveMissions(int idaccount)
    {

        string Missions = "";
        for (int i = 0; i < scr_Missions.MD_Progress.Length; i++)
        {
            if (i > 0) { Missions += "|"; }
            Missions += scr_Missions.MD_Progress[i].ToString();
        }

        for (int i = 0; i < scr_Missions.MW_Progress.Length; i++)
        {
            Missions += "|"+ scr_Missions.MW_Progress[i].ToString();
        }

        Missions += "|" + scr_Missions.DaysCounter.ToString();

        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Missions", Missions);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSaveMissions.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SaveMissions");
        }

        string stats_missions = getData.downloadHandler.text;

        Debug.Log(stats_missions);

        //Checamos si fue exitoso
        if (stats_missions.Length>0)
        if (stats_missions.Substring(0, 2) == "OK")
        {
            Debug.Log("Missions updated");
        }
        else if (stats_missions.Length > 4)
        {
            if (stats_missions.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(stats_missions.Substring(6));
                Application.Quit();
            }
        }
        StopCoroutine("SaveMissions");
    }

    IEnumerator SetAvatar(int idaccount, int avatar)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Avatar", avatar);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetAvatar.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            IsCHAvatar = false;
            Debug.Log(getData.error);
            StopCoroutine("SetAvatar");
        }

        string StatsAvatar = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsAvatar.Length>0)
        if (StatsAvatar.Substring(0, 2) == "OK")
        {
            IsCHAvatar = false;
            Debug.Log("Avatar Updated");
        }
        else if (StatsAvatar.Length > 4)
        {
            if (StatsAvatar.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsAvatar.Substring(6));
                Application.Quit();
            }
            IsCHAvatar = false;
        }   
        StopCoroutine("SetAvatar");
    }

    IEnumerator UpdateDeck(int idaccount)
    {
        string str_deck = scr_StatsPlayer.DeckToString();
        
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Deck", str_deck);
        sendInfo.AddField("IdDeck", scr_StatsPlayer.idc);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetDeckPlayer.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            IsCHDeck = false;
            Debug.Log(getData.error);
            StopCoroutine("UpdateDeck");
        }

        string DeckState = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (DeckState.Length>0)
        if (DeckState.Substring(0, 2) == "OK")
        {
            Debug.Log("Deck Updated");
            IsCHDeck = false;
        }
        else if (DeckState.Length > 4)
        {
            if (DeckState.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(DeckState.Substring(6));
                Application.Quit();
            }
            IsCHDeck = false;
        }
        StopCoroutine("UpdateDeck");
    }

    IEnumerator UpdatePlayerFriends(int iduser)
    {
        string str_friends = "";
        bool start = true;

        for (int i = 0; i < scr_StatsPlayer.Friends.Count; i++)
        {
            str_friends += scr_StatsPlayer.Friends[i];
            if (!start)
            {
                str_friends += "?";
            } else
            {
                start = false;
            }
        }
        if (str_friends!="" && scr_StatsPlayer.NewFriends.Count>0)
            str_friends += "?";
        for (int i = 0; i < scr_StatsPlayer.NewFriends.Count; i++)
        {
            str_friends += "+"+scr_StatsPlayer.NewFriends[i];
            if (!start)
            {
                str_friends += "?";
            }
            else
            {
                start = false;
            }
        }
        
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", iduser);
        sendInfo.AddField("Friends", str_friends);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlUpdateFriends.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("UpdatePlayerFriends");
        }

        string FriendsStats = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (FriendsStats.Length>0)
        if (FriendsStats.Substring(0, 2) == "OK")
        {
            Debug.Log("Friends Updated");
        }
        else if (FriendsStats.Length > 4)
        {
            if (FriendsStats.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(FriendsStats.Substring(6));
                Application.Quit();
            }
        }
        StopCoroutine("UpdatePlayerFriends");
    }

    IEnumerator AddPlayerFriend(string nameuser, string friend)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("NamePlayer", nameuser);
        sendInfo.AddField("Friend", friend);

        IsCHFriend = true;
        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlAddFriend.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            IsCHFriend = false;
            StopCoroutine("AddPlayerFriend");
        }

        string FriendsStats = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (FriendsStats.Length > 0)
            if (FriendsStats.Substring(0, 2) == "OK")
            {
                Debug.Log("Friend Add Updated");
            }
            else if (FriendsStats.Length > 4)
            {
                if (FriendsStats.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(FriendsStats.Substring(6));
                    Application.Quit();
                }

            }
        IsCHFriend = false;
        StopCoroutine("AddPlayerFriend");
    }

    IEnumerator AddPlayerFriendR(string nameuser, string newfriend)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("NamePlayer", nameuser);
        sendInfo.AddField("FriendR", newfriend);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlAddFriendR.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("AddPlayerFriendR");
        }

        string FriendsStats = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (FriendsStats.Length > 0)
            if (FriendsStats.Substring(0, 2) == "OK")
            {
                Debug.Log("Friend Request Add Updated");
            }
            else if (FriendsStats.Length > 4)
            {
                if (FriendsStats.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(FriendsStats.Substring(6));
                    Application.Quit();
                }

            }
        StopCoroutine("AddPlayerFriendR");
    }

    IEnumerator DeletePlayerFriend(string nameuser, string friend)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("NamePlayer", nameuser);
        sendInfo.AddField("Friend", friend);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlDeleteFriend.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("DeletePlayerFriend");
        }

        string FriendsStats = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (FriendsStats.Length > 0)
            if (FriendsStats.Substring(0, 2) == "OK")
            {
                Debug.Log("Friend Delete Updated");
            }
            else if (FriendsStats.Length > 4)
            {
                if (FriendsStats.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(FriendsStats.Substring(6));
                    Application.Quit();
                }

            }
        //Borramos usuario de lista de amigos para el otro
        //Formulario
        WWWForm sendInfo2 = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("NamePlayer", friend);
        sendInfo.AddField("Friend", nameuser);

        //Request
        UnityWebRequest getData2 = UnityWebRequest.Post(BaseUrl + "/sqlDeleteFriend.php", sendInfo2);
        yield return getData2; //esperamos
        if (!string.IsNullOrEmpty(getData2.error) && getData2.error.Contains("Could not resolve host"))
        {
            Debug.Log(getData.error);
            StopCoroutine("DeletePlayerFriend");
        }

        StopCoroutine("DeletePlayerFriend");
    }

    IEnumerator DeletePlayerFriendR(string nameuser, string friend, bool Accepted)
    {
        //Formulario
        IsCHFriend = true;
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("NamePlayer", nameuser);
        sendInfo.AddField("FriendR", friend);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlDeleteFriendR.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            IsCHFriend = false;
            StopCoroutine("DeletePlayerFriendR");
        }

        string FriendsStats = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (FriendsStats.Length > 0)
        {
            if (FriendsStats.Substring(0, 2) == "OK")
            {
                Debug.Log("Friend Delete Updated");
                scr_StatsPlayer.NewFriends.Remove(friend);
                if (Accepted)
                {
                    scr_StatsPlayer.Friends.Add(friend);
                    StartCoroutine(AddPlayerFriend(scr_StatsPlayer.Name, friend));
                }
                else
                {
                    IsCHFriend = false;
                }
            }
            else if (FriendsStats.Length > 4)
            {
                if (FriendsStats.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(FriendsStats.Substring(6));
                    Application.Quit();
                }
                IsCHFriend = false;
            }
        } else
        {
            IsCHFriend = false;
        }

        StopCoroutine("DeletePlayerFriendR");
    }

    IEnumerator GetHistoryMatchs(int idaccount, int idfriend)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("Idplayer", idaccount);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlGetHistoryMatchs.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("GetHistoryMatchs");
        }

        string CanGetData = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (CanGetData.Length>0)
        if (CanGetData.Substring(0, 2) == "OK")
        {
            //CARGAMOS Partidas
            string str_history = CanGetData.Substring(2);
            if (str_history.Length > 1)
            {
                string[] str_matchs = str_history.Split('♦');

                string[] HDecks = new string[str_matchs.Length-1];

                //Debug.Log(str_matchs.Length);

                for (int i = 0; i < str_matchs.Length - 1; i++)
                {
                    scr_DataMatch match = new scr_DataMatch();
                    string[] str_match = str_matchs[i].Split('■');
                    match.Id = int.Parse(str_match[0]);
                    match.Name1 = str_match[1].Substring(0,str_match[1].IndexOf(':'));
                    match.Name2 = str_match[2].Substring(0,str_match[2].IndexOf(':'));
                    match.Deck1 = str_match[1].Substring(str_match[1].IndexOf(':') + 1);
                    match.Deck2 = str_match[2].Substring(str_match[2].IndexOf(':') + 1);
                    match.Xp1 = int.Parse(str_match[3]);
                    match.Xp2 = int.Parse(str_match[4]);
                    match.Winer = int.Parse(str_match[5]);
                    match.Date = str_match[6];

                    if (match.Winer==0)
                    {
                        match.WinerName = scr_Lang.GetText("txt_game_info12");
                    } else if (match.Winer==1)
                    {
                        match.WinerName = match.Name1;
                    } else
                    {
                        match.WinerName = match.Name2;
                    }

                    string namecompare = scr_StatsPlayer.Name;
                    if (idfriend >= 0)
                    {
                        namecompare = scr_StatsPlayer.FriendsData[idfriend].Name;
                    }

                    if (namecompare == match.Name1)
                    {
                        HDecks[i] = match.Deck1;
                    } else
                    {
                        HDecks[i] = match.Deck2;
                    }

                    //Debug.Log(match.Id.ToString() + "/" + match.Name1 + "/" + match.Name2 + "/" + match.Deck1 + "/" + match.Deck2 + "/" + match.Winer.ToString() + "/" + match.Date);

                    if (idfriend >= 0)
                    {
                        scr_StatsPlayer.FriendsData[idfriend].HistoryMatchs.Add(match);
                    } else
                    {
                        scr_StatsPlayer.HistoryMatchs.Add(match);
                    }
                    
                }

                if (idfriend >= 0)
                {
                    scr_StatsPlayer.FriendsData[idfriend].FavoriteDeck = HDecks.GroupBy(i => i)  //Grouping same items
                    .OrderByDescending(g => g.Count()) //now getting frequency of a value
                    .First() //selecting key of the group
                    .Key;   //Finally, taking the most frequent value
                }
                else
                {
                    scr_StatsPlayer.FavoriteDeck = HDecks.GroupBy(i => i)  //Grouping same items
                    .OrderByDescending(g => g.Count()) //now getting frequency of a value
                    .First() //selecting key of the group
                    .Key;   //Finally, taking the most frequent value
                }
            }

        }
        else
        {
            if (CanGetData.Substring(0, 2) == "NO")
            {
                Debug.Log("No History Found");
            }

        }

        StopCoroutine("GetHistoryMatchs");
    }

    IEnumerator SetClan(int idaccount, string Clan)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Clan", Clan);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetClan.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetClan");
        }

        string StatsClan = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsClan.Length>0)
        if (StatsClan.Substring(0, 2) == "OK")
        {
            Debug.Log("Clan updated");
        }
        else if (StatsClan.Length > 4)
        {
            if (StatsClan.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsClan.Substring(6));
                Application.Quit();
            }
        }
        StopCoroutine("SetClan");
    }

    IEnumerator SetMatch(int id1, int id2, string team1, string team2, int Xp1, int Xp2, int winer, string date)
    {
        //Formulario 
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("Team1", team1);
        sendInfo.AddField("Team2", team2);
        sendInfo.AddField("Xp1", Xp1);
        sendInfo.AddField("Xp2", Xp2);
        sendInfo.AddField("Id1", id1);
        sendInfo.AddField("Id2", id2);
        sendInfo.AddField("Winer", winer);
        sendInfo.AddField("Date", date); //'0000-00-00 00:00:00'

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetMatch.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetMatch");
        }

        string StatsMatch = getData.downloadHandler.text;

        Debug.Log(StatsMatch);

        //Checamos si fue exitoso
        if (StatsMatch.Length > 0)
        if (StatsMatch.Substring(0, 2) == "OK")
        {
            Debug.Log("Match upload");
        }
        else if (StatsMatch.Length > 4)
        {
            if (StatsMatch.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsMatch.Substring(6));
                Application.Quit();
            }
        }
        StopCoroutine("SetMatch");
    }

    IEnumerator SetStatsGamesLeague(int idaccount, int victorys, int defeats, int draws)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Vic", victorys);
        sendInfo.AddField("Def", defeats);
        sendInfo.AddField("Dra", draws);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetGamesLeague.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetStatsGamesLeague");
        }

        string StatsLeague = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsLeague.Length > 0)
            if (StatsLeague.Substring(0, 2) == "OK")
            {
                Debug.Log("Stats League updated");
            }
            else if (StatsLeague.Length > 4)
            {
                if (StatsLeague.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsLeague.Substring(6));
                    Application.Quit();
                }
            }
        StopCoroutine("SetStatsGamesLeague");
    }

    IEnumerator SetStatsGamesIA(int idaccount, int victorys, int defeats, int draws)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Vic", victorys);
        sendInfo.AddField("Def", defeats);
        sendInfo.AddField("Dra", draws);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetGamesIA.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetStatsGamesIA");
        }

        string StatsIA = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsIA.Length > 0)
            if (StatsIA.Substring(0, 2) == "OK")
            {
                Debug.Log("Stats Ia updated");
            }
            else if (StatsIA.Length > 4)
            {
                if (StatsIA.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsIA.Substring(6));
                    Application.Quit();
                }
            }
        StopCoroutine("SetStatsGamesIA");
    }
    /*
    IEnumerator SetStatsGamesHI(int idaccount, int BestH, int BestI)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("BestH", BestH);
        sendInfo.AddField("BestI", BestI);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetGamesHI.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetStatsGamesHI");
        }

        string StatsHI = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsHI.Length > 0)
            if (StatsHI.Substring(0, 2) == "OK")
            {
                Debug.Log("Stats HI updated");
            }
            else if (StatsHI.Length > 4)
            {
                if (StatsHI.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsHI.Substring(6));
                    Application.Quit();
                }
            }
        StopCoroutine("SetStatsGamesHI");
    }
    */
    IEnumerator SetOrbes(int idaccount)
    {
        string Orbs = "";
        for (int i=0; i<scr_StatsPlayer.Orbes.Length; i++)
        {
            if (i > 0) { Orbs += "|"; }
            Orbs += scr_StatsPlayer.Orbes[i].ToString();
        }

        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Orbes", Orbs);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetOrbes.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetOrbes");
        }

        string StatsOrbes = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsOrbes.Length>0)
        if (StatsOrbes.Substring(0, 2) == "OK")
        {
            Debug.Log("Orbes updated");
        }
        else if (StatsOrbes.Length > 4)
        {
            if (StatsOrbes.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsOrbes.Substring(6));
                Application.Quit();
            }
                Debug.LogWarning(StatsOrbes);
        }
        StopCoroutine("SetOrbes");
    }

    IEnumerator BuyOrbes(int NewMoney)
    {
        string Orbs = "";
        for (int i = 0; i < scr_StatsPlayer.Orbes.Length; i++)
        {
            if (i > 0) { Orbs += "|"; }
            Orbs += scr_StatsPlayer.Orbes[i].ToString();
        }

        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", scr_StatsPlayer.id);
        sendInfo.AddField("Orbes", Orbs);
        sendInfo.AddField("NewMoney", NewMoney);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlBuyOrbes.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("BuyOrbes");
        }

        string StatsOrbes = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsOrbes.Length > 0)
            if (StatsOrbes.Substring(0, 2) == "OK")
            {
                scr_Store.WaitForDBResponse = true;
                Debug.Log("Orbes updated");
            }
            else if (StatsOrbes.Length > 4)
            {
                if (StatsOrbes.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsOrbes.Substring(6));
                    Application.Quit();
                }
                Debug.LogWarning(StatsOrbes);
            }
    }

    IEnumerator SetAllOrbes(int idaccount, bool WR)
    {
        string Orbs = "";
        for (int i = 0; i < scr_StatsPlayer.Orbes.Length; i++)
        {
            if (i > 0) { Orbs += "|"; }
            Orbs += scr_StatsPlayer.Orbes[i].ToString();
        }

        string SOrbs = "";
        for (int i = 0; i < scr_StatsPlayer.OpeningOrbs.Length; i++)
        {
            if (i > 0) { SOrbs += "|"; }
            SOrbs += scr_StatsPlayer.OpeningOrbs[i].Stars.ToString() + "?" + scr_StatsPlayer.OpeningOrbs[i].Type.ToString();
        }

        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Orbes", Orbs);
        sendInfo.AddField("StarsOrbes", SOrbs);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetAllOrbs.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetAllOrbes");
        }

        string StatsOrbes = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsOrbes.Length > 0)
            if (StatsOrbes.Substring(0, 2) == "OK")
            {
                if (WR)
                    scr_Store.WaitForDBResponse = true;
                Debug.Log("All Orbes updated");
            }
            else if (StatsOrbes.Length > 4)
            {
                if (StatsOrbes.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsOrbes.Substring(6));
                    Application.Quit();
                }
                Debug.LogWarning(StatsOrbes);
            }
        StopCoroutine("SetAllOrbes");
    }

    IEnumerator SetStarsOrbs(int idaccount, bool WR)
    {
        string Orbs = "";
        for (int i = 0; i < scr_StatsPlayer.OpeningOrbs.Length; i++)
        {
            if (i > 0) { Orbs += "|"; }
            Orbs += scr_StatsPlayer.OpeningOrbs[i].Stars.ToString()+"?"+ scr_StatsPlayer.OpeningOrbs[i].Type.ToString();
        }

        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Orbes", Orbs);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetStarsOrbs.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetStarsOrbs");
        }
        string StatsOrbes = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsOrbes.Length > 0)
            if (StatsOrbes.Substring(0, 2) == "OK")
            {
                if (WR)
                    scr_Store.WaitForDBResponse = true;
                Debug.Log("Stars Orbes updated");
            }
            else if (StatsOrbes.Length > 4)
            {
                if (StatsOrbes.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsOrbes.Substring(6));
                    Application.Quit();
                }
                Debug.Log(StatsOrbes);
            }
        StopCoroutine("SetStarsOrbs");
    }

    IEnumerator SetXpBuff(int idaccount)
    {

        string xpbuff = scr_StatsPlayer.XpBuff.ToString() + "|" + scr_StatsPlayer.XpBuffExpire.ToString("yyyy-MM-dd HH:mm:ss");

        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("XpBuff", xpbuff);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetXpBuff.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetXpBuff");
        }

        string StatsBuff = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsBuff.Length > 0)
            if (StatsBuff.Substring(0, 2) == "OK")
            {
                Debug.Log("Xp Buff updated");
            }
            else if (StatsBuff.Length > 4)
            {
                if (StatsBuff.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsBuff.Substring(6));
                    Application.Quit();
                }
            }
        StopCoroutine("SetXpBuff");
    }

    IEnumerator BuyXpBuff(int NewMoney)
    {

        string xpbuff = scr_StatsPlayer.XpBuff.ToString() + "|" + scr_StatsPlayer.XpBuffExpire.ToString("yyyy-MM-dd HH:mm:ss");

        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", scr_StatsPlayer.id);
        sendInfo.AddField("XpBuff", xpbuff);
        sendInfo.AddField("NewMoney", NewMoney);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlBuyXpBuff.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("BuyXpBuff");
        }

        string StatsBuff = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsBuff.Length > 0)
            if (StatsBuff.Substring(0, 2) == "OK")
            {
                scr_Store.WaitForDBResponse = true;
                Debug.Log("Xp Buff updated");
            }
            else if (StatsBuff.Length > 4)
            {
                if (StatsBuff.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsBuff.Substring(6));
                    Application.Quit();
                }
            }
    }

    IEnumerator SetNeutrinos(int idaccount, int value)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Neutrinos", value);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetNeutrinos.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetNeutrinos");
        }

        string StatsSetNeutrinos = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsSetNeutrinos.Length>0)
        if (StatsSetNeutrinos.Substring(0, 2) == "OK")
        {
            Debug.Log("Neutrinos updated");
        }
        else if (StatsSetNeutrinos.Length > 4)
        {
            if (StatsSetNeutrinos.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsSetNeutrinos.Substring(6));
                Application.Quit();
            }
        }
        StopCoroutine("SetNeutrinos");
    }

    IEnumerator SetRange(int idaccount, int Rango, int Lvl, int points)
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", idaccount);
        sendInfo.AddField("Rango", Rango);
        sendInfo.AddField("Level", Lvl);
        sendInfo.AddField("Points", points);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlSetRange.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("SetRange");
        }

        string StatsRange = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsRange.Length>0)
        if (StatsRange.Substring(0, 2) == "OK")
        {
            Debug.Log("Player Range updated");
        }
        else if (StatsRange.Length > 4)
        {
            if (StatsRange.Substring(0, 5) == "ERROR")
            {
                Debug.LogWarning(StatsRange.Substring(6));
                Application.Quit();
            }
        }
        StopCoroutine("SetRange");
    }

    IEnumerator CheckExistUser(string user)
    {
        ExistUser = 0;
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("NamePlayer", user);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlExistUser.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("CheckExistUser");
        }

        string StatsUser = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsUser.Length > 0)
        if (StatsUser.Substring(0, 2) == "OK")
        {
            ExistUser = 1;
            Debug.Log("User found");
        }
        else if (StatsUser.Length > 4)
        {
            if (StatsUser.Substring(0, 5) == "ERROR")
            {
                ExistUser = -1;
                Debug.Log("User not found");
            }
        }
        StopCoroutine("CheckExistUser");
    }

    IEnumerator GetDataFriends(string user)
    {
        //OPTENEMOS NUEVAS SOLICITUDES DE AMISTAD
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("NamePlayer", user);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlGetFR.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("GetDataFriends");
        }

        string StatsFriends = getData.downloadHandler.text;

        //Checamos si fue exitoso
        if (StatsFriends.Length > 0)
            if (StatsFriends.Substring(0, 2) == "OK")
            {
                NewRequest = StatsFriends.Substring(2);
            }
            else if (StatsFriends.Length > 4)
            {
                if (StatsFriends.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsFriends);
                }
            }

        //OBTENEMOS NUEVAS SOLICITUDES ACEPTADAS
        //Request
        UnityWebRequest getDataf = UnityWebRequest.Post(BaseUrl + "/sqlGetFN.php", sendInfo);

        getDataf.downloadHandler = new DownloadHandlerBuffer();

        yield return getDataf.SendWebRequest(); //esperamos

        if (getDataf.isNetworkError || getDataf.isHttpError)
        {
            Debug.Log(getDataf.error);
            StopCoroutine("GetDataFriends");
        }

        StatsFriends = getDataf.downloadHandler.text;
        
        //Checamos si fue exitoso
        if (StatsFriends.Length > 0)
            if (StatsFriends.Substring(0, 2) == "OK" && StatsFriends.Length > 2)
            {
                NewAcepted = StatsFriends.Substring(2);
            }
            else if (StatsFriends.Length > 4)
            {
                if (StatsFriends.Substring(0, 5) == "ERROR")
                {
                    Debug.LogWarning(StatsFriends);
                }
            }
        StopCoroutine("GetDataFriends");
    }

   IEnumerator IELogout()
    {
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("IdAC", scr_StatsPlayer.id);
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlLogOut.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest();

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("IELogout");
        }

        string canLogOut = getData.downloadHandler.text;

        if (canLogOut == "OK")
        {
            Scr_Database.isLoggedIn = false;
            scr_StatsPlayer.ClearAll();
            PhotonNetwork.Disconnect();
            ChatNewGui _Chat = FindObjectOfType<ChatNewGui>();
            if (_Chat != null)
            {
                _Chat.Disconnect();
                Destroy(_Chat.gameObject);
            }
            SceneManager.LoadScene(0);
            Debug.Log("Log Out OK");
        }
        else
        {
            Debug.Log("Error in Server: " + canLogOut);
        }
        LogOutInProcess = false;
    }

    IEnumerator CheckEmailExist(string email)
    {
        ExistEmail = -1;
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("Email", email);
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlExistEmail.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest();

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            ExistEmail = 2;
            StopCoroutine("CheckEmailExist");
        }

        string DatEmail = getData.downloadHandler.text;

        //Debug.Log(DatEmail);
        
        if (DatEmail.Substring(0,2) == "OK")
        {
            ExistEmail = 1;
            Debug.Log("Email Exist");
        }
        else
        {
            ExistEmail = 0;
            Debug.Log("Email no Exist");
        }
        
    }

    

   IEnumerator GetInfoUser(string nameuser, bool addfrienddata)
    {
        IsCHDataF = true;
        bool Restart = false;
        //Formulario
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("dw", Scr_Database.db_dm);

        sendInfo.AddField("NamePlayer", nameuser);

        //Request
        UnityWebRequest getData = UnityWebRequest.Post(BaseUrl + "/sqlGetInfoPlayer.php", sendInfo);

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("GetInfoUser");
        }

        string CanGetData = getData.downloadHandler.text;

        Debug.Log(CanGetData);

        //Checamos si fue exitoso
        if (CanGetData.Length>0)
        if (CanGetData.Substring(0, 2) == "OK")
        {
            UserData = new scr_BDUser();
            for (int i = 0; i < 3; i++)
            {
                UserData.MyDeck[i] = new List<string>();
            }

            UserData.Name = nameuser;

            string[] STATS;
            STATS = CanGetData.Split('•');

            //Inicializamos variables de stats
            UserData.idUser = int.Parse(STATS[1]);
            UserData.Level = int.Parse(STATS[2]);
            UserData.Range = int.Parse(STATS[3]);
            UserData.LRange = int.Parse(STATS[4]);
            UserData.IdCurrentTitle = int.Parse(STATS[5]);
            UserData.Xp = int.Parse(STATS[6]);
            UserData.RankPoints = int.Parse(STATS[7]);
            UserData.IdClan = int.Parse(STATS[8]);
            UserData.IDAvatar = int.Parse(STATS[9]);
            UserData.Region = STATS[10];
            string[] _np = STATS[11].Split('-');
            string[] _npia = STATS[12].Split('-');
            UserData.idAccount = int.Parse(STATS[13]);
            UserData.Email = STATS[14];
            UserData.Deck = STATS[15];
            string str_allUnits = STATS[16];
            /*
            for(int i=1; i<STATS.Length;i++)
                Debug.Log(STATS[i]);
              */
            //Data Games IA
            UserData.VIa = int.Parse(_npia[0]);
            UserData.LIa = int.Parse(_npia[1]);
            UserData.DIa = int.Parse(_npia[2]);
            //Data Games League
            UserData.VLeague = int.Parse(_np[0]);
            UserData.LLeague = int.Parse(_np[1]);
            UserData.DLeague = int.Parse(_np[2]);


            //CARGAMOS UNIDADES

            if (str_allUnits.Length > 1)
            {
                string[] str_units = str_allUnits.Split('♦');
                for (int i = 0; i < str_units.Length - 1; i++)
                {
                    scr_UnitProgress new_unit = new scr_UnitProgress();
                    string[] str_unitStats = str_units[i].Split('■');
                    new_unit.Name = str_unitStats[0];
                    new_unit.Id = int.Parse(str_unitStats[1]);
                    new_unit.Level = int.Parse(str_unitStats[2]);
                    int rarity = 0;
                    string rs = scr_GetStats.GetPropUnit(new_unit.Name, "Rarity");
                    int.TryParse(rs, out rarity);
                    new_unit.Rarity = rarity;
                    UserData.MyUnits.Add(new_unit);
                    //UserData.MyUnitsSkins.Add(int.Parse(str_unitStats[4]));
                }
            }

            string str_deck = UserData.Deck;
            int idc;
            string[] str_decks = str_deck.Split('?');
            for (int j = 0; j < 3; j++)
            {
                idc = j;
                string[] str_deckunits = str_decks[j].Split('-');
                if (str_decks.Length > 1)
                    for (int i = 0; i < str_deckunits.Length; i++)
                    {
                        if (i == 0)
                        {
                            UserData.Name_deck[j] = str_deckunits[i];
                        }
                        else
                        {
                            UserData.MyDeck[idc].Add(str_deckunits[i]);
                        }

                    }
            }
            if (addfrienddata)
            {
                scr_BDUser newdata = UserData;
                scr_StatsPlayer.FriendsData.Add(newdata);
                scr_StatsPlayer.IndexFriend++;

                if (scr_StatsPlayer.IndexFriend<scr_StatsPlayer.Friends.Count)
                {
                    Restart = true;
                    f_GetInfoUser(scr_StatsPlayer.Friends[scr_StatsPlayer.IndexFriend], true);
                }
            }

        }
        else
        {
            if (CanGetData.Substring(0, 2) == "NO")
            {
                Debug.Log(CanGetData.Substring(6));
            }
            else if (CanGetData.Length>4)
            {
                if (CanGetData.Substring(0, 5) == "ERROR")
                    Debug.Log("No Exist Acount");
            }

        }
        if (!Restart)
           IsCHDataF = false;
        StopCoroutine("GetInfoUser");
    }
}

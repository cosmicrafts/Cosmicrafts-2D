using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class scr_UIPlayerEditor : Photon.MonoBehaviour {


    public GameObject M_MainMenu;
    public GameObject M_Collection;
    public GameObject M_Store;
    public GameObject M_PlayModes;
    public GameObject B_Back;
    public GameObject M_Options;
    public GameObject M_Credits;

    public GameObject Tutorial;

    public Button Btn_ChangeAvatar;
    public GameObject AvatarsUI;
    public GameObject LevelUp;
    public Text InfoLevelUp;
    public Text InfoLUPname;
    public Image InfoLUPAvatar;
    public GameObject OpenOrbe;
    public Animator Cofre;
    public Text RemOrbs;
    public Button ButtonNextOrb;
    public Button ButtonCloseOrbs;
    public GameObject NoMoreOrbs;
    public scr_RewardsOrbs[] RewardsOrbs = new scr_RewardsOrbs[6];
    public InputField AddFriend;
    public Text AddFriendTitle;
    public GameObject ChatObject;
    public GameObject NoConnection;
    public GameObject MatchsRequests;
    public GameObject RefusedGame;
    public GameObject WaithForFriend;
    public GameObject Loading;
    public scr_Store Store;
    public Image StatusColor;
    public GameObject[] Reward = new GameObject[4];
    public Text[] RewardAmount = new Text[4];
    public GameObject RangeNot;
    public Text NewRange;
    public GameObject Promotion;
    public GameObject Downgrade;

    [HideInInspector]
    public Color[] LeaguesColors = new Color[9] { Color.black, new Color(0.6f, 0.6f, 0.6f), Color.white, new Color(0.6f, 1f, 0.6f), new Color(0.4f, 0.8f, 1f), new Color(0.8f, 0.4f, 1f), new Color(1f, 0.6f, 0.4f), Color.red, Color.yellow };
    int[] AmountsForRewards = new int[4]{0,0,0,0};

    AsyncOperation ProgresLoadLevel;

    public List<string> MRequests = new List<string>();
    public Sprite[] sp_avatars = new Sprite[7];

    public Text Neutrinos;
    public Text Orbs;

    public Text Pr_Name;
    public Text Pr_Lvl;

    public Text GameTip;

    [HideInInspector]
    public bool OkOpenOrbe = false;
    [HideInInspector]
    public bool SearchFriend = false;
    string NewFriend = "";

    [HideInInspector]
    public int cto = 0; //Current type orbs
    public Text[] Orbs_Types = new Text[6];

    public AudioSource LoBox;

    public scr_UIProfile UIProfile;
    public scr_UIDeckEditor UIDeck;
    public GameObject AfterBattleMetrics;
    public GameObject MissionCompleteAlert;

    public int DateDay = 0;
    public int DateMonth = 0;
    public int DateYear = 0;

    public GameObject AchivNot;
    public Text AchivNameNot;

    [HideInInspector]
    public List<string> NewCardsToDeck = new List<string>();
    [HideInInspector]
    public List<string> NewSkinsToAdd = new List<string>();
    [HideInInspector]
    public List<int> AchivNotList = new List<int>();
    [HideInInspector]
    public int CurrentAchivNotif = -1;

    [HideInInspector]
    public string NewsItemsData = "";

    [HideInInspector]
    public string StoreUrl = "http://cosmicrafts.com/StoreApp/";

    ChatNewGui chat;

    WaitForSeconds Delay05s = new WaitForSeconds(0.5f);

    Stack<GameObject> Menus = new Stack<GameObject>();

    GameObject CurrentMenu;

    void Start()
    {
        CurrentMenu = M_MainMenu;
        Menus.Push(M_MainMenu);
        scr_StatsPlayer.LastDateConection = System.DateTime.Now;
        scr_BDUpdate.f_UpdateLastConnection(scr_StatsPlayer.id, scr_StatsPlayer.LastDateConection);

        UpdateInfoFriends();
        UpdateHistoryMatchs();

        UpdateStatsUI();
        UpdateAvatarImage();
        UpdateCoins();
        if (!scr_Music.AudioBegin && scr_StatsPlayer.Op_Music)
        {
            scr_Music.AudioBegin = true;
            scr_Music.as_audio.Play();
        }

        PhotonNetwork.player.CustomProperties.Remove("Deck");
        PhotonNetwork.player.CustomProperties.Add("Deck", scr_StatsPlayer.SpecificDeckToString(scr_StatsPlayer.idc));

        chat = FindObjectOfType<ChatNewGui>();
        if (chat == null)
        {
            chat = Instantiate(ChatObject).GetComponent<ChatNewGui>();
        } else
        {
            ChangeStatePlayer(scr_StatsPlayer.LastState);
            chat.Chat.SetActive(false);
        }

        StartCoroutine(DownloadStoreNews());

        if (scr_StatsPlayer.Afterbattle)
        {
            scr_StatsPlayer.Tutorial = false;
            scr_StatsPlayer.Practice = false;
            //Missions
            scr_Missions.CheckExpire();
            scr_Missions.CheckProgressComplete();

            if ((!scr_Missions.Claim_Day && scr_Missions.Day_Complete) || (!scr_Missions.Claim_Week && scr_Missions.Week_Complete))
            {
                //Success Mission
                MissionCompleteAlert.SetActive(true);
            }
            AfterBattleMetrics.SetActive(true);
        }

        if (scr_StatsPlayer.FirstConnection)
        {
            scr_StatsPlayer.FirstConnection = false;
            Tutorial.transform.GetChild(0).gameObject.SetActive(true);
        }

        if (scr_StatsPlayer.FirstBattle)
        {
            scr_StatsPlayer.FirstBattle = false;
            Tutorial.transform.GetChild(1).gameObject.SetActive(true);
        }

    }

    public void OpenSection(GameObject Section)
    {
        CurrentMenu.SetActive(false);
        Section.SetActive(true);
        CurrentMenu = Section;
        Menus.Push(Section);

        B_Back.SetActive(true);
    }

    public void GoBack()
    {
        CurrentMenu = Menus.Pop();
        CurrentMenu.SetActive(false);
        CurrentMenu = Menus.Peek();
        CurrentMenu.SetActive(true);

        if (Menus.Count==1)
        {
            B_Back.SetActive(false);
        }
    }

    public void ChangeStatePlayer(int state)
    {
        if (chat == null)
            return;

        chat.ChangeStatePlayer(state);
        StatusColor.color = scr_StatsPlayer.GetColorStatusUser(scr_StatsPlayer.CurrentState);
    }

    public void RefInvitation()
    {
        WaithForFriend.SetActive(false);
        RefusedGame.SetActive(true);
        GetComponent<mach_friend>().CancelMatch();
    }

    public void UpdateInfoFriends()
    {
        if (scr_StatsPlayer.Friends.Count > 0)
        {
            scr_StatsPlayer.FriendsData.Clear();
            scr_StatsPlayer.IndexFriend = 0;
            scr_BDUpdate.f_GetInfoUser(scr_StatsPlayer.Friends[0], true);
        }
    }

    public void UpdateHistoryMatchs()
    {
        scr_StatsPlayer.HistoryMatchs.Clear();
        scr_BDUpdate.f_GetHistoryMatchs(scr_StatsPlayer.id, -1);
    }

    public void CheckStartNotifications()
    {
        if (scr_StatsPlayer.Offline)
        {
            ShowWarningNoConnection();
            return;
        }
        //Stars Orbs
        for (int i=0; i<scr_StatsPlayer.OpeningOrbs.Length; i++)
            scr_StatsPlayer.OpeningOrbs[i].Stars += scr_MNGame.BattleMetrics.Quarks_Win;

        scr_BDUpdate.f_SetStarsOrbs(scr_StatsPlayer.id, false);

        //LEVEL UP !
        if (scr_StatsPlayer.b_LevelUp && LevelUp != null)
        {
            LevelUp.SetActive(true);
            scr_StatsPlayer.b_LevelUp = false;
            InfoLevelUp.text = scr_StatsPlayer.Level.ToString();
            InfoLUPname.text = scr_StatsPlayer.Name;
            InfoLUPAvatar.sprite = scr_StatsPlayer.SAvatar;
            int new_lvl= scr_StatsPlayer.Level;
            if (new_lvl<=25)
            {
                AmountsForRewards[0] = 8 * scr_StatsPlayer.New_Levels;
                AmountsForRewards[1] = 4 * scr_StatsPlayer.New_Levels;
                Reward[2].SetActive(false);
                Reward[3].SetActive(false);
            }
            else if (new_lvl <= 50)
            {
                AmountsForRewards[0] = 16 * scr_StatsPlayer.New_Levels;
                AmountsForRewards[1] = 4 * scr_StatsPlayer.New_Levels;
                AmountsForRewards[2] = 1 * scr_StatsPlayer.New_Levels;
                Reward[3].SetActive(false);
            } else if (new_lvl <= 100)
            {
                AmountsForRewards[0] = 32 * scr_StatsPlayer.New_Levels;
                AmountsForRewards[1] = 8 * scr_StatsPlayer.New_Levels;
                AmountsForRewards[2] = 2 * scr_StatsPlayer.New_Levels;
                Reward[3].SetActive(false);
            } else if (new_lvl <= 250)
            {
                AmountsForRewards[0] = 64 * scr_StatsPlayer.New_Levels;
                AmountsForRewards[1] = 16 * scr_StatsPlayer.New_Levels;
                AmountsForRewards[2] = 4 * scr_StatsPlayer.New_Levels;
                AmountsForRewards[3] = 1 * scr_StatsPlayer.New_Levels;
            } else
            {
                AmountsForRewards[0] = 128 * scr_StatsPlayer.New_Levels;
                AmountsForRewards[1] = 32 * scr_StatsPlayer.New_Levels;
                AmountsForRewards[2] = 8 * scr_StatsPlayer.New_Levels;
                AmountsForRewards[3] = 2 * scr_StatsPlayer.New_Levels;
            }
            scr_StatsPlayer.New_Levels = 0;
            for (int i=0; i<4; i++)
            {
                RewardAmount[i].text = "x"+AmountsForRewards[i].ToString();
            }
        }
        //PROMOTION!
        if (scr_StatsPlayer.Promotion)
        {
            RangeNot.SetActive(true);
            NewRange.text = scr_Lang.GetText("txt_league_" + scr_StatsPlayer.Range.ToString()) + " " + scr_StatsPlayer.LRange.ToString();
            NewRange.color = LeaguesColors[scr_StatsPlayer.Range];
            Promotion.SetActive(true);
            Downgrade.SetActive(false);
            scr_StatsPlayer.Promotion = false;
        }
        //DOWNGRADE
        if (scr_StatsPlayer.Downgrade)
        {
            RangeNot.SetActive(true);
            NewRange.text = scr_Lang.GetText("txt_league_" + scr_StatsPlayer.Range.ToString()) + " " + scr_StatsPlayer.LRange.ToString();
            NewRange.color = LeaguesColors[scr_StatsPlayer.Range];
            Promotion.SetActive(false);
            Downgrade.SetActive(true);
            scr_StatsPlayer.Promotion = false;
        }
        //Revisamos si hay buffs de experiencia y si expiraron
        if (scr_StatsPlayer.XpBuff > 1f)
        {
            if (System.DateTime.Compare(System.DateTime.Now, scr_StatsPlayer.XpBuffExpire) > 0)//expiro
            {
                scr_StatsPlayer.XpBuff = 1f;
                scr_BDUpdate.f_SetXpBuff(scr_StatsPlayer.id);
            }
        }
        //Subimos de nivel progreso jugador
        scr_BDUpdate.f_SetLvlXpPlayer(scr_StatsPlayer.id, scr_StatsPlayer.Level, scr_StatsPlayer.Xp);
        //Agregamos avanze de logros
        for (int i = 0; i < scr_StatsPlayer.MyAchiv.Count; i++)
        {
            int new_levels = scr_StatsPlayer.MyAchiv[i].MoveProgressBattle();
            if (new_levels > 0)
            {
                AchivNotList.Add(scr_StatsPlayer.MyAchiv[i].Id);
            }
        }
        scr_BDUpdate.f_UpdateAchievements(scr_StatsPlayer.id);
        if (AchivNotList.Count > 0)
            ShowNotAchiv(false);

        UpdateCoins();
        scr_StatsPlayer.Afterbattle = false;
    }

    void Update()
    {
        if (SearchFriend)
        {
            if (scr_BDUpdate.ExistUser == 1)
            {
                SearchFriend = false;
                AddFriendTitle.text = scr_Lang.GetText("txt_mn_opts16");
                AddFriend.Select();
                AddFriend.text = "";
                scr_BDUpdate.f_AddFriendR(NewFriend, scr_StatsPlayer.Name);
                scr_BDUpdate.ExistUser = 0;
            } else if (scr_BDUpdate.ExistUser == -1) {
                SearchFriend = false;
                AddFriendTitle.text = scr_Lang.GetText("txt_mn_opts18");
                scr_BDUpdate.ExistUser = 0;
            }
        }

        if (!MatchsRequests.activeSelf && MRequests.Count>0)
        {
            MatchsRequests.SetActive(true);
            GetComponent<mach_friend>().StatusMatch.text = MRequests[0] + scr_Lang.GetText("txt_mn_info56"); ;
            GetComponent<mach_friend>().btn_Accept.interactable = true;
        }
    }

    public void AddProgressAchivInEditor(int id, int progress)
    {
        if (id >= scr_StatsPlayer.MyAchiv.Count)
            return;

        scr_StatsPlayer.MyAchiv[id].AddProgressBattle(progress);
        int new_levels = scr_StatsPlayer.MyAchiv[id].MoveProgressBattle();
        if (new_levels > 0)
        {
            AchivNotList.Add(scr_StatsPlayer.MyAchiv[id].Id);
            if (!AchivNot.activeSelf)
                ShowNotAchiv(false);
        }

        scr_BDUpdate.f_UpdateAchievements(scr_StatsPlayer.id);
        UpdateCoins();
    }

    public void AddRewardByNewLvl(int rarity)
    {
        List<scr_UnitProgress> selu = new List<scr_UnitProgress>();
        //Posibles unidades ya obtenidas
        for (int i=0; i<scr_StatsPlayer.MyUnits.Count; i++)
        {
            if (scr_StatsPlayer.MyUnits[i].Rarity == rarity)
                selu.Add(scr_StatsPlayer.MyUnits[i]);
        }

        //Posibles unidades nuevas
        List<string> newselu = new List<string>();
        for(int i=0; i<scr_StatsPlayer.UnitsNotAv.Count; i++)
        {
            string _rarity = scr_GetStats.GetPropUnit(scr_StatsPlayer.UnitsNotAv[i], "Rarity");
            if (_rarity== rarity.ToString())
            {
                newselu.Add(scr_StatsPlayer.UnitsNotAv[i]);
            }
        }

        //Unidades obtenidas y nuevas seleccionadas para agragarse o aumentarse
        List<scr_UnitProgress> edited_units = new List<scr_UnitProgress>();
        List<string> newtoadd = new List<string>();
        if (selu.Count>0)
        {
            for (int i = 0; i < AmountsForRewards[rarity]; i++)
            {
                if (Random.Range(0,2)==0)
                {
                    string _unitnew = newselu[Random.Range(0, newselu.Count)];
                    if (!newtoadd.Contains(_unitnew))
                    {
                        newtoadd.Add(_unitnew);
                        continue;
                    }
                }
                scr_UnitProgress _unit = selu[Random.Range(0, selu.Count)];
                _unit.Reinf++;
                if (!edited_units.Contains(_unit))
                    edited_units.Add(_unit);
            }
        }

        string units_list = "";
        for (int i = 0; i < newtoadd.Count; i++)
        {
            if (i != 0)
                units_list += "|";
            units_list += newtoadd[i];
        }
        scr_BDUpdate.f_AddNewPackUnits(scr_StatsPlayer.id,units_list);

        scr_BDUpdate.f_UpdateListUnitsProgress(edited_units);
        LevelUp.SetActive(false);
    }

    public void ShowNotAchiv(bool claim_all)
    {
        if (AchivNotList.Count==0)
        {
            CurrentAchivNotif = -1;
            AchivNot.SetActive(false);
            return;
        }

        if (claim_all)
        {
            CurrentAchivNotif = -1;
            AchivNotList.Clear();
            AchivNot.SetActive(false);
            return;
        }

        AchivNot.SetActive(true);
        CurrentAchivNotif = AchivNotList[0];
        scr_Achievements achiv = scr_StatsPlayer.MyAchiv.Find(a => a.Id == CurrentAchivNotif);
        AchivNameNot.text = scr_Lang.GetTitleName(achiv.IdTitle) + "(" + achiv.Level + "/" + achiv.MaxLevel + ")" ;
        AchivNotList.RemoveAt(0);
    }

    public void CheckOrbs(int type)
    {
        cto = type;
        if (scr_StatsPlayer.Orbes[cto] > 0 && !OkOpenOrbe && !OpenOrbe.activeSelf)
        {
            for (int j = 0; j < RewardsOrbs.Length; j++)
                RewardsOrbs[j].gameObject.SetActive(false);
            Cofre.Play("Entrada_Cofre");
            OpenOrbe.SetActive(true);
            ButtonCloseOrbs.gameObject.SetActive(true);
            ButtonNextOrb.gameObject.SetActive(false);
            LoBox.Play();
            if (scr_StatsPlayer.Op_Music)
                scr_Music.as_audio.Pause();
            if (scr_StatsPlayer.Orbes[cto] > 1)
            {
                RemOrbs.gameObject.SetActive(true);
                RemOrbs.text = scr_Lang.GetText("txt_mn_info83")+" "+ (scr_StatsPlayer.Orbes[cto]).ToString();
            }
            else
            {
                RemOrbs.gameObject.SetActive(false);
            }
        }
    }

    public void OpenStarsOrb(int type)
    {
        cto = type;
        if (!OkOpenOrbe && !OpenOrbe.activeSelf)
        {
            for (int j = 0; j < RewardsOrbs.Length; j++)
                RewardsOrbs[j].gameObject.SetActive(false);
            Cofre.Play("Entrada_Cofre");
            OpenOrbe.SetActive(true);
            RemOrbs.gameObject.SetActive(false);
            ButtonCloseOrbs.gameObject.SetActive(false);
            ButtonNextOrb.gameObject.SetActive(false);
            LoBox.Play();
            if (scr_StatsPlayer.Op_Music)
                scr_Music.as_audio.Pause();
        }
    }

    public void f_OpenOrbe()
    {
        if (!OkOpenOrbe)
        {
            if (scr_StatsPlayer.Offline)
            {
                ShowWarningNoConnection();
                return;
            }

            OkOpenOrbe = true;

            if (cto >= scr_StatsPlayer.Orbes.Length)
            {
                //Funcion de recompenzas
                StartCoroutine(GetOrbRewards());
            } else
            {
                scr_StatsPlayer.Orbes[cto]--;
                scr_BDUpdate.f_SetOrbes(scr_StatsPlayer.id);
                if (scr_StatsPlayer.Orbes[cto] > 0)
                    ButtonNextOrb.gameObject.SetActive(true);
                else
                    ButtonNextOrb.gameObject.SetActive(false);
                //Funcion de recompenzas
                StartCoroutine(GetOrbRewards());

                if (scr_StatsPlayer.Orbes[cto] > 1)
                {
                    RemOrbs.gameObject.SetActive(true);
                    RemOrbs.text = scr_Lang.GetText("txt_mn_info83") + " " + (scr_StatsPlayer.Orbes[cto]).ToString();
                }
                else
                {
                    RemOrbs.gameObject.SetActive(false);
                }
            }
            ButtonCloseOrbs.gameObject.SetActive(true);
            UpdateCoins();
        }
    }

    public void f_CloseOrbe()
    {
        OkOpenOrbe = false;
        LoBox.Stop();
        if (scr_StatsPlayer.Op_Music)
            scr_Music.as_audio.UnPause();
        OpenOrbe.SetActive(false);
        UpdateCoins();
    }

    public void f_NextOrbe()
    {
        Cofre.Play("Entrada_Cofre");
        //Ocultamos los iconos
        for (int j = 0; j < RewardsOrbs.Length; j++)
            RewardsOrbs[j].gameObject.SetActive(false);
        OkOpenOrbe = false;
        ButtonNextOrb.gameObject.SetActive(false);
    }

    IEnumerator GetOrbRewards()
    {
        ButtonNextOrb.interactable = false;
        ButtonCloseOrbs.interactable = false;
        int r_neutrinos = 0;
        int[] r_ref = new int[4] { 0,0,0,0};
        int r_skins = 0;

        List<int> Raritys = new List<int>();

        Raritys.Add(0);
        //Sorteamos la cantidad de premios para cada clase de orbe
        yield return Delay05s;
        switch (cto)
        {
            case 0://Neutrinos
                {
                    r_neutrinos = Random.Range(25, 75);
                    r_ref[0] = Random.Range(8, 32);
                    if (Random.Range(0, 1f) <= 0.75f) { r_ref[1] = Random.Range(4, 16); Raritys.Add(1); }
                    if (Random.Range(0, 1f) <= 0.5f) { r_ref[2] = Random.Range(2, 8); Raritys.Add(2); }
                    if (Random.Range(0, 1f) <= 0.25f) { r_ref[3] = Random.Range(1, 2); Raritys.Add(3); }
                }
                break;
            case 1://Magnetic
                {
                    Raritys.Add(1);
                    r_neutrinos = Random.Range(50, 125);
                    r_ref[0] = Random.Range(16, 48);
                    r_ref[1] = Random.Range(4, 16);
                    if (Random.Range(0, 1f) <= 0.75f) { r_ref[2] = Random.Range(2, 8); Raritys.Add(2); }
                    if (Random.Range(0, 1f) <= 0.5f) { r_ref[3] = Random.Range(1, 2); Raritys.Add(3); }
                }
                break;
            case 2://Antimatter
                {
                    r_neutrinos = Random.Range(50, 125);
                    r_ref[0] = Random.Range(16, 48);
                    r_ref[1] = Random.Range(4, 16);
                    Raritys.Add(1);
                    if (Random.Range(0, 1f) <= 0.75f) { r_ref[2] = Random.Range(2, 8); Raritys.Add(2); }
                    if (Random.Range(0, 1f) <= 0.5f) { r_ref[3] = Random.Range(1, 2); Raritys.Add(3); }
                    if (Random.Range(0, 1f) <= 0.10f) { r_skins = 1; }
                }
                break;
            case 3://Hadron
                {
                    r_neutrinos = Random.Range(10, 50);
                    r_ref[0] = Random.Range(4, 16);
                    if (Random.Range(0, 1f) <= 0.75f) { r_ref[1] = Random.Range(2, 8); Raritys.Add(1); }
                    if (Random.Range(0, 1f) <= 0.5f) { r_ref[2] = Random.Range(1, 4); Raritys.Add(2); }
                    if (Random.Range(0, 1f) <= 0.25f) { r_ref[3] = 1; Raritys.Add(3); }
                }
                break;

        }
        //Mostramos los iconos de los refuerzos ganados
        for (int j = 0; j < r_ref.Length; j++)
        {
            if (r_ref[j] > 0)
            {
                RewardsOrbs[j].gameObject.SetActive(true);
                RewardsOrbs[j].Amount.text = "x" + r_ref[j].ToString();
                yield return Delay05s;
            }
        }
        //Seleccionamos las naves candidatas para recivir refuerzos
        List<scr_UnitProgress> selu = new List<scr_UnitProgress>();
        for (int i = 0; i < scr_StatsPlayer.MyUnits.Count; i++)
        {
            if (Raritys.Contains(scr_StatsPlayer.MyUnits[i].Rarity))
                selu.Add(scr_StatsPlayer.MyUnits[i]);
        }
        //Repartimos los refuerzos ganados en las naves elejidas
        if (selu.Count > 0)
        {
            for (int i = 0; i < selu.Count; i++)
            {
                if (r_ref[selu[i].Rarity] <= 0)
                    continue;
                int amount = Random.Range(0, r_ref[selu[i].Rarity]+1);
                selu[i].Reinf+= amount;
                r_ref[selu[i].Rarity] -= amount;
            }
            for (int i = 0; i < selu.Count; i++)
            {
                if (r_ref[selu[i].Rarity] <= 0)
                    continue;
                selu[i].Reinf += r_ref[selu[i].Rarity];
            }
        }
        scr_BDUpdate.f_UpdateListUnitsProgress(selu);
        //Aumentamos los neutrinos ganados
        if (r_neutrinos>0)
        {
            scr_StatsPlayer.Neutrinos += r_neutrinos;
            scr_BDUpdate.f_SetNeutrinos(scr_StatsPlayer.id, scr_StatsPlayer.Neutrinos);
        }
        //Sorteamos y agregamos las skins ganadas
        for (int j=0; j< r_skins; j++)
        {
            bool ok_skin = false;
            int _start_unit = 0;
            int id_random_unit = _start_unit = Random.Range(0, scr_StatsPlayer.MyUnits.Count);
            string _skin = "";
            scr_UnitProgress _ru = null;

            while (!ok_skin)
            {
                _ru = scr_StatsPlayer.MyUnits[id_random_unit];
                string[] all_skins = scr_GetStats.GetSkins(_ru.Name);
                int id_skin = 1;
                _skin = all_skins[id_skin];
                string new_skin = _ru.Name + ":" + _skin;
                ok_skin = true;
                while (scr_StatsPlayer.MySkins.Contains(new_skin))
                {
                    id_skin++;
                    if (id_skin >= all_skins.Length) { ok_skin = false; break; }
                    new_skin = _ru.Name + ":" + all_skins[id_skin];
                }

                if (!ok_skin)
                {
                    id_random_unit++;
                    if (id_random_unit == _start_unit) { break; }
                    if (id_random_unit>= scr_StatsPlayer.MyUnits.Count)
                    {
                        id_random_unit = 0;
                    }
                }
            }
            if (!ok_skin)
                continue;

            scr_BDUpdate.f_AddSkins(_ru, _skin);
        }
        //MOstramos los neutrinos ganados
        RewardsOrbs[5].gameObject.SetActive(true);
        RewardsOrbs[5].Amount.text = "x"+r_neutrinos.ToString();

        yield return Delay05s;
        //Mostramos las skins ganadas
        if (r_skins>0)
        {
            RewardsOrbs[4].gameObject.SetActive(true);
            RewardsOrbs[4].Amount.text = "x" + r_skins.ToString();
        }
        //Actualizamos las monedas
        UpdateCoins();
        ButtonNextOrb.interactable = true;
        ButtonCloseOrbs.interactable = true;
    }

    void UpdateStatsUI()
    {
        Pr_Lvl.text = scr_StatsPlayer.Level.ToString();
        Pr_Name.text = scr_StatsPlayer.Name;

    }

    void UpdateAvatarImage()
    {
        scr_StatsPlayer.SAvatar = sp_avatars[scr_StatsPlayer.IDAvatar];
        Btn_ChangeAvatar.GetComponent<Image>().sprite = scr_StatsPlayer.SAvatar;
    }

    public void ChangeAvatar(int _id)
    {
        scr_StatsPlayer.ChangeAvatar(_id);
        AvatarsUI.SetActive(false);
        UpdateAvatarImage();
    }

    public void OpenAvatarSelection()
    {
        if (AvatarsUI.activeSelf)
        {
            AvatarsUI.SetActive(false);
        } else
        {
            AvatarsUI.SetActive(true);
        }
    }

    public void UpdateCoins()
    {
        int total_orbs = 0;
        for (int i = 0; i < scr_StatsPlayer.Orbes.Length; i++)
            total_orbs += scr_StatsPlayer.Orbes[i];
        Neutrinos.text = scr_StatsPlayer.Neutrinos.ToString();
        Orbs.text = scr_Lang.GetText("txt_mn_info57") + " [" + total_orbs.ToString() + "]";
        if (total_orbs > 0)
        {
            NoMoreOrbs.SetActive(false);
            for (int i = 0; i < scr_StatsPlayer.Orbes.Length; i++)
            {
                if (scr_StatsPlayer.Orbes[i] == 0)
                {
                    Orbs_Types[i].gameObject.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    Orbs_Types[i].gameObject.transform.parent.gameObject.SetActive(true);
                    Orbs_Types[i].text = scr_StatsPlayer.Orbes[i].ToString();
                }
            }
        }
        else
        {
            for (int i = 0; i < Orbs_Types.Length; i++)
            {
                Orbs_Types[i].gameObject.transform.parent.gameObject.SetActive(false);
            }
            NoMoreOrbs.SetActive(true);
        }
    }

    public void LoadIAMode()
    {
        Loading.SetActive(true);
        ProgresLoadLevel = SceneManager.LoadSceneAsync("Mapa_vsIA");
        ProgresLoadLevel.allowSceneActivation = true;
        int id_tip = Random.Range(1, 11);
        GameTip.text = scr_Lang.GetText("txt_game_tips_" + id_tip.ToString());
    }
    public void LoadTutorialMode()
    {
        scr_StatsPlayer.Tutorial = true;
        Loading.SetActive(true);
        ProgresLoadLevel = SceneManager.LoadSceneAsync("Mapa_vsIA");
        ProgresLoadLevel.allowSceneActivation = true;
        int id_tip = Random.Range(1, 11);
        GameTip.text = scr_Lang.GetText("txt_game_tips_" + id_tip.ToString());
    }

    public void LoadPracticeMode()
    {
        scr_StatsPlayer.Practice = true;
        Loading.SetActive(true);
        ProgresLoadLevel = SceneManager.LoadSceneAsync("Mapa_vsIA");
        ProgresLoadLevel.allowSceneActivation = true;
        int id_tip = Random.Range(1, 11);
        GameTip.text = scr_Lang.GetText("txt_game_tips_" + id_tip.ToString());
    }

    public void SearchAndAddFriend()
    {
        if (SearchFriend)
        {
            return;
        }

        if (scr_StatsPlayer.Friends.Contains(AddFriend.text) || AddFriend.text == scr_StatsPlayer.Name || scr_StatsPlayer.NewFriends.Contains(AddFriend.text))
        {
            AddFriendTitle.text = scr_Lang.GetText("txt_mn_opts17");
            return;
        }
        NewFriend = AddFriend.text;;
        scr_BDUpdate.f_CheckExistUser(NewFriend);
        AddFriendTitle.text = scr_Lang.GetText("txt_mn_opts15");
        SearchFriend = true;
    }

    public void ShowStatsPlayer()
    {
        UIProfile.gameObject.SetActive(true);
        UIProfile.User = null;
        UIProfile.UpdateProfileUIInfo();
    }

    public void ShowWarningNoConnection()
    {
        NoConnection.SetActive(true);
    }

    public void UpgradeUnitReinf(int index)
    {
        if (scr_StatsPlayer.Offline)
        {
            ShowWarningNoConnection();
            Store.ConfirmPay.SetActive(false);
            return;
        }

        int rarity = 0;
        int.TryParse(scr_GetStats.GetPropUnit(scr_StatsPlayer.MyUnits[index].Name, "Rarity"), out rarity);
        int nextlevel = scr_StatsPlayer.GetNextReinfLevel(scr_StatsPlayer.MyUnits[index].Level, rarity);

        scr_StatsPlayer.MyUnits[index].Reinf -= nextlevel;
        scr_StatsPlayer.MyUnits[index].Level++;
    
        scr_BDUpdate.f_UpdateUnit(scr_StatsPlayer.MyUnits[index]);
        UpdateCoins();
    }

    public void GoToSuuport()
    {
        Application.OpenURL("http://cosmicrafts.com");
    }

    IEnumerator DownloadStoreNews()
    {
        //Request
        UnityWebRequest getData = UnityWebRequest.Get(StoreUrl + "StartPage.php");

        getData.downloadHandler = new DownloadHandlerBuffer();

        yield return getData.SendWebRequest(); //esperamos

        if (getData.isNetworkError || getData.isHttpError)
        {
            Debug.Log(getData.error);
            StopCoroutine("DownloadStoreNews");
        }

        NewsItemsData = getData.downloadHandler.text;
        StopCoroutine("SetRange");
    }

    private void OnApplicationQuit()
    {
        scr_StatsPlayer.LastDateConection = System.DateTime.Now;
        scr_BDUpdate.f_UpdateLastConnection(scr_StatsPlayer.id, scr_StatsPlayer.LastDateConection);
    }
}

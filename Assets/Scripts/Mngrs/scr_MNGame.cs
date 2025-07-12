using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class scr_MNGame : Photon.MonoBehaviour {

    public static scr_MNGame GM;

    [HideInInspector]
    public bool b_DragUnit = false;

    [HideInInspector]
    public int UnitsInGame;
    [HideInInspector]
    public int MaxUnitsInGame;

    int Time_Min = 5;
    int Time_Sec = 0;

    [HideInInspector]
    public scr_CardsCntrl CntrlCards;

    [HideInInspector]
    public m_player MyPlayer;
    [HideInInspector]
    public int IDinGame = 0;
    [HideInInspector]
    public int TeamInGame = 0;

    public scr_Unit Station0;
    public scr_Unit Station1;

    public scr_Unit[] Inividores = new scr_Unit[4];
    public scr_Unit[] Satelites = new scr_Unit[4];
    public scr_Unit[] Torretas = new scr_Unit[10];

    public GameObject BaseUnit;
    public GameObject BaseSkill;

    public scr_IA IA;

    [HideInInspector]
    public bool IsHorda = false;

    //Practice Variables
    [HideInInspector]
    public bool InmortalEnnemys = false;
    [HideInInspector]
    public bool InmortalAllied = false;
    [HideInInspector]
    public bool InivitorsActive = true;
    [HideInInspector]
    public bool InfiniteEnergy = false;
    [HideInInspector]
    public bool FreezeDeck = false;
    [HideInInspector]
    public bool FreezeUnits = false;
    [HideInInspector]
    public bool FreeSpawn = false;
    [HideInInspector]
    public bool SpawnAsEnemy = false;
    [HideInInspector]
    public bool DeleteOnClick = false;

    //Game Status
    [HideInInspector]
    public bool b_RepeatCard = false;
    public bool b_InNetwork = true;
    [HideInInspector]
    public bool b_EndGame = false;
    [HideInInspector]
    public bool TERMINARPARTIDA = false;
    [HideInInspector]
    public bool UserEndGame = false;
    bool LostOtherPLayer = false;
    bool StartDisconect = false;

    //OFF LINE
    [HideInInspector]
    public float f_Resources = 3f;
    [HideInInspector]
    public float f_MaxResources = 10f;

    [HideInInspector]
    public float f_CamShakeTime = 0f;
    [HideInInspector]
    public float f_CamShakeIntensity = 0f;

    public Image EnergyBar;
    public Text txt_ShipsInGame;
    public Text txt_Resources;
    public Text txt_Time;
    public Text fpsText;

    float deltaTime;

    Animator Command_Sign;

    [HideInInspector]
    public scr_camera BattleCamera;

    [HideInInspector]
    public GameObject OtherPLayerOutCanvas;
    
    public GameObject Screens;
    public GameObject Alerts;
    public GameObject ConnectionDelay;

    public bool DebugMode = false;

    WaitForSeconds Delay1s = new WaitForSeconds(1f);
    WaitForSeconds Delay1Ms = new WaitForSeconds(0.1f);
    //WaitForFixedUpdate DelayUpdate = new WaitForFixedUpdate();

    public List<string> DeckForThisGame = new List<string>();

    [HideInInspector]
    public List<scr_Unit> Selected;
    [HideInInspector]
    public Color UnSelected = new Color(0f, 0f, 0f, 0f);

    bool Special_Command = false;

    [HideInInspector]
    public bool In_Ships_Selection = false;

    [HideInInspector]
    public scr_Unit Drag_Unit = null;
    [HideInInspector]
    public scr_Unit Cursor_on_enemy = null;


    public static scr_BattleMetrics BattleMetrics;

    // Use this for initialization
    void Awake ()
    {
        GM = this;

#if UNITY_EDITOR
        if (File.Exists("Assets/Resources/DebugLogs.txt"))
        {
            File.WriteAllText("Assets/Resources/DebugLogs.txt", "");
        }
#endif

    }

    void Start()
    {
        BattleMetrics = new scr_BattleMetrics();
        Selected = new List<scr_Unit>();
        Command_Sign = Instantiate(scr_Resources.Command_Icon, transform).GetComponent<Animator>();

        if (b_InNetwork)
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.Instantiate("MG_Lobby", Vector3.zero, Quaternion.identity, 0);
            }
            PhotonNetwork.Instantiate("UI", Vector3.zero, Quaternion.identity, 0);
        }

        BattleCamera = FindObjectOfType<scr_camera>();

        scr_StatsPlayer.FriendsData.Clear();

        if (scr_Music.AudioBegin)
        {
            scr_Music.AudioBegin = false;
            scr_Music.as_audio.Stop();
        }

        if (!scr_StatsPlayer.Op_Music)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().volume = 0;
        }

        UserEndGame = false;
        b_RepeatCard = false;
        b_DragUnit = false;

        UnitsInGame = 0;
        MaxUnitsInGame = 50;

        Time_Min = 5;
        Time_Sec = 0;

        if (!b_InNetwork)
        {
            IDinGame = 0;
        }

        ChatNewGui chat = FindObjectOfType<ChatNewGui>();
        if (chat != null)
        {
            chat.ChangeStatePlayer(6);
            chat.Chat.SetActive(false);
        }

        if (!b_InNetwork && !m_quickManager.InBotRoom)
        {
            if (IA && !scr_StatsPlayer.Tutorial && !scr_StatsPlayer.Practice)
               StartCoroutine(TimeCount());

            if (scr_StatsPlayer.Tutorial || scr_StatsPlayer.Practice) { txt_Time.gameObject.SetActive(false); }

            StartCoroutine(StartBaseStations());
        }

    }
	
    public void StartGame()
    {
        StartCoroutine(StartBaseStations());
    }
	// Update is called once per frame
	void Update () {

        if (f_CamShakeTime > 0f)
        {
            f_CamShakeTime -= Time.deltaTime;
            f_CamShakeIntensity -= Time.deltaTime;
        }
        /*
        if (Input.GetMouseButtonDown(0))
            StartSelection();*/

        Commands();
        UpdateUIShips();
        ShowFps();
    }

    public scr_StatsUnit GetStats(string id_unit)
    {
        if (CntrlCards)
        {
            if (CntrlCards.UnitsToUse.ContainsKey(id_unit))
                return CntrlCards.UnitsToUse[id_unit];
            else
                return LoadStats(id_unit);
        } else
        {
            return LoadStats(id_unit);
        }
    }

    public scr_StatsUnit LoadStats(string id_unit)
    {
        scr_StatsUnit Result = new scr_StatsUnit();

        float.TryParse(scr_GetStats.GetPropUnit(id_unit, "CastDelay"), out Result.CastDelay);
        float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Duration"), out Result.Duration);
        float.TryParse(scr_GetStats.GetPropUnit(id_unit, "RangeView"), out Result.Range_View);

        if (id_unit.Contains("Skill"))
        {
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Power"), out Result.Power);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Boost"), out Result.Boost);
        } else
        {
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Hp"), out Result.Hp);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Armor"), out Result.Armor);
            bool.TryParse(scr_GetStats.GetPropUnit(id_unit, "IsInmune"), out Result.IsInmune);
            bool.TryParse(scr_GetStats.GetPropUnit(id_unit, "Physics"), out Result.Kinematic);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "SpawnArea"), out Result.SpawnRange);
            int.TryParse(scr_GetStats.GetPropUnit(id_unit, "Poblation"), out Result.PoblationReq);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Size"), out Result.Size);
            //Passives
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Energize"), out Result.Energize);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "SpawnUnitTime"), out Result.TimeGen);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "HpRegen"), out Result.HpRegen);
            int.TryParse(scr_GetStats.GetPropUnit(id_unit, "AfterShockDMG"), out Result.AfterShock);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "AfterShockRange"), out Result.AfterShokRange);
            int.TryParse(scr_GetStats.GetPropUnit(id_unit, "Banck"), out Result.Banck);
            int.TryParse(scr_GetStats.GetPropUnit(id_unit, "ChargeDMG"), out Result.Charge);
            Result.GenUnit = scr_GetStats.GetPropUnit(id_unit, "SpawnUnit");
        }

        float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Speed"), out Result.Speed);
        //Move
        if (Result.Speed > 0f || Result.Charge > 0f)
        {
            Result.CanMove = true;
        }
        
        //Attack
        float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Attack"), out Result.Attack_Dmg);
        if (Result.Attack_Dmg > 0f) {
            Result.CanShoot = true;
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "RangeAtk"), out Result.Range_Attack);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "SpeedAttack"), out Result.Attack_Speed);
            int.TryParse(scr_GetStats.GetPropUnit(id_unit, "Ncannons"), out Result.Cannons);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "BulletSpeed"), out Result.BulletSpeed);
            int.TryParse(scr_GetStats.GetPropUnit(id_unit, "BulletType"), out Result.BulletType);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "BulletSize"), out Result.BulletSize);
            bool.TryParse(scr_GetStats.GetPropUnit(id_unit, "SyncAnimAngle"), out Result.SyncAngleShoot);
            //Pasives
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Critical"), out Result.Critical);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "Vampiric"), out Result.Vampiric);
            int.TryParse(scr_GetStats.GetPropUnit(id_unit, "BerserkMaxDMG"), out Result.Berserk);
            int.TryParse(scr_GetStats.GetPropUnit(id_unit, "Overheating"), out Result.Overheating);
            bool.TryParse(scr_GetStats.GetPropUnit(id_unit, "DirectShoot"), out Result.DirectShoot);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "DirectShootRadio"), out Result.DirectShootSize);
            float.TryParse(scr_GetStats.GetPropUnit(id_unit, "AttackRadio"), out Result.Area_Dmg);
        }

        return Result;
    }

    public void SetVisibleSpawnsAreas(bool _visible)
    {
        scr_SpawnArea[] AllSpawns = FindObjectsOfType<scr_SpawnArea>();
        for (int i = 0; i < AllSpawns.Length; i++)
            AllSpawns[i].EnableSpawnArea(_visible);
    }

    IEnumerator StartBaseStations()
    {
        if (CntrlCards)
        {
            yield return new WaitUntil(() => CntrlCards.RedyCards);
        }

        yield return Delay1Ms;

        Station0.gameObject.SetActive(true);
        Station1.gameObject.SetActive(true);
        yield return Delay1Ms;
        for (int i=0; i<Inividores.Length/2; i++)
        {
            Inividores[i].gameObject.SetActive(true);
            yield return Delay1Ms;
            Inividores[(Inividores.Length / 2)+i].gameObject.SetActive(true);
            yield return Delay1Ms;
        }
        for (int i = 0; i < Satelites.Length / 2; i++)
        {
            Satelites[i].gameObject.SetActive(true);
            yield return Delay1Ms;
            Satelites[(Satelites.Length / 2)+i].gameObject.SetActive(true);
            yield return Delay1Ms;
        }
        for (int i = 0; i < Torretas.Length / 2; i++)
        {
            Torretas[i].gameObject.SetActive(true);
            yield return Delay1Ms;
            Torretas[(Torretas.Length / 2)+i].gameObject.SetActive(true);
            yield return Delay1Ms;
        }
    }

    void ShowFps()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }

    public void DebugAndWriteToFile(string log)
    {
        /*
#if UNITY_EDITOR
        Debug.Log(log);
        try
        {
            StreamWriter fileWrite = new StreamWriter(PathLogFile, true);
            fileWrite.WriteLine(log);
            fileWrite.Close();
        }
        catch (System.Exception e)
        {
            Debug.Log("Error Write File Logs:"+e.Message);
        }
#endif
        */
    }

    public void ShakeCamera(float intensity, float time)
    {
        if (DebugMode)
            return;

        f_CamShakeIntensity = intensity;
        f_CamShakeTime = time;

        BattleCamera.StopAllCoroutines();
        BattleCamera.StartCoroutine("Shake");
    }

    public scr_Explo CreateExplo(Vector3 _pos, int _size)
    {
        scr_Explo exp = Instantiate(scr_Resources.Explo, _pos, Quaternion.identity).GetComponent<scr_Explo>();
        exp.size = _size;
        return exp;
    }

    public void AddMetricsProgressAchivs()
    {
        AddXpInBattle(Mathf.CeilToInt(BattleMetrics.all_dmg * 0.1f));
        scr_StatsPlayer.AddProgressBattleAchiv(8, BattleMetrics.all_dmg);
        scr_StatsPlayer.AddProgressBattleAchiv(10, BattleMetrics.all_charges);
        scr_StatsPlayer.AddProgressBattleAchiv(6, BattleMetrics.Kills_By_Skill);
        scr_StatsPlayer.AddProgressBattleAchiv(4, BattleMetrics.Ships_Kills);
        scr_StatsPlayer.AddProgressBattleAchiv(5, BattleMetrics.Stations_Kills);
        scr_StatsPlayer.AddProgressBattleAchiv(6, BattleMetrics.Kills_By_Skill);
        scr_StatsPlayer.AddProgressBattleAchiv(7, BattleMetrics.Gen_Units);

        scr_Missions.AddProgress(0, BattleMetrics.Ships_Kills + BattleMetrics.Stations_Kills);
        scr_Missions.AddProgress(1, BattleMetrics.Ships_Spawn + BattleMetrics.Stations_Kills);
        scr_Missions.AddProgress(2, BattleMetrics.Stations_Spawn + BattleMetrics.Stations_Kills);
        scr_Missions.AddProgress(3, BattleMetrics.Skills_Spawn + BattleMetrics.Stations_Kills);

        if (b_InNetwork)
            scr_Missions.AddProgress(4, 1);
    }
    /*
    public void StartSelection()
    {
        if (CntrlCards)
        {
            if (CntrlCards.CursorOnCards)
                return;
        }

        if (Drag_Drop_Mode)
            return;

        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(scr_Resources.AreaSelect, mouse, Quaternion.identity);
    }
    */
    public void SetTargetSelected(scr_Unit _target)
    {
        if (In_Ships_Selection)
            return;

        if (Selected.Count>0)
        {
            CreateCommandSign(1, _target.transform);
            Special_Command = true;
        }

        for (int i = 0; i < Selected.Count; i++)
            Selected[i].SetNewTarget(_target, true);
    }

    public void SetDestinationSelected(Vector2 _dest, bool is_objective)
    {
        for (int i = 0; i < Selected.Count; i++)
        {
            if (Selected[i].NS.CanMove)
            {
                Selected[i].MyMove.SetNewDestination(_dest, is_objective);
            }                
        }
    }

    public void DefenseSelected()
    {
        if (Selected.Count > 0)
        {
            CreateCommandSign(2, Selected[0].transform);
            Special_Command = true;
        }

        for (int i = 0; i < Selected.Count; i++)
        {
            if (Selected[i].NS.CanMove)
            {
                if (Selected[i].MyMove.Stops_Counter >= 3)
                    continue;

                Selected[i].MyMove.SetNewDestination(Selected[i].transform.position, false);
                Selected[i].MyMove.Delay_Position = 10f;
                Selected[i].MyMove.Stops_Counter++;
            }
        }

        CanUseCommands(false);
    }

    public void SetStopSelected()
    {
        for (int i = 0; i < Selected.Count; i++)
        {
            if (Selected[i].NS.CanMove)
                Selected[i].MyMove.StopShip();
        }
    }

    public void ClearSelected()
    {
        for (int i = 0; i < Selected.Count; i++)
        {
            Selected[i].MyUI.Image_Selection.color = UnSelected;
            Selected[i].isSelected = false;
        }
        Selected.Clear();
    }

    public void AddSelection(scr_Unit _add)
    {
        if (_add.FromSquad != "none" && _add.transform.parent!=null)
        {
            Transform parent = _add.transform.parent;
            for (int i=0; i< parent.childCount; i++)
            {
                scr_Unit _unit = parent.GetChild(i).GetComponent<scr_Unit>();
                SelectUnit(_unit);
            }
        } else
        {
            SelectUnit(_add);
        }
    }

    public void SelectUnit(scr_Unit _unit)
    {
        if (Selected.Contains(_unit) || !_unit.NS.CanMove)
            return;

        Selected.Add(_unit);
        _unit.MyUI.Image_Selection.color = Color.yellow;
        _unit.isSelected = true;
        _unit.RedySelect = false;
    }

    public void DeselectUnit(scr_Unit _delete)
    {
        if (Selected.Contains(_delete))
        {
            Selected.Remove(_delete);
            _delete.MyUI.Image_Selection.color = UnSelected;
            _delete.isSelected = false;
        }

        CanUseCommands(false);
    }

    public void RemoveSelection(scr_Unit _delete)
    {
        if (_delete.FromSquad != "none" && _delete.transform.parent != null)
        {
            Transform parent = _delete.transform.parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                scr_Unit _unit = parent.GetChild(i).GetComponent<scr_Unit>();
                DeselectUnit(_unit);
            }
        }
        else
        {
            DeselectUnit(_delete);
        }
    }

    public bool CanUseCommands(bool on_if_true)
    {
        bool OneControl = false;
        for (int i = 0; i < Selected.Count; i++)
        {
            if (Selected[i].MyMove.Stops_Counter < 3)
            {
                OneControl = true;
                break;
            }
        }

        return OneControl;
    }

    public void CreateCommandSign(int comamnd, Transform _paret)
    {
        if (!Command_Sign)
            return;

        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (_paret != null)
            position = _paret.position;

        Command_Sign.Play("Start");
        Command_Sign.SetInteger("Acction", comamnd);
        Command_Sign.transform.position = position;
        Command_Sign.transform.SetParent(_paret);
    }

    public void Commands()
    {
        if (CntrlCards)
        {
            if (CntrlCards.CursorOnCards)
                return;
        }

        if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) && !In_Ships_Selection) //Move Units
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            for (int i=0; i<Selected.Count; i++)
            {
                if (!Selected[i].RedySelect)
                    Selected[i].RedySelect = true;
                else if (Vector2.Distance(transform.position, mouse) > 1f)
                {
                    if (i == 0 && !Special_Command)
                        CreateCommandSign(0, null);
                    if (Selected[i].delay_defend) { Selected[i].delay_defend = false; } else
                    {
                        Selected[i].MyMove.SetNewDestination(mouse, false);
                        Selected[i].MyMove.Delay_Position = 0f;
                    } 
                }
            }
            Special_Command = false;
        }
    }

    public void AddXpInBattle(int xp)
    {
        BattleMetrics.XP_Win += xp;
    }

    public void CheckWiner()
    {
        StartCoroutine(EndGame());
    }

    public void EndTutorial()
    {
        GetComponent<AudioSource>().Stop();
        scr_Unit[] AllUnits = FindObjectsOfType<scr_Unit>();
        int destroyed = 1;
        for (int i = 0; i < AllUnits.Length; i++)
        {
            if (AllUnits[i].i_Team == destroyed)
                AllUnits[i].DestroyUnit();
            else
                AllUnits[i].Victory();
        }
        Screens.transform.GetChild(0).gameObject.SetActive(true); //Victoria
    }

    IEnumerator EndGame()
    {

        int destroyed = -1;
        int winer = 0;

        AddMetricsProgressAchivs();

        yield return Delay1s;
        GetComponent<AudioSource>().Stop();
        if (Station0 == null)
        {
            destroyed = 0;
            winer = 2;
        }
        else if (Station1 == null)
        {
            destroyed = 1;
            winer = 1;
        }

        scr_Unit[] AllUnits = FindObjectsOfType<scr_Unit>();
        for (int i = 0; i < AllUnits.Length; i++)
        {
            if (AllUnits[i].i_Team == destroyed)
                AllUnits[i].DestroyUnit();
            else
                AllUnits[i].Victory();
        }

        if (destroyed == -1) //enpate
        {
            Screens.transform.GetChild(2).gameObject.SetActive(true);
            if (m_quickManager.InBotRoom || b_InNetwork)
            {
                BattleMetrics.Quarks_Win += (BattleMetrics.XP_Win / 100);
                BattleMetrics.Quantums += 20;
                scr_StatsPlayer.DLeague++;
                scr_BDUpdate.f_SetStatsGamesLeague(scr_StatsPlayer.id, scr_StatsPlayer.VLeague, scr_StatsPlayer.LLeague, scr_StatsPlayer.DLeague);
            }
            else
            {
                BattleMetrics.Quantums += 10;
                scr_StatsPlayer.DIa++;
                scr_BDUpdate.f_SetStatsGamesIA(scr_StatsPlayer.id, scr_StatsPlayer.VIa, scr_StatsPlayer.LIa, scr_StatsPlayer.DIa);
            }
            scr_StatsPlayer.Neutrinos += BattleMetrics.Quantums;
            scr_BDUpdate.f_SetNeutrinos(scr_StatsPlayer.id, scr_StatsPlayer.Neutrinos);
        }
        else
        {
            if (destroyed == TeamInGame)
            {
                Screens.transform.GetChild(1).gameObject.SetActive(true); //Derrota
                BattleMetrics.I_Win = false;
                if (m_quickManager.InBotRoom || b_InNetwork)
                {
                    BattleMetrics.Quarks_Win += (BattleMetrics.XP_Win / 100);
                    BattleMetrics.Quantums += 10;
                    scr_StatsPlayer.LLeague++;
                    scr_BDUpdate.f_SetStatsGamesLeague(scr_StatsPlayer.id, scr_StatsPlayer.VLeague, scr_StatsPlayer.LLeague, scr_StatsPlayer.DLeague);
                }
                else
                {
                    BattleMetrics.Quantums += 5;
                    scr_StatsPlayer.LIa++;
                    scr_BDUpdate.f_SetStatsGamesIA(scr_StatsPlayer.id, scr_StatsPlayer.VIa, scr_StatsPlayer.LIa, scr_StatsPlayer.DIa);
                }
                scr_StatsPlayer.Neutrinos += BattleMetrics.Quantums;
                scr_BDUpdate.f_SetNeutrinos(scr_StatsPlayer.id, scr_StatsPlayer.Neutrinos);
            }
            else
            {
                Screens.transform.GetChild(0).gameObject.SetActive(true); //Victoria
                BattleMetrics.I_Win = true;
                if (b_InNetwork)
                    scr_StatsPlayer.AddProgressBattleAchiv(1, 1);
                if (m_quickManager.InBotRoom || b_InNetwork)
                {
                    BattleMetrics.Quarks_Win += (BattleMetrics.XP_Win / 100);
                    BattleMetrics.Quantums += 30;
                    scr_StatsPlayer.VLeague++;
                    scr_BDUpdate.f_SetStatsGamesLeague(scr_StatsPlayer.id, scr_StatsPlayer.VLeague, scr_StatsPlayer.LLeague, scr_StatsPlayer.DLeague);
                }
                else
                {
                    BattleMetrics.Quantums += 15;
                    scr_StatsPlayer.VIa++;
                    scr_BDUpdate.f_SetStatsGamesIA(scr_StatsPlayer.id, scr_StatsPlayer.VIa, scr_StatsPlayer.LIa, scr_StatsPlayer.DIa);
                }
                scr_StatsPlayer.Neutrinos += BattleMetrics.Quantums;
                scr_BDUpdate.f_SetNeutrinos(scr_StatsPlayer.id, scr_StatsPlayer.Neutrinos);
            }
        }
        //Historial de partidas para Jugador VS Jugador
        if (b_InNetwork)
        {
            
            if (photonView.isMine)
            {
                string date = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
                string time = System.DateTime.Now.TimeOfDay.ToString();

                string player1 = "";
                string player2 = "";
                int id1 = 0;
                int id2 = 0;
                int xp1 = 0;
                int xp2 = 0;
                if (TeamInGame == 0)
                {
                    id1 = scr_StatsPlayer.id;
                    player1 = scr_StatsPlayer.Name + ":" + scr_StatsPlayer.SpecificDeckToString(scr_StatsPlayer.idc);
                    xp1 = BattleMetrics.XP_Win;
                    int other_index = 0;
                    if (PhotonNetwork.playerList[0].NickName == scr_StatsPlayer.Name)
                    {
                        other_index = 1;
                    }

                    object deck;
                    PhotonNetwork.playerList[other_index].CustomProperties.TryGetValue("Deck", out deck);
                    player2 = PhotonNetwork.playerList[other_index].NickName + ":" + (string)deck;
                    object idp;
                    PhotonNetwork.playerList[other_index].CustomProperties.TryGetValue("IdPlayer", out idp);
                    id2 = (int)idp;
                    xp2 = PhotonNetwork.playerList[other_index].GetScore();

                    if (scr_StatsPlayer.Friends.Contains(player2) && BattleMetrics.I_Win)
                    {
                        scr_StatsPlayer.AddProgressBattleAchiv(2, 1);
                    }
                }
                else
                {
                    id2 = scr_StatsPlayer.id;
                    player2 = scr_StatsPlayer.Name + ":" + scr_StatsPlayer.SpecificDeckToString(scr_StatsPlayer.idc);
                    xp2 = BattleMetrics.XP_Win;
                    int other_index = 0;
                    if (PhotonNetwork.playerList[0].NickName == scr_StatsPlayer.Name)
                    {
                        other_index = 1;
                    }

                    object deck;
                    PhotonNetwork.playerList[other_index].CustomProperties.TryGetValue("Deck", out deck);
                    player1 = PhotonNetwork.playerList[other_index].NickName + ":" + (string)deck;
                    object idp;
                    PhotonNetwork.playerList[other_index].CustomProperties.TryGetValue("IdPlayer", out idp);
                    id1 = (int)idp;
                    object xpp;
                    PhotonNetwork.playerList[other_index].CustomProperties.TryGetValue("XpBattle", out xpp);
                    xp1 = PhotonNetwork.playerList[other_index].GetScore();

                    if (scr_StatsPlayer.Friends.Contains(player1) && BattleMetrics.I_Win)
                    {
                        scr_StatsPlayer.AddProgressBattleAchiv(2, 1);
                    }
                }
                Debug.Log(date + " " + time);
                scr_BDUpdate.f_SetMatch(id1, id2, player1, player2, xp1, xp2, winer, date + " " + time);
            }

            //Puntos de ranket
            int other_i = 0;
            if (PhotonNetwork.playerList[0].NickName == scr_StatsPlayer.Name)
            {
                other_i = 1;
            }
            BattleMetrics.Other_Player = PhotonNetwork.playerList[other_i].NickName;
            int other_rp = 0;
            object rp;
            PhotonNetwork.playerList[other_i].CustomProperties.TryGetValue("Rpoints", out rp);
            other_rp = (int)rp;
            int rp_game = scr_StatsPlayer.GetRankPointsGame(other_rp, BattleMetrics.I_Win);
            BattleMetrics.Rank_Points = rp_game;
            if (rp_game>0)
            {
                scr_StatsPlayer.AddRangePoints(rp_game);
            } else if (rp_game<0)
            {
                scr_StatsPlayer.RestRangePoints(Mathf.Abs(rp_game));
            }
            scr_BDUpdate.f_SetRange(scr_StatsPlayer.id, scr_StatsPlayer.Range, scr_StatsPlayer.LRange, scr_StatsPlayer.RankPoints);

        } else if (!m_quickManager.InBotRoom && IA!=null) //Partida contra IA
        {
            if (BattleMetrics.I_Win)
            {
                scr_StatsPlayer.AddProgressBattleAchiv(3, 1);
            }
        }
        //Historial de partidas para Jugador vs Bot
        if (m_quickManager.InBotRoom)
        {
            string date = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string time = System.DateTime.Now.TimeOfDay.ToString();

            int id1 = scr_StatsPlayer.id;
            int id2 = IA.GetIdBot();
            string player1 = scr_StatsPlayer.Name + ":" + scr_StatsPlayer.SpecificDeckToString(scr_StatsPlayer.idc);
            string player2 = IA.MyName+":"+IA.GetDeckToString();
            BattleMetrics.Other_Player = IA.MyName;
            int xp1 = BattleMetrics.XP_Win;
            int xp2 = (int)Random.Range(xp1*0.75f,xp1*1.25f);

            int ia_rp = Random.Range(scr_StatsPlayer.RankPoints - 200, scr_StatsPlayer.RankPoints + 200);
            if (ia_rp < 0) { ia_rp = 0; }
            int rp_game = scr_StatsPlayer.GetRankPointsGame(ia_rp, BattleMetrics.I_Win);
            BattleMetrics.Rank_Points = rp_game;
            if (rp_game > 0)
            {
                scr_StatsPlayer.AddRangePoints(rp_game);
            }
            else if (rp_game < 0)
            {
                scr_StatsPlayer.RestRangePoints(Mathf.Abs(rp_game));
            }

            scr_BDUpdate.f_SetMatch(id1, id2, player1, player2, xp1, xp2, winer, date + " " + time);
            scr_BDUpdate.f_SetRange(scr_StatsPlayer.id, scr_StatsPlayer.Range, scr_StatsPlayer.LRange, scr_StatsPlayer.RankPoints);
        }
        scr_StatsPlayer.MoveXpBattleToPlayer();
    }


    IEnumerator TimeCount()
    {
        while (true)
        {
            yield return Delay1s;
            if (!b_EndGame)
            {
                if (b_InNetwork)
                {
                    if (PhotonNetwork.isMasterClient)
                    {
                        photonView.RPC("RestTimedRPC", PhotonTargets.Others);
                        RestTime();
                    }
                }
                else
                {
                    RestTime();
                }
                if (!IsHorda && txt_Time != null)
                {
                    if (Time_Min>0)
                    {
                        txt_Time.text = Time_Min.ToString() + ":" + Time_Sec.ToString();
                    } else
                    {
                        txt_Time.text = Time_Sec.ToString() + "s";
                    }
                    
                }   

                BattleMetrics.Time_Sec++;
                if (BattleMetrics.Time_Sec>59)
                {
                    BattleMetrics.Time_Sec = 0;
                    BattleMetrics.Time_Minutes++;
                }
            }
            //Status Game
            if (b_InNetwork)
                CheckOnlineStatusGame();
        }
    }

    public void CheckOnlineStatusGame()
    {
        if (PhotonNetwork.room.PlayerCount < 2 && !LostOtherPLayer && !TERMINARPARTIDA && !b_EndGame)
        {
            StartCoroutine(WaitOtherPlayer());
        }
        //ESTACION DESTRUIDA, PANTALLA FINAL
        if (TERMINARPARTIDA && !StartDisconect)
        {
            if (LostOtherPLayer)
            {
                OtherPLayerOutCanvas.gameObject.SetActive(false);
                LostOtherPLayer = false;
                StopCoroutine(WaitOtherPlayer());
            }
            StartDisconect = true;
            if (photonView.isMine)
                photonView.RPC("EndGameRPC", PhotonTargets.AllBuffered);
        }
        //SALIR DEL ROOM
        if (UserEndGame && TERMINARPARTIDA)
        {
            TERMINARPARTIDA = false;
            PhotonNetwork.LeaveRoom();
            print("Terminando partida y guardamos experiencia");
        }
    }

    IEnumerator WaitOtherPlayer()
    {
        LostOtherPLayer = true;
        OtherPLayerOutCanvas.gameObject.SetActive(true);
        yield return new WaitUntil(() => PhotonNetwork.room.PlayerCount == 2);
        OtherPLayerOutCanvas.gameObject.SetActive(false);
        LostOtherPLayer = false;
    }

    [PunRPC]
    public void EndGameRPC()
    {
        TERMINARPARTIDA = true;
        b_EndGame = true;
        CheckWiner();
    }

    public void TimeOut()
    {
        //Fin del juego por terminarse el timepo
        if (!b_InNetwork)
        {
            if (Station0!=null)
                Station0.gameObject.GetComponent<scr_Unit>().AddDamage(-1f, false);
        }
        else
        {
            GM.b_EndGame = true;
            TERMINARPARTIDA = true;
        }
    }

    void UpdateUIResources()
    {
        if (EnergyBar != null)
            EnergyBar.fillAmount = f_Resources / f_MaxResources;

        if (txt_Resources != null)
            txt_Resources.text = (Mathf.Floor(f_Resources)).ToString();
    }

    void UpdateUIShips()
    {
        if (txt_ShipsInGame != null)
        {
            if (UnitsInGame >= MaxUnitsInGame) { txt_ShipsInGame.color = Color.red; } else { txt_ShipsInGame.color = Color.white; }
            txt_ShipsInGame.text = UnitsInGame.ToString() + "/" + MaxUnitsInGame.ToString();
        }
    }

    public void AddResources(float res)
    {
        f_Resources += res;
        if (f_Resources > f_MaxResources)
        {
            f_Resources = f_MaxResources;
        }

        UpdateUIResources();
    }

    public bool CheckCost(int cost)
    {
        bool result = false;

        if (f_Resources >= cost)
        {
            result = true;
        }

        return result;
    }

    public void UseEnergy(int _cost)
    {
        if (InfiniteEnergy)
            return;

        f_Resources -= _cost;
       if (f_Resources <= 0) { f_Resources = 0f; }

        UpdateUIResources();
    }

    public void ShowAlert(string alert)
    {
        if (b_InNetwork)
        {
            MyPlayer.ShowAlert(alert);
            Debug.Log("Online Alert");
        } else
        {
            Alerts.GetComponent<Text>().text = alert;
            Alerts.GetComponent<Animator>().Play("ShowWarning");
        }
    }

    public void CreateShip(string _name, float _x, float _y, int _team, float hp, float scale, float _skin)
    {
        if (b_InNetwork)
        {
            if (_team == -1) { _team = TeamInGame; }
            BattleMetrics.Ships_Spawn++;
            MyPlayer.CreateShipNET(_name, _x, _y, _team, hp, scale, _skin, "none");
        }

    }

    public void CreateStation(string _name, float _x, float _y, int _team, float _skin)
    {
        if (b_InNetwork)
        {
            if (_team == -1) { _team = TeamInGame; }
            BattleMetrics.Stations_Spawn++;
            MyPlayer.CreateStationNET(_name, _x, _y, _team, _skin);
        }

    }

    public void CreateSkill(string _name, float _x, float _y, int _team)
    {
        if (b_InNetwork)
        {
            if (_team == -1) { _team = TeamInGame; }
            BattleMetrics.Skills_Spawn++;
            MyPlayer.CreateSkillNET(_name, _x, _y, _team);
        }

    }

    public void CreateSquad(string _squad, float _xorigin, float _yorigin, int _team, float _skin)
    {
        if (b_InNetwork)
        {
            if (_team == -1) { _team = TeamInGame; }
            string[] Squad = GetUnitsFromSquad(_squad);
            BattleMetrics.Ships_Spawn+=Squad.Length;

            if (CntrlCards != null)
            {
                if (CntrlCards.Formations.ContainsKey(_squad))
                {
                    if (CntrlCards.Formations[_squad].Length == Squad.Length)
                    {
                        GameObject new_parent = new GameObject();
                        new_parent.name = _squad;
                        Transform _parent = Instantiate(new_parent).transform;
                        for (int i = 0; i < Squad.Length; i++)
                        {
                            float _x = CntrlCards.Formations[_squad][i].x;
                            float _y = CntrlCards.Formations[_squad][i].y;
                            GameObject lastunit = MyPlayer.CreateShipNET(Squad[i], _xorigin + _x, _yorigin + _y, _team, -1f, -1f, _skin, _squad);
                            lastunit.transform.SetParent(_parent);
                        }
                        return;
                    }
                }
            }

            string[] Positions = GetPositionsFromSquad(_squad);
            if (Positions.Length > 1)
            {
                //FORMACIONES UNICAS
                if (Positions.Length == Squad.Length)
                {
                    GameObject new_parent = new GameObject();
                    new_parent.name = _squad;
                    Transform _parent = Instantiate(new_parent).transform;
                    for (int i = 0; i < Positions.Length; i++)
                    {
                        int _x = 0;
                        int _y = 0;
                        int.TryParse(Positions[i].Split(',')[0], out _x);
                        int.TryParse(Positions[i].Split(',')[1], out _y);
                        GameObject lastunit = MyPlayer.CreateShipNET(Squad[i], _xorigin + _x, _yorigin + _y, _team, -1f, -1f, _skin, _squad);
                        lastunit.transform.SetParent(_parent);
                    }
                }
            }
            else
            {//FORMACIONES DEFAULT
                int d = 0;
                if (Positions[0] == "pyramid")
                    d = -1;
                else if (Positions[0] == "mouth")
                    d = 1;

                if (TeamInGame == 1)
                    d *= -1;

                float f = 0f;
                float t = 0.5f;
                float deltaF = 6f / Squad.Length;
                GameObject new_parent = new GameObject();
                new_parent.name = _squad;
                Transform _parent = Instantiate(new_parent).transform;
                for (int i = 0; i < Squad.Length; i++)
                {
                    GameObject lastunit = MyPlayer.CreateShipNET(Squad[i], _xorigin + (Mathf.Abs(t) * d), _yorigin + f, _team, -1f, -1f, _skin, _squad);
                    f += deltaF * Mathf.Sign(f);
                    f *= -1;
                    t += 0.5f;
                    lastunit.transform.SetParent(_parent);
                }
            }
        }

    }

    public GameObject CreateUnitOff(string unit, Vector2 pos, int team, float _skin)
    {
        string type = scr_GetStats.GetTypeUnit(unit);

        if (type == "Squad")
        {
            GameObject lastunit = null;
            string[] Squad = GetUnitsFromSquad(unit);
            BattleMetrics.Ships_Spawn += Squad.Length;

            if (CntrlCards!=null)
            {
                if (CntrlCards.Formations.ContainsKey(unit))
                {
                    if (CntrlCards.Formations[unit].Length == Squad.Length)
                    {
                        GameObject new_parent = new GameObject();
                        new_parent.name = unit;
                        Transform _parent = Instantiate(new_parent).transform;
                        for (int i = 0; i < Squad.Length; i++)
                        {
                            lastunit = CreateUnitOff(Squad[i], pos+CntrlCards.Formations[unit][i], team, _skin);
                            lastunit.GetComponent<scr_Unit>().FromSquad = unit;
                            lastunit.transform.SetParent(_parent);
                        }
                        return lastunit;
                    }
                }
            }

            string[] Positions = GetPositionsFromSquad(unit);
            
            if (Positions.Length > 1)
            {
                //FORMACIONES UNICAS
                if (Positions.Length == Squad.Length)
                {
                    GameObject new_parent = new GameObject();
                    new_parent.name = unit;
                    Transform _parent = Instantiate(new_parent).transform;
                    for (int i = 0; i < Positions.Length; i++)
                    {
                        int _x = 0;
                        int _y = 0;
                        int.TryParse(Positions[i].Split(',')[0], out _x);
                        int.TryParse(Positions[i].Split(',')[1], out _y);
                        Vector2 _formation = new Vector2(pos.x + _x, pos.y + _y);
                        lastunit = CreateUnitOff(Squad[i], _formation, team, _skin);
                        lastunit.GetComponent<scr_Unit>().FromSquad = unit;
                        lastunit.transform.SetParent(_parent);
                    }
                }
            }
            else
            {//FORMACIONES DEFAULT

                GameObject new_parent = new GameObject();
                new_parent.name = unit;
                Transform _parent = Instantiate(new_parent).transform;

                int d = 0;
                if (Positions[0] == "pyramid")
                    d = -1;
                else if (Positions[0] == "mouth")
                    d = 1;

                if (TeamInGame == 1)
                    d *= -1;

                float f = 0f;
                float t = 0.5f;
                float deltaF = 6f / Squad.Length;
                for (int i = 0; i < Squad.Length; i++)
                {
                    lastunit = CreateUnitOff(Squad[i], new Vector2(pos.x + (Mathf.Abs(t) * d), pos.y + f), team, _skin);
                    lastunit.GetComponent<scr_Unit>().FromSquad = unit;
                    lastunit.transform.SetParent(_parent);
                    f += deltaF * Mathf.Sign(f);
                    f *= -1;
                    t += 0.5f;
                }
            }
            return lastunit;
        } else
        {
            GameObject _unit = null;

            if (type == "Skill")
            {
                _unit = Instantiate(BaseSkill, pos, Quaternion.identity);
                BattleMetrics.Skills_Spawn++;
            }
            else
            {
                _unit = Instantiate(BaseUnit, pos, Quaternion.identity);
                if (type=="Ship")
                    BattleMetrics.Ships_Spawn++;
                else
                    BattleMetrics.Stations_Spawn++;
            }

            if (_unit != null)
            {
                scr_BaseStats _u = _unit.GetComponent<scr_BaseStats>();
                _u.i_Team = team;
                _u.s_IdName = unit;
                _u.MySkin = _skin;
            }

            return _unit;
        }


    }

    public string[] GetUnitsFromSquad(string squad)
    {
        string[] Squad = new string[2];
        string _squad = scr_GetStats.GetPropUnit(squad, "Ships");
        if (_squad.Length > 0)
        {
            Squad = _squad.Split(',');
        }
        return Squad;
    }

    public string[] GetPositionsFromSquad(string squad)
    {
        string[] Positions = new string[2];
        string _positions = scr_GetStats.GetPropUnit(squad, "Positioning");
        if (_positions.Length > 0)
        {
            if (_positions.Contains(","))
            {
                Positions = _positions.Split('!');
            }
            else
            {
                Positions = new string[1] { _positions };
            }
        }
        return Positions;
    }

    [PunRPC]
    public void RestTimedRPC()
    {
        RestTime();
    }

    public void RestTime()
    {
        Time_Sec -= 1;
        if (Time_Sec <= 0)
        {

            if (Time_Min <= 0)
            {
                //Time Out
                TimeOut();
            }
            else
            {
                Time_Min--;
                Time_Sec = 59;
            }

        }
    }

    public bool IsNetAndMine()
    {
        if (b_InNetwork)
        {
            if (photonView.isMine)
                return true;
            else
                return false;
        }

        return false;
    }

    void OnLeftRoom()
    {
        print("SalioDelRoom y carga Nivel");
        if (b_InNetwork)
            PhotonNetwork.LoadLevel("Menu-Interfas");
    }
    /*
    private void OnPlayerConnected(NetworkPlayer player)
    {
        print("+" + player.ToString());
    }
    private void OnPlayerDisconnected(NetworkPlayer player)
    {
        print("-" + player.ToString());
    }
    */
}

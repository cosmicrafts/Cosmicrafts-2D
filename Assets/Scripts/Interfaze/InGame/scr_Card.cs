using UnityEngine;
using UnityEngine.UI;

public class scr_Card : MonoBehaviour {

    GameObject go_DragShip;
    scr_DragUnit DragUnitSCR;

    [HideInInspector]
    public scr_StatsCard MyStats;

    public int index = 0;

    bool is_dnd = false;
    //bool can_ndrag = false;
    //bool b_spawn_canceled = false;

    [HideInInspector]
    public bool Is_Lock = false;

    public Image RadialTTS;

    scr_CardsCntrl CntrlCards;

    public Image sp_mysprite;
    public Text txt_cost;
    public Button Mybutton;
    public bool IsNext = false;

    Color NoEnergy;
    Color DragCard;
    Color Normal;

    // Use this for initialization
    void Start () {

        NoEnergy = new Color(0.5f, 0.5f, 0.5f, 1f);
        DragCard = new Color(0f, 0f, 0.75f, 1f);
        Normal = new Color(1f,1f,1f,1f);

        CntrlCards = FindObjectOfType<scr_CardsCntrl>();
        if (!IsNext)
        {
            if (!scr_StatsPlayer.Op_RadialFill)
            {
                RadialTTS.fillMethod = Image.FillMethod.Horizontal;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!CntrlCards.RedyCards)
            return;

        if (!IsNext && Mybutton && !go_DragShip)
        {
            if (scr_MNGame.GM.CheckCost(MyStats.i_Cost) && scr_MNGame.GM.UnitsInGame + MyStats.i_Pcost < scr_MNGame.GM.MaxUnitsInGame && !Is_Lock)
            {
                Mybutton.interactable = true;
                sp_mysprite.color = Normal;
            }
            else
            {
                Mybutton.interactable = false;
                sp_mysprite.color = NoEnergy;
            }
        }

        if (MyStats.i_Cost !=0 && !IsNext)
        {
            if (scr_MNGame.GM.CheckCost(MyStats.i_Cost))
            {
                RadialTTS.fillAmount = 0f;
            } else
            {
                RadialTTS.fillAmount = 1f - (scr_MNGame.GM.f_Resources / MyStats.i_Cost);
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            EndPress();
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (is_dnd) { CancelSpawn(); }
        }

        if ((Input.GetKeyDown(CntrlCards.PCButtonsL[index]) || Input.GetKeyDown(CntrlCards.PCButtonsN[index])) && !IsNext && CntrlCards.PlacingByKey==-1)
        {
            DragShip();
            CntrlCards.PlacingByKey = index;
        }

        if ((Input.GetKeyUp(CntrlCards.PCButtonsL[index]) || Input.GetKeyUp(CntrlCards.PCButtonsN[index])) && !IsNext && CntrlCards.PlacingByKey == index)
        {
            DropShip();
            CntrlCards.PlacingByKey = -1;
        }
    }

    public void PressDown()
    {
        /*
        if (!CntrlCards.RedyCards  || Is_Lock)
            return;

        if (MyStats.AutoSpawn && CanSpawnUnit())
        {
            SpawnUnit(25f, 15f, false);
            return;
        }

        if (CntrlCards.is_placing_unit)
        {
            CancelSpawn();
        }
        else
        {
            can_ndrag = true;
        }*/
    }

    public void EndPress()
    {
        /*
        if (!CntrlCards.RedyCards)
            return;

        if (is_dnd)
        {
            DropShip();
        }
        else
        {
            if (b_spawn_canceled)
            {
                b_spawn_canceled = false;
            }
            else
            if (!CntrlCards.is_placing_unit && can_ndrag)
            {
                DragShip();
                is_dnd = true;
                can_ndrag = false;
                CntrlCards.is_placing_unit = true;
            }
        }
        */
    }

    bool CanSpawnUnit()
    {
        if (scr_MNGame.GM.b_EndGame || CntrlCards.PlacingByKey != -1 || !CntrlCards.RedyCards) { return false; }

        if (!scr_MNGame.GM.CheckCost(MyStats.i_Cost))
        {
            scr_MNGame.GM.ShowAlert(scr_Lang.GetText("txt_game_info23"));
            return false;
        }

        if (scr_MNGame.GM.UnitsInGame + MyStats.i_Pcost >= scr_MNGame.GM.MaxUnitsInGame && MyStats.Type !="Skill")
        {
            scr_MNGame.GM.ShowAlert(scr_Lang.GetText("txt_game_info25"));
            return false;
        }

        return true;
    }

    public void DragShip()
    {

        if (!CanSpawnUnit() || Is_Lock)
            return;

        scr_MNGame.GM.ClearSelected();

        //CntrlCards.KillCard.gameObject.SetActive(true);
        if (!is_dnd)
        {
            if (GetComponent<Button>().interactable)
            {
                //CntrlCards.is_drag_unit = true;
                is_dnd = true;
                //can_ndrag = false;
                sp_mysprite.color = DragCard;
                go_DragShip = Instantiate(scr_Resources.DragUnit, transform.position, Quaternion.identity);
                //go_DragShip.transform.localScale = new Vector3(0.4f+ MyStats.Size, 0.4f+ MyStats.Size, 0.4f+ MyStats.Size);
                DragUnitSCR = go_DragShip.GetComponent<scr_DragUnit>();
                //Sprite unitsprite = Resources.Load<Sprite>("Units/Iconos/" + scr_GetStats.GetPropUnit(MyUnit, "Icon"));
                if (MyStats.DragPreview != null)
                {
                    DragUnitSCR.MySprite.sprite = MyStats.DragPreview;
                }
                if (MyStats.Type =="Skill")
                {
                    DragUnitSCR.SetOriginSkill(MyStats.MyUnit);
                } else
                    scr_MNGame.GM.SetVisibleSpawnsAreas(true);
                DragUnitSCR.i_team = scr_MNGame.GM.TeamInGame;
                DragUnitSCR.Type = MyStats.Type;
            }
        }

    }

    public void DropShip()
    {
        //can_ndrag = true;
        is_dnd = false;
        scr_MNGame.GM.SetVisibleSpawnsAreas(false);
        if (scr_MNGame.GM.b_EndGame || !CntrlCards.RedyCards || CntrlCards.CursorOnCards)
        {
            CancelSpawn();
            return;
        }

        CntrlCards.is_placing_unit = false;
        if (go_DragShip != null)
        {
            Destroy(go_DragShip);
            if (DragUnitSCR.okPosition && DragUnitSCR.PlaceFree)
            {
                Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SpawnUnit(mouse.x, mouse.y, true);
            }
            sp_mysprite.color = Normal;
            DragUnitSCR = null;
        }
    }

    public void Set_Ship(string _ship)
    {
        if (CntrlCards.StatsCards.ContainsKey(_ship))
        {
            MyStats = CntrlCards.StatsCards[_ship];
        }

        UpdateInfo();
    }

    public string Get_Ship()
    {
        return MyStats.MyUnit;
    }

    public void UpdateInfo()
    {
        if (MyStats.Icon)
            sp_mysprite.sprite = MyStats.Icon;
        if (!IsNext)
            txt_cost.text = (MyStats.i_Cost).ToString();
    }

    void SpawnUnit(float x, float y, bool NeedFiguration)
    {
        bool OkPosition = true;

        if (NeedFiguration)
        {
            if (go_DragShip == null)
            {
                return;
            }
            scr_DragUnit _dr = go_DragShip.GetComponent<scr_DragUnit>();
            OkPosition = _dr.okPosition;
        }

        if (scr_MNGame.GM.CheckCost(MyStats.i_Cost) && OkPosition)
        {
            if (MyStats.Type != "Skill"  || scr_MNGame.GM.UnitsInGame+ MyStats.i_Pcost < scr_MNGame.GM.MaxUnitsInGame)
            {
                //create Unit
                if (scr_MNGame.GM.b_InNetwork)
                {
                    if (MyStats.Type == "Station")
                    {
                        scr_MNGame.GM.CreateStation(MyStats.MyUnit, x, y, -1, MyStats.SkinHue);
                    }
                    else if (MyStats.Type == "Skill")
                    {
                        scr_MNGame.GM.CreateSkill(MyStats.MyUnit, x, y, -1);
                        
                    }
                    else if (MyStats.Type == "Squad")
                    {
                        scr_MNGame.GM.CreateSquad(MyStats.MyUnit, x, y, -1, MyStats.SkinHue);

                    } else
                    {
                        scr_MNGame.GM.CreateShip(MyStats.MyUnit, x, y, -1, -1f, 1f, MyStats.SkinHue);
                    }
                } else
                {
                    int team = 0;
                    if (scr_MNGame.GM.SpawnAsEnemy)
                    {
                        team = 1;
                    }
                    scr_MNGame.GM.CreateUnitOff(MyStats.MyUnit, new Vector2(x, y), team, MyStats.SkinHue);
                }

                scr_MNGame.GM.UseEnergy(MyStats.i_Cost);
                CntrlCards.UseCard(MyStats.MyUnit, index);
            }
        }
    }


    void CancelSpawn()
    {
        if (go_DragShip != null) { Destroy(go_DragShip); }
        //b_spawn_canceled = true;
        is_dnd = false;
        //can_ndrag = false;
        CntrlCards.is_placing_unit = false;
        scr_MNGame.GM.SetVisibleSpawnsAreas(false);
    }

}

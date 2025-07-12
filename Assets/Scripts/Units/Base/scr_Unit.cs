using UnityEngine;

public class scr_Unit : scr_BaseStats{

    [HideInInspector]
    public bool IsDeath = false;

    [HideInInspector]
    public float TimeKillPhoton = 0f;

    public float f_Maxhp = 100f;
    public float f_armor = 0f;
    public float f_mirror = 0f;
    public float f_size = 1f;
    public bool IsInmune = false;
    public bool IsVisible = true;
    public bool IsInitial = false;

    [HideInInspector]
    public bool IsBaseStation = false;

    public CircleCollider2D TriggerArea;
    public CircleCollider2D SolidArea;

    public scr_Detections MyDetections;

    float double_tap = 0f;
    [HideInInspector]
    public bool delay_defend = false;
    int i_startTeam = 0;
    bool InRevelion = false;

    [HideInInspector]
    public bool Im_Aliad = false;

    [HideInInspector]
    public float Priority_distance = 0f;

    [HideInInspector]
    public float f_hp = 100f;

    [HideInInspector]
    public float Sp_scale = 1f;

    [HideInInspector]
    public scr_Unit Objetive;

    [HideInInspector]
    public scr_BaseStats LEA; //Ultimo enemigo que nos ataco

    [HideInInspector]
    public bool CanSelect = true;

    [HideInInspector]
    public string FromSquad = "none";

    [HideInInspector]
    public bool isSelected = false;

    [HideInInspector]
    public bool RedySelect = false;

    [HideInInspector]
    public Vector3 Frontal_Side;

    [HideInInspector]
    public Color MyColor = Color.white;

    public scr_Shooter MyShooter;

    public scr_Move MyMove;

    public Rigidbody2D MyRB2d;

    public scr_SpawnArea MySpawnArea;

    public scr_GenUnit MyGenUnits;

    // Use this for initialization
    void Start()
    {
        InitStatsData();

        if (s_IdName != "" && scr_MNGame.GM.b_InNetwork && photonView.instantiationData != null)
        {
            FromSquad = photonView.instantiationData[3].ToString();
            if (photonView.instantiationData.Length > 4)
            {
                if ((float)photonView.instantiationData[4] != -1f)
                {
                    f_Maxhp = (float)photonView.instantiationData[4];
                }
                if ((float)photonView.instantiationData[5] != -1f)
                {
                    float newscale = (float)photonView.instantiationData[5];
                    transform.localScale = new Vector3(newscale, newscale, newscale);
                }
            }
        }

        if (s_IdName == "Station_Central_Command")
            IsBaseStation = true;

        if (!StatsFromInspec)
        {
            f_Maxhp = NS.Hp;
            f_armor = NS.Armor;
            Sp_scale = NS.Size;
            IsInmune = NS.IsInmune;
            if (NS.Kinematic) { RemovePhysics(); }
            if (NS.CanShoot) { MyShooter.enabled = true; }
        }

        if (CompareTag("Station"))
        {
            RemovePhysics();
            if (NS.CanMove)
                MyMove.enabled = true;
        } else
        {
                MyMove.enabled = true;
        }

        float start_time = 0f;

        if (i_Team == 0)
        {
            Objetive = scr_MNGame.GM.Station1;
            Frontal_Side = new Vector3(1f,0f,0f);
        }
        else
        {
            Objetive = scr_MNGame.GM.Station0;
            start_time = 0.5f;
            Frontal_Side = new Vector3(-1f, 0f,0f);
        }

        MyAnimator.Play(s_IdName, -1, start_time);
        MyAnimator.SetFloat("CastSpeed", f_CastDelay);
        MyAnimator.SetFloat("Angle", start_time);
        StartCastPortal();

        MyDetections.UpdateCollider();

        f_hp = f_Maxhp;
        i_startTeam = i_Team;

        if (scr_MNGame.GM.TeamInGame == i_Team)
        {
            Im_Aliad = true;
            scr_MNGame.GM.UnitsInGame += i_poblation;
            if (scr_MNGame.GM.IA)
                scr_MNGame.GM.IA.Enemys.Add(gameObject);
        }

        MyUI.InitUI();
    }
	
	// Update is called once per frame
	void Update () {

        if (scr_MNGame.GM.FreezeUnits)
            return;

        UpdateStats();

        if (ImClone)
            return;

        if (TimeKillPhoton > 0f)
        {
            TimeKillPhoton -= Time.deltaTime;
            if (TimeKillPhoton <= 0f)
            {
                if (scr_MNGame.GM.Selected.Contains(this))
                    scr_MNGame.GM.Selected.Remove(this);
                PhotonNetwork.Destroy(gameObject);
                return;
            }
        }

        if (double_tap > 0f) { double_tap -= Time.deltaTime; }

        if (scr_MNGame.GM.b_EndGame || IsDeath)
            return;

        AddHP(NS.HpRegen * Time.deltaTime);
    }

    public void StartCastPortal()
    {
        GameObject portal = Instantiate(scr_Resources.Portal, transform.position, Quaternion.AngleAxis(90f, new Vector3(0f, 0f, 1f)));
        portal.GetComponent<AudioSource>().mute = true;
        portal.transform.localScale = new Vector3(f_size * 0.4f, f_size * 0.4f, 1f);
        if (CompareTag("Station"))
            portal.GetComponent<Animator>().SetInteger("Type", 1);
    }

    public void EndCast()
    {
        if (MySprite.sprite)
            Sp_scale = MySprite.sprite.bounds.size.x;
        
        if (CompareTag("Station"))
        {
            MyAnimator.speed = 1f;
        }
        else
        {
            SolidArea.enabled = true;
            SolidArea.radius = Sp_scale * 0.25f;
        }

        f_CastDelay = 0f;
        TriggerArea.enabled = true;
        TriggerArea.radius = Sp_scale * 0.25f;
        Frontal_Side *= Sp_scale*0.5f;
        UnitRedy = true;
    }

    public void GetCritical()
    {
        MyUI.GetCritical = true;
    }

    public void SetNewTarget(scr_Unit _target, bool remplace)
    {
        if ((NS.DirectShoot && _target != Objetive) || !_target.IsVisible || !UnitRedy)
            return;

        if (NS.CanShoot && (MyShooter.TargetShoot==null || remplace))
            MyShooter.TargetShoot = _target;

        if (NS.CanMove)
        {
            if (MyMove.TargetCharge == null || remplace)
                MyMove.SetTargetCharge(_target);
        }
    }

    public void SetNullTarget()
    {
        if (NS.CanShoot)
            MyShooter.TargetShoot = null;

        if (NS.CanMove)
            MyMove.SetTargetCharge(null);
    }

    public void SetVisible(bool _visible)
    {
        if (!UnitRedy)
            return;

        if (scr_MNGame.GM.b_InNetwork)
        {
            photonView.RPC("SetVisibleRPC", PhotonTargets.AllBuffered, _visible);
        }
        else
        {
            IsVisible = _visible;

            if (_visible)
            {
                MyAnimator.Play("Alpha", 1, 1f);
                MyUI.EnableUI(true);
            }
            else
            {
                if (IsMyTeam(scr_MNGame.GM.TeamInGame))
                    MyAnimator.Play("Alpha", 1, 0.4f);
                else
                {
                    MyAnimator.Play("Alpha", 1, 0.0f);
                    MyUI.EnableUI(false);
                }
            }
            MyAnimator.speed = 0;
        }
    }

    [PunRPC]
    public void SetVisibleRPC(bool _visible)
    {
        IsVisible = _visible;

        if (_visible)
        {
            MyAnimator.Play("Alpha", 1, 1f);
            MyUI.EnableUI(true);
        }
        else
        {
            if (IsMyTeam(scr_MNGame.GM.TeamInGame))
                MyAnimator.Play("Alpha", 1, 0.4f);
            else
            {
                MyAnimator.Play("Alpha", 1, 0.0f);
                MyUI.EnableUI(false);
            }
        }
        MyAnimator.speed = 0;
    }

    public void SetInmortal(bool inmortal)
    {
        if (scr_MNGame.GM.b_InNetwork)
        {
            photonView.RPC("SetInmortalRPC", PhotonTargets.AllBuffered, inmortal);
        }
        else
        {
            IsInmune = inmortal;
            if (MyUI)
            {
                if (inmortal)
                    MyUI.SetColorBar(MyUI.c_Inmortal);
                else
                    MyUI.SetColorTeam();
            }
        }
    }

    [PunRPC]
    public void SetInmortalRPC(bool inmortal)
    {
        IsInmune = inmortal;
        if (MyUI)
        {
            if (inmortal)
                MyUI.SetColorBar(MyUI.c_Inmortal);
            else
                MyUI.SetColorTeam();
        }
    }

    public void Revelion(int newteam)
    {
        if (IsBaseStation || InRevelion)
            return;

        if (scr_MNGame.GM.b_InNetwork)
        {
            photonView.RPC("RevelionRPC", PhotonTargets.AllBuffered, newteam);
        } else
        {
            i_startTeam = i_Team;
            i_Team = newteam;
            InRevelion = true;
            if (NS.CanMove && MyMove.OriginBase)
                MyMove.SetNewDestination(MyMove.OriginBase.transform.position, false);
            if (MyUI)
                MyUI.SetColorTeam();
        }
        
    }

    [PunRPC]
    public void RevelionRPC(int _newteam)
    {
        i_startTeam = i_Team;
        i_Team = _newteam;
        InRevelion = true;
        if (NS.CanMove && MyMove.OriginBase)
            MyMove.SetNewDestination(MyMove.OriginBase.transform.position, false);
        if (MyUI)
            MyUI.SetColorTeam();
    }

    public void EndRevelion()
    {
        if (IsBaseStation)
            return;

        if (scr_MNGame.GM.b_InNetwork)
        {
            photonView.RPC("EndRevelionRPC", PhotonTargets.AllBuffered);
        }
        else
        {
            i_Team = i_startTeam;
            InRevelion = false;
            SetNullTarget();
            if (NS.CanMove && Objetive)
                MyMove.SetNewDestination(Objetive.transform.position, true);
            if (MyUI)
                MyUI.SetColorTeam();
            scr_MNGame.GM.RemoveSelection(this);
        }
    }

    [PunRPC]
    public void EndRevelionRPC()
    {
        i_Team = i_startTeam;
        InRevelion = false;
        SetNullTarget();
        if (NS.CanMove && Objetive)
            MyMove.SetNewDestination(Objetive.transform.position, true);
        if (MyUI)
            MyUI.SetColorTeam();
        scr_MNGame.GM.RemoveSelection(this);
    }

    public void DestroyUnit()
    {
        if (ImClone || IsDeath)
            return;

        IsEnable = false;

        scr_MNGame.GM.ShakeCamera(f_Maxhp * 0.001f, f_Maxhp * 0.001f);

        float size_explo = f_size;
        if (MySprite.sprite != null)
            size_explo = MySprite.sprite.bounds.size.x;
        
        scr_Explo expl = scr_MNGame.GM.CreateExplo(transform.position, Mathf.RoundToInt(size_explo));

        if (NS.AfterShock>0)
            expl.IsInpactExplo(NS.AfterShock, i_Team, NS.AfterShokRange, true);

        IsDeath = true;

        if (!scr_MNGame.GM.b_EndGame && IsBaseStation && !scr_MNGame.GM.TERMINARPARTIDA && (!scr_MNGame.GM.IsHorda || (scr_MNGame.GM.IsHorda && PhotonNetwork.player.IsMasterClient)))
        {
            Debug.Log("Termino partida");
            scr_MNGame.GM.TERMINARPARTIDA = true;
        }

        ClearData();
        if (scr_MNGame.GM.b_InNetwork)
        {
            TimeKillPhoton = 0.1f;
            photonView.RPC("FakeExplosionRPC", PhotonTargets.Others);
        }
        else
            Destroy(gameObject, 0.3f);
    }

    [PunRPC]
    public void FakeExplosionRPC()
    {
        float size_explo = f_size;
        if (MySprite.sprite != null)
            size_explo = MySprite.sprite.bounds.size.x;
        
        scr_MNGame.GM.CreateExplo(transform.position, Mathf.RoundToInt(size_explo));
    }


    void ClearData()
    {
        if (scr_MNGame.GM.Selected.Contains(this))
            scr_MNGame.GM.Selected.Remove(this);

        if (transform.parent != null)
        {
            if (transform.parent.childCount==1)
                    Destroy(transform.parent.gameObject, 1f);
        }
            
        if (scr_MNGame.GM.TeamInGame == i_Team)
        {
            if (!ImClone)
            {
                scr_MNGame.GM.UnitsInGame -= i_poblation;
            }
            scr_MNGame.GM.f_MaxResources -= NS.Banck;
        }
        else if (scr_MNGame.GM.IA && i_Team == 1)
        {
            scr_MNGame.GM.IA.RemoveUnit(this);
        }
        f_hp = 0f;
        IsInmune = true;

        if (IsBaseStation)
        {
            if (scr_MNGame.GM.IA)
            {
                scr_MNGame.GM.b_EndGame = true;
                if (i_Team == 0)
                    scr_MNGame.GM.IA.ShowEmoteIA(2);
                else
                    scr_MNGame.GM.IA.ShowEmoteIA(3);
            }
        }

        if (scr_MNGame.GM.Selected.Contains(this))
            scr_MNGame.GM.Selected.Remove(this);
    }

    public void SelectUnit()
    {
        if (scr_MNGame.GM.DeleteOnClick && f_hp > 0f && !IsBaseStation)
        {
            AddDamage(-1f, false);
            return;
        }

        if (ImClone || scr_MNGame.GM.Drag_Unit || !MyUI)
            return;

        if (scr_MNGame.GM.CntrlCards)
        {
            if (scr_MNGame.GM.CntrlCards.is_placing_unit)
                return;
        }

        if (!IsMyTeam(scr_MNGame.GM.TeamInGame))
        {
            MyMove.Delay_Position = 0;
            scr_MNGame.GM.SetTargetSelected(this);
        } else
        {
            if (isSelected)
            {
                if (double_tap > 0f)
                {
                    delay_defend = true;
                    scr_MNGame.GM.DefenseSelected();
                }
                else
                    scr_MNGame.GM.RemoveSelection(this);
                
            } else
            {
                scr_MNGame.GM.ClearSelected();
                scr_MNGame.GM.AddSelection(this);
                double_tap = 0.4f;
            }
        }
    }

    public void DragUnit()
    {
        if (isSelected || !MyMove)
            return;

        scr_MNGame.GM.Drag_Unit = this;
    }

    public void DropUnit()
    {
        if (isSelected || !MyMove || !scr_MNGame.GM.Drag_Unit)
            return;

        MyMove.Delay_Position = 0;

        if (scr_MNGame.GM.Cursor_on_enemy != null)
        {
            scr_MNGame.GM.Drag_Unit.SetNewTarget(scr_MNGame.GM.Cursor_on_enemy, true);
            scr_MNGame.GM.CreateCommandSign(1, scr_MNGame.GM.Cursor_on_enemy.transform);
            scr_MNGame.GM.Drag_Unit = null;
        }
        else
        {
            scr_MNGame.GM.CreateCommandSign(0, null);
            scr_MNGame.GM.Drag_Unit.MyMove.SetNewDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition), false);
            scr_MNGame.GM.Drag_Unit = null;
        }
    }

    public void CursorOn()
    {
        if (!IsMyTeam(scr_MNGame.GM.TeamInGame))
        {
            scr_MNGame.GM.Cursor_on_enemy = this;
        }
    }

    public void CursorOut()
    {
        if (!IsMyTeam(scr_MNGame.GM.TeamInGame) && scr_MNGame.GM.Cursor_on_enemy == this )
        {
            scr_MNGame.GM.Cursor_on_enemy = null;
        }
    }

    public void RemovePhysics()
    {
        MyRB2d.isKinematic = true;
    }

    public void AddDamage(float dmg, bool iseffect)
    {
        if (scr_MNGame.GM.b_EndGame || IsDeath || dmg == 0f)
            return;

        if (scr_MNGame.GM.b_InNetwork)
        {
            photonView.RPC("AddDamageRPC", PhotonTargets.AllBuffered, dmg, iseffect);
        }
        else
        {
            AddDmgLocal(dmg, iseffect);
        }
    }

    [PunRPC]
    public void AddDamageRPC(float dmg, bool iseffect)
    {
        AddDmgLocal(dmg, iseffect);
    }

    void AddDmgLocal(float dmg, bool iseffect)
    {
        bool evade = false;
        if (dmg == -1f) { f_hp = 0f; }
        else if (IsInmune || dmg == -2f) { evade = true; }

        if(!evade)
        {
            if (!iseffect)
            {
                dmg -= f_armor+ NS.p_Armor;
                if (dmg < 0) { dmg = 1f; }
            }

            f_hp -= dmg;

            if (f_hp < 0) { dmg += f_hp; }

            if (MyUI)
                MyUI.SetHpBar( f_hp / f_Maxhp);
        }

        if (IsMyTeam(scr_MNGame.GM.TeamInGame))
            scr_MNGame.BattleMetrics.all_get_dmg += (int)dmg;
        else
            scr_MNGame.BattleMetrics.all_dmg += (int)dmg;

        if (!iseffect)
        {
            if (evade)
            {
                if (MyUI)
                    MyUI.ShowEvade();
                //if (LEA != null)
                //  scr_MNGame.GM.DebugAndWriteToFile(s_IdName + " (team " + i_Team + ")" + " evade " + dmg + " dmg by " + LEA.s_IdName + " (team " + LEA.i_Team + ")");
            }
            else
            {
                if (MyUI)
                    MyUI.i_DmgToShow += dmg;
                //if (LEA != null)
                  //  scr_MNGame.GM.DebugAndWriteToFile(s_IdName + " (team " + i_Team + ")" + " gets " + dmg + " dmg by " + LEA.s_IdName + " (team " + LEA.i_Team + ")");
            }
        }
        else
        {
            if (MyUI)
                MyUI.f_DmgEffectToShow += dmg;
            //if (LEA != null)
              //  scr_MNGame.GM.DebugAndWriteToFile(s_IdName + " (team " + i_Team + ")" + " gets " + dmg + " passive dmg by " + LEA.s_IdName + " (team " + LEA.i_Team + ")");
        }

        if (f_hp <= 0)
        {
            f_hp = 0;

            if ((scr_MNGame.GM.InmortalAllied && i_Team == scr_MNGame.GM.TeamInGame) || (scr_MNGame.GM.InmortalEnnemys && i_Team != scr_MNGame.GM.TeamInGame))
            {
                f_hp = 1;
                return;
            }

            if (!IsMyTeam(scr_MNGame.GM.TeamInGame))
            {
                if (CompareTag("Ship"))
                    scr_MNGame.BattleMetrics.Ships_Kills++;
                else
                    scr_MNGame.BattleMetrics.Stations_Kills++;
            }
            else
            {
                if (CompareTag("Ship"))
                    scr_MNGame.BattleMetrics.Ships_Lost++;
                else
                    scr_MNGame.BattleMetrics.Stations_Lost++;
            }

            DestroyUnit();
            if (LEA != null && !IsMyTeam(scr_MNGame.GM.TeamInGame))
            {
                if (LEA.CompareTag("Skill"))
                    scr_MNGame.BattleMetrics.Kills_By_Skill++;
            }
        }

    }

    public void AddHP(float hp)
    {
        if (IsDeath || hp <= 0f || f_hp >= f_Maxhp)
            return;

        //scr_MNGame.GM.DebugAndWriteToFile(MyBS.s_IdName + " (team " + MyBS.i_Team + ")" + " heals " + hp + " hp");

        if (scr_MNGame.GM.b_InNetwork)
        {
            photonView.RPC("AddHPRPC", PhotonTargets.All, hp);
        }
        else
        {
            AddHpLocal(hp);
        }
    }

    [PunRPC]
    public void AddHPRPC(float hp)
    {
        AddHpLocal(hp);
    }

    void AddHpLocal(float hp)
    {
        f_hp += hp;

        if (scr_StatsPlayer.Op_DMGText && MyUI)
        {
            MyUI.i_HealToShow += hp;
        }

        if (f_hp > f_Maxhp)
        {
            f_hp = f_Maxhp;
        }

        if (MyUI)
        {
            MyUI.SetHpBar(f_hp / f_Maxhp);
        }
    }

    /*
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ship"))
        {
            Rigidbody2D OtherRB = col.gameObject.GetComponent<Rigidbody2D>();
            if (OtherRB.mass <= MyRB2d.mass)
            {
                scr_Unit other = col.gameObject.GetComponent<scr_Unit>();
                float ang1 = other.f_direction;
                float ang2 = scr_Math.AngleBetweenVector2(transform.position, col.gameObject.transform.position);

                if (ang1 > 180)
                    ang1 -= 180;
                if (ang2 > 180)
                    ang2 -= 180;

                float dif = scr_Math.AngleDiference(ang1, ang2);
                int sign = scr_Math.GetSignAngles(ang1, ang2);
                float dir = scr_Math.AngleBetweenVector2(transform.position, col.gameObject.transform.position) + ((90f - dif) * sign);
                Vector2 OtherMove = new Vector3(Mathf.Cos(Mathf.Deg2Rad * dir) * 2f, Mathf.Sin(Mathf.Deg2Rad * dir) * 2f);
                Vector2 OtherPos = other.transform.position;
                OtherRB.MovePosition(OtherPos + (OtherMove * Time.deltaTime)); //movernos segun direccion y velocidad

            }
        }
    }
    */
    public void StopAnimation()
    {
        MyAnimator.speed = 0;
    }

    public void Victory()
    {
        MyAnimator.SetBool("Win", true);
        GameObject portal = Instantiate(scr_Resources.Portal, transform.position, Quaternion.AngleAxis(90f, new Vector3(0f, 0f, 1f)));
        portal.GetComponent<AudioSource>().mute = true;
        portal.transform.localScale = new Vector3(0.25f + Sp_scale, 0.25f + Sp_scale, 1f);

        UnitRedy = false;
        IsEnable = false;
        IsDeath = true;

        if (MyUI)
            MyUI.EnableUI(false);
        if (MyDetections)
            MyDetections.gameObject.SetActive(false);
        if (MySpawnArea)
            MySpawnArea.gameObject.SetActive(false);
        MyGenUnits.gameObject.SetActive(false);
    }

}

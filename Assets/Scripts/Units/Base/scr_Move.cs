using UnityEngine;

public class scr_Move : Photon.MonoBehaviour {

    public float f_MaxSpeed = 5f;

    public scr_Unit MySU;

    [HideInInspector]
    public Vector2 Target;

    [HideInInspector]
    public Transform OriginBase;

    [HideInInspector]
    public bool b_SyncMoveAngle = true;

    [HideInInspector]
    public scr_Unit TargetCharge = null;

    [HideInInspector]
    public float Immobilized = 0f;

    [HideInInspector]
    public bool go_objetive = true;

    [HideInInspector]
    public float Delay_Position = 0f;

    [HideInInspector]
    public int Stops_Counter = 0;

    float f_direction = 0f;

    float BestRT = 0f; //Best Range target

    float Current_speed = 0f;

    float TimeAnim = 0f;

    bool b_move = false;
    bool HaveEnemy = false;
    bool b_Charge = false;
    float Delay_Charge = 0f;

    public GameObject Propulsores;

    // Use this for initialization
    void Start () {

        if (MySU.Objetive && CompareTag("Ship"))
            SetNewDestination(MySU.Objetive.transform.position+MySU.Objetive.Frontal_Side, true);
        else
            StopShip();

        if (scr_MNGame.GM.Station0 && scr_MNGame.GM.Station1)
        {
            if (MySU.i_Team == 1)
            {
                f_direction = 180f;
                OriginBase = scr_MNGame.GM.Station1.transform;
            }
            else
            {
                OriginBase = scr_MNGame.GM.Station0.transform;
            }
        }

        f_MaxSpeed = MySU.NS.Speed;
        b_Charge = (MySU.NS.Charge > 0);

        if (CompareTag("Ship"))
            b_SyncMoveAngle = true;

        if (scr_StatsPlayer.OP_Graphics < 2 && Propulsores)
            Propulsores.SetActive(false);

        BestRT = 0.5f + f_MaxSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (!MySU.enabled || scr_MNGame.GM.b_EndGame || scr_MNGame.GM.FreezeUnits)
            return;

        if (MySU.ImClone)
        {
            MySU.MyRB2d.linearVelocity = Vector2.zero;
        } else
        {
            if (TargetCharge)
                Charge();
            else
                Move();

            Step();
        }
    }

    void Step()
    {
        if (Immobilized >0f)
        {
            Immobilized -= Time.deltaTime;
            return;
        }

        if (Delay_Position > 0f)
        {
            Delay_Position -= Time.deltaTime;
            return;
        }

        //CheckMove
        if (Current_speed > 0)
        {
            //Final Speed
            Vector2 speed = new Vector2(Current_speed * Mathf.Cos(Mathf.Deg2Rad * f_direction), Current_speed * Mathf.Sin(Mathf.Deg2Rad * f_direction));
            //Move Ship
            MySU.MyRB2d.MovePosition(MySU.MyRB2d.position + speed * Time.fixedDeltaTime);
        }
        //Friccion
        if (!b_move)
        {
            if (Current_speed > 0f) { Current_speed -= 6f * Time.deltaTime; }
            if (Current_speed < 0f) { Current_speed = 0f; }
        }

    }

    void Move()
    {
        if (!MySU.UnitRedy || f_MaxSpeed<=0f)
            return;

        HaveEnemy = false;
        BestRT = 1f;
        b_move = false;

        //offsettarget
        if (MySU.MyShooter != null && !MySU.isSelected)
        {
            if (MySU.MyShooter.TargetShoot)           
                HaveEnemy = true;
        }

        if (HaveEnemy)
        {
            BestRT = MySU.MyShooter.f_range_atk + MySU.MyShooter.TargetShoot.Sp_scale - 0.25f;
            Target = MySU.MyShooter.TargetShoot.transform.position;
        }

        f_direction = scr_Math.AngleBetweenVector2(transform.position, Target);
        
        if (!HaveEnemy)
            AngleAnimation();

        //Revisar distancias
        if (Vector2.Distance(transform.position, Target) > BestRT)
            b_move = true;
        else if (!go_objetive && CompareTag("Ship"))
        {
            if (MySU.Objetive)
            {
                if (MySU.isSelected)
                    scr_MNGame.GM.SetDestinationSelected(MySU.Objetive.transform.position + MySU.Objetive.Frontal_Side, true);
                else
                    SetNewDestination(MySU.Objetive.transform.position + MySU.Objetive.Frontal_Side, true);
            }
        }

        //Movimiento
        if (b_move)
        {
            //Aceleration
            if (Current_speed < f_MaxSpeed+ MySU.NS.p_Speed) { Current_speed += 4f * Time.deltaTime; }
            if (Current_speed > f_MaxSpeed+ MySU.NS.p_Speed) { Current_speed = f_MaxSpeed+ MySU.NS.p_Speed; }
        }

    }

    public void Charge()
    {
        b_move = false;

        if (Delay_Charge > 0f)
        {
            Delay_Charge -= Time.deltaTime;
            //Reverse
            float rev_dir = f_direction - 180;
            if (rev_dir<0) { rev_dir += 360; }
            Vector2 speed = new Vector2(Mathf.Cos(Mathf.Deg2Rad * rev_dir), Mathf.Sin(Mathf.Deg2Rad * rev_dir));
            MySU.MyRB2d.MovePosition(MySU.MyRB2d.position + speed * 2.25f * Time.fixedDeltaTime);
            return;
        }

        b_move = true;

        Target = TargetCharge.transform.position;
        f_direction = scr_Math.AngleBetweenVector2(transform.position, Target);

        if (Current_speed < 8f) { Current_speed += 16f * Time.deltaTime; }
        if (Current_speed > 8f) { Current_speed = 8f; }

        if (Vector2.Distance(transform.position, Target) < MySU.Sp_scale *0.6f)
        {
            Current_speed = 0f;
            TargetCharge.AddDamage(MySU.NS.Charge, true);
            Delay_Charge = 0.25f;
            if (MySU.IsMyTeam(scr_MNGame.GM.TeamInGame))
                scr_MNGame.BattleMetrics.all_charges++;
        }
    }

    public void SetTargetCharge(scr_Unit _tc)
    {
        if (b_Charge)
        {
            TargetCharge = _tc;
        }
    }

    public void StopShip()
    {
        if (!MySU.enabled || MySU.ImClone)
            return;

        Target = transform.position;
        Current_speed = 0f;
    }

    void AngleAnimation()
    {
        if (!b_SyncMoveAngle)
            return;

        if (MySU.MyShooter)
        {
            if (MySU.MyShooter.b_in_range)
                return;
        }

        //Animacion
        TimeAnim = (0.5f + ((f_direction / 22.5f))) * 0.0625f; //Antes era f_SpriteDir para giro diferente
        if (TimeAnim >= 1f)
            TimeAnim = 0f;
        MySU.MyAnimator.SetFloat("Angle", TimeAnim);//0.0625
    }

    public void SetNewDestination(Vector2 dest, bool is_objetive)
    {
        if (!MySU.enabled || MySU.ImClone)
            return;

        go_objetive = is_objetive;
        Target = dest;
    }
    /*
    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject g_other = other.gameObject;
        if (g_other.CompareTag("Ship") && b_Charge && Delay_Charge <= 0f)
        {
            scr_Unit scr_other = g_other.gameObject.GetComponent<scr_Unit>();
            if (scr_other.IsDeath || !scr_other.MySU.IsEnable)
                return;

            if (!scr_other.MySU.IsMyTeam(MySU.i_Team))
            {
                Current_speed = 0f;
                Propulsores.SetActive(false);
                scr_other.AddDamage(MySU.NS.Charge, true);
                Delay_Charge = 2f;
                if (MySU.IsMyTeam(scr_MNGame.GM.TeamInGame))
                    scr_MNGame.BattleMetrics.all_charges++;
            }
        }
    }
    */
}

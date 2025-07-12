using UnityEngine;

public class scr_Shooter : Photon.MonoBehaviour {

    public AudioSource AS_Shoot;

    public float f_atk = 10f;
    public float f_AtkSpeed = 1f;
    public float f_range_atk = 1f;
    public int i_Ncannons = 1;

    public Transform[] Cannons = new Transform[4];

    float CDatk = 0.5f;
    float Overheating = 0f;

    [HideInInspector]
    public float TimeBlind = 0f;

    [HideInInspector]
    public bool InAttack = false;

    [HideInInspector]
    public bool b_SyncAnimAngle = true;

    [HideInInspector]
    public bool b_in_range = false;

    [HideInInspector]
    public scr_Bullet BaseBullet;

    public scr_Unit MyUS;

    public scr_Unit TargetShoot;

    float Direction_Shoot = 0f;

    float TimeAnim = 0f;

    // Use this for initialization
    void Start () {

        if (MyUS.ImClone)
            return;

        f_atk = MyUS.NS.Attack_Dmg;
        f_AtkSpeed = MyUS.NS.Attack_Speed;
        f_range_atk = MyUS.NS.Range_Attack + MyUS.Sp_scale;
        i_Ncannons = MyUS.NS.Cannons;

        if (CompareTag("Station") && MyUS.NS.DirectShoot)
            f_range_atk = 30;
        else if(f_range_atk > MyUS.NS.Range_View)
            f_range_atk = MyUS.NS.Range_View;

        InitBullet();
    }

    public void InitBullet()
    {
        BaseBullet = Instantiate(scr_Resources.Bullet, transform).GetComponent<scr_Bullet>();
        BaseBullet.Origin = MyUS;
        BaseBullet.Speed = MyUS.NS.BulletSpeed;
        BaseBullet.i_Type = MyUS.NS.BulletType;
        BaseBullet.i_team = MyUS.i_Team;
        BaseBullet.SetSize(MyUS.NS.BulletSize);
        if (MyUS.NS.DirectShoot)
            BaseBullet.SetDirectShoot(MyUS.NS.DirectShootSize);
        BaseBullet.gameObject.SetActive(false);
        if (MyUS.NS.BulletType < 5)
            AS_Shoot.clip = scr_Resources.SD_Shoots[MyUS.NS.BulletType];
    }

    // Update is called once per frame
    void Update () {


        if (MyUS.ImClone || scr_MNGame.GM.b_EndGame || scr_MNGame.GM.FreezeUnits)
            return;

        Attack();
    }

    void Attack()
    {
        if (!MyUS.UnitRedy)
            return;

        if (Overheating > 0f)
            Overheating -= Time.deltaTime;
        else
            Overheating = 0f;

        if (TimeBlind>0)
            TimeBlind -= Time.deltaTime;

        if (!InAttack && TargetShoot)
            InAttack = true;
        
        if (InAttack && !TargetShoot)
        {
            MyUS.MyDetections.FindeNewTarget();
            InAttack = false;
            if (MyUS.MyMove && MyUS.Objetive)
                MyUS.MyMove.Target = MyUS.Objetive.transform.position;
        }

        float bestrange = 0;

        b_in_range = false;

        if (TargetShoot)
        {
            if (!TargetShoot.IsVisible)
            {
                TargetShoot = null;
                return;
            }

            bestrange = f_range_atk + TargetShoot.Sp_scale;

            if (Vector2.Distance(transform.position, TargetShoot.transform.position) <= bestrange)
            {
                b_in_range = true;
                Direction_Shoot = scr_Math.AngleBetweenVector2(transform.position, TargetShoot.transform.position);
                AngleAnimation();
            } else
            {
                return;
            }
        }
        else
            return;

        if (CDatk <= 0f)
        {
            AS_Shoot.Play();

            GameObject bullet = Instantiate(BaseBullet.gameObject, Cannons[0].position, Quaternion.AngleAxis(Direction_Shoot, new Vector3(0f, 0f, 1f)));
            bullet.SetActive(true);
            scr_Bullet scrbullet = bullet.GetComponent<scr_Bullet>();
            scrbullet.Target = TargetShoot;

            if (TimeBlind>0f)
            {
                scrbullet.DMG = -2f;
            }
            else
            {
                scrbullet.DMG = f_atk+ MyUS.NS.p_Atk + Overheating + ((1f - (MyUS.f_hp / MyUS.f_Maxhp)) * MyUS.NS.Berserk);
                if (Random.Range(0f, 1f) <= MyUS.NS.Critical)
                {
                    scrbullet.Critical = true;
                    scrbullet.DMG *= 1.5f;
                }
                Overheating += MyUS.NS.Overheating;
            }
            
            for (int i = 1; i < i_Ncannons; i++)
            {
                Instantiate(bullet, Cannons[i].position, Quaternion.AngleAxis(Direction_Shoot, new Vector3(0f, 0f, 1f)));
            }

            scrbullet.CC = MyUS.NS.Area_Dmg;

            if (scr_MNGame.GM.b_InNetwork)
                photonView.RPC("FakeShootRPC", PhotonTargets.Others,TargetShoot.transform.position.x, TargetShoot.transform.position.y);
            
            CDatk = f_AtkSpeed+MyUS.NS.p_Atk_Speed;
        }
        else if (CDatk > 0f)
        {
            CDatk -= Time.deltaTime;
        }
    }

    public void SetBlind(float _time)
    {
        if (scr_MNGame.GM.b_InNetwork)
        {
            photonView.RPC("SetBlindRPC", PhotonTargets.AllBuffered, _time);
        }
        else
        {
            TimeBlind = _time;
        }
    }

    [PunRPC]
    public void SetBlindRPC(float _time)
    {
        if (MyUS.ImClone)
            return;

        TimeBlind = _time;
    }

    void AngleAnimation()
    {
        if (!MyUS.NS.SyncAngleShoot) { return; }

        //Animacion
        TimeAnim = (0.5f + ((Direction_Shoot / 22.5f))) * 0.0625f; //Antes era f_SpriteDir para giro diferente
        if (TimeAnim >= 1f)
            TimeAnim = 0f;
        MyUS.MyAnimator.Play(MyUS.s_IdName, -1, TimeAnim);//0.0625
        MyUS.MyAnimator.SetFloat("Angle", TimeAnim);//0.0625
    }

    [PunRPC]
    public void FakeShootRPC(float x, float y)
    {
        if (photonView.isMine)
            return;

        GameObject bullet = Instantiate(scr_Resources.Bullet, Cannons[0].position, Quaternion.identity);
        scr_Bullet scrbullet = bullet.GetComponent<scr_Bullet>();
        scrbullet.Speed = MyUS.NS.BulletSpeed;
        scrbullet.i_Type = MyUS.NS.BulletType;
        scrbullet.i_team = MyUS.i_Team;
        scrbullet.TargetPosition = new Vector2(x,y);
        scrbullet.SetSize(MyUS.NS.BulletSize);

        AS_Shoot.Play();

        for (int i = 1; i < i_Ncannons; i++)
        {
            Instantiate(bullet, Cannons[i].position, Quaternion.identity);
        }
    }
}

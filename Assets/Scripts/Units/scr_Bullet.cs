using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Bullet : MonoBehaviour {

    [HideInInspector]
    public scr_Unit Target;
    [HideInInspector]
    public scr_Unit Origin;
    [HideInInspector]
    public Vector2 TargetPosition;

    [HideInInspector]
    public float DMG = 1.0f;
    public float Speed = 7.0f;
    public float CC = -1f;
    float DisImpact = 1f;
    bool IsDirect = false;

    [HideInInspector]
    public int i_team = 0;
    [HideInInspector]
    public int i_Type;
    [HideInInspector]
    public bool Critical = false;

    bool Hit = false;

    public CircleCollider2D TriggerCollider;

    public Animator BulletAnim;

    // Use this for initialization
    void Start () {

        BulletAnim.SetInteger("Type", i_Type);

        if (IsDirect)
            Destroy(gameObject, 12f);
        else
            Destroy(gameObject, 6f);

        if (Target != null)
            DisImpact = (Target.Sp_scale * 0.4f);
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Hit)
            return;

        if (Target != null)
            TargetPosition = Target.transform.position;
        else
            DisImpact = 1f;

        if (Vector2.Distance(transform.position, TargetPosition) > DisImpact)
        {
            MoveTo(transform, TargetPosition);
        } else
        {
            HitTarget();
        }
    }

    public void HitTarget()
    {
        AddDammageTarget();
        BulletAnim.speed = 2f;
        Hit = true;
    }

    void AddDammageTarget()
    {
        if (Target == null)
            return;

        if (Origin != null)
        {
            Origin.AddHP(DMG * Origin.NS.Vampiric);
            Origin.AddDamage(DMG * Target.f_mirror, false);
            Target.LEA = Origin;
        }

        if (Critical)
            Target.GetCritical();

        if (CC > 0f)
        {
            scr_Explo expl = scr_MNGame.GM.CreateExplo(transform.position, 0);
            expl.IsInpactExplo(DMG, Origin.i_Team, CC, false);
        }
        else
        {
            Target.AddDamage(DMG,false);
        }

        Target = null;
    }

    void MoveTo(Transform tra, Vector2 tar)
    {
        float f_direction = scr_Math.AngleBetweenVector2(tra.position, tar);
        tra.rotation = Quaternion.AngleAxis(f_direction, new Vector3(0f, 0f, 1f));// ROTACION INSTANTANEA
        tra.Translate(new Vector2(Speed * Time.deltaTime, 0));
        
        //Quaternion Q_Dir = Quaternion.AngleAxis(f_direction, new Vector3(0f, 0f, 1f));
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Q_Dir, 40f* Speed * Time.deltaTime); //Rotacion modular
    }

    public void StopAnim()
    {
        if (!Hit)
            BulletAnim.speed = 0f;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void SetSize(float size)
    {
        transform.localScale = new Vector3(size, size, size);
    }

    public void SetDirectShoot(float radio)
    {
        IsDirect = true;
        TriggerCollider.radius = radio;
        Target = Origin.Objetive;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Hit)
            return;

        if (!IsDirect)
            return;

        if (other.gameObject.CompareTag("Ship") || other.gameObject.CompareTag("Station"))
        {
            scr_Unit st = other.gameObject.GetComponent<scr_Unit>();
            if (!st.IsMyTeam(i_team))
            {
                st.AddDamage(DMG, false);
            }
        }
    }

}

using System.Collections.Generic;
using UnityEngine;

public class scr_Skill_01 : Photon.MonoBehaviour {

    public scr_Skill MySS;

    public GameObject Effect;

    public CircleCollider2D Range;

    List<scr_Unit> Affected = new List<scr_Unit>();

    // Inspire

    void InitSkill()
    {
        if (MySS.ImClone)
            return;

        Range.enabled = true;
        Range.radius = MySS.f_range;
        Effect.transform.localScale = new Vector3(MySS.f_range, MySS.f_range, MySS.f_range);
    }

    void EndAnimation()
    {
        if (MySS.ImClone)
            return;

        Range.enabled = false;
    }

    void EndSkill()
    {
        if (MySS.ImClone)
            return;

        for (int i=0; i<Affected.Count; i++)
        {
            if (!Affected[i]) { continue; }
            Affected[i].NS.p_Atk -= (Affected[i].MyShooter.f_atk * MySS.f_power);
            Affected[i].NS.p_Speed -= (Affected[i].MyMove.f_MaxSpeed * MySS.f_boost);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ship") || other.CompareTag("Station"))
        {
            scr_Unit otherscr = other.gameObject.GetComponent<scr_Unit>();
            if (otherscr.IsMyTeam(MySS.i_Team) && (otherscr.MyShooter || otherscr.MyMove))
            {
                if (!Affected.Contains(otherscr))
                {
                    Affected.Add(otherscr);
                    otherscr.NS.p_Atk += (otherscr.MyShooter.f_atk * MySS.f_power);
                    otherscr.NS.p_Speed += (otherscr.MyMove.f_MaxSpeed * MySS.f_boost);
                }
            }
        }
    }


}

using System.Collections.Generic;
using UnityEngine;

public class scr_Skill_10 : MonoBehaviour {

    public scr_Skill MySS; //Contamination

    List<scr_Unit> Affected = new List<scr_Unit>();

    float DelayDmg = 0f;

    public CircleCollider2D Range;

    void InitSkill()
    {
        if (MySS.ImClone)
            return;

        Range.enabled = true;
        Range.radius = MySS.f_range;
    }

    void EndSkill()
    {
        if (MySS.ImClone)
            return;

        for (int i = 0; i < Affected.Count; i++)
        {
            if (Affected[i] == null)
                continue;

            if (Affected[i].MyMove)
                Affected[i].NS.p_Speed += (Affected[i].MyMove.f_MaxSpeed * MySS.f_boost);
        }
    }

    void EndAnimation()
    {
        Range.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (MySS.ImClone)
            return;

        if (DelayDmg > 0f)
            DelayDmg -= Time.deltaTime;
        else
        {
            DelayDmg = 1f;
            for (int i = 0; i < Affected.Count; i++)
            {
                Affected[i].LEA = MySS;
                Affected[i].AddDamage(MySS.f_power, true);
            }
                
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ship") || other.CompareTag("Station"))
        {
            scr_Unit otherscr = other.gameObject.GetComponent<scr_Unit>();
            if (!otherscr.IsMyTeam(MySS.i_Team))
            {
                if (!Affected.Contains(otherscr))
                {
                    Affected.Add(otherscr);
                    if (otherscr.MyMove)
                        otherscr.NS.p_Speed-= (otherscr.MyMove.f_MaxSpeed * MySS.f_boost);
                }
            }
        }
    }


}

using System.Collections.Generic;
using UnityEngine;

public class scr_Skill_08 : MonoBehaviour {

    // Black Hole
    public scr_Skill MySS;
    public CircleCollider2D Range;

    List<scr_Unit> Affected = new List<scr_Unit>();

    float DelayDmg = 0f;

    void InitSkill()
    {
        transform.localScale = new Vector3(MySS.f_range, MySS.f_range, MySS.f_range);

        if (MySS.ImClone)
            return;

        Range.enabled = true;
    }

    void EndSkill()
    {

    }

    void EndAnimation()
    {

    }

    private void Update()
    {
        if (DelayDmg > 0f)
            DelayDmg -= Time.deltaTime;
        else
            DelayDmg = 1f;

        
        for (int i = 0; i < Affected.Count; i++)
        {
            if (Affected[i] == null)
                continue;

            if (DelayDmg<=0)
            {
                Affected[i].MyMove.Immobilized = 1f;
                Affected[i].LEA = MySS;
                Affected[i].AddDamage(MySS.f_power, true);
            }
            if (Affected[i].CompareTag("Ship"))
            {
                if (Vector2.Distance(transform.position, Affected[i].transform.position) > 0.25f)
                {
                    Vector3 direction = (transform.position - Affected[i].transform.position).normalized;
                    Affected[i].MyRB2d.MovePosition(Affected[i].transform.position + direction * MySS.f_boost * Time.deltaTime);
                }
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
                    Affected.Add(otherscr);
                
            }
        }
    }
}

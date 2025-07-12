using System.Collections.Generic;
using UnityEngine;

public class scr_Skill_14 : MonoBehaviour {

    public scr_Skill MySS; //Heal

    public CircleCollider2D Range;

    List<scr_Unit> Affected = new List<scr_Unit>();

    float DelayHeal = 0f;

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
	
	// Update is called once per frame
	void Update () {
        if (MySS.ImClone)
            return;

        if (DelayHeal > 0f)
            DelayHeal -= Time.deltaTime;
        else
        {
            DelayHeal = 1f;
        }

        for (int i = 0; i < Affected.Count; i++)
        {
            if (Affected[i] == null)
                continue;

            if (DelayHeal <= 0)
            {
                if (MySS.IsMyTeam(Affected[i].i_Team))
                    Affected[i].AddHP(MySS.f_power);
            }
            if (Affected[i].CompareTag("Ship") && !MySS.IsMyTeam(Affected[i].i_Team))
            {
                if (Vector2.Distance(transform.position, Affected[i].transform.position) > 0.25f)
                {
                    Vector3 direction = (Affected[i].transform.position - transform.position).normalized;
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
            if (!Affected.Contains(otherscr))
                Affected.Add(otherscr);
        }
    }

}

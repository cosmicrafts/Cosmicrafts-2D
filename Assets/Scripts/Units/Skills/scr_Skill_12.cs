using System.Collections.Generic;
using UnityEngine;

public class scr_Skill_12 : MonoBehaviour {

    public scr_Skill MySS; //Ghost Bomb

    List<scr_Unit> Affected = new List<scr_Unit>();

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

            Affected[i].SetVisible(true);
        }
    }

    void EndAnimation()
    {
        Range.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ship") || other.CompareTag("Station"))
        {
            scr_Unit otherscr = other.gameObject.GetComponent<scr_Unit>();
            if (otherscr.IsMyTeam(MySS.i_Team))
            {
                if (!Affected.Contains(otherscr))
                {
                    Affected.Add(otherscr);
                    otherscr.SetVisible(false);
                }
            }
        }
    }
}

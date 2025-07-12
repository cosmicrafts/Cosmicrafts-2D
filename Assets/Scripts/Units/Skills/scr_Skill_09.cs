using UnityEngine;

public class scr_Skill_09 : Photon.MonoBehaviour
{

    public scr_Skill MySS; //Solar Mirror

    scr_Unit Affected;

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

        if (Affected)
        {
            Affected.f_mirror -= MySS.f_power;
            Affected.f_armor -= MySS.f_boost;
        }
    }

    void EndAnimation()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Affected)
            return;

        if (other.CompareTag("Ship") || other.CompareTag("Station"))
        {
            scr_Unit otherscr = other.gameObject.GetComponent<scr_Unit>();
            if (otherscr.IsMyTeam(MySS.i_Team))
            {
                Affected = otherscr;
                Affected.f_mirror += MySS.f_power;
                Affected.f_armor += MySS.f_boost;
                Range.enabled = false;
            }
        }
    }
}

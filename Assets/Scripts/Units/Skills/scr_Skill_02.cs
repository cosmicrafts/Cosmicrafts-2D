using UnityEngine;

public class scr_Skill_02 : MonoBehaviour {

    public scr_Skill MySS;
    public CircleCollider2D Range;
    bool ok_dmg = false;
    // Ping

    void InitSkill()
    {
        if (MySS.ImClone)
            return;

        Range.enabled = true;
        Range.radius = MySS.f_range;   
    }

    void EndAnimation()
    {
        if (MySS.ImClone)
            return;

        MySS.DestroyUnit();
    }

    void EndSkill()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (ok_dmg)
            return;

        if (other.CompareTag("Ship") || other.CompareTag("Station"))
        {
            scr_Unit otherscr = other.gameObject.GetComponent<scr_Unit>();
            if (!otherscr.IsMyTeam(MySS.i_Team))
            {
                otherscr.LEA = MySS;
                otherscr.AddDamage(MySS.f_power, true);
                ok_dmg = true;
            }
        }
    }
}

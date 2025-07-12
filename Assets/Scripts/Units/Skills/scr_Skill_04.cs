using UnityEngine;

public class scr_Skill_04 : Photon.MonoBehaviour {

    public scr_Skill MySS;
    public CircleCollider2D Range;
    public GameObject Shield;

    void InitSkill() //Shield
    {
        Shield.transform.localScale = new Vector3(MySS.f_range, MySS.f_range, -1f);
        Range.enabled = true;
        Range.radius = MySS.f_range*0.5f;
    }

    void EndSkill()
    {

    }

    void EndAnimation()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            scr_Bullet otherscr = other.GetComponent<scr_Bullet>();
            if (!MySS.IsMyTeam(otherscr.i_team))
            {
                otherscr.Target = null;
                otherscr.HitTarget();
            }
        }
    }
}

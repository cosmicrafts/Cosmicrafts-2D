using System.Collections.Generic;
using UnityEngine;

public class scr_Skill_13 : Photon.MonoBehaviour {
    public scr_Skill MySS; //Mallstrom

    List<scr_Unit> Affected = new List<scr_Unit>();

    public CircleCollider2D Range;

    public LineRenderer Conections;

    int Index = 0;

    float DelayEffect = 0f;

    void InitSkill()
    {
        Range.enabled = true;
        Range.radius = MySS.f_range;
    }

    void EndSkill()
    {

    }

    void EndAnimation()
    {
        MySS.DestroyUnit();
    }

    // Update is called once per frame
    void Update () {

        if (DelayEffect>0f)
        {
            DelayEffect-=Time.deltaTime;
            if (DelayEffect <= 0f)
            {
                Conections.enabled = false;
                Range.enabled = false;
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
                    Conections.positionCount = Index + 1;
                    Affected.Add(otherscr);
                    if (!MySS.ImClone)
                    {
                        otherscr.LEA = MySS;
                        otherscr.AddDamage(MySS.f_power, true);
                    }
                    transform.position = other.transform.position;
                    Conections.SetPosition(Index, other.transform.position);
                    if (Index == 0)
                        DelayEffect = 0.2f;
                    Index++;
                    if (Index >= (int)MySS.f_boost)
                    {
                        Range.enabled = false;
                        return;
                    }
                }
            }
        }
    }
}

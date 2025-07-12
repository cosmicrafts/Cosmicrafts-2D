using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Explo : MonoBehaviour {

    public int team = 0;
    [HideInInspector]
    public float dmg = 0;
    public int size = 0;

    public Animator Explosion;
    public AudioSource Ad_Explo;
    public CircleCollider2D Range;

    public AudioClip[] ExpInt = new AudioClip[5];

    private void Start()
    {
        if (size>=1)
        {
            if (size > 5)
                size = 5;

            Explosion.SetInteger("Size", size);
            Ad_Explo.clip = ExpInt[size - 1];
            Ad_Explo.enabled = true;

            transform.Rotate(new Vector3(0f, 0f, 1f), Random.Range(0, 360));
        }
        Destroy(gameObject, 3f);
    }

    public void IsInpactExplo(float _dmg, int _team, float raduis, bool visible)
    {
        if (!visible)
        {
            Explosion.gameObject.SetActive(false);
        }
        Range.enabled = true;
        Range.radius = raduis;
        dmg = _dmg;
        team = _team;
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ship") || other.CompareTag("Station"))
        {
            scr_Unit _Unit = other.gameObject.GetComponent<scr_Unit>();
            if (!_Unit.IsMyTeam(team))
            {
                _Unit.AddDamage(dmg, false);
                return;
            }
        }
    }

}

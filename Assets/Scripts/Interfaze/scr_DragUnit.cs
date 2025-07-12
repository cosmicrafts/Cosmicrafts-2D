using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_DragUnit : MonoBehaviour {

    [HideInInspector]
    public bool okPosition = false;
    [HideInInspector]
    public bool PlaceFree = true;
    public int i_team = 0;
    [HideInInspector]
    public bool skillSpawn = false;
    [HideInInspector]
    public bool in_interfaz = false;
    [HideInInspector]
    public string Type = "Ship";

    Vector2 Origin;
    public SpriteRenderer MySprite;
    public SpriteRenderer SpriteBase;

    public SpriteRenderer Negative;

    public bool OriInMouse = true;

    // Update is called once per frame
    void Start()
    {
        if (in_interfaz)
        {
            SpriteBase.gameObject.SetActive(false);
            Negative.gameObject.SetActive(false);
        } else
        {
            SpriteBase.sprite = MySprite.sprite;
            if (skillSpawn)
            {
                okPosition = true;
                transform.GetChild(0).gameObject.SetActive(false);
            }
            okPosition = scr_MNGame.GM.FreeSpawn;
            if (i_team == 1)
            {
                MySprite.flipX = true;
                SpriteBase.flipX = true;
            }
            if (!skillSpawn) { scr_MNGame.GM.b_DragUnit = true; }
        }

        PlaceFree = true;
    }

    public void SetOriginSkill(string idskill)
    {
        skillSpawn = true;
        if (idskill == "U_Skill_05" || idskill == "U_Skill_06")
        {
            OriInMouse = false;
            if (i_team == 0)
                Origin = scr_MNGame.GM.Station0.transform.position;
            if (i_team == 1)
                Origin = scr_MNGame.GM.Station1.transform.position;
        }
    }

    void OnDestroy()
    {
        if (!in_interfaz) {
            scr_MNGame.GM.b_DragUnit = false;
        }
    }

    void Update()
    {
        if (!in_interfaz)
        {
            Negative.enabled = !(okPosition && PlaceFree);
        }

        if (OriInMouse)
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mouse.x, mouse.y);
        }
        else
        {
            transform.position = Origin;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!skillSpawn && !in_interfaz)
        {
            if (other.gameObject.CompareTag("ShipsArea"))
            {
                okPosition = true;
            }
        }

    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (!skillSpawn && !in_interfaz)
        {
            if (other.gameObject.CompareTag("ShipsArea"))
            {
                okPosition = scr_MNGame.GM.FreeSpawn;
            }
        }
    }

}

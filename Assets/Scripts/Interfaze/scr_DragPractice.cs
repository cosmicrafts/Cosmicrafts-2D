using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_DragPractice : MonoBehaviour
{

    public scr_Practice Practice;

    public SpriteRenderer MySprite;

    [HideInInspector]
    public int team_spawn = 0;
    [HideInInspector]
    public bool ToDelete = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mouse.x, mouse.y);

        if (Input.GetMouseButtonDown(0))
        {
            if (!ToDelete)
            {
                GameObject tets_unit = scr_MNGame.GM.CreateUnitOff("U_Target_Practice", transform.position, team_spawn, 1f);
                Practice.UnitsTest.Add(tets_unit);
                gameObject.SetActive(false);
            }
        }
    }
}

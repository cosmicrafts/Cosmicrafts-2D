using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_UnitPlaceFree : MonoBehaviour {

    public scr_DragUnit ParentScr;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ParentScr.Type == "Ship" || ParentScr.Type == "Skill")
            return;
        if (other.gameObject.CompareTag("Ship") || other.gameObject.CompareTag("Station"))
        {
            ParentScr.PlaceFree = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ship") || other.gameObject.CompareTag("Station"))
        {
            ParentScr.PlaceFree = true;
        }
    }
}

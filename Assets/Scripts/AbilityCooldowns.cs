using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityCooldowns : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] abilities;

    public void UpdateCooldowns(float[] timers)
    {
        if(timers[0] == 99f)
        {
            abilities[0].text = "Speed boost (Q): N/A";
        }
        else
        {
            abilities[0].text = "Speed boost (Q): " + Mathf.Max(Mathf.RoundToInt(timers[0]), 0).ToString() + "s";
        }

        if(timers[1] == 99f)
        {
            abilities[1].text = "Mass Grab (R): N/A";
        }
        else
        {
            abilities[1].text = "Mass Grab (R): " + Mathf.Max(Mathf.RoundToInt(timers[1]), 0).ToString() + "s";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability3 : MonoBehaviour
{
    public List<PointCarrier> myCarriers;
    [SerializeField]
    private PlayerController pc;

    private void Awake()
    {
        myCarriers = new List<PointCarrier>();
    }
    private void Update()
    {
        transform.position = pc.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PointCarrier pc = collision.GetComponent<PointCarrier>();
        if (pc != null)
        {
            myCarriers.Add(pc);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PointCarrier pc = collision.GetComponent<PointCarrier>();
        if (pc != null)
        {
            myCarriers.Remove(pc);
        }
    }
}

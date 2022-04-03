using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMaw : MonoBehaviour
{
    [SerializeField]
    private PlayerController pc;
    [SerializeField]
    private BoxCollider2D bc;

    [SerializeField]
    private ManagerChannel managerChannel;

    public bool unleashed;

    [SerializeField]
    private Sprite unleashedSprite;
    [SerializeField]
    private AudioSource audiounleash;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!unleashed)
        {
            pc = collision.gameObject.GetComponent<PlayerController>();
        }
        else if(collision.CompareTag("Player"))
        {
            managerChannel.EndGame();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!unleashed) pc = null;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if(pc != null)
            {
                pc.DevourUnits();
            }
        }

        if (unleashed) Unleash();
    }

    public void StartUnleash()
    {
        bc.offset = new Vector2(0f, 0f);
        GetComponent<SpriteRenderer>().sprite = unleashedSprite;
        pc = Manager.globalPlayer;
        audiounleash.PlayOneShot(audiounleash.clip);
    }

    public void Unleash()
    {
        Vector3 newPos = Vector3.MoveTowards(transform.position, pc.transform.position, 45 * Time.deltaTime);
        transform.position = newPos;
        
    }
}

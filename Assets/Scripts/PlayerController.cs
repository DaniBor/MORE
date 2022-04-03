using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private List<PointCarrier> pcInRange;
    private List<PointCarrier> curPayload;

    private const int xRot = -70;
    public float offset = 0.4f;
    public float offsetPerUnit = 0.15f;

    [SerializeField]
    private Manager manager;
    [SerializeField]
    private ManagerChannel managerChannel;
    [SerializeField]
    private PlayerStatSets[] stats;
    private int curTier;


    [SerializeField]
    private Ability3 ability3;
    private float[] abilityTimers = { 0f, 0f, 0f, 0f };
    private float[] cooldowns = { 20f, 0f, 30f, 0f };

    private float ability1dur = 0f;
    private float ability1speed = 8f;

    [SerializeField]
    private AbilityCooldowns UiTimers;


    private void Awake()
    {
        pcInRange = new List<PointCarrier>();
        curPayload = new List<PointCarrier>();
        curTier = 0;
        speed = stats[0].speed;

        managerChannel.onTierUpgrade += UpgradeTier;
        managerChannel.onGameEnd += OnGameEnd;
    }
    private void Start()
    {
        
    }
    void Update()
    {
        HandleInput();
        UpdateCurPayloadPosition();
        HandleAbilities();
        UpdateAbilitytimers();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if(pcInRange.Count > 0)
            {
                var toStack = pcInRange[0];
                var tsSr = toStack.sr;

                int randZRot = Random.Range(-180, 181);

                toStack.beingCarried = true;
                toStack.deactivateLightOnDistance = false;
                toStack.lght.intensity = 0.25f;

                curPayload.Add(toStack);
                toStack.transform.Rotate(new Vector3(xRot, 0, randZRot));
                toStack.GetComponent<Collider2D>().enabled = false;
                tsSr.sortingOrder += curPayload.Count;
            }
        }
    }
    private void ExecuteAbility3()
    {
        for(int i = ability3.myCarriers.Count-1; i >= 0; i--)
        {
            var pc = ability3.myCarriers[i];
            var pcSr = pc.sr;

            int randZRot = Random.Range(-180, 181);

            pc.beingCarried = true;
            pc.deactivateLightOnDistance = false;
            pc.lght.intensity = 0.25f;
            curPayload.Add(pc);
            pc.transform.Rotate(new Vector3(xRot, 0, randZRot));
            pc.GetComponent<Collider2D>().enabled = false;
            pcSr.sortingOrder += curPayload.Count;
        }

        ability3.myCarriers = new List<PointCarrier>();
    }
    private void UpdateAbilitytimers()
    {
        float[] temp = new float[2];
        temp[0] = stats[curTier].ability1Unlocked ? abilityTimers[0] : 99f;
        temp[1] = stats[curTier].ability3Unlocked ? abilityTimers[2] : 99f;

        UiTimers.UpdateCooldowns(temp);
    }


    private void HandleMovement()
    {
        float xIn = Input.GetAxisRaw("Horizontal");
        float yIn = Input.GetAxisRaw("Vertical");

        var ab1spd = ability1dur > 0f ? ability1speed : 0f;

        float xTo = transform.position.x + xIn * (speed + ab1spd) * Time.deltaTime;
        float yTo = transform.position.y + yIn * (speed + ab1spd) * Time.deltaTime;

        Vector2 newPos = new Vector2(xTo, yTo);

        rb.MovePosition(newPos);
    }
    private void HandleAbilities()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(abilityTimers[0] <= 0f && stats[curTier].ability1Unlocked)
            {
                abilityTimers[0] = cooldowns[0];
                ability1dur = 10f;
                
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            if (abilityTimers[1] <= 0f && stats[curTier].ability2Unlocked)
            {
                abilityTimers[1] = cooldowns[1];
            }
        }
        if (Input.GetButtonDown("Fire3"))
        {
            if (abilityTimers[2] <= 0f && stats[curTier].ability3Unlocked)
            {
                abilityTimers[2] = cooldowns[2];
                ExecuteAbility3();
            }
        }
        if (Input.GetButtonDown("Fire4"))
        {
            if (abilityTimers[3] <= 0f && stats[curTier].ability4Unlocked)
            {
                abilityTimers[3] = cooldowns[3];
            }
        }

        for(int i = 0; i < abilityTimers.Length; i++)
        {
            abilityTimers[i] -= Time.deltaTime;
        }
    }
    private void UpdateCurPayloadPosition()
    {
        int index = 0;
        foreach(PointCarrier pc in curPayload)
        {
            float yOffset = transform.position.y + offset + offsetPerUnit * index;
            Vector3 pos = new Vector3(transform.position.x, yOffset, transform.position.z);
            pc.transform.position = pos;
            index++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PointCarrier pc = collision.GetComponent<PointCarrier>();
        if(pc != null)
        {
            pcInRange.Add(pc);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PointCarrier pc = collision.GetComponent<PointCarrier>();
        if (pc != null)
        {
            pcInRange.Remove(pc);
        }
    }

    public void DevourUnits()
    {
        int result = 0;
        float timeResult = 0;
        for(int i = curPayload.Count-1; i >= 0; i--)
        {
            PointCarrier pc = curPayload[i];
            result += pc.GetPoints();
            timeResult += pc.GetTime();
            curPayload.Remove(pc);
            Destroy(pc.gameObject);

        }
        manager.AddToScore(result);
        manager.AddTime(timeResult);
    }

    private void UpgradeTier(int newTier)
    {
        curTier = newTier;
        speed = stats[curTier].speed;
    }

    private void OnGameEnd()
    {
        enabled = false;
    }

    public void ResetAbilities()
    {
        abilityTimers = new float[] { 0f, 0f, 0f, 0f };
    }
}

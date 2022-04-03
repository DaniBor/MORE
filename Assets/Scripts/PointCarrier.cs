using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PointCarrier : MonoBehaviour
{
    [SerializeField]
    private int points;
    [SerializeField]
    private float timeAdded;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform toReach;

    [SerializeField]
    private PCStatSet[] stats;

    private float xDir = 0;
    private float yDir = 0;

    public SpriteRenderer sr;
    [SerializeField]
    private Sprite[] walkAnim;
    [SerializeField]
    private float waitTime;

    public Light2D lght;
    public bool deactivateLightOnDistance;
    public bool beingCarried = true;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(Animate());
    }

    public int GetPoints()
    {
        return points;
    }

    public float GetTime()
    {
        return timeAdded;
    }

    private void Update()
    {
        if (!beingCarried) UpdatePosition();
    }

    private void FixedUpdate()
    {
        if (deactivateLightOnDistance)
        {
            if (Vector2.Distance(transform.position, Manager.globalPlayer.transform.position) > 15f) lght.enabled = false;
            else lght.enabled = true;
        }
    }

    private void UpdatePosition()
    {
        Vector3 target = new Vector3(transform.position.x + speed * xDir, transform.position.y + speed * yDir, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, toReach.position) <= 1f)
        {
            Manager.enemyCount--;
            Destroy(gameObject);
        }
    }

    public void SetValues(Vector3 dirs, Transform despawnPoint, int tier)
    {
        speed = stats[tier].speed;
        timeAdded = stats[tier].timeAdd;

        xDir = dirs.x;
        yDir = dirs.y;
        Vector3 scale = new Vector3(xDir > 0 ? 0.5f : -0.5f, 0.5f, 0.5f);
        transform.localScale = scale;

        toReach = despawnPoint;

        beingCarried = false;
    }

    private IEnumerator Animate()
    {
        int index = 0;
        while (true)
        {
            if (index == walkAnim.Length - 1) index = 0;
            else index++;

            sr.sprite = walkAnim[index];

            yield return new WaitForSeconds(waitTime);
        }
    }
}

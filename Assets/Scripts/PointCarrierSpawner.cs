using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCarrierSpawner : MonoBehaviour
{
    [SerializeField]
    private float minTime;
    [SerializeField]
    private float maxTime;

    private float curTime;
    private int curTier;

    public BoxCollider2D entrySpawner;
    public BoxCollider2D exitPoint;

    private Vector3 relativeDir;

    [SerializeField]
    private GameObject[] pcPrefabPool;
    [SerializeField]
    private GameObject enemyParent;
    [SerializeField]
    private ManagerChannel managerChannel;
    

    private void Awake()
    {
        relativeDir = (exitPoint.transform.position - entrySpawner.transform.position).normalized;

        managerChannel.onTierUpgrade += UpgradeTier;
        curTier = 0;
        curTime = Random.Range(minTime, maxTime);
    }

    private void Update()
    {
        Run();
    }

    private void FixedUpdate()
    {
        
    }

    private void Run()
    {
        curTime -= Time.deltaTime;
        if(curTime <= 0f)
        {
            int rand = Random.Range(0, pcPrefabPool.Length);
            GameObject pcprefab = pcPrefabPool[rand];
            PointCarrier test = Instantiate(pcprefab, entrySpawner.transform.position, Quaternion.identity, enemyParent.transform).GetComponent<PointCarrier>();
            test.SetValues(relativeDir, exitPoint.transform, curTier);
            Manager.enemyCount++;

            curTime = Random.Range(minTime, maxTime);
        }
    }

    private void UpgradeTier(int newTier)
    {
        curTier = newTier;
    }
}

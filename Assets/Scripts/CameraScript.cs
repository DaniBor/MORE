using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    public Transform toFollow;

    private float randx;
    private float randy;
    private Vector3 randShake;
    [SerializeField]
    private float shakeIntensity = 0.1f;

    private void Update()
    {
        if (Manager.alertstate)
        {
            transform.position = new Vector3(toFollow.position.x, toFollow.position.y, transform.position.z) + randShake;
        }
        else
        {
            transform.position = new Vector3(toFollow.position.x, toFollow.position.y, transform.position.z);
        }

        
    }

    private void FixedUpdate()
    {
        randx = Random.Range(-shakeIntensity, shakeIntensity);
        randy = Random.Range(-shakeIntensity, shakeIntensity);
        randShake = new Vector3(randx, randy, 0f);
    }
}

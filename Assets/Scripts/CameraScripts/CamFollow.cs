using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    public static CamFollow instance;

    private Vector3 offSet;
    public Transform target;
    [SerializeField] private float smoothTime;
    private Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        instance = this;
        offSet = transform.position - target.position;
    }

    private void Update()
    {
        Vector3 targetPosition = target.position + offSet;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}

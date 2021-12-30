using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(3, 0.6f, -2.5f), 0.6f);
    }
}

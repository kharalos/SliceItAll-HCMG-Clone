using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    KnifeManager knife;
    void Start()
    {
        knife = GetComponentInParent<KnifeManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            knife.SpinBack();
        }
        if (other.CompareTag("Sliceable") && !other.GetComponent<SlicedManager>().Sliced)
        {
            knife.SpinBack();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut : MonoBehaviour
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
            knife.Freeze();
        }
        if (other.CompareTag("Sliceable"))
        {
            other.GetComponent<SlicedManager>().Slice();
            FindObjectOfType<GameManager>().AddScore();
        }
        if (other.CompareTag("Indestructible"))
        {
            FindObjectOfType<GameManager>().Defeat();
        }
        if (other.CompareTag("Finish"))
        {
            FindObjectOfType<GameManager>().Finish(other.GetComponent<Finish>().scoreMultiplier);
            
        }
    }
}

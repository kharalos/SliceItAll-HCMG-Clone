using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedManager : MonoBehaviour
{
    public enum ObjectType { brick, cake, other}
    public ObjectType type;

    [SerializeField]
    private bool sliced = false;

    [SerializeField]
    private bool soft = false;

    public bool Sliced
    {
        get { return sliced; }
    }

    private MeshRenderer meshRenderer;
    void Start()
    {
        meshRenderer = this?.GetComponent<MeshRenderer>();

        Physics.IgnoreCollision(FindObjectOfType<Handle>().gameObject.GetComponent<Collider>(), GetComponent<Collider>());

        if (type == ObjectType.brick)
        {
            meshRenderer.materials[1].color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
    }
    private void Update()
    {
        if(transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }
    public void Slice()
    {
        if (sliced) return;


        FindObjectOfType<KnifeManager>().SpinUp(soft);

        sliced = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;


        rb.AddForce(transform.forward * rb.mass * 150);
        GetComponentInChildren<ParticleSystem>().Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Handle") && sliced)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * rb.mass * 150);
        }
    }
}

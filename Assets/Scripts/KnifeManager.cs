using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    Rigidbody rb;
    SoundManager sm;

    private float spinSpeed = 100f;
    private bool canStuck = true;
    private bool froze = false;
    private bool spin = true;
    private bool blinking = false;

    public float force = 100;

    [SerializeField]
    private MeshRenderer handleBlinkRend;

    // Býçaðýn yavaþladýðý açý
    private Vector3 knifeDir = ((Vector3.forward * 4) + (Vector3.down * 2)).normalized;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 10f;
        sm = FindObjectOfType<SoundManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Spin();
        }


        float knifeAngle = Vector3.Angle(knifeDir, transform.forward);
        Debug.DrawRay(transform.position, knifeDir, Color.red);


        //Bu açýda býçak yavaþlýyor
        if (canStuck && knifeAngle < 35)
        {
            rb.maxAngularVelocity = 2f;
        }
        else rb.maxAngularVelocity = 10f;

        if (transform.position.y < -1)
        {
            FindObjectOfType<GameManager>().Defeat();
        }
    }
    private void FixedUpdate()
    {
        if (spin)
            rb.AddTorque(Vector3.right * force * spinSpeed, ForceMode.Force);
    }

    private void Spin()
    {
        if (blinking) return;
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.AddTorque(Vector3.right * force * spinSpeed * 2, ForceMode.Impulse);
        rb.AddForce((Vector3.forward + Vector3.up * 2.5f).normalized * force, ForceMode.Impulse);
        canStuck = false;
        froze = false;
        sm.Play("Spin");
        CancelInvoke();
        Invoke("StuckDelay", 0.4f);
    }
    public void Freeze()
    {
        if (canStuck && !froze)
        {
            froze = true;
            sm.Play("MetalHit");
            rb.isKinematic = true;
        }
    }
    private void StuckDelay()
    {
        canStuck = true;
    }
    private void SpinDelay()
    {
        spin = true;
    }

    public void SpinUp(bool isSoft)
    {
        if (!isSoft && canStuck && !blinking)
        {
            rb.angularVelocity = Vector3.zero;
            spin = false;
            CancelInvoke();
            Invoke("SpinDelay", 0.1f);
            rb.velocity = new Vector3(0, rb.velocity.y * 0.9f, 0);
        }
        sm.Play("Cut");
    }
    public void SpinBack()
    {
        if (blinking) return;
        rb.isKinematic = false;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        //rb.AddTorque((Vector3.left + Vector3.up).normalized * force * spinSpeed, ForceMode.Impulse);
        rb.AddForce(-knifeDir * force / 3, ForceMode.Impulse);
        Debug.Log("Spun back.");

        canStuck = false;
        StopAllCoroutines();
        StartCoroutine(BlinkHandle());
        Invoke("StuckDelay", 0.2f);
    }

    private IEnumerator BlinkHandle()
    {
        blinking = true;
        //spin = false;
        for (float i = 0; i < 1; i += 0.04f)
        {
            handleBlinkRend.material.SetColor("_EmissionColor", new Color(i, i, i));
            handleBlinkRend.material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.01f);
        }
        for (float i = 1; i > 0; i -= 0.04f)
        {
            handleBlinkRend.material.SetColor("_EmissionColor", new Color(i, i, i));
            handleBlinkRend.material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.01f);
        }
        handleBlinkRend.material.SetColor("_EmissionColor", new Color(0, 0, 0));
        handleBlinkRend.material.EnableKeyword("_EMISSION");
        //spin = true;
        blinking = false;
    }
}

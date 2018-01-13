using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Stuff : MonoBehaviour
{
    public Rigidbody Body
    { get; private set; }

    private MeshRenderer[] meshRenderers;

    private void Awake()
    {
        FindObjectOfType<Stuff>();
        Body = GetComponent<Rigidbody>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider enteredCollider)
    {
        if(enteredCollider.CompareTag("Kill Zone"))
        {
            Destroy(gameObject);
        }
    }

    public void SetMaterial(Material m)
    {
        for(int i = 0; i < meshRenderers.Length; ++i)
        {
            meshRenderers[i].material = m;
        }
    }
}

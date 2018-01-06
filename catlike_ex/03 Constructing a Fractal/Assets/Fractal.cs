using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    public Mesh mesh;
    public Mesh[] meshes;
    public Material material;

    public int maxDepth;
    public int depth;

    public Material[,] materials;

    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1, 2];
        for(int i = 0; i <= maxDepth; ++i)
        {
            float t = i / (maxDepth - 1f);
            t *= t;
            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
        }
        materials[maxDepth, 0].color = Color.magenta;
        materials[maxDepth, 1].color = Color.red;
    }

    public float maxRotationSpeed;
    private float rotationSpeed;
    public float maxTwist;

    private void Start()
    {
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);
        if(materials == null)
        {
            InitializeMaterials();
        }
        //gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        //gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0, 2)];
        //GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.white, Color.yellow, (float)depth / maxDepth);
        if (depth < maxDepth)
        {
            //new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.up);
            //new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.right);
            StartCoroutine("CreateChildren");
        }
    }

    private static Vector3[] childDirections =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    private static Quaternion[] childOrientation =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

    public float spawnProbability;

    private IEnumerator CreateChildren()
    {
        for(int i = 0; i< childDirections.Length; ++i)
        {
            if(Random.value < spawnProbability)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
            }
            //yield return new WaitForSeconds(0.5f);
        }
        //new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.up, Quaternion.identity);
        //yield return new WaitForSeconds(0.5f);
        //new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.right, Quaternion.Euler(0f, 0f, -90f));
        //yield return new WaitForSeconds(0.5f);
        //new GameObject("Fractal child").AddComponent<Fractal>().Initialize(this, Vector3.left, Quaternion.Euler(0f, 0f, 90f));
    }

    public float childScale;

    private void Initialize(Fractal parent, int childIndex)
    {
        meshes = parent.meshes;
        mesh = parent.mesh;
        materials = parent.materials;
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        spawnProbability = parent.spawnProbability;
        maxRotationSpeed = parent.maxRotationSpeed;
        maxTwist = parent.maxTwist;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = Vector3.up * (0.5f + 0.5f * childScale);
        transform.localPosition = childDirections[childIndex]* (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientation[childIndex];
    }

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}

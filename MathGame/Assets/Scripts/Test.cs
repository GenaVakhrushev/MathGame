using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Mesh mesh;
    float s = 2f;

    Vector3[] origVerts;

    void Start()
    {
        mesh = GetComponentInChildren<MeshFilter>().mesh;

        origVerts = mesh.vertices;

        SetScale(s);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScale(float value)
    {
        Vector3[] vertices = mesh.vertices;

        int leftestVertexIndex = 0;

        for (int i = 0; i < origVerts.Length; i++)
        {
            if(origVerts[i].x < origVerts[leftestVertexIndex].x)
            {
                leftestVertexIndex = i;
            }
        }

        float step = Mathf.Abs(origVerts[leftestVertexIndex].x * value - origVerts[leftestVertexIndex].x);

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            if (origVerts[i].x < 0)
            {
                vertices[i] = new Vector3(origVerts[i].x - step, origVerts[i].y, origVerts[i].z);
            }
            else
            {
                vertices[i] = new Vector3(origVerts[i].x + step, origVerts[i].y, origVerts[i].z);
            }
        }

        mesh.vertices = vertices;
    }
}

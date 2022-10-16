using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Box : MonoBehaviour
{
    [HideInInspector]
    public Vector3 textOffset;

    protected TMP_Text text;
    public TMP_Text Text => text;
    public string Value => text.text;

    protected Renderer rendererComponent;

    public Material material => rendererComponent.material;

    protected Color defaultColor;
    protected Color emissionDefaultColor;

    private Color currentColor => material.GetColor("_BaseColor");
    private Color currentEmissionColor => material.GetColor("_EmissionColor");

    public Color DefaultColor => defaultColor;
    public Color EmissionDefaultColor => emissionDefaultColor;

    //for scale
    private Mesh mesh;
    private Vector3[] origVerts;
    private float currentScale = 1;
    private int leftestVertexIndex = 0;
    private Vector3 leftestVectex => mesh.vertices[leftestVertexIndex] + transform.position;

    protected virtual void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        rendererComponent = GetComponentInChildren<Renderer>();

        textOffset = text.transform.localPosition;

        defaultColor = material.GetColor("_BaseColor");
        emissionDefaultColor = material.GetColor("_EmissionColor");

        mesh = GetComponentInChildren<MeshFilter>().mesh;
        origVerts = mesh.vertices;
        currentScale = 1;
        EvaluateLeftestVertexIndex();
    }

    public void SetColor(Color baseColor, Color emmisionColor, float emissionLevel)
    {
        material.SetColor("_BaseColor", baseColor);
        StartCoroutine(ChangeEmmision(emmisionColor, emissionLevel, 2));
    }

    private IEnumerator ChangeEmmision(Color emmisionColor, float emissionLevel, float speed)
    {
        Color targetColor = emmisionColor * emissionLevel;
       
        while (Vector4.Magnitude(currentEmissionColor - targetColor) > 0.5f)
        {
            material.SetColor("_EmissionColor", Color.Lerp(currentEmissionColor , targetColor, speed * Time.deltaTime));
            yield return null;
        }
        material.SetColor("_EmissionColor", targetColor);
    }

    public void FitToText(float speed)
    {
        StartCoroutine(ExpandBox(speed, 0.1f));
    }

    public void FitToText(float speed, float offset)
    {
        StartCoroutine(ExpandBox(speed, offset));
    }

    private IEnumerator ExpandBox(float speed, float offset)
    {
        Vector3 firstLetterPos = text.textInfo.characterInfo[0].bottomLeft + transform.position;
        Vector3 targetPos = firstLetterPos + new Vector3(-offset, 0, 0);

        while (leftestVectex.x > targetPos.x)
        {
            SetScale(currentScale + speed * Time.deltaTime);
            yield return null;
        }
    }

    public void BackToNormalSize(float duration)
    {
        StartCoroutine(CompressBox(duration));
    }

    private IEnumerator CompressBox(float duration)
    {
        float delta = currentScale - 1;

        while (currentScale > 1)
        {
            SetScale(currentScale - delta * Time.deltaTime / duration);
            yield return null;
        }
    }

    private float EvaluateLeftestVertexIndex()
    {
        leftestVertexIndex = 0;

        for (int i = 0; i < origVerts.Length; i++)
        {
            if (origVerts[i].x < origVerts[leftestVertexIndex].x)
            {
                leftestVertexIndex = i;
            }
        }

        return leftestVertexIndex;
    }

    public void SetScale(float value)
    {
        currentScale = value;

        Vector3[] vertices = mesh.vertices;

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

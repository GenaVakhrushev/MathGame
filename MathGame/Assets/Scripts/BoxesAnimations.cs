using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoxesAnimations : Singleton<BoxesAnimations>
{
    [SerializeField] private Box EquationBox;
    [SerializeField] private Box[] AnswerBoxes;

    private Transform EquationBoxModelTransform => EquationBox.transform.GetChild(0);
    private TMP_Text EquationBoxText => EquationBox.Text;

    public IEnumerator CompressEquationBox(float duration)
    {
        float step = (EquationBoxModelTransform.localScale.x - 1);
        while (EquationBoxModelTransform.localScale.x > 1f)
        {
            EquationBoxModelTransform.localScale -= new Vector3(step * Time.deltaTime / duration, 0, 0);
            yield return null;
        }
    }

    public IEnumerator ExpandEquationBox(float duration)
    {
        Vector3 firstLetterPos = EquationBoxText.textInfo.characterInfo[0].bottomLeft;
        Vector3 lastLetterPos = EquationBoxText.textInfo.characterInfo[EquationBoxText.text.Length - 1].bottomRight;

        float targetScale = Mathf.Abs(firstLetterPos.x - lastLetterPos.x) * 3;
        float step = (targetScale - EquationBoxModelTransform.localScale.x);
        while (EquationBoxModelTransform.localScale.x < targetScale)
        {
            EquationBoxModelTransform.localScale += new Vector3(step * Time.deltaTime / duration, 0, 0);
            yield return null;
        }
    }

    public IEnumerator MakeGlowAllBoxes(float duration)
    {
        float currentIntensity = 1;
        float tagretIntensity = 5;
        float step = tagretIntensity - currentIntensity;
        while (currentIntensity < tagretIntensity)
        {
            EquationBox.SetColor(EquationBox.DefaultColor, EquationBox.EmissionDefaultColor, currentIntensity);
            foreach (Box box in AnswerBoxes)
            {
                box.SetColor(box.DefaultColor, box.EmissionDefaultColor, currentIntensity);
            }
            currentIntensity += step / duration * Time.deltaTime;

            yield return null;
        }
    }

    public IEnumerator MoveRightAnswerText(TMP_Text answerText, float duration, float pause)
    {
        TMP_CharacterInfo xInfo = EquationBoxText.textInfo.characterInfo[EquationBoxText.text.IndexOf("X")];

        Vector3 xPos = (xInfo.bottomLeft + xInfo.bottomRight + xInfo.topLeft + xInfo.topRight) / 4;
        xPos.z = answerText.transform.position.z - 0.1f;

        float dist = Vector3.Distance(answerText.transform.position, xPos);

        while (Vector3.Distance(answerText.transform.position, xPos) > 0.01f)
        {
            Vector3 newPos = Vector3.Slerp(answerText.transform.position, xPos, Time.deltaTime / duration);
            newPos.z = xPos.z;
            answerText.transform.position = newPos;

            yield return null;
        }

        yield return new WaitForSeconds(pause);

        GameManager.MoveToNextEquasion();
    }

    public void ChangeEquationText()
    {
        EquationDisplay.DisplayEquationParams(GameManager.Instance.CurrentEquation);

        foreach (Box box in AnswerBoxes)
        {
            box.Text.transform.position = box.transform.position + box.textOffset;
        }
    }
}

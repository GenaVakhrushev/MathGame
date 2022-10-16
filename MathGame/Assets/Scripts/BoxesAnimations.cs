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

    public void CompressBoxes(float duration)
    {
        EquationBox.BackToNormalSize(duration);

        foreach (Box box in AnswerBoxes)
        {
            box.BackToNormalSize(duration);
        }
    }

    public void ExpandBoxes(float speed)
    {
        EquationBox.FitToText(speed);

        foreach (Box box in AnswerBoxes)
        {
            box.FitToText(speed);
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
        float remainingTime = duration;
        Vector3 answerTextPos = answerText.transform.position;

        Vector3 direction = Vector3.left;

        TMP_CharacterInfo xInfo = EquationBoxText.textInfo.characterInfo[EquationBoxText.text.IndexOf("X")];
        Vector3 xPos = (xInfo.bottomLeft + xInfo.bottomRight + xInfo.topLeft + xInfo.topRight) / 4;
        xPos.z = answerTextPos.z - 0.15f;

        Vector3 center = (answerTextPos + xPos) * 0.5f;
        center -= direction;    
        Vector3 toStart = answerTextPos - center;
        Vector3 toEnd = xPos - center;

        while (remainingTime > 0)
        {
            answerText.transform.position = Vector3.Slerp(toStart, toEnd,  (duration - remainingTime) / duration);
            answerText.transform.position += center;

           remainingTime -= Time.deltaTime;

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

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class EquationDisplay : Singleton<EquationDisplay>
{
    [SerializeField] private TMP_Text equationText;

    [SerializeField] private TMP_Text[] answerBoxTextes;

    private Box EquationBox;

    private static int rightAnswerIndex = -1;

    private static List<string> currentAnswers = new List<string>();

    private void Start()
    {
        EquationBox = equationText.GetComponentInParent<Box>();
    }

    public static void DisplayEquationParams(Equation equation)
    {
        currentAnswers.Clear();

        int xNumsCount = equation.EquationText.Split(new string[] { equation.X }, System.StringSplitOptions.None).Length - 1;
        int replaceCount = xNumsCount > (equation.XOrderNumber + 1) ? (equation.XOrderNumber + 1) : xNumsCount;

        Regex regex = new Regex(Regex.Escape(equation.X));
        string result = regex.Replace(equation.EquationText, "X", replaceCount);

        regex = new Regex(Regex.Escape("X"));
        result = regex.Replace(result, equation.X, replaceCount - 1);

        Instance.equationText.text = result;

        rightAnswerIndex = Random.Range(0, Instance.answerBoxTextes.Length);

        Instance.answerBoxTextes[rightAnswerIndex].text = equation.X;

        for (int i = 0; i < Instance.answerBoxTextes.Length; i++)
        {
            if(i == rightAnswerIndex)
            {
                continue;
            }

            string newWrongAnswer = GetWrongAnswer(equation, i < rightAnswerIndex ? i : i - 1);

            currentAnswers.Add(newWrongAnswer);

            Instance.answerBoxTextes[i].text = newWrongAnswer;
        }
    }

    public static void HideX(string x)
    {
        string replacement = "";

        for (int i = 0; i < x.Length; i++)
        {
            replacement += "  ";
        }
        
        Regex regex = new Regex(Regex.Escape("X"));
        Instance.equationText.text = regex.Replace(Instance.equationText.text, replacement);
        Instance.EquationBox.FitToText(3, 0.1f * x.Length);
    }

    private static string GetWrongAnswer(Equation equation, int answerIndex)
    {
        switch (equation.WrongAnswerMethod)
        {
            case WrongAnswerMethod.Random:
                string answer = "";
                do
                {
                    answer = GetGandomAnswer(equation);
                } while (currentAnswers.Contains(answer));
                return answer;
            case WrongAnswerMethod.Manual:
                return equation.WrongAnswers[answerIndex];
        }

        return "";
    }

    private static string GetGandomAnswer(Equation equation)
    {
        int rightAnswerInt;

        if (int.TryParse(equation.X, out rightAnswerInt))
        {
            return (rightAnswerInt + Random.Range(1, 5) * (Random.Range(0, 2) == 0 ? -1 : 1)).ToString();
        }
        else
        {
            float rightAnswer = float.Parse(equation.X);
            return (rightAnswer + Random.Range(1f, 5f) * (Random.Range(0, 2) == 0 ? -1 : 1)).ToString();
        }
    }
}

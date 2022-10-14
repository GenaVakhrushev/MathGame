using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "Equation", menuName = "Equations/Equation")]
public class Equation : ScriptableObject
{
    //information to input
    public string EquationText;
    
    public WrongAnswerMethod WrongAnswerMethod;

    public string[] WrongAnswers;


    //information about X, can be changed by SetX function or by EquationEditor
    public int XOrderNumber { get; private set; }

    private string x = "";
    public string X
    {
        get
        {
            //value is not setted or not formatted
            if (x == "" || x.Contains("(") || x.Contains(")"))
            {
                UpdateNumbersList();
                x = GetFirstFloatFromString(numbers[0]);
            }
            return x;
        }
        private set
        {
            x = GetFirstFloatFromString(value);
        }
    }


    //data about numbers of equation
    private List<string> numbers = new List<string>();
    public List<string> Numbers => numbers;

    public void SetX(string x, int orderNum)
    {
        X = x;
        XOrderNumber = orderNum;
    }

    public void UpdateNumbersList()
    {
        numbers.Clear();

        string equationCopy = EquationText;

        string value = GetFirstFloatFromString(equationCopy);

        int numberPos = 1;

        while (value != "")
        {
            numbers.Add(value + " (" + numberPos.ToString() + ")");

            Regex regex = new Regex(Regex.Escape(value));
            equationCopy = regex.Replace(equationCopy, "|", 1);

            value = GetFirstFloatFromString(equationCopy);

            numberPos++;
        }
    }

    private string GetFirstFloatFromString(string str)
    {
        string pattern = @"([0-9]*\.?[0-9]+)|([\(*/]-[0-9]*\.?[0-9]+)|(^-[0-9]*\.?[0-9]+)";

        Match match = Regex.Match(str, pattern);

        return Regex.Replace(match.Groups[0].Value, @"[\(*/]", "");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationsManager : Singleton<EquationsManager>
{
    [SerializeField] private List<Equation> equations = new List<Equation>();

    public static Equation GetRandomEquation()
    {
        if(Instance.equations.Count == 0)
        {
            return null;
        }

        return Instance.equations[Random.Range(0, Instance.equations.Count)];
    }

    public static Equation[] GetRandomEquations(int count)
    {
        List<Equation> equationsCopy = new List<Equation>(Instance.equations);
        List<Equation> result = new List<Equation>();

        for (int i = 0; i < count; i++)
        {
            if(equationsCopy.Count == 0)
            {
                equationsCopy = new List<Equation>(Instance.equations);

                Debug.LogWarning("Not enough equations, equations will duplicate");
            }

            Equation equation = equationsCopy[Random.Range(0, equationsCopy.Count)];

            result.Add(equation);

            equationsCopy.Remove(equation);
        }

        return result.ToArray();
    }
}

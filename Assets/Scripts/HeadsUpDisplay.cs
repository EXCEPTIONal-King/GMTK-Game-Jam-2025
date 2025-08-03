using System.Collections.Generic;
using UnityEngine;

public class HeadsUpDisplay : MonoBehaviour
{
    [SerializeField] Transform objectives;
    [SerializeField] Transform limitations;

    [SerializeField] GameObject objectiveTemplatePrefab;
    [SerializeField] GameObject limitationTemplatePrefab;
    [SerializeField] MaterialBundle defaultMaterials;

    private Dictionary<BoxColor, int> countPerColor = new Dictionary<BoxColor, int>();
    private GameObject lastInstantiatedElement;

    public void AddObjective(BoxColor boxColor)
    {
        if (!countPerColor.ContainsKey(boxColor))
        {
            countPerColor.Add(boxColor, 1);
            lastInstantiatedElement = Instantiate(objectiveTemplatePrefab, objectives);
            lastInstantiatedElement.name = $"{boxColor}"BoxCount";
            // TODO: set material for UI
            // lastInstantiatedElement.
        }
        else
        {
            countPerColor[boxColor]++;
        }
    }
}

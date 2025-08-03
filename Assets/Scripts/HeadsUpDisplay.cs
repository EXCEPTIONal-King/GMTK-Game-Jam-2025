using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeadsUpDisplay : MonoBehaviour
{
    [SerializeField] Transform objectives;
    [SerializeField] Transform limitations;

    [SerializeField] GameObject objectiveTemplatePrefab;
    [SerializeField] GameObject limitationTemplatePrefab;
    [SerializeField] MaterialBundle defaultMaterials;

    private Dictionary<BoxColor, int> countPerColor = new Dictionary<BoxColor, int>();
    private Dictionary<BoxColor, int> successPerColor = new Dictionary<BoxColor, int>();
    private GameObject lastInstantiatedElement;

    public void AddObjective(BoxColor boxColor)
    {
        if (!countPerColor.ContainsKey(boxColor))
        {
            countPerColor.Add(boxColor, 1);
            lastInstantiatedElement = Instantiate(objectiveTemplatePrefab, objectives);
            lastInstantiatedElement.name = $"{boxColor}BoxCount";
            List<Material> mats = new List<Material>();
            mats.Add(defaultMaterials.FromBoxColor(boxColor));
            mats.Add(defaultMaterials.yellowMat);
            lastInstantiatedElement.GetComponentInChildren<MeshRenderer>().SetMaterials(mats);
        }
        else
        {
            countPerColor[boxColor]++;
            UpdateText(boxColor);
        }
    }

    public void CompleteObjective(BoxColor boxColor)
    {
        if (!successPerColor.ContainsKey(boxColor))
        {
            successPerColor.Add(boxColor, 1);
        }
        else
        {
            successPerColor[boxColor]++;
        }
        UpdateText(boxColor);
    }

    void UpdateText(BoxColor boxColor)
    {
        int successes = 0;
        if (successPerColor.ContainsKey(boxColor))
        {
            successes = successPerColor[boxColor];
        }
        objectives.Find($"{boxColor}BoxCount").GetComponent<TextMeshProUGUI>()
            .text = $"{successes} of {countPerColor[boxColor]}";
    }

    public void AddLimitation(int charges)
    {
        lastInstantiatedElement = Instantiate(limitationTemplatePrefab, limitations);
        lastInstantiatedElement.GetComponent<TextMeshProUGUI>().text = $"Remaining: {charges}";
    }

    public void ConsumeLimitation(int remainingCharges)
    {
        limitations.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Remaining: {remainingCharges}";
    }
}

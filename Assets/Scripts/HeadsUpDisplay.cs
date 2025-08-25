using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HeadsUpDisplay : MonoBehaviour
{
    [SerializeField] Transform objectives;
    [SerializeField] Transform limitations;

    [SerializeField] GameObject objectiveTemplatePrefab;
    [SerializeField] GameObject limitationTemplatePrefab;
    [SerializeField] GameObject conveyorLabelPrefab;
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

    public void AddLimitation(string label, int charges)
    {
        lastInstantiatedElement = Instantiate(limitationTemplatePrefab, limitations);
        lastInstantiatedElement.name = $"{label}Limit";
        lastInstantiatedElement.GetComponent<TextMeshProUGUI>().text = $"{label} Left: {charges}";
    }

    public void ConsumeLimitation(string label, int remainingCharges)
    {
        limitations.Find($"{label}Limit").GetComponent<TextMeshProUGUI>()
            .text = $"{label} Left: {remainingCharges}";
    }

    public void LabelConveyor(int conveyorId, Vector3 worldPosition)
    {
        lastInstantiatedElement = Instantiate(conveyorLabelPrefab, transform);
        lastInstantiatedElement.transform.position = worldPosition;
        lastInstantiatedElement.name = $"ConveyorLabel{conveyorId}";
        lastInstantiatedElement.GetComponent<TextMeshProUGUI>().text = "" + conveyorId;
    }

    public void SelectConveyor(int conveyorId)
    {
        transform.Find($"ConveyorLabel{conveyorId}").GetComponent<Animator>()
            .SetBool("Selected", true);
    }

    public void DeselectConveyor(int conveyorId)
    {
        transform.Find($"ConveyorLabel{conveyorId}").GetComponent<Animator>()
            .SetBool("Selected", false);
    }

    public Boolean IsLevelWon()
    {
        foreach (BoxColor color in countPerColor.Keys)
        {
            if (!successPerColor.ContainsKey(color)) return false;
            if (countPerColor[color] != successPerColor[color]) return false;
        }
        return true;
    }
}

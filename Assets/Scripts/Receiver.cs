using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    [SerializeField]
    float spinSpeed = 5;

    // match the colour of the box
    [SerializeField] BoxColor expectedBoxColor;
    [SerializeField] MaterialBundle defaultMaterials;
    List<Material> materialList = new List<Material>();
    MeshRenderer meshRenderer;

    // pickup logic
    [SerializeField] Vector2Int receiverPos;
    [SerializeField] Vector2Int pickupPos;
    GridSystem grid;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // grid init TODO: ben please check if I did this right
        grid = GameObject.FindAnyObjectByType<GridSystem>();
        grid.AddReceiver(this, pickupPos.y, pickupPos.x);

        // Set the color according to the expected box color
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        materialList.Add(defaultMaterials.FromBoxColor(expectedBoxColor));
        meshRenderer.SetMaterials(materialList);
    }

    // Update is called once per frame
    void Update()
    {
        Spin();
    }

    void Spin()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    public Vector2Int GetPickupPos()
    {
        return pickupPos;
    }

    public void CheckBox(Box box)
    {
        // TODO: score & lose condition
        if (box.GetBoxColor() == expectedBoxColor)
        {
            Debug.Log($"I just got my correct {expectedBoxColor} box!");
        }
        else
        {
            Debug.Log("Package was misdelivered!");
        }
    }
}

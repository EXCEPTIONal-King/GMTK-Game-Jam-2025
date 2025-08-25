using System;
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
    Boolean complete = false;
    HeadsUpDisplay hud;
    LevelScreens screens;
    LevelHandler levelHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // grid init TODO: ben please check if I did this right
        grid = GameObject.FindAnyObjectByType<GridSystem>();
        grid.AddReceiver(this, pickupPos.y, pickupPos.x);
        screens = GameObject.FindAnyObjectByType<LevelScreens>();

        // Set the color according to the expected box color
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        materialList.Add(defaultMaterials.FromBoxColor(expectedBoxColor));
        meshRenderer.SetMaterials(materialList);

        hud = GameObject.FindAnyObjectByType<HeadsUpDisplay>();
        levelHandler = GameObject.FindAnyObjectByType<LevelHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        Spin();
        if (complete) Ascend();
    }

    void Spin()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    void Ascend()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,transform.position.y+1f,transform.position.z), Time.deltaTime * 2.0f);
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
            hud.CompleteObjective(box.GetBoxColor());
            grid.AddReceiver(null, pickupPos.y, pickupPos.x); //remove receiver from grid
            complete = true;
            if (hud.IsLevelWon()) levelHandler.AdvanceLevel();//screens.EndLevel(true);
        }
        else
        {
            Debug.Log("Package was misdelivered!");
            levelHandler.ResetLevel();//screens.EndLevel(false);
        }
    }
}

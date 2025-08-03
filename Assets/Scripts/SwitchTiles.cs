using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchTiles : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    GridSystem.Location[] targets_locations = new GridSystem.Location[2];
    [SerializeField] Vector2Int[] targets = new Vector2Int[2];
    GridSystem grid;

    void Start()
    {
        Controls con = new Controls();
        con.GamePlay.Enable();

        con.GamePlay.TransferPoint.performed += MarkUpcomingTransfer;

        grid = GameObject.FindAnyObjectByType<GridSystem>();
        targets_locations[0] = grid.CheckLocation(targets[0].y, targets[0].x);
        targets_locations[1] = grid.CheckLocation(targets[1].y, targets[1].x);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MarkUpcomingTransfer(InputAction.CallbackContext context)
    {
        grid.UpcomingTransfer();
        SwitchBoxes();
    }

    public void SwitchBoxes()
    {

        print("Switch Boxes");
        Box box_1;
        Box box_2;

        box_1 = targets_locations[0].GetBox();
        box_2 = targets_locations[1].GetBox();

        if (box_1 != null)
        {
            targets_locations[1].Add(box_1);//replace box in loc[1] with loc[0] box
            targets_locations[1].GetConveyor().GetConveyorLoop().AddBox(box_1);
            box_1.SetPos(targets_locations[1].GetPos());
            box_1.RecalcDestinations(); //make box start moving on another conveyor
            targets_locations[0].RemoveBox();
            targets_locations[0].GetConveyor().GetConveyorLoop().RemoveBox(box_1);
        }


        if (box_2 != null)
        {
            targets_locations[0].Add(box_2); //replace box in loc[0] with box originally in loc[1]
            targets_locations[0].GetConveyor().GetConveyorLoop().AddBox(box_2);
            box_2.SetPos(targets_locations[0].GetPos());
            box_2.RecalcDestinations();
            if (box_1 == null) targets_locations[1].RemoveBox();
            targets_locations[1].GetConveyor().GetConveyorLoop().RemoveBox(box_2);
        }
        

        Boolean b1moved = box_1 == targets_locations[1].GetBox();
        Boolean b2moved = box_2 == targets_locations[0].GetBox();
        Boolean b1removed = box_1 != targets_locations[0].GetBox();
        Boolean b2removed = box_2 != targets_locations[1].GetBox();
        print("Box 1 moved" + b1moved + b1removed);
        print("Box 2 moved" + b2moved + b2removed);
        if (targets_locations[0].GetBox() != null) print(targets_locations[0].GetPos());
        if (targets_locations[1].GetBox() != null) print(targets_locations[0].GetPos());


    }
}

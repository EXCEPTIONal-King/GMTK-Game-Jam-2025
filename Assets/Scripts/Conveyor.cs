using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Rendering;

public class Conveyor : MonoBehaviour
{
    Box[] boxes;
    ConveyorUnit[] segments;
    [SerializeField] ConveyorUnit starting_rotation;
    GridSystem grid;

    void Start()
    {
        boxes = FindObjectsByType<Box>(FindObjectsSortMode.None);
        // TODO: use Box.SetPoints to get the box moving

        grid = GameObject.FindAnyObjectByType<GridSystem>();
        print(grid.GetWidth());

        //add the children to segments
        segments = gameObject.GetComponentsInChildren<ConveyorUnit>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //sets initial rotation of entire conveyor
    //also registers each segment to this conveyor while it's at it
    void SetRotation()
    {
        ConveyorUnit.ConveyorDirection prev_tile;

        //do not start w/ a right corner piece
        if (starting_rotation.GetConveyorType() == ConveyorUnit.ConveyorType.Horizontal)        
            starting_rotation.SetDirection(ConveyorUnit.ConveyorDirection.Right);        
        else if (starting_rotation.GetConveyorType() == ConveyorUnit.ConveyorType.Vertical)        
            starting_rotation.SetDirection(ConveyorUnit.ConveyorDirection.Down);        
        else        
            starting_rotation.SetDirection(ConveyorUnit.ConveyorDirection.Right);
        

        prev_tile = starting_rotation.GetConveyorDirection();
        starting_rotation.SetConveyorLoop(this);

        ConveyorUnit curr = grid.NextLocationOnConveyor(starting_rotation.GetPos().y, starting_rotation.GetPos().x).GetConveyor();
        
        while (curr != starting_rotation)
        {
            curr.SetConveyorLoop(this);

            if (curr.GetConveyorType() == ConveyorUnit.ConveyorType.Horizontal || curr.GetConveyorType() == ConveyorUnit.ConveyorType.Vertical)
            {
                curr.SetDirection(prev_tile);
            }

            // Corner pieces spinning
            if (curr.GetConveyorType() == ConveyorUnit.ConveyorType.CornerTR) {
                if (prev_tile == ConveyorUnit.ConveyorDirection.Right) {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Down);
                } else {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Left);
                }
            }
            if (curr.GetConveyorType() == ConveyorUnit.ConveyorType.CornerTL) {
                if (prev_tile == ConveyorUnit.ConveyorDirection.Left) {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Down);
                } else {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Right);
                }
            }
            if (curr.GetConveyorType() == ConveyorUnit.ConveyorType.CornerBL) {
                if (prev_tile == ConveyorUnit.ConveyorDirection.Left) {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Up);
                } else {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Right);
                }
            }
            if (curr.GetConveyorType() == ConveyorUnit.ConveyorType.CornerBR) {
                if (prev_tile == ConveyorUnit.ConveyorDirection.Right) {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Up);
                } else {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Left);
                }
            }

            //iterate
            prev_tile = curr.GetConveyorDirection();            
            curr = grid.NextLocationOnConveyor(curr.GetPos().y, curr.GetPos().x).GetConveyor();
        }
    }

    public Vector3[] BuildDestinations()
    {
        float elevation = .5f;
        ConveyorUnit curr = starting_rotation;
        Vector3[] destinations = new Vector3[segments.Length];
        //first destination
        int index = 0;
        destinations[index++] = new Vector3(curr.GetPos().y + 1.25f, elevation, curr.GetPos().x + 1.25f;

        curr = grid.NextLocationOnConveyor(curr.GetPos().y, curr.GetPos().x).GetConveyor();
        while (curr != starting_rotation)
        {
            if (index >= segments.Length) break;
            destinations[index++] = new Vector3(curr.GetPos().y + (float)1.25, elevation, curr.GetPos().x + (float)1.25);
            curr = grid.NextLocationOnConveyor(curr.GetPos().y, curr.GetPos().x).GetConveyor();
        }

        return destinations;
        
    }

    //reverses rotation of entire conveyor
    void ReverseRotation()
    {
        
    }

}

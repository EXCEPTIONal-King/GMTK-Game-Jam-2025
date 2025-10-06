using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Conveyor : MonoBehaviour
{
    Box[] boxes;
    ConveyorUnit[] segments;
    [SerializeField] ConveyorUnit starting_rotation;
    [SerializeField] float conveyorSpeed;
    GridSystem grid;
    [SerializeField] int reverse_limit;
    [SerializeField] Boolean irreversible;

    HeadsUpDisplay hud;
    [SerializeField] int conveyorId;
    string limitationLabel;

    void Start()
    {


        //con_schem.GamePlay.SetCallbacks(this);

        //boxes = FindObjectsByType<Box>(FindObjectsSortMode.None);
        // TODO: use Box.SetPoints to get the box moving

        grid = GameObject.FindAnyObjectByType<GridSystem>();
        print(grid.GetWidth());

        boxes = new Box[grid.GetNumBoxes()];

        //add the children to segments
        segments = gameObject.GetComponentsInChildren<ConveyorUnit>();
        foreach (ConveyorUnit segment in segments)
        {
            grid.AddConveyor(segment, segment.GetPos().y, segment.GetPos().x);
        }

        SetRotation();

        hud = GameObject.FindAnyObjectByType<HeadsUpDisplay>();
        limitationLabel = $"#{conveyorId}";
        hud.AddLimitation(limitationLabel, reverse_limit);
        hud.LabelConveyor(conveyorId, transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //sets initial rotation of entire conveyor
    //also registers each segment to this conveyor while it's at it
    void SetRotation()
    {
        print("SETTING ROTATION");
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
        print(starting_rotation.GetConveyorDirection());

        ConveyorUnit curr = grid.NextLocationOnConveyor(starting_rotation.GetPos().y, starting_rotation.GetPos().x).GetConveyor();

        while (curr != starting_rotation)
        {
            curr.SetConveyorLoop(this);

            if (curr.GetConveyorType() == ConveyorUnit.ConveyorType.Horizontal || curr.GetConveyorType() == ConveyorUnit.ConveyorType.Vertical)
            {
                curr.SetDirection(prev_tile);
            }

            // Corner pieces spinning
            if (curr.GetConveyorType() == ConveyorUnit.ConveyorType.CornerTR)
            {
                if (prev_tile == ConveyorUnit.ConveyorDirection.Right)
                {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Down);
                }
                else
                {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Left);
                }
            }
            if (curr.GetConveyorType() == ConveyorUnit.ConveyorType.CornerTL)
            {
                if (prev_tile == ConveyorUnit.ConveyorDirection.Left)
                {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Down);
                }
                else
                {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Right);
                }
            }
            if (curr.GetConveyorType() == ConveyorUnit.ConveyorType.CornerBL)
            {
                if (prev_tile == ConveyorUnit.ConveyorDirection.Left)
                {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Up);
                }
                else
                {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Right);
                }
            }
            if (curr.GetConveyorType() == ConveyorUnit.ConveyorType.CornerBR)
            {
                if (prev_tile == ConveyorUnit.ConveyorDirection.Right)
                {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Up);
                }
                else
                {
                    curr.SetDirection(ConveyorUnit.ConveyorDirection.Left);
                }
            }

            if (curr.GetConveyorDirection() == ConveyorUnit.ConveyorDirection.Up || curr.GetConveyorDirection() == ConveyorUnit.ConveyorDirection.Left)
            {
                curr.SetArrowFlip(true);
            }
            else
            {
                curr.SetArrowFlip(false);
            }
            print(curr.GetConveyorDirection());

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
        destinations[index++] = new Vector3(2.5f * curr.GetPos().y + 1.25f, elevation, 2.5f * curr.GetPos().x + 1.25f);

        curr = grid.NextLocationOnConveyor(curr.GetPos().y, curr.GetPos().x).GetConveyor();
        while (curr != starting_rotation)
        {
            if (index >= segments.Length) break;
            destinations[index++] = new Vector3(2.5f * curr.GetPos().y + 1.25f, elevation, 2.5f * curr.GetPos().x + 1.25f);
            curr = grid.NextLocationOnConveyor(curr.GetPos().y, curr.GetPos().x).GetConveyor();
        }

        return destinations;

    }


    //reverses rotation of entire conveyor
    // TODO: update boxes current index after changes to stop box from moving erratically on reverse
    // Check that reverse only flips around direction, does not affect coordinates
    public void ReverseRotation(Boolean all_conveyors)
    {
        //stop reverse
        if ((!all_conveyors && reverse_limit <= 0) || irreversible)
        {
            return;
        }

        //individual reverses counter
        if (!all_conveyors) reverse_limit--;

        ConveyorUnit curr = starting_rotation;
        curr.Reverse();
        curr = grid.NextLocationOnConveyor(curr.GetPos().y, curr.GetPos().x).GetConveyor();
        while (curr != starting_rotation)
        {
            curr.Reverse();
            curr = grid.NextLocationOnConveyor(curr.GetPos().y, curr.GetPos().x).GetConveyor();
        }

        foreach (Box box in boxes)
        {
            if (box != null)
            {
                print("Box recalc" + box.GetBoxID());

                box.SetPoints(BuildDestinations());
                box.RecalcCurrentIndex(true);
            }
        }

        hud.ConsumeLimitation(limitationLabel, reverse_limit);
    }

    public void AddBox(Box box)
    {
        boxes[box.GetBoxID()] = box;
        box.SetSpeed(conveyorSpeed);
    }

    public int GetConveyorId()
    {
        return conveyorId;
    }

    public void RemoveBox(Box box)
    {
        boxes[box.GetBoxID()] = null;
    }
}

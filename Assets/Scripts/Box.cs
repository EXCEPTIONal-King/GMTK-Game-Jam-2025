using System;
using UnityEngine;

public class Box : MonoBehaviour
{

    [SerializeField] Vector2Int pos;

    [SerializeField]
    float speed = 5;
    [SerializeField]
    float threshold = 0.01f;

    [SerializeField]  // TODO: remove when grid is in place - handled in Conveyor
    Vector3[] destinations;
    int currentIndex = 0;
    Boolean conveyor_initialized = false;



    [SerializeField] int box_id; //unique to a box, used to index position in array in conveyor (don't just use color in case we want to make puzzles with multiple boxes of the same color)


    GridSystem grid;
    Conveyor conveyor_loop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //I may only want to call the starting code for box after the conveyors get added to the GridSystem... I can't think of how to do that right now
    void Start()
    {
        grid = GameObject.FindAnyObjectByType<GridSystem>();
        print(grid);
        grid.AddBox(this, pos.y, pos.x);

        //need to find next location in destinations to start
        currentIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //sets conveyor stuff after everything has a chance to get into the grid system - there's probably a better way to do this than running it every frame
        if (Time.time > .2f && conveyor_initialized == false)
        {

            conveyor_loop = grid.CheckLocation(pos.y, pos.x).GetConveyor().GetConveyorLoop();
            conveyor_loop.AddBox(this); //register the box to the loop
            SetPoints(conveyor_loop.BuildDestinations());
            RecalcCurrentIndex(false);
            
            conveyor_initialized = true;
        }
        if (conveyor_initialized) //only do movement after conveyor initialized
            if (grid.NextLocationOnConveyor(pos.y, pos.x).IsClear() && Vector3.Distance(transform.position, destinations[currentIndex]) < threshold)
            {
                print(currentIndex);
                print(destinations[currentIndex]);
                currentIndex++;
                grid.PushBox(this, pos.y, pos.x);


                if (currentIndex >= destinations.Length)
                {
                    currentIndex = 0;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, destinations[currentIndex], Time.deltaTime * speed);
            }
    }

    //Main logic in Conveyor, called in Update
    public void SetPoints(params Vector3[] points)
    {
        destinations = points;
    }

    //Updates currentIndex (and returns the new index)
    //made to fix box movenment upon reversing
    public int RecalcCurrentIndex(Boolean reverse)
    {

        float elevation;
        if (reverse)
        {
            ConveyorUnit reverse_to_pos = grid.CheckLocation(pos.y, pos.x).GetConveyor();
            elevation = .5f; //must match the value in Conveyor's BuildDestinations
            currentIndex = Array.IndexOf(destinations, new Vector3(2.5f * reverse_to_pos.GetPos().y + 1.25f, elevation, 2.5f * reverse_to_pos.GetPos().x + 1.25f));
            return currentIndex;
        }
        ConveyorUnit next_pos = grid.NextLocationOnConveyor(pos.y, pos.x).GetConveyor();
        elevation = .5f; //must match the value in Conveyor's BuildDestinations
        currentIndex = Array.IndexOf(destinations, new Vector3(2.5f * next_pos.GetPos().y + 1.25f, elevation, 2.5f * next_pos.GetPos().x + 1.25f));
        
        return currentIndex;
    }

    public void SetPos(Vector2Int new_pos)
    {
        pos = new_pos;
    }

    public int GetBoxID()
    {
        return box_id;
    }
}

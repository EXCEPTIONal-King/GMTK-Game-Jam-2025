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

    GridSystem grid;
    Conveyor conveyor_loop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //I may only want to call the starting code for box after the conveyors get added to the GridSystem... I can't think of how to do that right now
    void Start()
    {
        grid = GameObject.FindAnyObjectByType<GridSystem>();

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

        }

        if (Vector3.Distance(transform.position, destinations[currentIndex]) < threshold)
        {
            currentIndex++;
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

    public void SetPoints(params Vector3[] points)
    {
        destinations = points;
        // TODO: this will eventually depend on the grid system methinks
        // but I'll just set the points in the inspector for now
    }
}

using UnityEngine;
using System.Collections;
using System;

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
    void SetRotation()
    {

    }


    //reverses rotation of entire conveyor
    void ReverseRotation()
    {
        
    }

}

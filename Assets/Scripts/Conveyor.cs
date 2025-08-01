using UnityEngine;
using System.Collections;
using System;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    ConveyorUnit[] segments;
    [SerializeField] ConveyorUnit starting_rotation;
    GridSystem grid;

    void Start()
    {

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

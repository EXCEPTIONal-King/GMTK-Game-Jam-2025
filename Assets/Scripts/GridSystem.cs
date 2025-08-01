using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;

//Could be made obsolete if conveyors and conveyor units handle the objects on them. I'll assume we use this for now though
public class GridSystem : MonoBehaviour
{

    [SerializeField] int horizontal_size;
    [SerializeField] int vertical_size;

    public int GetWidth()
    {
        return horizontal_size;
    }

    //contain all data for a grid position
    class Location
    {
        Box box;
        protected ConveyorUnit conveyor;
        //Recipient
        //IObstacle or obj
        //Wifi beam ...

        public Location()
        {

        }

        public void Add(ConveyorUnit c)
        {
            conveyor = c;
        }
    }

    Location[][] grid;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //generate grid with a Location object in each location
        grid = new Location[vertical_size][];

        for (int i = 0; i < vertical_size; i++)
        {
            for (int j = 0; j < horizontal_size; j++)
            {
                grid[i][j] = new Location();
            }
        }

        //populate other objects into their locations using their initial pos values in a scene

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddConveyor(ConveyorUnit conveyor, int x_pos, int z_pos)
    {
        grid[x_pos][z_pos].Add(conveyor);
    }
}

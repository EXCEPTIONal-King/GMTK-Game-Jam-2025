using System;
using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;

//Could be made obsolete if conveyors and conveyor units handle the objects on them. I'll assume we use this for now though


/**
x_pos and z_pos parameter match w/ unity scene x and z
x is row index and z is column index (vertical and horizontal)

When using vector2 w/ x is horizontal, y is vertical translate oddly
y becomes x and x becomes z
*/

public class GridSystem : MonoBehaviour
{

    [SerializeField] int horizontal_size;
    [SerializeField] int vertical_size;

    public int GetWidth()
    {
        return horizontal_size;
    }

    //contain all data for a grid position
    public class Location
    {
        Box box;
        protected ConveyorUnit conveyor;
        //Recipient
        //IObstacle or obj
        //Wifi beam ...
        Vector2Int pos;

        public Location(int x_pos, int z_pos)
        {
            print("Location created");
            pos = new Vector2Int(z_pos, x_pos);//x is vertical, z is horizontal
        }

        public Vector2Int GetPos()
        {
            return pos;
        }

        public void Add(ConveyorUnit c)
        {
            conveyor = c;
        }

        public void Add(Box b)
        {
            box = b;
        }

        public void RemoveBox()
        {
            box = null;
        }

        public ConveyorUnit GetConveyor()
        {
            return conveyor;
        }

        public Boolean IsClear()
        {
            return this.box == null; //and no IObstacles
        }
    }

    Location[][] grid;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print("Grid System Initializing");
        //generate grid with a Location object in each location
        grid = new Location[vertical_size][];

        for (int i = 0; i < vertical_size; i++)
        {
            grid[i] = new Location[horizontal_size];
            for (int j = 0; j < horizontal_size; j++)
            {
                grid[i][j] = new Location(i, j);
                print(i + " " + j);
            }
        }
        print("Grid Initialized");

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

    public void AddBox(Box box, int x_pos, int z_pos)
    {
        grid[x_pos][z_pos].Add(box);
    }

    public Location CheckLocation(int x_pos, int z_pos)
    {
        return grid[x_pos][z_pos];
    }

    public Location NextLocationOnConveyor(int x_pos, int z_pos)
    {
        Vector2Int conveyordir = grid[x_pos][z_pos].GetConveyor().GetGridDirection();
        Location next = grid[x_pos + conveyordir.y][z_pos + conveyordir.x];
        return next;
    }

    public void PushBox(Box box, int prev_x_pos, int prev_z_pos)
    {
        Location curr = CheckLocation(prev_x_pos, prev_z_pos);
        Location next = NextLocationOnConveyor(prev_x_pos, prev_z_pos);
        if (next.IsClear())
        {
            next.Add(box);
            curr.RemoveBox();
            print("new pos" + next.GetPos());
            print("old pos" + curr.GetPos());
        }
    }

    

}

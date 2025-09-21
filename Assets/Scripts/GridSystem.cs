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

    [SerializeField] int num_boxes;
    SwitchTiles[] transfer_points;

    Boolean upcoming_transfer = false;

    public int GetNumBoxes()
    {
        return num_boxes;
    }
    public int GetWidth()
    {
        return horizontal_size;
    }

    //contain all data for a grid position
    public class Location
    {
        Box box;
        Boolean blocked = false;
        protected ConveyorUnit conveyor;
        protected Receiver receiver;
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

        public void Add(Receiver rec)
        {
            receiver = rec;
            // TODO: does this need more logic if the receiver isn't directly on the grid location?
        }

        public void RemoveBox()
        {
            box = null;
        }

        public Box GetBox()
        {
            return box;
        }

        public ConveyorUnit GetConveyor()
        {
            return conveyor;
        }

        public Receiver GetReceiver()
        {
            // null if no receiver
            return receiver;
        }

        public bool IsClear()
        {
            return this.box == null && !blocked; //and no IObstacles
        }

        public void markBlocked()
        {
            blocked = true;
        }

        public bool IsPickup()
        {
            if (receiver == null)
            {
                return false;
            }
            return receiver.GetPickupPos() == this.pos;
        }
    }

    Location[][] grid;

    HeadsUpDisplay hud;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
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
            }
        }
        transfer_points = GameObject.FindObjectsByType<SwitchTiles>(FindObjectsSortMode.None);
        print("Grid Initialized");

        hud = GameObject.FindAnyObjectByType<HeadsUpDisplay>();

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
        print("Box added" + x_pos + " , " + z_pos);
        grid[x_pos][z_pos].Add(box);

        hud.AddObjective(box.GetBoxColor());
    }

    public void AddReceiver(Receiver rec, int xPos, int zPos)
    {
        Debug.Log($"Receiver picking up at {xPos}, {zPos}");
        grid[xPos][zPos].Add(rec);
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
            curr.RemoveBox();
            box.SetPos(next.GetPos());//update box location
        }
        if (upcoming_transfer)
        {
            foreach (SwitchTiles transfer_point in transfer_points)
            {
                print("box switch check - pending");
                if (transfer_point.SwitchBoxes()) upcoming_transfer = false; // toggle flag when boxes switched over
            }
        }

        // check if the box should be picked up before moving on
        if (next.IsPickup())
        {
            box.TriggerPickup(next.GetReceiver());
            Debug.Log("Grid found a pickup!");
        }
        // otherwise, move if clear
        else
        {
            next.Add(box); //update grid location
        }
    }

    public void UpcomingTransfer()
    {
        if (upcoming_transfer)
        {
            upcoming_transfer = false;
            return;
        }
        upcoming_transfer = true;
        //To Do: show this functionality - honestly forgot it was a thing
    }
}    
    


    



using UnityEngine;

public class ConveyorUnit : MonoBehaviour
{
    public enum ConveyorDirection
    {
        Up,
        Left,
        Down,
        Right
    }

    public enum ConveyorType
    {
        Horizontal,
        Vertical,
        CornerTR,
        CornerTL,
        CornerBL,
        CornerBR
    }

    [SerializeField] ConveyorType type;

    private ConveyorDirection dir; //each type only has 2 options... don't know if we should have some mechanism to change this

    [SerializeField] Vector2Int pos;
    Vector2Int target_pos; //position going to
    Vector2Int prior_pos; //position feeding into it, used for easy turn around, maybe should do it differently
    //need someway to turn corner different than straight-away
    GridSystem grid;
    Conveyor conveyor_loop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GameObject.FindAnyObjectByType<GridSystem>();
        grid.AddConveyor(this, pos.y, pos.x);
        print(grid.CheckLocation(pos.y, pos.x).GetConveyor());
    }

    // Update is called once per frame
    void Update()
    {

    }

    //convert direction to vector towards next tile in grid
    public Vector2Int GetGridDirection()
    {
        if (dir == ConveyorDirection.Up) return new Vector2Int(0, -1);
        else if (dir == ConveyorDirection.Left) return new Vector2Int(-1, 0);
        else if (dir == ConveyorDirection.Down) return new Vector2Int(0, 1);
        else return new Vector2Int(1, 0);
    }

    public Vector2Int GetPos()
    {
        return pos;
    }

    //for building
    public ConveyorDirection GetConveyorDirection()
    {
        return dir;
    }

    public void SetDirection(ConveyorDirection dir)
    {
        this.dir = dir;
    }

    public ConveyorType GetConveyorType()
    {
        return type;
    }

    public Conveyor GetConveyorLoop()
    {
        return conveyor_loop;
    }

    public void SetConveyorLoop(Conveyor loop)
    {
        conveyor_loop = loop;
    }
}

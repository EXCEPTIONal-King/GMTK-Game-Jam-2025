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

    // the arrow to indicate direction
    SpriteRenderer arrowSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GameObject.FindAnyObjectByType<GridSystem>();

        arrowSprite = GetComponentInChildren<SpriteRenderer>();
        if (arrowSprite == null) return;
        // TODO: fix this so it's actually accurate

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

    /** ConveyorLoop used to distinguish the collective conveyor object that represents a full conveyor
    from the ConveyorUnit object which represent 1 point of the grid that is a conveyor */
    public Conveyor GetConveyorLoop()
    {
        return conveyor_loop;
    }

    public void SetConveyorLoop(Conveyor loop)
    {
        conveyor_loop = loop;
    }

    public void Reverse()
    {
        // Change appearance
        if (type == ConveyorType.Horizontal || type == ConveyorType.Vertical)
        {
            arrowSprite.flipY = !arrowSprite.flipY;
        }

        if (type == ConveyorType.Horizontal && dir == ConveyorDirection.Right)
        {
            dir = ConveyorDirection.Left;

        }
        else if (type == ConveyorType.Horizontal && dir == ConveyorDirection.Left)
        {
            dir = ConveyorDirection.Right;
        }

        if (type == ConveyorType.Vertical && dir == ConveyorDirection.Down)
        {
            dir = ConveyorDirection.Up;
        }
        else if (type == ConveyorType.Vertical && dir == ConveyorDirection.Up)
        {
            dir = ConveyorDirection.Down;
        }

        // Corner tiles
        if (type == ConveyorType.CornerTR && dir == ConveyorDirection.Left)
        {
            dir = ConveyorDirection.Down;
        }
        else if (type == ConveyorType.CornerTR && dir == ConveyorDirection.Down)
        {
            dir = ConveyorDirection.Left;
        }

        if (type == ConveyorType.CornerTL && dir == ConveyorDirection.Right)
        {
            dir = ConveyorDirection.Down;

        }
        else if (type == ConveyorType.CornerTL && dir == ConveyorDirection.Down)
        {
            dir = ConveyorDirection.Right;
        }

        if (type == ConveyorType.CornerBL && dir == ConveyorDirection.Right)
        {
            dir = ConveyorDirection.Up;

        }
        else if (type == ConveyorType.CornerBL && dir == ConveyorDirection.Up)
        {
            dir = ConveyorDirection.Right;
        }

        if (type == ConveyorType.CornerBR && dir == ConveyorDirection.Left)
        {
            dir = ConveyorDirection.Up;

        }
        else if (type == ConveyorType.CornerBR && dir == ConveyorDirection.Up)
        {
            dir = ConveyorDirection.Left;
        }
    }

    public SpriteRenderer GetArrowSprite()
    {
        return arrowSprite;
    }
}

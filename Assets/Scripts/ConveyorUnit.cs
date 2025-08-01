using UnityEngine;

public class ConveyorUnit : MonoBehaviour
{
    public ConveyorUnitType type;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GameObject.FindAnyObjectByType<GridSystem>();
        grid.AddConveyor(this, pos.x, pos.y);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum ConveyorUnitType {
    Straight,
    Corner
}

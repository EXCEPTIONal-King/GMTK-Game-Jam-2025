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

    int[] pos;
    int[] target_pos; //position going to
    int[] prior_pos; //position feeding into it, used for easy turn around, maybe should do it differently
    //need someway to turn corner different than straight-away

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

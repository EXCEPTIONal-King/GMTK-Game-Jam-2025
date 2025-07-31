using UnityEngine;

public class Conveyor : MonoBehaviour
{
    ConveyorUnit[] loopUnits;
    BoxCollider collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loopUnits = GetComponentsInChildren<ConveyorUnit>();
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

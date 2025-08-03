using UnityEngine;
using UnityEngine.InputSystem;

public class ConveyorSelector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    int index = 0;
    Conveyor selected;
    Conveyor[] all_conveyors;
    [SerializeField] int all_conveyor_reverses;

    HeadsUpDisplay hud;

    void Start()
    {
        all_conveyors = FindObjectsByType<Conveyor>(FindObjectsSortMode.None);
        selected = all_conveyors[index];

        Controls con = new Controls();
        con.GamePlay.Enable();
        con.GamePlay.ShiftConveyorSelection.performed += ShiftConveyor;
        con.GamePlay.ReverseSelected.performed += ReverseSelected;
        con.GamePlay.Reverse.performed += ReverseAll;

        hud = GameObject.FindAnyObjectByType<HeadsUpDisplay>();
        hud.AddLimitation(all_conveyor_reverses);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShiftConveyor(InputAction.CallbackContext context)
    {
        index++;
        if (index >= all_conveyors.Length) index = 0;
        selected = all_conveyors[index];


    }

    void ReverseSelected(InputAction.CallbackContext context)
    {
        selected.ReverseRotation(false);
    }
    
    public void ReverseAll(InputAction.CallbackContext context)
    {
        if (all_conveyor_reverses <= 0) return;

        all_conveyor_reverses--;
        foreach (Conveyor conveyor in all_conveyors)
        {
            conveyor.ReverseRotation(true);
        }

        hud.ConsumeLimitation(all_conveyor_reverses);
    }

}

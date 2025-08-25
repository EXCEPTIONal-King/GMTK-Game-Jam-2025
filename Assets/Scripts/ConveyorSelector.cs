using System;
using System.Collections;
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
    [Tooltip("Controls how the HUD displays the remaining reverse count.")]
    [SerializeField] string limitationLabel;

    class ConveyorComparer : IComparer
    {
        public int Compare(object one, object two)
        {
            Conveyor conveyorOne = one as Conveyor;
            Conveyor conveyorTwo = two as Conveyor;
            if (conveyorOne == null || conveyorTwo == null)
            {
                return 0;
            }
            return conveyorOne.GetConveyorId().CompareTo(conveyorTwo.GetConveyorId());
        }
    }

    void Start()
    {
        all_conveyors = FindObjectsByType<Conveyor>(FindObjectsSortMode.None);
        Array.Sort(all_conveyors, new ConveyorComparer());
        selected = all_conveyors[index];

        Controls con = new Controls();
        con.GamePlay.Enable();
        con.GamePlay.ShiftConveyorSelection.performed += ShiftConveyor;
        con.GamePlay.ReverseSelected.performed += ReverseSelected;
        con.GamePlay.Reverse.performed += ReverseAll;

        hud = GameObject.FindAnyObjectByType<HeadsUpDisplay>();
        hud.AddLimitation(limitationLabel, all_conveyor_reverses);
        hud.SelectConveyor(index);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShiftConveyor(InputAction.CallbackContext context)
    {
        hud.DeselectConveyor(index);

        index++;
        if (index >= all_conveyors.Length) index = 0;
        selected = all_conveyors[index];

        hud.SelectConveyor(index);
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

        hud.ConsumeLimitation(limitationLabel, all_conveyor_reverses);
    }

}

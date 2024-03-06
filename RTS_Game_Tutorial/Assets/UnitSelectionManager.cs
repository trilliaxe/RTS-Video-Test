using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{

   public static UnitSelectionManager Instance {get;set;}

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();
    
    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarker;
    private Camera cam;




   private void Awake()
   {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }
   }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
         if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

           if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
           {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else 
                {
                 SelectByClicking(hit.collider.gameObject);
                }
           } 
           else
           {
                if(Input.GetKey(KeyCode.LeftShift)== false)
                {
                    DeselectetAll();
                }
               
           }
        }


        if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

           if(Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
           {
                groundMarker.transform.position = hit.point;
                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
           } 
        }
    }

    private void MultiSelect(GameObject unit)
    {
        if(unitsSelected.Contains(unit)==false)
        {
            unitsSelected.Add(unit);
            SelectUnit(unit,true);
    
        }
        else
        {
            SelectUnit(unit,false);
            unitsSelected.Remove(unit);
        }
    }

    private void SelectUnit(GameObject unit, bool isSelected)
    {
        TriggerSelectionIndicator(unit, isSelected);
        EnabledUnitMovement(unit,isSelected);
        
    }
    public void DeselectetAll()
    {
        foreach(var unit in unitsSelected)
        {
            SelectUnit(unit,false);
        }
        groundMarker.SetActive(false);
        unitsSelected.Clear();
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectetAll();
        unitsSelected.Add(unit);
        SelectUnit(unit,true);
    }

    private void EnabledUnitMovement(GameObject unit, bool shouldMove)
    {
       unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isVisible);
    }

    internal void DragSelect(GameObject unit)
    {
        if(unitsSelected.Contains(unit)==false)
        {
            unitsSelected.Add(unit);
            SelectUnit(unit,true);
            
        }
    }
}

using UnityEngine;

public class Interact : MonoBehaviour
{
    public float playerReach = 3f;
    Selectable currentSelect;
    // Update is called once per frame
    void Update()
    {
        CheckInteraction();
        if(Input.GetKeyDown(KeyCode.F) && currentSelect != null)
        {
            currentSelect.Interact();

        }
    }
    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(Physics.Raycast(ray, out hit, playerReach))
        {
            if (hit.collider.tag == "Mannequin")
            {
                Selectable newSelectable = hit.collider.GetComponent<Selectable>();
                if(currentSelect && newSelectable != currentSelect)
                {
                    currentSelect.DisableOutline();
                }
                if (newSelectable.enabled)
                {
                    SetNewCurrentSelectable(newSelectable);
                }
                else
                {
                    DisableCurrentSelectable();
                }
            }
            else
            {
                DisableCurrentSelectable();
            }
        }
        else
        {
            DisableCurrentSelectable();
        }
    }
    void SetNewCurrentSelectable(Selectable newSelectable) 
    {
        currentSelect = newSelectable;
        currentSelect.EnableOutline();
    }
    void DisableCurrentSelectable()
    {
        if (currentSelect) 
        {
            currentSelect.DisableOutline();
            currentSelect = null;
        }
    }
}

using UnityEngine;
using UnityEngine.UIElements.Experimental;
using TMPro;
public class Interact : MonoBehaviour
{
    public TextMeshProUGUI currentTagText;
    public TextMeshProUGUI tooManyTagged;
    string text = "Used up all tags";
    public float playerReach = 3f;
    Selectable currentSelect;
    public int maxTagged;
    public int currentTagged;
    public int monsterTagged;
    private void Start()
    {
        maxTagged = 3;
        tooManyTagged.text = text;
        tooManyTagged.gameObject.SetActive(false);
        currentTagged = 0;
        monsterTagged = 0;
        currentSelect = null;
    }
    // Update is called once per frame
    void Update()
    {
        currentTagText.text = currentTagged.ToString();
        CheckInteraction();
        if (currentTagged == maxTagged && currentSelect.selected == false)
        {
            if (Input.GetKeyDown(KeyCode.F) && currentSelect != null)
            {
                tooManyTagged.gameObject.SetActive(true);
            }
           
        }
        else if (currentSelect.selected == true && currentTagged == maxTagged)
        {
            if (Input.GetKeyDown(KeyCode.F) && currentSelect != null)
            {
                currentSelect.Interact();
                tooManyTagged.gameObject.SetActive(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F) && currentSelect != null)
            {
                currentSelect.Interact();
            }
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

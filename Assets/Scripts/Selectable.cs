using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
public class Selectable : MonoBehaviour
{
    Outline outline;
    public string message;
    public Transform face;
    public GameObject tag;
    public bool selected;
    public UnityEvent onInteraction;
    public Interact interact;

    private void Start()
    {
        selected = false;
        outline = GetComponent<Outline>();
        DisableOutline();
    }
    public void Interact()
    {
        if (selected)
        {
            tag.SetActive(false);
            selected = false;
            interact.currentTagged -= 1;
        }
        else 
        {
            tag.SetActive(true);
            selected = true;
            interact.currentTagged += 1;
        }
    }
    public void DisableOutline()
    {
        outline.enabled = false;
    }
    public void EnableOutline() 
    {
        outline.enabled = true; 
    } 

}


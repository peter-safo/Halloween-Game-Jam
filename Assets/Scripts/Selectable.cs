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

    public UnityEvent onInteraction;

    private void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }
    public void Interact()
    {
        onInteraction.Invoke();
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


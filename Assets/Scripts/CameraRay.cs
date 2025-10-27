using UnityEngine;

public class CameraRay : MonoBehaviour
{
    public float rayDistance = 5f; // how far the ray goes
    public GameObject objectToPlace; // assign your prefab
    private Selectable currentSelection;

    void Update()
    {
        // Cast a short ray from the camera forward (through the mouse position)
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            Selectable selectable = hit.collider.GetComponent<Selectable>();

            // If we hit a new selectable object
            if (selectable != null)
            {
                if (currentSelection != selectable)
                {
                    ClearSelection();
                    currentSelection = selectable;
                    currentSelection.EnableOutline();
                }

                // Handle click
                if (Input.GetMouseButtonDown(0))
                {
                    Instantiate(objectToPlace, hit.collider.transform.position, Quaternion.identity);
                }
            }
            else
            {
                // Hit something that's not selectable
                ClearSelection();
            }
        }
        else
        {
            // Nothing hit
            ClearSelection();
        }
    }

    void ClearSelection()
    {
        if (currentSelection != null)
        {
            currentSelection.DisableOutline();
            currentSelection = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InteractableRaycast : MonoBehaviour
{
    [SerializeField] private int rayLength = 7;
    [SerializeField] private LayerMask layerMaskInteract=Physics.DefaultRaycastLayers;
    [SerializeField] private GameObject interactButton;
    [SerializeField] private Image crosshair = null;

    private IInteractable raycastedObject;
    private bool isCrosshairActive;
    private Camera mainCamera;
    private bool doOnce;
    bool isOnCooldown=false;

    private string[] interactableTags = { "Door", "Drawer", "Cabinet", "TvDrawer","KeyPad","SmallDrawer","HideCabinet","CabinetOne","XabinDrawerOne","XabinDrawerTwo","LowerXabinOne","LowerXabinTwo" };
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, rayLength, layerMaskInteract))
        {         
            if (interactableTags.Contains(hit.collider.tag)) 
            {
               
                if (!doOnce)
                {
                    raycastedObject = hit.collider.gameObject.GetComponent<IInteractable>();
                    CrosshairChange(true);
                }
                isCrosshairActive = true;
                doOnce = true;
            }
        }
        else
        {
            if (isCrosshairActive)
            {
                CrosshairChange(false);
                doOnce = false;
            }   
            interactButton.SetActive(false);
        }
    }

    void CrosshairChange(bool on)
    {
        if (on && !doOnce)
        {
            crosshair.color = Color.red;
            interactButton.SetActive(true);
        }
        else
        {
            crosshair.color = Color.white;
            isCrosshairActive = false;
            interactButton.SetActive(false);
        }
    }

    public void Interact()
    {
        if (raycastedObject != null)
        {
            if (!isOnCooldown)
            { raycastedObject.Interact();
                StartCoroutine(CoolDownTimer());
            }
            else
            {
                return;
            }
        }
    }
    IEnumerator CoolDownTimer()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(1);
        isOnCooldown=false;
    }
}

public interface IInteractable
{
    void Interact();    
}
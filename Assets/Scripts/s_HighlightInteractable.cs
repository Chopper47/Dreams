using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class s_HighlightInteractable : MonoBehaviour
{
    [SerializeField] private LayerMask TargetLayer;
    [SerializeField] private GameObject hoverTargetGameObject;
    [SerializeField] private GameObject targetGameObject;

    [SerializeField] public s_CharacterStats currentCharacter;
    // Update is called once per frame

    private void Start()
    {

    }
    void Update()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

         if (Physics.Raycast(ray, out hit, 100f, TargetLayer))
        {
            if (hit.transform.gameObject != null)
            {
                hoverTargetGameObject = hit.transform.gameObject;
                if (hoverTargetGameObject.GetComponent<Outline>() == null)
                {
                    var outline = hoverTargetGameObject.AddComponent<Outline>();

                    outline.OutlineWidth = 10f;

                }
                hoverTargetGameObject.GetComponent<Outline>().enabled = true;

            }


        }

        else
        {
            if (hoverTargetGameObject != null)
            {
                if (hoverTargetGameObject.GetComponent<Outline>() != null)
                {
                    hoverTargetGameObject.GetComponent<Outline>().enabled = false;

                }

            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            
            RaycastHit clickHit;

            Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(clickRay, out clickHit, 100f, TargetLayer))
            {

                targetGameObject = clickHit.transform.gameObject;

                StartDialogue();
            }




        }
    }

    void StartDialogue()
    {
        if (targetGameObject != null)
        {
            if (targetGameObject.GetComponent<s_CreateDialogue>())
            {
                targetGameObject.GetComponent<s_CreateDialogue>().enabled = true;

            }
            if(targetGameObject.GetComponent<s_CampFire>())
            {
                targetGameObject.GetComponent<s_CampFire>().enabled = true;

            }



        }
        targetGameObject = null;
    }


   
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;

// !! this script goes on the sub-panels, not the parent panel !!
//TODO: figure out what to put here

public class AbilityButton : MonoBehaviour
{


    [SerializeField] private Color readyColor;
    [SerializeField] private Color cooldownColor;
    [SerializeField] private bool isReady;

    void Update()
    {
        if (!isReady)
        {
            gameObject.GetComponent<Button>().interactable = false;
            gameObject.GetComponent<SpriteRenderer>().color = cooldownColor;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = true;
            gameObject.GetComponent<SpriteRenderer>().color = readyColor;
        }
    }

}

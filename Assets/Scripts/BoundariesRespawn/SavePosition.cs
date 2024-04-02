using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavePosition : MonoBehaviour
{
    internal Vector3 playerPosition;
    public TextMeshProUGUI positionText;

    public void Start()
    {
        if (positionText == null)
            Debug.LogError("TextMeshPro Text component not assigned!");
        else
            UpdatePositionText();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Boundary"))
        {
            playerPosition = this.gameObject.transform.position;
            UpdatePositionText();
        }
    }

    public void UpdatePositionText()
    {
        positionText.text = "Last Saved Position" + playerPosition.ToString();
    }
}

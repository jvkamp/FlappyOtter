using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonHandler : MonoBehaviour
{
    public GameObject toChange;

   public void ButtonChange()
    {
        toChange.SetActive(!toChange.activeSelf);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class closeUI : MonoBehaviour
{
    [SerializeField] GameObject uitutorial;
   public void Close()
   {
        uitutorial.SetActive(false);
   }
}

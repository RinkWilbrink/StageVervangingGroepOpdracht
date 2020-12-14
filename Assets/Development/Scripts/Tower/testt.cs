using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class testt : MonoBehaviour
{

    [SerializeField] private Text textPOGGERS;

    private GameObject text;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetText()
    {
        text.GetComponentInChildren<Text>().text = "idk wat de fuck wil je hier in zetten";

    }


}

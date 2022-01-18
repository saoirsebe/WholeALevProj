using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class taskTextScript : MonoBehaviour
{
    Text instruction;
    // Start is called before the first frame update
    void Start()
    {
        instruction = gameObject.GetComponent<Text>();
    }

    public void ChangeText(string objectToFindName)
    {
        instruction.text = objectToFindName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

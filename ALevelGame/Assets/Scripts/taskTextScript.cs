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

    /// <summary>
    /// Called after ObjectToFind is picked and shows text of which object is to be found in the instructions panel
    /// </summary>
    /// <param name="objectToFindName"></param>
    public void ChangeText(string objectToFindName)
    {
        instruction.text = objectToFindName;
    }
}

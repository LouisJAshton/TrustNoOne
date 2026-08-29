using UnityEngine;

public class RulesPage : MonoBehaviour
{
    [SerializeField] private GameObject go;
    
    // Update is called once per frame
    void Update()
    {
        //This sucks whatever
        go?.SetActive(Input.GetKey(KeyCode.Tab));
    }
}

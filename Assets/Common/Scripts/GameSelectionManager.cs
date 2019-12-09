using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSelectionManager : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlaySe("select-guide");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("MultiLines_0");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("ShingleLine_Misawa");
        }
    }
}

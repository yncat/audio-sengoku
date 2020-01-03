using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MultiLines_0
{
public class TimeKeeper : MonoBehaviour
{
    // param:１ターンの時間
    private const int INTERVAL = 10;
    // --
    void Start()
    {
        StartCoroutine("Call");
    }
    void Update(){}
    IEnumerator Call()
    {
        while(true)
        {
            yield return new WaitForSeconds(INTERVAL);
            // --
            SoundManager.Instance.PlaySe("solemnity1");
            var solders = GameObject.FindGameObjectsWithTag("Soldier");
            foreach(var solder in solders)
            {
                solder.GetComponent<Soldier>().Move();
            }
        }
    }
}
}
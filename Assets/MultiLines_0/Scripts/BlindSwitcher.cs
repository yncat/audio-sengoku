using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace MultiLines_0
{
public class BlindSwitcher : MonoBehaviour
{
    [Header("> 目隠しを切り替えるキーを指定")]
    [SerializeField] KeyCode switchKey;
    void Start()
    {
        var image = GetComponent<UnityEngine.UI.Image>();
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(switchKey))
        .Subscribe(_ => {
            image.enabled = !image.enabled;
        });
    }
    void Update(){}
}
}
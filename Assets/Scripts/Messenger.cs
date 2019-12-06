using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
public class Messenger : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textField;
    private const float SPEED = 200f;
    void Start(){}
    void Update(){}
    public void Run(BattleResult result, Transform parent)
    {
        this.textField.text = "";
        // --
        Observable.EveryUpdate().Subscribe(_ => {
            transform.position = Vector2.MoveTowards(
                transform.position,
                parent.position,
                SPEED * Time.deltaTime
            );
        }).AddTo(this);
        // --
        var OnTriggerEnterPlayer = this.OnTriggerEnter2DAsObservable()
            .Select(collision => collision.tag)
            .Where(tag => tag == "Player")
            .Subscribe(_ => {
                // todo: あとで適当な文章に入れ替える
                this.textField.text = result.ToString();
                GetComponent<UnityEngine.UI.Image>().enabled = false;
                // --
                Observable.Timer(TimeSpan.FromSeconds(1.5f)).Subscribe(time =>
                {
                    Destroy(gameObject);
                })
                .AddTo(this);
            });
    }
}

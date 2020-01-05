using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
namespace MultiLines_0
{
public class Messenger : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textField;
    // param: メッセージの速度
    private const float SPEED = 400f;
    // --
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
                var handle = SoundManager.Instance.PlaySe(GetVoiceName(result));

                if (result == BattleResult.Tie)
                {
                    handle.panning = 0.5f;

                } else
                {
                    handle.panning = parent.GetComponent<PlayerInput>().GetSide() == Side.Right ? 1f : -1f;
                }
                
                
                
                // Debug.Log(parent.GetComponent<PlayerInput>().GetSide());

                // --
                Observable.Timer(TimeSpan.FromSeconds(1.5f)).Subscribe(time =>
                {
                    Destroy(gameObject);
                });
            }).AddTo(this);
    }
    // se-param: 伝令, 3種類, 上から勝ち、引き分け、負け
    private string GetVoiceName(BattleResult result)
    {
        switch(result)
        {
            case BattleResult.Win :  return "t_win";
            case BattleResult.Tie :  return "t_lose";
            case BattleResult.Lose : return "t_lose";
        }
        return "";
    }
}
}
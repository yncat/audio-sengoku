using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace MultiLines_0
{
public class PlayerInput : MonoBehaviour
{
    // param:
    private const int LINE_LENGTH = 2;
    // --
    private int currentLineIdx;
    private SoldierType selectedSoldierType = SoldierType.NotSet;
    [SerializeField] Side side;
    [SerializeField] KeyCode moveKeyLeft;
    [SerializeField] KeyCode moveKeyRight;
    [SerializeField] KeyCode selectKeyCanon;
    [SerializeField] KeyCode selectKeyArmor;
    [SerializeField] KeyCode selectKeyHorse;
    [SerializeField] KeyCode okKey;
    void Start()
    {
        this.currentLineIdx = (int)Mathf.Floor(LINE_LENGTH / 2);
        // --
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(moveKeyLeft))
        .Where(_ => 0 < this.currentLineIdx)
        .Subscribe(_ => {
            this.currentLineIdx --;
            PlaySeWithPan("decision1");
        });
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(moveKeyRight))
        .Where(_ => this.currentLineIdx < LINE_LENGTH - 1)
        .Subscribe(_ => {
            this.currentLineIdx ++;
            PlaySeWithPan("decision2");
        });
        // --
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(selectKeyCanon))
        .Subscribe(_ => {
            this.selectedSoldierType = SoldierType.Canon;
            PlaySeWithPan("Canon_select");
        });
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(selectKeyArmor))
        .Subscribe(_ => {
            this.selectedSoldierType = SoldierType.Armor;
            PlaySeWithPan("Armor_select");
        });
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(selectKeyHorse))
        .Subscribe(_ => {
            this.selectedSoldierType = SoldierType.Horse;
            PlaySeWithPan("Horse_select");
        });
        // 決定
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(okKey))
        .Where(_ => this.selectedSoldierType != SoldierType.NotSet)
        .Subscribe(_ => {
            var soldier = Instantiate(Resources.Load("Prefabs/" + this.selectedSoldierType.ToString())
            ,transform.position, Quaternion.identity, transform.parent) as GameObject;
            soldier.GetComponent<Soldier>().Init(IsLeftSide(), this.transform);
            soldier.transform.SetSiblingIndex(0); // 目隠しの下に来るように
            PlaySeWithPan("decision6");
            // PlaySeWithPan(this.selectedSoldierType.ToString()+"_spawn");
        });
        // update
        this.ObserveEveryValueChanged(x => x.currentLineIdx).Subscribe(_ => {    
            // ラインによる移動距離の調整, 親要素にGameManagerがついていることが前提
            var canvasScaler = transform.parent.GetComponent<UnityEngine.UI.CanvasScaler>();
            var PADDING = 100;
            int moveInterval = Mathf.FloorToInt(canvasScaler.referenceResolution.y - PADDING) / LINE_LENGTH;
            var y = (this.currentLineIdx - Mathf.Floor(LINE_LENGTH / 2)) * moveInterval;
            var anchor = GetComponent<RectTransform>().anchoredPosition;
            var pos = new Vector3(anchor.x, y, 0f);
            GetComponent<RectTransform>().anchoredPosition = pos;
        });
    }
    void Update()
    {
    }
    public Side GetSide()
    {
        return side;
    }
    private bool IsLeftSide()
    {
        return GetComponent<RectTransform>().anchoredPosition.x < 0f;
    }
    private void PlaySeWithPan(string name)
    {
        var handle = SoundManager.Instance.PlaySe(name);
        handle.panning = this.side == Side.Right ? 1f : -1f;
    }
}
}
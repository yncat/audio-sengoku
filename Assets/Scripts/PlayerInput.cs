using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class PlayerInput : MonoBehaviour
{
    private const float MOVE_INTERVAL = 300f;
    private int currentLineIdx;
    private SoldierType selectedSoldierType = SoldierType.NotSet;
    [SerializeField] KeyCode moveKeyLeft;
    [SerializeField] KeyCode moveKeyRight;
    [SerializeField] KeyCode selectKeyCanon;
    [SerializeField] KeyCode selectKeyArmor;
    [SerializeField] KeyCode selectKeyHorse;
    [SerializeField] KeyCode okKey;
    private int LINE_LENGTH = 3;
    void Start()
    {
        this.currentLineIdx = (int)Mathf.Floor(LINE_LENGTH / 2);
        // --
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(moveKeyLeft))
        .Where(_ => 0 < this.currentLineIdx)
        .Subscribe(_ => {
            this.currentLineIdx --;
        });
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(moveKeyRight))
        .Where(_ => this.currentLineIdx < LINE_LENGTH - 1)
        .Subscribe(_ => {
            this.currentLineIdx ++;
        });
        // --
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(selectKeyCanon))
        .Subscribe(_ => {
            this.selectedSoldierType = SoldierType.Canon;
        });
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(selectKeyArmor))
        .Subscribe(_ => {
            this.selectedSoldierType = SoldierType.Armor;
        });
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(selectKeyHorse))
        .Subscribe(_ => {
            this.selectedSoldierType = SoldierType.Horse;
        });
        // 決定
        Observable.EveryUpdate()
        .Where(_ => Input.GetKeyDown(okKey))
        .Where(_ => this.selectedSoldierType != SoldierType.NotSet)
        .Subscribe(_ => {
            var obj = Instantiate(Resources.Load(this.selectedSoldierType.ToString())
            ,transform.position, Quaternion.identity, transform.parent) as GameObject;
            obj.GetComponent<Soldier>().Init(IsLeftSide(), this.transform);
        });
        // update
        this.ObserveEveryValueChanged(x => x.currentLineIdx).Subscribe(_ => {
            var y = (this.currentLineIdx - Mathf.Floor(LINE_LENGTH / 2)) * MOVE_INTERVAL;
            var anchor = GetComponent<RectTransform>().anchoredPosition;
            var pos = new Vector3(anchor. x, y, 0f);
            GetComponent<RectTransform>().anchoredPosition = pos;
        });
    }
    void Update()
    {
    }
    private bool IsLeftSide()
    {
        return GetComponent<RectTransform>().anchoredPosition.x < 0f;
    }
}

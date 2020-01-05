using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MultiLines_0
{
public class Soldier : MonoBehaviour
{
    // param:1ターン毎の移動力の定数
    // これとは別にユニットごとのスピードはプレハブで指定できる
    private const float STEP = 600f;
    // --
    [SerializeField] private SoldierType type;
    [SerializeField] private float speed;
    private Transform parent;
    private Vector3 target;
    void Start()
    {
        this.target = transform.position;
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 1000f);
    }
    public void Move()
    {
        var relativeMove = Vector3.right * this.speed * Time.deltaTime * STEP;
        this.target = this.target + relativeMove;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Soldier")
        {
            if (IsGoLeft() == other.GetComponent<Soldier>().IsGoLeft()) return;
            // --
            SoundManager.Instance.PlaySe(this.type.ToString()+"_combat");
            var result = Judge(other.name);
            if (result == BattleResult.Win)
            {
                // note: よくよく考えたら勝った負けたの音は伝えなくてよい
                // 勝った
                // PlayWinSe();
            }
            else
            {
                // 負けた or 引き分け
                Destroy(this.gameObject);
            }
            GenerateMessenger(result);
        }
    }
    private void GenerateMessenger(BattleResult result)
    {
        var pos = transform.position;
        var rot = transform.rotation;
        var parent = transform.parent;
        var messanger = Instantiate(Resources.Load("Prefabs/Messenger"), pos, rot, parent) as GameObject;
        messanger.GetComponent<Messenger>().Run(result, this.parent);
        messanger.transform.SetSiblingIndex(0); // 目隠しの下に来るように
    }
    public void Init(bool isGoRight, Transform parent)
    {
        this.speed *= isGoRight ? -1f : 1f;
        this.parent = parent;
    }
    public bool IsGoLeft()
    {
        return this.speed < 0f;
    }
    private BattleResult Judge(string name)
    {
        if (name.Contains(this.type.ToString())) return BattleResult.Tie;
        if (this.type == SoldierType.Armor)
            if (name.Contains("Canon")) return BattleResult.Win;
        if (this.type == SoldierType.Canon)
            if (name.Contains("Horse")) return BattleResult.Win;
        if (this.type == SoldierType.Horse)
            if (name.Contains("Armor")) return BattleResult.Win;
        return BattleResult.Lose;
    }
    // private void PlayWinSe()
    // {
    //     var handle = SoundManager.Instance.PlaySe("decision12");
    //     handle.panning = IsGoLeft() ? 1f : -1f;

    // }
}
}
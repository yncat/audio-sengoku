using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Soldier : MonoBehaviour
{
    [SerializeField] private SoldierType type;
    [SerializeField] private float speed;
    private Transform parent;
    void Start()
    {
    }
    void Update()
    {
        transform.Translate(Vector2.right * this.speed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Soldier")
        {
            if (IsGoLeft() == other.GetComponent<Soldier>().IsGoLeft()) return;
            // --
            var result = Judge(other.name);
            if (result != BattleResult.Win)
            {
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
        var messanger = Instantiate(Resources.Load("Messenger"), pos, rot, parent) as GameObject;
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
        if (this.type.ToString() == name) return BattleResult.Tie;
        if (this.type == SoldierType.Armor)
            if (name.Contains("Canon")) return BattleResult.Win;
        if (this.type == SoldierType.Canon)
            if (name.Contains("Horse")) return BattleResult.Win;
        if (this.type == SoldierType.Horse)
            if (name.Contains("Armor")) return BattleResult.Win;
        return BattleResult.Lose;
    }
}

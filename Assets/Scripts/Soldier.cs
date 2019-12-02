using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Soldier : MonoBehaviour
{
    [SerializeField] private SoldierType type;
    [SerializeField] private float speed;
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
            if (!IsSuccess(other.name))
            {
                Destroy(this.gameObject);
            }
        }
    }
    public void SetIsLeftSide(bool isGoRight)
    {
        this.speed *= isGoRight ? -1f : 1f;
    }
    public bool IsGoLeft()
    {
        return this.speed < 0f;
    }
    private bool IsSuccess(string name)
    {
        if (this.type == SoldierType.Armor)
            if (name.Contains("Canon")) return true;
        if (this.type == SoldierType.Canon)
            if (name.Contains("Horse")) return true;
        if (this.type == SoldierType.Horse)
            if (name.Contains("Armor")) return true;
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnM
{
    [System.Serializable]
    public class SoldierInfo
    {
        public SARTS.Soldier.PieceType pieceType = SARTS.Soldier.PieceType.Pawn;
        public Sprite sprite = null;
        [Range(0.01f, 1f)] public float fraction = 1f;
        public bool isFlip = false;
    }
    public class GameManager : MonoBehaviour
    {
        [SerializeField] SoldierInfo[] m_soldierInfoArr = null;
        public SoldierInfo[] soldierInfoArr { get { return m_soldierInfoArr; } }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

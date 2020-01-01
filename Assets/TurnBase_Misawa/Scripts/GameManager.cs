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
        public AudioClip moveAudioClip = null;
        public AudioClip battleAudioClip = null;
        public AudioClip winAudioClip = null;
        public AudioClip loseAudioClip = null;
    }
    public class GameManager : MonoBehaviour
    {
        [SerializeField] AudioSource m_gameAc = null;
        public AudioSource gameAc { get { return m_gameAc; } }
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

        public void PlayMoveSe(SARTS.Soldier.PieceType _plType, int _posX)
        {
            for (int i = 0; i < m_soldierInfoArr.Length; ++i)
            {
                if (_plType == m_soldierInfoArr[i].pieceType)
                {
                    m_gameAc.PlayOneShot(m_soldierInfoArr[i].moveAudioClip);
                    break;
                }
            }
        }
    }
}

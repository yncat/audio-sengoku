using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnM
{
    public enum SEKind
    {
        Move=0,
        Battle,
        Win,
        Lose
    };

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
            playGameSe(_plType, _posX, SEKind.Move);
        }

        public void PlayBattleSe(SARTS.Soldier.PieceType _plType, int _posX)
        {
            playGameSe(_plType, _posX, SEKind.Battle);
        }

        public void PlayWinSe(SARTS.Soldier.PieceType _plType, int _posX)
        {
            playGameSe(_plType, _posX, SEKind.Win);
        }

        public void PlayLoseSe(SARTS.Soldier.PieceType _plType, int _posX)
        {
            playGameSe(_plType, _posX, SEKind.Lose);
        }


        /// <summary>
        /// Plaies the game se.
        /// </summary>
        /// <param name="_plType">Pl type.</param>
        /// <param name="_posX">Position x.</param>
        /// <param name="_kind">Kind.</param>
        void playGameSe(SARTS.Soldier.PieceType _plType, int _posX, SEKind _kind)
        {
            for (int i = 0; i < m_soldierInfoArr.Length; ++i)
            {
                if (_plType == m_soldierInfoArr[i].pieceType)
                {
                    AudioClip ac = null;
                    switch (_kind)
                    {
                        case SEKind.Move:   ac = m_soldierInfoArr[i].moveAudioClip; break;
                        case SEKind.Battle: ac = m_soldierInfoArr[i].battleAudioClip; break;
                        case SEKind.Win:    ac = m_soldierInfoArr[i].winAudioClip; break;
                        case SEKind.Lose:   ac = m_soldierInfoArr[i].loseAudioClip; break;
                    }
                    m_gameAc.PlayOneShot(ac);
                    break;
                }
            }
        }
    }
}

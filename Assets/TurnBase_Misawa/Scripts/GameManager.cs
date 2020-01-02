using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField, ReadOnlyWhenPlaying] int m_fieldW = 10;
        [SerializeField, ReadOnlyWhenPlaying] int m_fieldH = 1;
        [SerializeField, ReadOnlyWhenPlaying] AnimationCurve m_panAc = AnimationCurve.Linear(0f, -1f, 1f, 1f);
        [SerializeField, ReadOnlyWhenPlaying] TickTimer m_tickTimerScr = null;
        public TickTimer tickTimerScr { get { return m_tickTimerScr; } }
        [SerializeField, ReadOnlyWhenPlaying] VerticalLayoutGroup m_baseLineGroup = null;
        public VerticalLayoutGroup baseLineGroup { get { return m_baseLineGroup; } }
        public int fieldW { get { return m_fieldW; } }
        public int fieldH { get { return m_fieldH; } }
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

        public float GetStereoPan(float _ddx)
        {
            return m_panAc.Evaluate(Mathf.Clamp01(_ddx));
        }

        /// <summary>
        /// Plaies the game se.
        /// </summary>
        /// <param name="_plType">Pl type.</param>
        /// <param name="_posX">Position x.</param>
        /// <param name="_kind">Kind.</param>
        public AudioClip GetSeClip(SARTS.Soldier.PieceType _plType, SEKind _kind)
        {
            AudioClip ac = null;
            for (int i = 0; i < m_soldierInfoArr.Length; ++i)
            {
                if (_plType == m_soldierInfoArr[i].pieceType)
                {
                    switch (_kind)
                    {
                        case SEKind.Move:   ac = m_soldierInfoArr[i].moveAudioClip; break;
                        case SEKind.Battle: ac = m_soldierInfoArr[i].battleAudioClip; break;
                        case SEKind.Win:    ac = m_soldierInfoArr[i].winAudioClip; break;
                        case SEKind.Lose:   ac = m_soldierInfoArr[i].loseAudioClip; break;
                    }
                    break;
                }
            }
            return ac;
        }

        public bool IsMyTurn(SARTS.Soldier.PlSide _plSide)
        {
            bool ret = false;
            if (m_tickTimerScr.isOdd && (_plSide == SARTS.Soldier.PlSide.Pl2))
            {
                ret = true;
            }
            else if (m_tickTimerScr.isEven && (_plSide == SARTS.Soldier.PlSide.Pl1))
            {
                ret = true;
            }
            return ret;
        }

        public bool IsMyMoveTurn(SARTS.Soldier.PlSide _plSide)
        {
            bool ret = false;
            if (m_tickTimerScr.isOddPrevious && (_plSide == SARTS.Soldier.PlSide.Pl1))
            {
                ret = true;
            }
            else if (m_tickTimerScr.isEvenPrevious && (_plSide == SARTS.Soldier.PlSide.Pl2))
            {
                ret = true;
            }
            return ret;
        }

    }
}

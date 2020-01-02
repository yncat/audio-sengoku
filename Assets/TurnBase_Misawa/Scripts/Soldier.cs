using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TurnM
{
    public class Soldier : MonoBehaviour
    {
        [SerializeField,ReadOnlyWhenPlaying] GameManager m_gameManagerScr = null;
        [SerializeField,ReadOnlyWhenPlaying] SARTS.Soldier.PlSide m_plSide = SARTS.Soldier.PlSide.Pl1;
        [SerializeField,ReadOnlyWhenPlaying] SARTS.Soldier.PieceType m_pieceType = SARTS.Soldier.PieceType.Pawn;
        [SerializeField] Vector2Int m_pos = Vector2Int.zero;
        [SerializeField] AudioSource m_as = null;
        [SerializeField] Image m_soldierImage = null;
        float m_fraction;

        // Start is called before the first frame update
        void Start()
        {
            m_fraction = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_gameManagerScr.tickTimerScr.isTick)
            {
                // IsMyTurn は思考時間を示しているので、移動時間は相手の思考時間中
                //if (m_gameManagerScr.IsMyTurn(m_plSide == SARTS.Soldier.PlSide.Pl1 ? SARTS.Soldier.PlSide.Pl2 : SARTS.Soldier.PlSide.Pl1)) ;
                if(m_gameManagerScr.IsMyMoveTurn(m_plSide))
                {
                    updatePosition();
                    checkPosition();
                }
            }
            fixPosition();
        }
        public bool Init(GameManager _gameManagerScr, SARTS.Soldier.PlSide _plSide, SARTS.Soldier.PieceType _pieceType, Vector2Int _pos)
        {
            m_gameManagerScr = _gameManagerScr;
            m_plSide = _plSide;
            m_pieceType = _pieceType;
            m_pos = _pos;
            fixPosition();
            return true;
        }

        void updatePosition()
        {
            float spd = 1f;
            if (m_gameManagerScr != null)
            {
                for (int i = 0; i < m_gameManagerScr.soldierInfoArr.Length; ++i)
                {
                    if (m_pieceType == m_gameManagerScr.soldierInfoArr[i].pieceType)
                    {
                        spd = m_gameManagerScr.soldierInfoArr[i].fraction;
                        break;
                    }
                }
            }

            m_fraction += spd;
            if (m_fraction >= 1f)
            {
                m_fraction -= 1f;
                m_pos.x += ((m_plSide == SARTS.Soldier.PlSide.Pl1) ? 1 : -1);

                if (m_gameManagerScr != null)
                {
                    playSe(m_pieceType,SEKind.Move, m_pos.x);
                }
            }
        }

        bool checkPosition()
        {
            bool ret = false;
            int w = m_gameManagerScr.baseLineGroup.transform.GetChild(0).childCount;
            if ((m_pos.x < 0) || (m_pos.x >= w))
            {
                ret = true;
                Destroy(gameObject);
            }
            return ret;
        }

        void fixPosition()
        {
            if (m_gameManagerScr.baseLineGroup != null)
            {
                int h = m_gameManagerScr.baseLineGroup.transform.childCount;
                int w = m_gameManagerScr.baseLineGroup.transform.GetChild(0).childCount;
                int x = Mathf.Clamp(m_pos.x, 0, w - 1);
                int y = Mathf.Clamp(m_pos.y, 0, h - 1);
                m_pos.x = x;
                m_pos.y = y;
                if ((x >= 0) && (x < w) && (y >= 0) && (y < h))
                {
                    RectTransform lineTr = m_gameManagerScr.baseLineGroup.transform.GetChild(y) as RectTransform;
                    RectTransform boxTr = lineTr.GetChild(x) as RectTransform;
                    RectTransform imageBaseTr = boxTr.GetChild(0) as RectTransform;
                    transform.position = imageBaseTr.position; // + (Vector3)(pos2d);
                }
                if (m_gameManagerScr != null)
                {
                    for (int i = 0; i < m_gameManagerScr.soldierInfoArr.Length; ++i)
                    {
                        if (m_pieceType == m_gameManagerScr.soldierInfoArr[i].pieceType)
                        {
                            if (m_soldierImage != null)
                            {
                                m_soldierImage.sprite = m_gameManagerScr.soldierInfoArr[i].sprite;
                                Vector3 scl = Vector3.one;
                                if (m_gameManagerScr.soldierInfoArr[i].isFlip)
                                {
                                    scl.x *= -1f;
                                }
                                if (m_plSide == SARTS.Soldier.PlSide.Pl2)
                                {
                                    scl.x *= -1f;
                                }
                                transform.localScale = scl;
                                break;
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plaies the se.
        /// </summary>
        /// <param name="_plType">Pl type.</param>
        /// <param name="_posX">Position x.</param>
        /// <param name="_kind">Kind.</param>
        void playSe(SARTS.Soldier.PieceType _plType, SEKind _kind, int _posX)
        {
            AudioClip ac = m_gameManagerScr.GetSeClip(_plType, _kind);
            if (ac != null)
            {
                float ddx = Mathf.Clamp01((float)_posX / (float)m_gameManagerScr.fieldW);
                m_as.panStereo = m_gameManagerScr.GetStereoPan(ddx);
                m_as.Stop();
                m_as.clip = ac;
                m_as.pitch = Random.Range(0.98f, 1.02f)+(m_plSide== SARTS.Soldier.PlSide.Pl1 ? 0f:0.2f);
                m_as.PlayDelayed(Random.Range(0f,0.2f));
            }
        }

    }
}

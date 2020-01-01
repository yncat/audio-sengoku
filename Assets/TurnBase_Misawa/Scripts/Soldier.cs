using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TurnM
{
    public class Soldier : MonoBehaviour
    {
        [SerializeField, ReadOnlyWhenPlaying] TickTimer m_tickTimerScr = null;
        [SerializeField, ReadOnlyWhenPlaying] GameManager m_gameManagetScr = null;
        [SerializeField, ReadOnlyWhenPlaying] VerticalLayoutGroup m_lineGroup = null;
        [SerializeField] AudioSource m_as = null;
        [SerializeField] Vector2Int m_pos = Vector2Int.zero;
        [SerializeField] SARTS.Soldier.PieceType m_pieceType = SARTS.Soldier.PieceType.Pawn;
        [SerializeField] SARTS.Soldier.PlSide m_plType = SARTS.Soldier.PlSide.Pl1;
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
            if (m_tickTimerScr.isTick)
            {
                if (isMyTurn())
                {
                    updatePosition();
                }
            }
            fixPosition();
        }

        bool isMyTurn()
        {
            bool ret = false;
            if (m_tickTimerScr.isOddPrevious && (m_plType == SARTS.Soldier.PlSide.Pl1))
            {
                ret = true;
            }
            else if (m_tickTimerScr.isEvenPrevious && (m_plType == SARTS.Soldier.PlSide.Pl2))
            {
                ret = true;
            }
            return ret;
        }

        void updatePosition()
        {
            float spd = 1f;
            if (m_gameManagetScr != null)
            {
                for (int i = 0; i < m_gameManagetScr.soldierInfoArr.Length; ++i)
                {
                    if (m_pieceType == m_gameManagetScr.soldierInfoArr[i].pieceType)
                    {
                        spd = m_gameManagetScr.soldierInfoArr[i].fraction;
                        break;
                    }
                }
            }

            m_fraction += spd;
            if (m_fraction >= 1f)
            {
                m_fraction -= 1f;
                m_pos.x += ((m_plType == SARTS.Soldier.PlSide.Pl1) ? 1 : -1);

                if (m_gameManagetScr != null)
                {
                    playSe(m_pieceType,SEKind.Move, m_pos.x);
                }
            }
        }

        void fixPosition()
        {
            if (m_lineGroup != null)
            {
                int h = m_lineGroup.transform.childCount;
                int w = m_lineGroup.transform.GetChild(0).childCount;
                int x = Mathf.Clamp(m_pos.x, 0, w - 1);
                int y = Mathf.Clamp(m_pos.y, 0, h - 1);
                m_pos.x = x;
                m_pos.y = y;
                if ((x >= 0) && (x < w) && (y >= 0) && (y < h))
                {
                    RectTransform lineTr = m_lineGroup.transform.GetChild(y) as RectTransform;
                    RectTransform boxTr = lineTr.GetChild(x) as RectTransform;
                    RectTransform imageBaseTr = boxTr.GetChild(0) as RectTransform;
                    transform.position = imageBaseTr.position; // + (Vector3)(pos2d);
                }
                if (m_gameManagetScr != null)
                {
                    for (int i = 0; i < m_gameManagetScr.soldierInfoArr.Length; ++i)
                    {
                        if (m_pieceType == m_gameManagetScr.soldierInfoArr[i].pieceType)
                        {
                            if (m_soldierImage != null)
                            {
                                m_soldierImage.sprite = m_gameManagetScr.soldierInfoArr[i].sprite;
                                Vector3 scl = Vector3.one;
                                if (m_gameManagetScr.soldierInfoArr[i].isFlip)
                                {
                                    scl.x *= -1f;
                                }
                                if (m_plType == SARTS.Soldier.PlSide.Pl2)
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
            AudioClip ac = m_gameManagetScr.GetSeClip(_plType, _kind);
            if (ac != null)
            {
                float ddx = Mathf.Clamp01((float)_posX / (float)m_gameManagetScr.fieldW);
                m_as.panStereo = m_gameManagetScr.GetStereoPan(ddx);
                m_as.PlayOneShot(ac);
            }
        }

    }
}

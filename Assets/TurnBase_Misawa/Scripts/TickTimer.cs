using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TurnM
{
    public class TickTimer : MonoBehaviour
    {
        public bool isActive = false;
        [SerializeField, ReadOnlyWhenPlayingAttribute, Range(1f, 30f)] float m_tickTime = 10f;
        [SerializeField, ReadOnlyWhenPlayingAttribute] Slider m_slider = null;
        [SerializeField, ReadOnly] bool m_isTick = false;
        [SerializeField] AudioSource m_tickAc = null;
        [SerializeField] AudioClip m_tickClipP1 = null;
        [SerializeField] AudioClip m_tickClipP2 = null;
        public bool isTick { get { return m_isTick; } }
        public bool isOdd { get { return ((m_tickCount >= 0) && ((m_tickCount & 1) == 1)); } }
        public bool isEven { get { return ((m_tickCount >= 0) && ((m_tickCount & 1) == 0)); } }
        public bool isOddPrevious { get { return ((m_tickCountPrevious >= 0) && ((m_tickCount & 1) == 1)); } }
        public bool isEvenPrevious { get { return ((m_tickCountPrevious >= 0) && ((m_tickCount & 1) == 0)); } }
        float m_timer;
        int m_tickCount=-1;
        int m_tickCountPrevious = -1;

        // Start is called before the first frame update
        void Start()
        {
            m_isTick = false;
            m_timer = m_tickTime;
            m_tickCount = m_tickCountPrevious = - 1;
        }

        // Update is called once per frame
        void Update()
        {
            m_isTick = false;
            if (isActive)
            {
                m_timer -= Time.deltaTime;
                if (m_timer <= 0f)
                {
                    m_isTick = true;
                    m_timer += m_tickTime;
                    m_tickCountPrevious = m_tickCount;
                    m_tickCount++;
                    if (m_tickAc != null)
                    {
                        m_tickAc.PlayOneShot(isEven ? m_tickClipP1 : m_tickClipP2);
                        if (m_slider != null)
                        {
                            ColorBlock cblk = m_slider.colors;
                            cblk.normalColor = (isOdd ? Color.blue : Color.red);
                            m_slider.colors = cblk;
                        }
                    }
                }
                if (m_slider != null)
                {
                    m_slider.value = 1f-(m_timer / m_tickTime);
                }
            }
            else
            {
                m_tickCount = -1;
            }

        }
    }
}

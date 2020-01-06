using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TurnM
{
    public class GameField : MonoBehaviour
    {
        [SerializeField, ReadOnlyWhenPlaying] int m_fieldW = 7;
        [SerializeField, ReadOnlyWhenPlaying] int m_fieldH = 4;
        [SerializeField, ReadOnlyWhenPlaying] VerticalLayoutGroup m_baseLineGroup = null;
        public VerticalLayoutGroup baseLineGroup { get { return m_baseLineGroup; } }
        public int fieldW { get { return m_fieldW; } }
        public int fieldH { get { return m_fieldH; } }

        // Start is called before the first frame update
        void Awake()
        {
            if (m_baseLineGroup != null)
            {
                if (m_baseLineGroup.transform.childCount > 0)
                {
                    int num = m_baseLineGroup.transform.childCount;
                    for (int i=0; i< num - 1; ++i)
                    {
                        DestroyImmediate(m_baseLineGroup.transform.GetChild(1).gameObject);
                    }
                    Transform lineTr = m_baseLineGroup.transform.GetChild(0);
                    num = lineTr.childCount;
                    if (num > m_fieldW)
                    {
                        for (int i = m_fieldW; i < num; ++i)
                        {
                            DestroyImmediate(lineTr.GetChild(m_fieldW).gameObject);
                        }
                    }else if(num < m_fieldW)
                    {
                        for (int i = num; i < m_fieldW; ++i)
                        {
                            Instantiate(lineTr.GetChild(0).gameObject, lineTr);
                        }

                    }
                    num = m_baseLineGroup.transform.childCount;
                    if (num < m_fieldH)
                    {
                        for (int i = num; i < m_fieldH; ++i)
                        {
                            Instantiate(m_baseLineGroup.transform.GetChild(0).gameObject, m_baseLineGroup.transform);
                        }

                    }
                }
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

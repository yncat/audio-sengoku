using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TurnM
{
    public class GameField : MonoBehaviour
    {
        [SerializeField, ReadOnlyWhenPlaying] int m_fieldW = 10;
        [SerializeField, ReadOnlyWhenPlaying] int m_fieldH = 1;
        [SerializeField, ReadOnlyWhenPlaying] VerticalLayoutGroup m_baseGroup = null;

        // Start is called before the first frame update
        void Awake()
        {
            if (m_baseGroup != null)
            {
                if (m_baseGroup.transform.childCount > 0)
                {
                    int num = m_baseGroup.transform.childCount;
                    for (int i=0; i< num - 1; ++i)
                    {
                        DestroyImmediate(m_baseGroup.transform.GetChild(1).gameObject);
                    }
                    Transform lineTr = m_baseGroup.transform.GetChild(0);
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
                    num = m_baseGroup.transform.childCount;
                    if (num < m_fieldH)
                    {
                        for (int i = num; i < m_fieldH; ++i)
                        {
                            Instantiate(m_baseGroup.transform.GetChild(0).gameObject, m_baseGroup.transform);
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

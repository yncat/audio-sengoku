using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TurnM
{
    public class Soldier : MonoBehaviour
    {
        [SerializeField,ReadOnlyWhenPlayingAttribute] VerticalLayoutGroup m_lineGroup = null;
        [SerializeField] Vector2Int m_pos = Vector2Int.zero;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (m_lineGroup != null)
            {
                int h = m_lineGroup.transform.childCount;
                int w = m_lineGroup.transform.GetChild(0).childCount;
                int x = Mathf.Clamp(m_pos.x, 0, w-1);
                int y = Mathf.Clamp(m_pos.y, 0, h-1);
                m_pos.x = x;
                m_pos.y = y;
                if ((x>=0)&&(x<w)&&(y>=0) && (y <h))
                {
                    RectTransform lineTr = m_lineGroup.transform.GetChild(y) as RectTransform;
                    RectTransform boxTr = lineTr.GetChild(x) as RectTransform;
                    RectTransform imageBaseTr = boxTr.GetChild(0) as RectTransform;
                    transform.position = imageBaseTr.position; // + (Vector3)(pos2d);
                }
            }
        }
    }
}

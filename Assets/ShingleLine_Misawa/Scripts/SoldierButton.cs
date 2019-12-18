using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SARTS
{
    public class SoldierButton : MonoBehaviour
    {
        readonly int SET_SPD = 10;
        readonly float PUSH_OVER_TIME_MAX = 1f;
        [SerializeField] GameObject m_soldierPrefab = null;
        [SerializeField] Soldier.PieceType m_pieceType = Soldier.PieceType.Pawn;
        [SerializeField,Range(0,100000)] int m_soldierNum = 1000;
        [SerializeField, Range(0f, 1f)] float m_moveSpd = 1f;
        [SerializeField] Text m_buttonText = null;
        [SerializeField] Text m_remainText = null;
        [SerializeField] string m_layerName = "LayerPl1";
        [SerializeField] KeyCode m_keyCode = KeyCode.Alpha0;
        [SerializeField] AudioSource m_sysAudioSource = null;
        [SerializeField] internal AudioClip[] m_sysAcArr = null; // 0:EMPTY,1:FULL,2:NG
        float pow;
        float m_pushOverTime;

        Soldier m_soldierScr;
        private void Awake()
        {
            m_soldierScr = null;
        }

        // Start is called before the first frame update
        void Start()
        {
            string str = "足軽";
            switch (m_pieceType)
            {
                default: break;
                case Soldier.PieceType.Knight: str = "騎馬"; break;
                case Soldier.PieceType.Tank: str = "鉄砲"; break;
            }
            m_buttonText.text = str + "\n[" + m_keyCode.ToString()+"]";
            m_remainText.text = m_soldierNum.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_soldierScr != null)
            {
                if (m_soldierNum > 0)
                {
                    if (m_soldierNum >= SET_SPD)
                    {
                        pow += (float)SET_SPD;
                        m_soldierNum -= SET_SPD;
                    }
                    else
                    {
                        pow += (float)m_soldierNum;
                        m_soldierNum = 0;
                    }
                    m_soldierScr.SetPower(pow);
                    if (m_soldierNum==0)
                    {
                        m_sysAudioSource.PlayOneShot(m_sysAcArr[1], 1f);
                        m_pushOverTime = PUSH_OVER_TIME_MAX;
                    }
                }
                else
                {
                    m_pushOverTime -= Time.deltaTime;
                    if (m_pushOverTime <= 0f)
                    {
                        m_sysAudioSource.PlayOneShot(m_sysAcArr[0], 1f);
                        Destroy(m_soldierScr.gameObject);
                        m_soldierNum = (int)(m_soldierScr.GetPower());
                        m_soldierScr = null;
                    }
                }
            }
            if (Input.GetKeyDown(m_keyCode))
            {
                PointerDown();
            }
            else if (Input.GetKeyUp(m_keyCode))
            {
                PointerUp();
            }
            m_remainText.text = Mathf.FloorToInt(m_soldierNum).ToString();
        }

        public void PointerDown()
        {
            if (m_soldierNum <= 0)
            {
                m_sysAudioSource.PlayOneShot(m_sysAcArr[2], 1f);
                return;
            }
            if ((m_soldierNum>0)&&(m_soldierPrefab != null) &&(m_soldierScr == null))
            {
                m_pushOverTime = 0f;
                GameObject go = Instantiate(m_soldierPrefab);
                go.layer = LayerMask.NameToLayer(m_layerName);

                m_soldierScr = go.GetComponent<Soldier>();
                m_soldierScr.plSide = Soldier.PlSide.Pl1;
                m_soldierScr.meshTr.gameObject.layer = LayerMask.NameToLayer(m_layerName);
                m_soldierScr.moveSpd = m_moveSpd;

                Vector3 pos = new Vector3(0.05f, 0.5f, -Camera.main.transform.position.z);
                int mm = go.layer;
                int nn = LayerMask.NameToLayer("LayerPl2");
                //Debug.Log(mm + "," + nn);
                if (mm == nn)
                {
                    m_soldierScr.plSide = Soldier.PlSide.Pl2;
                    pos.x = 1f - pos.x;
                }
                pos = Camera.main.ViewportToWorldPoint(pos);
                go.transform.position = pos;
                m_soldierScr.pieceType = m_pieceType;
                pow = 0f;
                m_soldierScr.SetPower(pow);
                m_soldierScr.moveState = Soldier.MoveState.Wait;
            }

        }
        public void PointerUp()
        {
            if (m_soldierScr != null)
            {
                m_pushOverTime = 0f;
                m_soldierScr.moveState = Soldier.MoveState.Move;
                m_soldierScr = null;
            }
        }
    }
}

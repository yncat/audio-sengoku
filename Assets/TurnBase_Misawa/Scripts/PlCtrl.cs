using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TurnM
{
    public class PlCtrl : MonoBehaviour
    {
        readonly float  PL_OFS_X = 0.12f;
        [SerializeField, ReadOnlyWhenPlaying] SARTS.Soldier.PlSide m_plSide = SARTS.Soldier.PlSide.Pl1;
        [SerializeField] KeyCode m_plUpKey = KeyCode.UpArrow;
        [SerializeField] KeyCode m_plDownKey = KeyCode.DownArrow;
        [SerializeField] KeyCode m_plChangeKey = KeyCode.LeftArrow;
        [SerializeField] KeyCode m_plGenerateKey = KeyCode.RightArrow;
        [SerializeField] AudioClip m_UpDownClip = null;
        [SerializeField, ReadOnlyWhenPlaying] GameManager m_gameManagerScr = null;
        [SerializeField, ReadOnlyWhenPlaying] Image m_playerImage = null;
        [SerializeField, ReadOnlyWhenPlaying] Transform m_soldiersParentTr = null;
        [SerializeField, ReadOnlyWhenPlaying] GameObject m_soldierPrefab = null;
        [SerializeField, ReadOnlyWhenPlaying] Image m_soldierIconImage = null;
        [SerializeField, ReadOnlyWhenPlaying] Text m_remainText = null;
        [SerializeField, ReadOnlyWhenPlaying] AudioSource m_outputAudioSource = null;
        [SerializeField] Vector2Int m_pos = Vector2Int.zero;
        Vector2Int m_previousPos;
        int m_selectedPieceId;

        // Start is called before the first frame update
        void Start()
        {
            m_previousPos = m_pos;
            m_selectedPieceId = 0;
            m_remainText.text = "";
            if (m_outputAudioSource != null)
            {
                m_outputAudioSource.panStereo = (m_plSide == SARTS.Soldier.PlSide.Pl1) ? -1 : 1;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(m_plUpKey))
            {
                    OnLineUp();
            }
            else if (Input.GetKeyDown(m_plDownKey))
            {
                    OnLineDown();
            }
            else if (Input.GetKeyDown(m_plChangeKey))
            {
                    OnPlChange();
            }
            else if (Input.GetKeyDown(m_plGenerateKey))
            {
                    OnSoldierGenerate();
            }

            fixPosition();
        }

        public void OnLineUp()
        {
            if (m_gameManagerScr.IsMyTurn(m_plSide) || m_gameManagerScr.is1stTurn)
                m_pos.y -= 1;
        }

        public void OnLineDown()
        {
            if (m_gameManagerScr.IsMyTurn(m_plSide) || m_gameManagerScr.is1stTurn)
                m_pos.y += 1;
        }

        public void OnPlChange()
        {
            if (m_gameManagerScr.IsMyTurn(m_plSide) || m_gameManagerScr.is1stTurn)
                changePieceType();
        }

        public void OnSoldierGenerate()
        {
            if (m_gameManagerScr.IsMyTurn(m_plSide))
            {
                if (m_soldiersParentTr != null) { }
                if (m_outputAudioSource != null)
                {
                    m_outputAudioSource.Stop();
                    m_outputAudioSource.PlayOneShot(m_gameManagerScr.soldierInfoArr[m_selectedPieceId].battleAudioClip);
                }
                GameObject go = Instantiate(m_soldierPrefab, m_soldiersParentTr);
                go.GetComponent<Soldier>().Init(
                    m_gameManagerScr,
                    m_plSide,
                    m_gameManagerScr.soldierInfoArr[m_selectedPieceId].pieceType,
                    m_pos);
                Destroy(go, 1000f);
            }
        }

        int changePieceType()
        {
            if (m_gameManagerScr != null)
            {
                m_selectedPieceId = (m_selectedPieceId + 1) % m_gameManagerScr.soldierInfoArr.Length;
                if (m_soldierIconImage) {
                    m_soldierIconImage.sprite = m_gameManagerScr.soldierInfoArr[m_selectedPieceId].sprite;
                }
                if (m_outputAudioSource != null)
                {
                    m_outputAudioSource.Stop();
                    m_outputAudioSource.PlayOneShot(m_gameManagerScr.soldierInfoArr[m_selectedPieceId].battleAudioClip);
                }
            }
            return m_selectedPieceId;
        }

        void fixPosition()
        {
            int h = m_gameManagerScr.baseLineGroup.transform.childCount;
            int w = m_gameManagerScr.baseLineGroup.transform.GetChild(0).childCount;
            int x = (m_plSide == SARTS.Soldier.PlSide.Pl1 ? 0: w - 1);  //Mathf.Clamp(m_pos.x, 0, w - 1);
            int y = Mathf.Clamp(m_pos.y, 0, h - 1);
            m_pos.x = x;
            m_pos.y = y;
            if (m_pos.y != m_previousPos.y)
            {
                m_outputAudioSource.PlayOneShot(m_UpDownClip);
            }

            if ((x >= 0) && (x < w) && (y >= 0) && (y < h))
            {
                RectTransform lineTr = m_gameManagerScr.baseLineGroup.transform.GetChild(y) as RectTransform;
                RectTransform boxTr = lineTr.GetChild(x) as RectTransform;
                RectTransform imageBaseTr = boxTr.GetChild(0) as RectTransform;
                Vector3 pos = imageBaseTr.position;
                pos.x += (m_plSide == SARTS.Soldier.PlSide.Pl1 ? -1 : 1) * (Screen.width * PL_OFS_X);
                transform.position = pos;
            }
            if (m_gameManagerScr != null)
            {
                Vector3 scl = Vector3.one;
                if (m_plSide == SARTS.Soldier.PlSide.Pl1)
                {
                    scl.x *= -1f;
                }
                m_playerImage.transform.localScale = scl;
            }
            m_previousPos = m_pos;
        }
    }
}

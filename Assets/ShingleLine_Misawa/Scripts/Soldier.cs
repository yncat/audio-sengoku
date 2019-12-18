using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SARTS
{
    //[ExecuteInEditMode]
    public class Soldier : MonoBehaviour
    {
        public enum PieceType
        {
            Pawn = 0, // チョキ
            Knight, // パー
            Tank,   // グー
        }
        public enum MoveState
        {
            Wait=0,
            Move,
            Finish
        }
        public enum PlSide
        {
            Pl1 = 0,
            Pl2,
        }
        internal readonly float HEIGHT_GAIN = 0.001f;
        public PieceType pieceType;
        public MoveState moveState;
        public PlSide plSide;
        public float moveSpd;
        [SerializeField] internal AnimationCurve m_volumeAc = null;
        [SerializeField] internal AnimationCurve m_panAc = null;
        [SerializeField] internal GameObject m_messangerPrefab = null;
        [SerializeField, Range(0f, 100000f)] internal float m_power =1000f;
        [SerializeField] internal Transform m_meshTr = null;
        public Transform meshTr { get { return m_meshTr; } }
        [SerializeField] internal TMPro.TMP_Text m_text = null;
        [SerializeField, Range(0.01f,1f)] internal float m_moveSpdGain = 0.1f;
        [SerializeField, Range(0.1f, 10f)] internal float m_BattleSpdGain = 1f;
        [SerializeField] internal AudioClip m_winAc = null;

        [SerializeField, Range(0f, 1f)] internal float m_PKTvsPKT = 0.5f;
        [SerializeField, Range(0f, 1f)] internal float m_PvsK = 0.35f;
        [SerializeField, Range(0f, 1f)] internal float m_PvsT = 0.25f;
        [SerializeField, Range(0f, 1f)] internal float m_KvsT = 0.4f;

        [SerializeField] internal AudioClip[] m_footAcArr = null;
        [SerializeField] internal AudioClip[] m_hitAcArr = null;
        [SerializeField] internal AudioMixerGroup[] m_audioMixerGroupArr=null;
        internal AudioSource m_footAudioSource;
        internal AudioSource m_hitAudioSource;
        internal AudioSource m_ac;


        private void Awake()
        {
            m_power = 0f;
        }

        // Start is called before the first frame update
        public virtual void Start()
        {
            m_ac = GetComponent<AudioSource>();
            m_ac.volume = 0f;
            m_ac.outputAudioMixerGroup = m_audioMixerGroupArr[(plSide == PlSide.Pl1)?0:1];

            Color col;
            switch (pieceType)
            {
                case PieceType.Knight: col = Color.yellow; break;
                case PieceType.Tank: col = Color.green; break;
                default: col = Color.white; break;
            }
            if(plSide== PlSide.Pl2)
            {
                moveSpd *= -1f;
            }

            m_footAudioSource = gameObject.AddComponent<AudioSource>();
            m_footAudioSource.loop = true;
            m_footAudioSource.clip = m_footAcArr[(int)pieceType];
            m_footAudioSource.outputAudioMixerGroup = m_ac.outputAudioMixerGroup;
            m_footAudioSource.Play();
            m_hitAudioSource = gameObject.AddComponent<AudioSource>();
            m_hitAudioSource.loop = false;
            m_hitAudioSource.outputAudioMixerGroup = m_ac.outputAudioMixerGroup;
           // m_hitAudioSource.Play();

#if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                m_meshTr.GetComponent<MeshRenderer>().sharedMaterial.color = col;
            }
            else
            {
                m_meshTr.GetComponent<MeshRenderer>().material.color = col;
            }
#else
            m_meshTr.GetComponent<MeshRenderer>().material.color = col;
#endif
        }

        // Update is called once per frame
        void Update()
        {
            setVolumeAndPan();

            if (m_meshTr != null)
            {
                m_meshTr.transform.localScale = new Vector3(1f, m_power * HEIGHT_GAIN, 1f);
                m_meshTr.transform.localPosition = new Vector3(0f, m_power * HEIGHT_GAIN * 0.5f, 0f);
                m_text.text = Mathf.CeilToInt(m_power).ToString();
            }
            if(moveState== MoveState.Move)
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.MovePosition(transform.position + Time.deltaTime * moveSpd * m_moveSpdGain * Vector3.right);
                //transform.position += Time.deltaTime * moveSpd * m_moveSpdGain * Vector3.right;

            }
#if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Color col;
                switch (pieceType)
                {
                    case PieceType.Knight: col = Color.yellow; break;
                    case PieceType.Tank: col = Color.green; break;
                    default: col = Color.white; break;
                }
                m_meshTr.GetComponent<MeshRenderer>().sharedMaterial.color = col;
            }
#endif
        }

        private void OnCollisionStay(Collision collision)
        {
            Soldier enemyScr = collision.gameObject.GetComponent<Soldier>();
            if (enemyScr != null)
            {
                float myRate = m_PKTvsPKT;
                if((pieceType == PieceType.Pawn) && (enemyScr.pieceType == PieceType.Knight)){
                    myRate = m_PvsK;
                }
                if ((pieceType == PieceType.Pawn) && (enemyScr.pieceType == PieceType.Tank))
                {
                    myRate = m_PvsT;
                }
                if ((pieceType == PieceType.Knight) && (enemyScr.pieceType == PieceType.Pawn))
                {
                    myRate = (1f-m_PvsK);
                }
                if ((pieceType == PieceType.Knight) && (enemyScr.pieceType == PieceType.Tank))
                {
                    myRate = m_KvsT;
                }
                if ((pieceType == PieceType.Tank) && (enemyScr.pieceType == PieceType.Pawn))
                {
                    myRate = (1f- m_PvsT);
                }
                if ((pieceType == PieceType.Tank) && (enemyScr.pieceType == PieceType.Knight))
                {
                    myRate = (1f- m_KvsT);
                }

                float ret;
                ret = AddPower(-m_BattleSpdGain * (1f - myRate));
                int result = 0;
                if (ret <= 0f)
                {
                    result |= 1; // pl全滅
                }
                ret = enemyScr.AddPower(-m_BattleSpdGain * myRate);
                if (ret <= 0f)
                {
                    result |= 2; // em全滅
                }

                // sound test---------------------
                if (((result & 1) == 0) && (Random.value < 0.01f))
                {
                    m_hitAudioSource.PlayOneShot(m_hitAcArr[(int)pieceType]);
                }
                if (result == 2)
                {
                    m_ac.PlayOneShot(m_winAc, 2f);
                }
                // sound test---------------------


                if (result != 0)
                {
                    GameObject emGo = collision.gameObject;
                    Soldier emSoldier = emGo.GetComponent<Soldier>();


                    if (result == 1)
                    {
                        GameObject plMesGo = Instantiate(m_messangerPrefab, transform.position, transform.rotation);
                        Messenger plMessenger = plMesGo.GetComponent<Messenger>();
                        //GameObject emMesGo = Instantiate(m_messangerPrefab, emGo.transform.position, emGo.transform.rotation);
                        //Messenger emMessenger = emMesGo.GetComponent<Messenger>();
                        plMessenger.SetResultMessage(this, false);
                        //emMessenger.SetResultMessage(emSoldier, true);
                    }
                    if (result == 2)
                    {
                        //GameObject plMesGo = Instantiate(m_messangerPrefab, transform.position, transform.rotation);
                        //Messenger plMessenger = plMesGo.GetComponent<Messenger>();
                        GameObject emMesGo = Instantiate(m_messangerPrefab, emGo.transform.position, emGo.transform.rotation);
                        Messenger emMessenger = emMesGo.GetComponent<Messenger>();
                        //plMessenger.SetResultMessage(this, true);
                        emMessenger.SetResultMessage(emSoldier, false);
                    }
                    if (result == 3)
                    {
                        GameObject plMesGo = Instantiate(m_messangerPrefab, transform.position, transform.rotation);
                        Messenger plMessenger = plMesGo.GetComponent<Messenger>();
                        GameObject emMesGo = Instantiate(m_messangerPrefab, emGo.transform.position, emGo.transform.rotation);
                        Messenger emMessenger = emMesGo.GetComponent<Messenger>();
                        plMessenger.SetResultMessage(this, false);
                        emMessenger.SetResultMessage(emSoldier, false);
                    }
                }

                if ((result & 1) !=0)
                {
                    gameObject.GetComponent<BoxCollider>().enabled = false; ;
                    Destroy(gameObject);
                }else 
                if ((result & 2) != 0)
                {
                    collision.gameObject.GetComponent<BoxCollider>().enabled = false; ;
                    Destroy(collision.gameObject);
                }

            }
        }

        public void SetPower(float _power)
        {
            m_power = _power;
        }
        public float AddPower(float _power)
        {
            m_power = Mathf.Max(m_power + _power,0f);
            return m_power;
        }
        private void setVolumeAndPan()
        {
            float pan = Mathf.Clamp01(Camera.main.WorldToViewportPoint(transform.position).x);
            float distVol = (plSide == PlSide.Pl2) ? pan : (1f-pan);
            float amountVol = Mathf.Min(m_power * 0.001f, 1f);
            m_ac.volume = m_volumeAc.Evaluate(distVol)*amountVol;
            m_ac.panStereo = m_panAc.Evaluate(pan);

            m_footAudioSource.volume = m_ac.volume;
            m_footAudioSource.panStereo = m_ac.panStereo;
            m_hitAudioSource.volume = Mathf.Min(m_ac.volume*2f,1f);
            m_hitAudioSource.panStereo = m_ac.panStereo;
        }

    }
}

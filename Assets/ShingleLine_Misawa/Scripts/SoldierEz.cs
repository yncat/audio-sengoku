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
    public class SoldierEz : Soldier
    {
        public override void Start()
        {
            base.Start();
            m_PvsK = 0.5f;
            m_PvsT = 0.5f;
            m_KvsT = 0.5f;
        }
        // Update is called once per frame
        public virtual void Update()
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

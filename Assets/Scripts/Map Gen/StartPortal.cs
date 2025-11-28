using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPortal : Portal
{
    RoomManager m_RoomManager;
    [SerializeField] bool m_OpenSesame;
    [SerializeField] Color m_ChangeColor;
    SpriteRenderer m_SpriteR;
    Coroutine m_OpenSesameC;

    private void Awake()
    {
        m_SpriteR = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        m_RoomManager = FindObj.instance.roomManagerScript;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && m_RoomManager.finish && m_OpenSesame)
        {
            MapUIManager.instance.UpdateMinimapUI(true);
            Destination = m_RoomManager.rooms[0].transform;
            other.transform.position = Destination.position;
            CameraManager.instance.CameraMove(other.gameObject);
            CameraManager.instance.CenterMove(other.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_RoomManager.finish && !m_OpenSesame)
        {
            if(m_OpenSesameC == null)
            {
                m_OpenSesameC = StartCoroutine("ChangeColorC");
            }
        }
    }

    IEnumerator ChangeColorC()
    {
        float time = 1.0f;
        Color StartColor = m_SpriteR.color;

        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            m_SpriteR.color = StartColor * time + (1 - time) * m_ChangeColor;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        m_OpenSesame = true;
    }

}

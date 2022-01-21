using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player_sync : MonoBehaviourPunCallbacks, IPunObservable
{
    public MonoBehaviour[] localScripts;
    public GameObject[] localObjects;

    Rigidbody rb;

    Vector3 latestPosition;
    Quaternion latestRotation;
    Vector3 latestVelocity;

    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;
    Vector3 velocityAtLastPacket = Vector3.zero;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = !photonView.IsMine;
        for (int i = 0; i < localScripts.Length; i++)
        {
            localScripts[i].enabled = photonView.IsMine;
        }
        for (int i = 0; i < localObjects.Length; i++)
        {
            localObjects[i].SetActive(photonView.IsMine);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rb.velocity);
        }
        else
        {
            latestPosition = (Vector3)stream.ReceiveNext();
            latestRotation = (Quaternion)stream.ReceiveNext();
            latestVelocity = (Vector3)stream.ReceiveNext();

            // Lag compensation
            currentTime = 0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;
            velocityAtLastPacket = rb.velocity;

        }
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            // Lag compensation
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            transform.position = Vector3.Lerp(positionAtLastPacket, latestPosition, (float)(currentTime / timeToReachGoal));
            transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRotation, (float)(currentTime / timeToReachGoal));
            rb.velocity = Vector3.Lerp(velocityAtLastPacket, latestVelocity, (float)(currentTime / timeToReachGoal));
        }
    }
}

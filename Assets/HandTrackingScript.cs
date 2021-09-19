using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
// Code adapted from Magic Leap Docs: 
public class HandTrackingScript : MonoBehaviour
{
    public enum HandPoses { Ok, Finger, Thumb, OpenHand, Fist, Pinch, NoPose };
    public HandPoses pose = HandPoses.NoPose;
    public Vector3[] pos;
    public GameObject sphereThumb, sphereIndex, sphereWrist;
    public GameObject ObjectToPlace;
    public GameObject selectedGameObject;
    bool canPlace;
    bool canGrab;

    private MLHandTracking.HandKeyPose[] _gestures;

    private void Start()
    {
        MLHandTracking.Start();
        _gestures = new MLHandTracking.HandKeyPose[6];
        _gestures[0] = MLHandTracking.HandKeyPose.Ok;
        _gestures[1] = MLHandTracking.HandKeyPose.Finger;
        _gestures[2] = MLHandTracking.HandKeyPose.OpenHand;
        _gestures[3] = MLHandTracking.HandKeyPose.Fist;
        _gestures[4] = MLHandTracking.HandKeyPose.Thumb;
        _gestures[5] = MLHandTracking.HandKeyPose.Pinch;
        MLHandTracking.KeyPoseManager.EnableKeyPoses(_gestures, true, false);
        pos = new Vector3[3];
        canPlace = true;
        canGrab = false;
    }
    private void OnDestroy()
    {
        MLHandTracking.Stop();
    }

    public void OnTriggerEnter(Collider other) {
        if(!canGrab) {
            selectedGameObject = other.gameObject;
            canGrab = true;
            other.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    public void OnTriggerExit(Collider other) {
        canGrab = false;
        other.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }


    private void Update()
    {
        ShowPoints();
        transform.position = pos[1];
        if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Ok))
        {
            pose = HandPoses.Ok;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Finger))
        {
            pose = HandPoses.Finger;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.OpenHand))
        {
            pose = HandPoses.OpenHand;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Fist))
        {
            pose = HandPoses.Fist;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Thumb))
        {
            pose = HandPoses.Thumb;
            if(canPlace) {
                GameObject objPlaced = Instantiate(ObjectToPlace, pos[0], transform.rotation);
                canPlace = false;
            }
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Pinch)) {
            pose = HandPoses.Pinch;
            if(canGrab) {
                selectedGameObject.transform.position = transform.position;
                gameObject.GetComponent<SphereCollider>().radius = 0.1f;
            }
        } else
        {
            pose = HandPoses.NoPose;
            canPlace = true;
            if(selectedGameObject && !canGrab) {
                selectedGameObject = null;
                gameObject.GetComponent<SphereCollider>().radius = 0.01f; 
            }
        }

    }

    private void ShowPoints()
    {
        // Left Hand Thumb tip
        pos[0] = MLHandTracking.Left.Thumb.KeyPoints[2].Position;
        // Left Hand Index finger tip 
        pos[1] = MLHandTracking.Left.Index.KeyPoints[2].Position;
        // Left Hand Wrist 
        pos[2] = MLHandTracking.Left.Wrist.KeyPoints[0].Position;
        sphereThumb.transform.position = pos[0];
        sphereIndex.transform.position = pos[1];
        sphereWrist.transform.position = pos[2];
    }

    private bool GetGesture(MLHandTracking.Hand hand, MLHandTracking.HandKeyPose type)
    {
        if (hand != null)
        {
            if (hand.KeyPose == type)
            {
                if (hand.HandKeyPoseConfidence > 0.9f)
                {
                    return true;
                }
            }
        }
        return false;
    }

}

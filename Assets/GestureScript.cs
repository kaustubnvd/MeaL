using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class GestureScript : MonoBehaviour
{
  private MLHandTracking.HandKeyPose[] gestures; // Holds the different hand poses we will look for
  private GameObject cube; // Reference to our Cube
  private Renderer cubeRender;
  private bool openFist;
  public Vector3[] pos;
  public GameObject objectToPlace;
  public GameObject selectedGameObject;
  bool canIPlace = false;
  bool canIGrab = false;

  void Start()
  {
    MLHandTracking.Start(); // Start the hand tracking.

    gestures = new MLHandTracking.HandKeyPose[4]; //Assign the gestures we will look for.
    gestures[0] = MLHandTracking.HandKeyPose.Ok;
    gestures[1] = MLHandTracking.HandKeyPose.Fist;
    gestures[2] = MLHandTracking.HandKeyPose.OpenHand;
    gestures[3] = MLHandTracking.HandKeyPose.Finger;
    // Enable the hand poses.
    MLHandTracking.KeyPoseManager.EnableKeyPoses(gestures, true, false);
    openFist = true;
    cube = GameObject.Find("Cube"); // Find our Cube in the scene.

    cubeRender = cube.GetComponent<Renderer>();
    cube.SetActive(false);

    pos = new Vector3[3];
    InitPoints();
  }

  private void InitPoints()
  {
    pos[0] = MLHandTracking.Left.Thumb.KeyPoints[2].Position;
    pos[1] = MLHandTracking.Left.Index.KeyPoints[2].Position;
    pos[2] = MLHandTracking.Left.Wrist.KeyPoints[0].Position;
  }

  private void OnTriggerEnter(Collider other)
  {
    if(!canIGrab) 
    {
      selectedGameObject = this.gameObject;
      canIGrab = true;
      this.gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
        
  }

  private void OnTriggerExit(Collider other)
  {
      canIGrab = false;
      this.gameObject.GetComponent<Renderer>().material.color = Color.white;

  }
  // Update is called once per frame
  const bool HIGHER_THRESHOLD = true;
  void Update()
  {
    //Debug.Log("Update");
    //Press Button
    //transform.position = pos[0];
    if (GestureMade(MLHandTracking.HandKeyPose.Fist, HIGHER_THRESHOLD) && openFist)
    {
      openFist = false;
      cubeRender.material.SetColor("_Color", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f));
    }
    else if (!GestureMade(MLHandTracking.HandKeyPose.Fist, !HIGHER_THRESHOLD))
    {
      openFist = true;
      cube.SetActive(true);
    }

    if (GestureMade(MLHandTracking.HandKeyPose.Pinch, HIGHER_THRESHOLD))
    {
      if (canIGrab)
      {
        selectedGameObject.transform.position = transform.position;
        gameObject.GetComponent<SphereCollider>().radius = 1f;
      }
    }
    else if (GestureMade(MLHandTracking.HandKeyPose.Pinch, !HIGHER_THRESHOLD))
    {
      GameObject placedObject = Instantiate(objectToPlace, pos[0], transform.rotation);
      canIPlace = false;
    }
  }

  bool GestureMade(MLHandTracking.HandKeyPose type, bool useHigherThresh)
  {
    float thresh = useHigherThresh ? 0.9f : 0.1f;
    return GetGesture(MLHandTracking.Left, type, thresh) || GetGesture(MLHandTracking.Right, type, thresh);
  }

  bool GetGesture(MLHandTracking.Hand hand, MLHandTracking.HandKeyPose type, float thresh)
  {
    if (hand != null)
    {
      if (hand.KeyPose == type)
      {
        if (hand.HandKeyPoseConfidence > thresh)
        {
          return true;
        }
      }
    }
    return false;
  }

}

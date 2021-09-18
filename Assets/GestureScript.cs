using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class GestureScript : MonoBehaviour
{
  // Start is called before the first frame update
  private MLHandTracking.HandKeyPose[] gestures; // Holds the different hand poses we will look for
  private GameObject cube; // Reference to our Cube
  private Renderer cubeRender;
  private bool openFist;
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
  }

    // Update is called once per frame
    void Update()
    {
    if ((GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Fist)
       || GetGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Fist)) && openFist) {
           openFist = false;
            cubeRender.material.SetColor("_Color", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f));
        } else if (!(GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Fist)
       || GetGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Fist))){
            openFist = true;
        }
    }

  bool GetGesture(MLHandTracking.Hand hand, MLHandTracking.HandKeyPose type)
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

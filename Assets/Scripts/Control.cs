using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
public class Control : MonoBehaviour {

  #region Public Variables  
  public Material FrameMat;
  public GameObject WorldCanvas;
  public GameObject Light;
  public GameObject Camera;
  #endregion
  
  #region Private Variables
  private HeadLockScript _headlock;
  #endregion
  
  #region Unity Methods
  private void Start() {
    // Get the HeadLockScript script
    _headlock = GetComponentInChildren<HeadLockScript>();

  }
  private void Update() {
    _headlock.HeadLock(WorldCanvas, 5.0f, -0.25f, 0.2f);
  }
  #endregion
}
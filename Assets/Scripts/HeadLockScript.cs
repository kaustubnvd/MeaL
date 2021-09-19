using System;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
public class HeadLockScript : MonoBehaviour {

  #region Public Variables
  public GameObject Camera;
  #endregion
  
  #region Private Variables
  private const float _distance = 1.5f;
  #endregion
  
  #region Public Methods
  public void HardHeadLock(GameObject obj) {
    obj.transform.position = Camera.transform.position + Camera.transform.forward * _distance;
    obj.transform.rotation = Camera.transform.rotation;
  }
  public void HeadLock(GameObject obj, float speed, float offsetX, float offsetY) {	
    Vector3 newPos = new Vector3(Camera.transform.position.x + offsetX, Camera.transform.position.y + offsetY, Camera.transform.position.z);
    speed = Time.deltaTime * speed;
    Vector3 posTo = newPos + (Camera.transform.forward * _distance);
    obj.transform.position = Vector3.SlerpUnclamped(obj.transform.position, posTo, speed);
    Quaternion rotTo = Quaternion.LookRotation(obj.transform.position - newPos);
    obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, rotTo, speed);	
  }
  #endregion
  
}

using System;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using TMPro;
public class TextChange : MonoBehaviour {

  #region Public Variables
  public TextMeshPro textMesh;
  #endregion
  
  #region Private Variables
  private const float _triggerThreshold = 0.2f;
  private bool _triggerPressed = false;
  private bool _bumperPressed = false;
  private string[] steps = {"PB & J", "Ingredients: Peanut Butter, Jelly, Bread", "Toast Bread", "Spread Peanut Butter on one slice", "Spread Jelly on other slice", "Close and done"};
  private int index;
  private MLInput.Controller _control;
  #endregion
  
  #region Public Methods
//   public void HardHeadLock(GameObject obj) {
//     obj.transform.position = Camera.transform.position + Camera.transform.forward * _distance;
//     obj.transform.rotation = Camera.transform.rotation;
//   }
//   public void HeadLock(GameObject obj, float speed, float offsetX, float offsetY) {	
//     Vector3 newPos = new Vector3(Camera.transform.position.x + offsetX, Camera.transform.position.y + offsetY, Camera.transform.position.z);
//     speed = Time.deltaTime * speed;
//     Vector3 posTo = newPos + (Camera.transform.forward * _distance);
//     obj.transform.position = Vector3.SlerpUnclamped(obj.transform.position, posTo, speed);
//     Quaternion rotTo = Quaternion.LookRotation(obj.transform.position - newPos);
//     obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, rotTo, speed);	
//   }
  #endregion

  #region Private Methods
  private void Start() {
    // Setup and Start Magic Leap input and add a button event (will be used for the HomeTap)
    MLInput.OnControllerButtonDown += OnButtonDown;
    MLInput.OnControllerButtonUp += OnButtonUp;
    _control = MLInput.GetController(MLInput.Hand.Left);
    Reset();
  }

  private void Reset() {
    index = 0;
    textMesh.text = steps[index];
  }

  private void Update() {
    if (!_triggerPressed && _control.TriggerValue > _triggerThreshold) {
       _triggerPressed = true;
       incrementStep();
     }
     else if (_control.TriggerValue == 0.0f && _triggerPressed) {
       _triggerPressed = false;
     }
  }

  private void OnDestroy () {
    MLInput.OnControllerButtonUp -= OnButtonUp;
    MLInput.OnControllerButtonDown -= OnButtonDown;
  }
  
  public void incrementStep() {
  if (index < steps.Length) {
      index += 1;
      textMesh.text = "(" + index + ") " + steps[index];
    }
  }

  public void decrementStep() {
    if (index > 0) {
      index -= 1;
      if (index == 0)
        Reset();
      else
        textMesh.text = "(" + index + ") " + steps[index];
    }
  }



  /// OnButtonUp
   /// Button event - reset scene when home button is tapped
   ///
   private void OnButtonUp(byte controller_id, MLInput.Controller.Button button) {
     if (button == MLInput.Controller.Button.HomeTap) {
       Reset();
     }
     else if (button == MLInput.Controller.Button.Bumper) {
       _bumperPressed = false;
     }
   }

   private void OnButtonDown(byte controller_id, MLInput.Controller.Button button) {
     if (button == MLInput.Controller.Button.Bumper && !_bumperPressed) {
       _bumperPressed = true;
       decrementStep();
     }
   }
   #endregion
}


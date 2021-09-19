using System;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using TMPro;
public class Timer : MonoBehaviour {

  #region Public Variables
  public TextMeshPro timerText;
  #endregion
  
  #region Private Variables
    private MLInput.Controller _controller;
    private bool timerStarted; 
    private float secondsLeft;
  #endregion
  
  #region Public Methods
  #endregion

  #region Private Methods
  private void Start() {
    // Setup and Start Magic Leap input and add a button event (will be used for the HomeTap)
    //_control = MLInput.GetController(MLInput.Hand.Left);
    ResetTimer();
    secondsLeft = 60;
    StartTimer();
  }

  private void ResetTimer() {
      timerText.color = new Color(255, 255, 255);
      timerStarted = false;
      secondsLeft = 0;
  }

  private void StartTimer() {
      timerStarted = true;
  }

  private void StopTimer() {
      timerStarted = false;
  }

  private void IncrementSeconds() {
      secondsLeft += 10;
  }

  private void DecrementSeconds() {
      secondsLeft -= 10;
  }

  private void Update() {
    if (timerStarted) {
        if (secondsLeft > 0)
            secondsLeft -= Time.deltaTime;
        else {
            timerText.color = new Color(255, 0, 0);
            secondsLeft = 0;
        }
    }
    UpdateTime();
  }

  private void UpdateTime() {
    string minutes = Mathf.Floor(secondsLeft / 60).ToString("00");
    string seconds = Mathf.Floor(secondsLeft % 60).ToString("00");

    timerText.text = minutes + ":" + seconds;
  }

  private void OnDestroy () {
  }
  
  #endregion
}


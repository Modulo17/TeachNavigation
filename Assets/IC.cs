//Code (C) 2017 Richard Leinfellner
//Permission given to use for educational use
using UnityEngine;
using System.Collections;


//Adds a static GameController Input module to the Scene
//Can be attached to a GameObject or will add itself if not

namespace RL_Helpers {
    public class IC : Singleton {

        public readonly static string[,] sJoySticks = new string[,] {
        {"Joy 1 X","Joy 1 Y"}
        ,{"Joy 2 X","Joy 2 Y"}
        ,{"L Trigger", "R Trigger"}
    };

        public readonly static string[] sButtons = new string[] {
        "A Button"
        ,"B Button"
        ,"Y Button"
        ,"X Button"
    };

        string sDebugText;

        public static string DebugText {
            get {
                if (sIC != null) {
                    return sIC.sDebugText;
                }
                return "Not initialised";
            }
        }

        public enum Directions {
            None = 0
            , MoveX
            , MoveY
            , ShiftMoveX
            , ShiftMoveY
            , RotateX
            , RotateY
            , Zoom
            , Fire
            , Jump
            , Thrust
            , Brake
            , Count
        }

        private Vector3 mLastPosition;      //Use this to work out changes in mouse position

        private float[] mInputs;        //Array of inputs

        //static reference to Game Controller
        static IC sIC;

        public static uint InputCount {       //Get number of inputs
            get {
                return (uint)Directions.Count;
            }
        }

        //Create Singleton
        void Awake() {
            if (CreateSingleton(ref sIC)) {
                mInputs = new float[InputCount];        //Make Array for all the inputs I am using
            }
        }

        static public float GetInput(Directions vFlag) {       //Read Specific normalised value
            if (sIC == null || sIC.gameObject == null) {            //If this has not been added to the scene, add it
                GameObject tGO = new GameObject("InputController");
                tGO.AddComponent<IC>();     //Add this script to the GameObject
                Debug.Log("Auto added InputController on first use");
            }

            uint tIndex = (uint)vFlag;
            if (tIndex < InputCount) {
                return sIC.mInputs[tIndex];           //Get last read value
            }
            return 0f;      //Default if invalid index
        }

        private void SetInput(Directions vFlag, float vValue) {       //Write Specific normalised value
            uint tIndex = (uint)vFlag;
            if (tIndex < InputCount) {
                mInputs[tIndex] = vValue;           //Set Value
            }
        }

        private void UpdateInput(Directions vFlag, float vValue) {        //Update Specific normalised value, if update is zero then damp it, to float to zero
            uint tIndex = (uint)vFlag;
            if (tIndex < InputCount) {
                if (Mathf.Abs(vValue) > Mathf.Epsilon) {
                    mInputs[tIndex] = Mathf.Clamp(mInputs[tIndex] + vValue, -1f, 1f);   //Set Value with clamp
                } else {
                    mInputs[tIndex] = FloatToZero(mInputs[tIndex]);
                }
            }
        }

        static float FloatToZero(float vValue) {        //Gradually damp a number to zero, regardless of sign
            float tAbsValue = System.Math.Abs(vValue);
            if (tAbsValue > Mathf.Epsilon) {
                return vValue * 0.9f;
            } else {
                return 0f;
            }
        }


        Vector2 ReadJoyStick(int vStick) {
            Vector2 tJoyStick = Vector2.zero;
            if (vStick < sJoySticks.GetLength(0)) {
                tJoyStick.x = Input.GetAxis(sJoySticks[vStick, 0]);
                tJoyStick.y = Input.GetAxis(sJoySticks[vStick, 1]);
            }
            return tJoyStick;
        }

        bool ReadButton(int vButton) {
            if (vButton < sButtons.Length) {
                return Input.GetButton(sButtons[vButton]);
            }
            return false;
        }


        void Update() {
            UpdateInput();
        }
        //Map input to game movements, can be accessed via a static method
        //Read using static public float GetInput(Directions vFlag)
        void UpdateInput() {        //Update Input Array which can be read by all clients

            Vector2 tLeftStick = ReadJoyStick(0);
            Vector2 tRightStick = ReadJoyStick(1);
            bool tUsingJoyStick = false;

            if (tRightStick.sqrMagnitude > Mathf.Epsilon) {
                SetInput(Directions.ShiftMoveX, tRightStick.x);
                SetInput(Directions.ShiftMoveY, tRightStick.y);
                tUsingJoyStick = true;
            }

            if (tLeftStick.sqrMagnitude > Mathf.Epsilon) {
                SetInput(Directions.MoveX, tLeftStick.x);
                SetInput(Directions.MoveY, tLeftStick.y);
                tUsingJoyStick = true;
            }

            Vector2 tThrust = ReadJoyStick(2);
            if (tThrust.sqrMagnitude > Mathf.Epsilon) {
                SetInput(Directions.Thrust, tThrust.y);
                SetInput(Directions.Brake, tThrust.x);
                tUsingJoyStick = true;
            }

            if (!tUsingJoyStick) {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                    if (Input.GetKey(KeyCode.UpArrow)) {        //Map control to game input
                        SetInput(Directions.ShiftMoveY, 1.0f);
                    } else if (Input.GetKey(KeyCode.DownArrow)) {
                        SetInput(Directions.ShiftMoveY, -1.0f);
                    } else {
                        SetInput(Directions.ShiftMoveY, 0f);
                    }


                    if (Input.GetKey(KeyCode.LeftArrow)) {
                        SetInput(Directions.ShiftMoveX, -1.0f);
                    } else if (Input.GetKey(KeyCode.RightArrow)) {
                        SetInput(Directions.ShiftMoveX, 1.0f);
                    } else {
                        SetInput(Directions.ShiftMoveX, 0f);
                    }
                } else {
                    if (Input.GetKey(KeyCode.UpArrow)) {        //Map control to game input
                        SetInput(Directions.MoveY, 1.0f);
                    } else if (Input.GetKey(KeyCode.DownArrow)) {
                        SetInput(Directions.MoveY, -1.0f);
                    } else {
                        SetInput(Directions.MoveY, 0f);
                    }

                    if (Input.GetKey(KeyCode.LeftArrow)) {
                        SetInput(Directions.MoveX, -1.0f);
                    } else if (Input.GetKey(KeyCode.RightArrow)) {
                        SetInput(Directions.MoveX, 1.0f);
                    } else {
                        SetInput(Directions.MoveX, 0f);
                    }

                    if (Input.GetKey(KeyCode.A)) {        //Map control to game input
                        SetInput(Directions.Thrust, 1.0f);
                    } else if (Input.GetKey(KeyCode.Z)) {
                        SetInput(Directions.Thrust, -1.0f);
                    } else {
                        SetInput(Directions.Thrust, 0f);
                    }
                }
            }


            sDebugText = "";
            for (int tI = 0; tI < sButtons.Length; tI++) {
                sDebugText += string.Format("{0} {1}={2}\n", tI, sButtons[tI], ReadButton(tI));
            }
            for (int tI = 0; tI < sJoySticks.GetLength(0); tI++) {
                sDebugText += string.Format("Stick {0}={1:f2}\n", tI, ReadJoyStick(tI));
            }

            if (Input.GetKey(KeyCode.Period)) {
                SetInput(Directions.Zoom, -1.0f);
            } else if (Input.GetKey(KeyCode.Comma)) {
                SetInput(Directions.Zoom, 1.0f);
            } else {
                SetInput(Directions.Zoom, 0f);
            }

            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0) || ReadButton(1)) {
                SetInput(Directions.Fire, 1.0f);
            } else {
                SetInput(Directions.Fire, 0f);
            }

            if (Input.GetKey(KeyCode.Return) || ReadButton(0)) {
                SetInput(Directions.Jump, 1.0f);
            } else {
                SetInput(Directions.Jump, 0f);
            }
            Vector3 tCurrentPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);      //Map Mouse to rotation
            if (tCurrentPosition.x >= 0 && tCurrentPosition.x <= 1f && tCurrentPosition.y >= 0f && tCurrentPosition.y < 1f) {       //Only Tumble camera if mouse in viewport
                Vector3 tMouseDelta = tCurrentPosition - mLastPosition;
                UpdateInput(Directions.RotateX, tMouseDelta.x);
                UpdateInput(Directions.RotateY, tMouseDelta.y);
            } else {
                SetInput(Directions.RotateX, 0f);
                SetInput(Directions.RotateY, 0f);
            }
            mLastPosition = tCurrentPosition;       //If outside view still update old position so it does not snap on reentering view
        }
    }
}
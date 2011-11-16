using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{

    private enum ActiveCamera {  Title, PlayerGrounded, PlayerFly, ChallengeIntro, ChallengeWin, Gameover };
    private static ActiveCamera _cameraEnum;

    private bool cameraChanged = true;
    private GameObject _mainCamera;
    private bool cameraSmooth = false;
    private float cameraTimer = 0.0f;

    //store old and new values for camera properties for animating
    private float oldHeight;
    private float oldDistance;
    private float oldHDamping;
    private float oldRDamping;

    private float newHeight;
    private float newDistance;
    private float newHDamping;
    private float newRDamping;


    // Use this for initialization
    void Start()
    {
        _cameraEnum = ActiveCamera.PlayerGrounded;
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        //only change camera if camera changed
        if (cameraChanged)
        {
            
            
            //remove any camera components before switching anything on
            //store old values
            if (_mainCamera.GetComponent<PlayerCamera>())
            {
                oldHeight = _mainCamera.GetComponent<PlayerCamera>().height;
                oldDistance = _mainCamera.GetComponent<PlayerCamera>().distance;
                oldHDamping = _mainCamera.GetComponent<PlayerCamera>().heightDamping;
                oldRDamping = _mainCamera.GetComponent<PlayerCamera>().rotationDamping;
                Destroy(_mainCamera.GetComponent<PlayerCamera>());
            }
            if (_mainCamera.GetComponent<PlayerCameraFlying>())
            {
                oldHeight = _mainCamera.GetComponent<PlayerCameraFlying>().height;
                oldDistance = _mainCamera.GetComponent<PlayerCameraFlying>().distance;
                oldHDamping = _mainCamera.GetComponent<PlayerCameraFlying>().heightDamping;
                oldRDamping = _mainCamera.GetComponent<PlayerCameraFlying>().rotationDamping;
                Destroy(_mainCamera.GetComponent<PlayerCameraFlying>());
            }

            //switches camera component to new one
            switch (_cameraEnum)
            {
         
                case ActiveCamera.PlayerGrounded:
                    {
                        _mainCamera.AddComponent<PlayerCamera>();
                        newHeight = _mainCamera.GetComponent<PlayerCamera>().height;
                        newDistance = _mainCamera.GetComponent<PlayerCamera>().distance;
                        newHDamping = _mainCamera.GetComponent<PlayerCamera>().heightDamping;
                        newRDamping = _mainCamera.GetComponent<PlayerCamera>().rotationDamping;

                        cameraChanged = false;
                        break;
                    }
                case ActiveCamera.PlayerFly:
                    {
                        _mainCamera.AddComponent<PlayerCameraFlying>();

                        newHeight = _mainCamera.GetComponent<PlayerCameraFlying>().height;
                        newDistance = _mainCamera.GetComponent<PlayerCameraFlying>().distance;
                        newHDamping = _mainCamera.GetComponent<PlayerCameraFlying>().heightDamping;
                        newRDamping = _mainCamera.GetComponent<PlayerCameraFlying>().rotationDamping;

                        cameraChanged = false;
                        break;
                    }
            }
        }




        //--- creates smooth animation between cameras


        if (cameraSmooth)
        {
            //how long the animation takes (seconds)     * multiplier
            float timerDelta = (Time.time - cameraTimer) * 0.2f;

            //has time reached 1 - or close enough?
            if (timerDelta < 0.95f)
            {
                
                if (_cameraEnum == ActiveCamera.PlayerFly)
                {
                    _mainCamera.GetComponent<PlayerCameraFlying>().height = Mathf.Lerp(oldHeight, newHeight, timerDelta);
                    _mainCamera.GetComponent<PlayerCameraFlying>().distance = Mathf.Lerp(oldDistance, newDistance, timerDelta);
                    _mainCamera.GetComponent<PlayerCameraFlying>().rotationDamping = Mathf.Lerp(oldRDamping, newRDamping, timerDelta);
                    _mainCamera.GetComponent<PlayerCameraFlying>().heightDamping = Mathf.Lerp(oldHDamping, newHDamping, timerDelta);
                }
                else if (_cameraEnum == ActiveCamera.PlayerGrounded)
                {
                    _mainCamera.GetComponent<PlayerCamera>().height = Mathf.Lerp(oldHeight, newHeight, timerDelta);
                    _mainCamera.GetComponent<PlayerCamera>().distance = Mathf.Lerp(oldDistance, newDistance, timerDelta);
                    _mainCamera.GetComponent<PlayerCamera>().rotationDamping = Mathf.Lerp(oldRDamping, newRDamping, timerDelta);
                    _mainCamera.GetComponent<PlayerCamera>().heightDamping = Mathf.Lerp(oldHDamping, newHDamping, timerDelta);
                }
                
            }
            else
            {
                //done with smoothing
                cameraSmooth = false;
            }

           // Debug.Log(_mainCamera.GetComponent<PlayerCameraFlying>().height + "old value" + oldHeight + " new height:" + newHeight);

        }


    }


    IEnumerator SmoothCamera()
    {
        int counter = 1;
        
        Debug.Log("SmoothCameraRoutine");

        counter++;
        
        if (counter == 20)
             yield return 0;
        
    }


    public void CameraChanged(string cameraType)
    {


        switch (cameraType)
        {
            case "Title":
                _cameraEnum = ActiveCamera.Title;
                break;
            case "PlayerFly":
                _cameraEnum = ActiveCamera.PlayerFly;
                break;
            case "PlayerGrounded":
                _cameraEnum = ActiveCamera.PlayerGrounded;
                break;
        }

        //change to make sure the Update() changes the active camera
        cameraChanged = true;
        //allows for camera smoothing function to run
        cameraSmooth = true;
        cameraTimer = Time.time;
    }

}

using UnityEngine;
using System.Collections;

public class SmoothInputAxes : MonoBehaviour
{
    private float _v;
    private float _h;

    private float temp_v;
    private float temp_h;


    private float transitionTimeV;
    private float transitionEmptyTimeV;
    private float transitionTimeH;
    private float transitionEmptyTimeH;

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        _v = 0.0f;
        _h = 0.0f;
        
        //sets how long it takes for the transition to 1 or -1
        transitionTimeV = 3.0f;
        transitionEmptyTimeV = 3.0f;

        transitionTimeH = 0.2f;
        transitionEmptyTimeH = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        temp_v = Input.GetAxisRaw("Vertical");
        temp_h = Input.GetAxisRaw("Horizontal");

       
        
        //vertical movement and constraints
        if (temp_v > 0.1f)
        {
            if (_v <= 1.0f)
                _v += Time.deltaTime / transitionTimeV;
            else
                _v = 1.0f;
        }
        
        
        if (temp_v < 0.0f)
        {
            if (_v >= -1.0f)
                _v -= Time.deltaTime / transitionTimeV;
            else
                _v = -1.0f;
        }

        //horizontal movement and constraints
        if (temp_h > 0.1f)
        {
            if (_h <= 1.0f)
                _h += Time.deltaTime / transitionTimeH;
            else
                _h = 1.0f;
        }


        if (temp_h < 0.0f)
        {
            if (_h >= -1.0f)
                _h -= Time.deltaTime / transitionTimeH;
            else
                _h = -1.0f;
        }

        
        //resetting axis back to zero if nothing pressed
        if (temp_v <= 0.1 && temp_v > -0.1f)
        {
       
            if (_v > 0.1f)
                _v -= Time.deltaTime / transitionEmptyTimeV;

            if (_v < -0.1f)
                _v += Time.deltaTime / transitionEmptyTimeV;

            if (-0.1f < _v && _v < 0.1f)
                _v = 0.0f;
        }


        if (temp_h <= 0.1 && temp_h > -0.1f)
        {

            if (_h > 0.1f)
                _h -= Time.deltaTime / transitionEmptyTimeH;

            if (_h < -0.1f)
                _h += Time.deltaTime / transitionEmptyTimeH;

            if (-0.1f < _h && _h < 0.1f)
                _h = 0.0f;
        }

   
    }

    public float GetV()
    {
        return _v;
    }

    public float GetH()
    {
        return _h;
    }



}

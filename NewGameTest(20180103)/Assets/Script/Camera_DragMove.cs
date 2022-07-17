using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Camera_DragMove : MonoBehaviour {
    public float Clamp_X_a;
    public float Clamp_X_b;

    Vector2 initMousePos;
    Camera _camera;


	// Use this for initialization
	void Start () {
        _camera = Camera.main;
	}

    // Update is called once per frame

    Vector3 m_vt3CameraPos;
    void Update() {

        if (Input.GetMouseButtonDown(0))//버튼 누름과 동시에
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                m_vt3CameraPos = _camera.transform.position;
                initMousePos = Input.mousePosition;
                //initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);
            }
            //Debug.Log("mouse Down : " + initMousePos);
        }

        if (Input.GetMouseButton(0))//버튼을 누르고 있는동안
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {

                Vector2 worldPoint = Input.mousePosition;
                //worldPoint = Camera.main.ScreenToWorldPoint(worldPoint - initMousePos);
                
                Vector2 diffPos = (worldPoint - initMousePos) * - ( (Camera.main.orthographicSize * 2.0f) / (float)UnityEngine.Screen.width);
                diffPos.y = 0;

                //if (Input.GetAxis("Mouse X") != 0)
                //{
                  //  initMousePos = Input.mousePosition;
                 //   initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);
                //}
                _camera.transform.position =
                    new Vector3(Mathf.Clamp(m_vt3CameraPos.x + diffPos.x, Clamp_X_a, Clamp_X_b),
                               m_vt3CameraPos.y,
                               m_vt3CameraPos.z);

               
                //Debug.Log("mouse drag" + diffPos);
            }
        }
        /*
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 initMousePos;
        }
        */
    }
}

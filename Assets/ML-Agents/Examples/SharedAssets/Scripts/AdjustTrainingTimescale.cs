//This script lets you change time scale during training. It is not a required script for this demo to function

using UnityEngine;

namespace MLAgentsExamples
{
    public class AdjustTrainingTimescale : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("1");
                Time.timeScale = 1f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("2");
                Time.timeScale = 2f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("3");
                Time.timeScale = 3f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log("4");
                Time.timeScale = 4f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Debug.Log("5");
                Time.timeScale = 5f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Time.timeScale = 6f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                Time.timeScale = 7f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                Time.timeScale = 8f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                Time.timeScale = 9f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Time.timeScale *= 2f;
            }
        }
    }
}

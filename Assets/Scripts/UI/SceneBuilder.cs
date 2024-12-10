using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBuilder : MonoBehaviour
{
    public void LoadStage(int index)
    {
        SceneManager.LoadScene(index);
    }
}

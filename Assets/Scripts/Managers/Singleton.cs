using UnityEngine;

public class Singleton : MonoBehaviour
{
    static Singleton instance;

    private void Start()
    {
        if (instance != null)
            GameObject.Destroy(this.gameObject);
        else
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
}

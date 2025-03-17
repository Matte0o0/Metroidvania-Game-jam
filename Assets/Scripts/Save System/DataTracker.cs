using UnityEngine;

//this script is used only to track the type of data which is not clearly assignable to an object, like time.
public class DataTracker : MonoBehaviour
{
    private void Update()
    {
        DataManager.Instance.UpdateTimePassed(Time.deltaTime);
    }
}

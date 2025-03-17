using UnityEngine;


//voglio window debug. per ora non va non m'interessa.
public class DebugWindow : MonoBehaviour
{
    public EnemyController enemyStateMachine; // Reference to the enemy state machine

    private bool showDebug = true; // Flag to toggle the debug window visibility

    private void Update()
    {
        // Toggle the debug window on/off with F1 key
        if (Input.GetKeyDown(KeyCode.F1))
        {
            showDebug = !showDebug;
        }
    }

    private void OnGUI()
    {
        // Only show debug UI if it's enabled
        if (!showDebug) return;

        if (enemyStateMachine == null)
        {
            GUILayout.Label("No Enemy State Machine Assigned");
            return;
        }

        // Get the current state of the enemy state machine
        string currentStateName = enemyStateMachine != null && enemyStateMachine.currentState != null
            ? enemyStateMachine.currentState.GetType().Name
            : "No State";

        // Create a box with some styling
        GUI.Box(new Rect(10, 10, 300, 150), "Enemy State Machine Debug");

        // Show the current state of the enemy state machine
        GUI.Label(new Rect(20, 40, 280, 20), "Current State: " + currentStateName);


    }
}

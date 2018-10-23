using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private Dictionary<GlowObject, bool> targetStates = new Dictionary<GlowObject, bool>();

    private bool isLevelComplete = false;

	// Use this for initialization
	void Start () {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in targets)
        {
            GlowObject glow = target.GetComponent<GlowObject>();
            targetStates.Add(glow, glow.isFound);
            if (glow.isFound) Debug.LogWarning("Target object " + target.name + " is starting the game marked as already found. That's weird.");
        }
	}
	
	// Update is called once per frame
	void Update () {

        bool searched = false;
        bool allFound = true;

        List<GlowObject> foundInPassing = new List<GlowObject>();
        foreach (GlowObject glow in targetStates.Keys)
        {
            if (!targetStates[glow] && glow.isFound)
            {
                // First time found
                foundInPassing.Add(glow);

                Dictionary<string, object> analytics = new Dictionary<string, object>();
                analytics.Add("level_id", SceneManager.GetActiveScene().name);
                analytics.Add("object_id", gameObject.name);
                analytics.Add("found_time", (int)Time.timeSinceLevelLoad);

                AnalyticsEvent.Custom("obj_found", analytics);
            }

            searched = true;
            allFound &= glow.isFound;
        }
        foreach (GlowObject glow in foundInPassing) targetStates[glow] = true;  // Really?

        if (!isLevelComplete && searched && allFound)
        {
            isLevelComplete = true;
            AnalyticsEvent.LevelComplete(SceneManager.GetActiveScene().name, new Dictionary<string, object>() { { "time", (int)Time.timeSinceLevelLoad } });
        }

	}

    private void OnApplicationQuit()
    {
        if (!isLevelComplete)
        {
            AnalyticsEvent.LevelQuit(SceneManager.GetActiveScene().name, new Dictionary<string, object>() { { "time", (int)Time.timeSinceLevelLoad } });
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsDemo : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Dictionary<string, object> additionalData = new Dictionary<string, object>();
        additionalData.Add("sample_number", 15);

        AnalyticsEvent.GameStart(additionalData);


        AnalyticsEvent.Custom("custom_demo_event", additionalData);

        AnalyticsEvent.Custom("obj_found_level1_1", new Dictionary<string, object> { { "time", (int)Time.timeSinceLevelLoad } });

        // Alternative
        Dictionary<string, object> findData = new Dictionary<string, object>();
        findData.Add("level", "myLevelName");
        findData.Add("object_id", 1);
        findData.Add("time", (int)Time.timeSinceLevelLoad);
        AnalyticsEvent.Custom("obj_found", findData);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

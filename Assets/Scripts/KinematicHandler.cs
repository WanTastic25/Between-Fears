using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KinematicHandler : MonoBehaviour
{
    public HandIK kinematicToControl;
    public GameObject flashLight;
    public GameObject lightPoint;
    public Light lightSource;

    public float maxBattery;
    public float currentBattery;
    public Image batteryFill;

    public bool holsterToggle;
    public bool lightToggle;

    private void Awake()
    {
        lightSource = lightPoint.GetComponent<Light>();
        lightSource.enabled = false;  // Explicitly turn off the light
        lightToggle = false;

        currentBattery = maxBattery;

        flashLight.SetActive(false);
        kinematicToControl = GetComponent<HandIK>();
        kinematicToControl.enabled = false;
        holsterToggle = false;  // Flashlight starts holstered
    }

    void Update()
    {
        batteryFill.fillAmount = currentBattery / maxBattery;

        // Toggle flashlight holster with 1
        if (Input.GetKeyDown(KeyCode.Alpha1) && holsterToggle == false)
        {
            flashLight.SetActive(true);  // flashlight holster yes
            kinematicToControl.enabled = true;  // HandIK enable, causing arm to rise
            holsterToggle = true;  // Toggle flashlight state
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && holsterToggle == true)
        {
            flashLight.SetActive(false);  // flashlight holster no
            kinematicToControl.enabled = false;  // HandIK disable, causing arm to drop
            holsterToggle = false;  // Toggle flashlight state
        }

        // Toggle light source on/off when Mouse0 is pressed and flashlight is out
        if (Input.GetKeyDown(KeyCode.Mouse0) && holsterToggle == true && lightToggle == false && currentBattery > 0f)
        {
            Debug.Log("Flashlight On.");

            var flashSound = FindObjectOfType<audioManager>().sounds.FirstOrDefault(s => s.name == "flashlight");
            if (flashSound != null && !flashSound.source.isPlaying)
            {
                FindObjectOfType<audioManager>().Play("flashlight");
            }

            lightToggle = true;
            lightSource.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && holsterToggle == true && lightToggle == true && currentBattery > 0f)
        {
            Debug.Log("Flashlight Off.");

            var flashSound = FindObjectOfType<audioManager>().sounds.FirstOrDefault(s => s.name == "flashlight");
            if (flashSound != null && !flashSound.source.isPlaying)
            {
                FindObjectOfType<audioManager>().Play("flashlight");
            }

            lightToggle = false;
            lightSource.enabled = false;
        }

        if (currentBattery <= 0f && lightToggle)
        {
            lightToggle = false;
            lightSource.enabled = false;
            Debug.Log("Battery is depleted. Light turned off.");
        }

        if (lightToggle == true)
        {
            currentBattery = Mathf.Max(currentBattery - (2f * Time.deltaTime), 0f);
        }
    }

    public void gainBattery()
    {
        currentBattery = Mathf.Min(currentBattery + 5f, maxBattery); // Clamp battery to max value
        Debug.Log($"Battery gained. Current battery: {currentBattery}");
    }
}

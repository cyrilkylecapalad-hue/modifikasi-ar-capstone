using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ITEM_MANAGER : MonoBehaviour
{
    // Scene loading functions
    public void OpenTWPOSWING() => LoadScene("TWPO_BLACK_SWINGARM");
    public void OpenTWPOSLVSWING() => LoadScene("TWPO_SLV_SWINGARM");
    public void OpenADLCALBL() => LoadScene("ADLBL");
    public void OpenADLCALRD() => LoadScene("ADLRD");
    public void OpenADLCALGLD() => LoadScene("ADLGLD");
    public void OpenADLCALSLV() => LoadScene("ADLSLV");
    public void OpenOHL1() => LoadScene("OHL1");
    public void OpenOHL2() => LoadScene("OHL2");
    public void OpenKAMUIIPINK() => LoadScene("KAMUII_PINK_SWINGARM");
    public void OpenKAMUIIGRN() => LoadScene("KAMUII_GRN_SWINGARM");
    public void OpenKAMUIIblue1() => LoadScene("KAMUII_blue1_SWINGARM");

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);  // Normal load
        Resources.UnloadUnusedAssets();      // Optional cleanup
        System.GC.Collect();                 // Optional garbage collection
    }

    // Optional: automatic button binding for this scene
    void Start()
    {
        BindButtons();
    }

    void BindButtons()
    {
        Button TWPOBTN = GameObject.Find("TWPOSTART")?.GetComponent<Button>();
        Button TWPO2BTN = GameObject.Find("TWPO2TART")?.GetComponent<Button>();
        Button ADLBLBTN = GameObject.Find("ADLSTART")?.GetComponent<Button>();
        Button ADLRDBTN = GameObject.Find("ADLRDSTART")?.GetComponent<Button>();
        Button ADLGLDBTN = GameObject.Find("ADLGLDSTART")?.GetComponent<Button>();
        Button ADLSLVBTN = GameObject.Find("ADLSLVSTART")?.GetComponent<Button>();
        Button OHL1BTN = GameObject.Find("OHLINS1ST")?.GetComponent<Button>();
        Button OHL2BTN = GameObject.Find("OHLINS2ST")?.GetComponent<Button>();
        Button KMUBTN = GameObject.Find("KMUPNK")?.GetComponent<Button>();
        Button KMU3BTN = GameObject.Find("KMUGRN")?.GetComponent<Button>();
        Button KMU2BTN = GameObject.Find("KMUBLU")?.GetComponent<Button>();


        if (TWPOBTN) TWPOBTN.onClick.AddListener(OpenTWPOSWING);
        if (TWPO2BTN) TWPO2BTN.onClick.AddListener(OpenTWPOSLVSWING);
        if (ADLBLBTN) ADLBLBTN.onClick.AddListener(OpenADLCALBL);
        if (ADLRDBTN) ADLRDBTN.onClick.AddListener(OpenADLCALRD);
        if (ADLGLDBTN) ADLGLDBTN.onClick.AddListener(OpenADLCALGLD);
        if (ADLSLVBTN) ADLSLVBTN.onClick.AddListener(OpenADLCALSLV);
        if (OHL1BTN) OHL1BTN.onClick.AddListener(OpenOHL1);
        if (OHL2BTN) OHL2BTN.onClick.AddListener(OpenOHL2);
        if (KMUBTN) KMUBTN.onClick.AddListener(OpenKAMUIIPINK);
        if (KMU3BTN) KMU3BTN.onClick.AddListener(OpenKAMUIIGRN);
        if (KMU2BTN) KMU2BTN.onClick.AddListener(OpenKAMUIIblue1);

    }
}

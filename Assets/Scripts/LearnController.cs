using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LearnController : MonoBehaviour {

    public Cursor cursor;
    private GestureRecognizer Gr;
    private GUIText GuiText;
    // Use this for initialization
    void Start ()
    {
        Gr = GestureRecognizer.Instance;
        GuiText = GameObject.Find("GuiText").GetComponent<GUIText>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            cursor.ClearLog();
            cursor.StartWriteLog();
        }

        if (Input.GetMouseButtonUp(1))
        {
            cursor.StopWriteLog();
            Gr.recordTemplate(cursor.GetLog());
        }
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("Main");
    }

    void OnGUI()
    {
        if (Gr.recordDone == 1)
        {
            GUI.Window(0, new Rect(350, 220, 300, 100), DoMyWindow, "Save the template?");
        }
    }

    void DoMyWindow(int windowID)
    {
        Gr.stringToEdit = GUILayout.TextField(Gr.stringToEdit);

        if (GUI.Button(new Rect(100, 50, 50, 20), "Save"))
        {
            ArrayList temp = new ArrayList();
            ArrayList a = (ArrayList)GestureTemplates.Instance.Templates[GestureTemplates.Instance.Templates.Count - 1];

            for (int i = 0; i < Gr.newTemplateArr.Count; i++)
                temp.Add(Gr.newTemplateArr[i]);

            GestureTemplates.Instance.Templates.Add(temp);
            GestureTemplates.Instance.TemplateNames.Add(Gr.stringToEdit);
            GestureTemplates.Instance.TemplateAngles.Add(Gr.newTemplateAngle);
            Gr.recordDone = 0;
            Gr.newTemplateArr.Clear();

            GuiText.GetComponent<GUIText>().text = "TEMPLATE: " + Gr.stringToEdit + "\n STATUS: SAVED";
        }

        if (GUI.Button(new Rect(160, 50, 50, 20), "Cancel"))
        {
            Gr.recordDone = 0;
            GuiText.GetComponent<GUIText>().text = "";
        }
    }
}

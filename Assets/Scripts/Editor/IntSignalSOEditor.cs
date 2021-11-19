using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IntSignalSO))]
class IntSignalSOEditor : Editor {

    string val;
    bool raiseSignalButton;

    override public void OnInspectorGUI() {
        base.OnInspectorGUI();

        IntSignalSO signal = target as IntSignalSO;

        GUILayout.Space(20);

        GUILayout.Label("Enter the int to raise the Signal with");
        val = GUILayout.TextField(val);

        GUILayout.Space(10);

        raiseSignalButton = GUILayout.Button("Raise Signal");
        
        if (raiseSignalButton)
        {
            signal.RaiseSignal(int.Parse(val));
        }
    }
}
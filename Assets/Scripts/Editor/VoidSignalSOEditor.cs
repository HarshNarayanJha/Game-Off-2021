using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VoidSignalSO))]
class VoidSignalSOEditor : Editor {
    override public void OnInspectorGUI() {
        base.OnInspectorGUI();

        VoidSignalSO signal = target as VoidSignalSO;

        GUILayout.Space(20);

        bool raiseSignalButton = GUILayout.Button("Raise Signal");
        
        if (raiseSignalButton)
        {
            signal.RaiseSignal();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypedText : MonoBehaviour
{
    [SerializeField] private StringValueSO textRef;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private bool typeAtStart;
    [SerializeField] private float typeSpeed;

    [Header("Signals Broadcasting On")]
    [SerializeField] private VoidSignalSO introTypingDone;

    private char[] textCharArray;
    bool skip;

    private void Start()
    {
        textCharArray = textRef.Value.ToCharArray();
        skip = false;

        if (typeAtStart)
            StartCoroutine(TypeText());
    }

    public void SkipTypedText()
    {
        skip = true;
    }

    private IEnumerator TypeText()
    {
        text.SetText("");
        for (int i = 0; i < textCharArray.Length; i++)
        {
            if (skip)
                break;

            text.text += textCharArray[i];
            if (!textCharArray[i].Equals(""))
                yield return new WaitForSeconds(1/typeSpeed);
        }
        text.text = textRef.Value;
        introTypingDone.RaiseSignal();
    }
}

using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

using SolidUtilities;

using GameFramework.System;

public class StatesWindow : EditorWindow
{
    static readonly List<ValueTuple<string, string>> States = new();
    bool wasChanged = false;

    ScrollView scrollView;
    TextField newStateText;

    [MenuItem("Tools/GameFramework/States")]
    static void Init()
    {
        var window = CreateInstance<StatesWindow>();
        window.titleContent = new("States");
        window.Show();
    }

    private void OnEnable()
    {
        wasChanged = false;

        string[] StatesInFile = File.ReadAllLines("Assets/Resources/States.txt");
        States.Clear();

        for (int i = 0; i < StatesInFile.Length; i++)
        {
            string[] StateAndDescription = StatesInFile[i].Split(',');
            States.Add(new(StateAndDescription[0], StateAndDescription.Length > 1 ? StateAndDescription[1] : ""));
        }
    }

    private void OnDisable()
    {
        if (wasChanged) SaveStates();
    }

    private void OnLostFocus()
    {
        if (wasChanged) SaveStates();
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualElement newStateVis = new();
        newStateVis.style.flexDirection = FlexDirection.Row;
        newStateVis.style.alignItems = Align.Center;
        newStateVis.style.flexBasis = 20f;
        newStateVis.style.minHeight = 20f;
        root.Add(newStateVis);

        Label newStateTextLabel = new("State name:");
        newStateVis.Add(newStateTextLabel);

        newStateText = new();
        newStateText.style.flexGrow = 1;
        newStateVis.Add(newStateText);

        VisualElement newStateDescriptionVis = new();
        newStateDescriptionVis.style.flexDirection = FlexDirection.Row;
        newStateDescriptionVis.style.alignItems = Align.Center;
        newStateDescriptionVis.style.flexBasis = 20f;
        newStateDescriptionVis.style.minHeight = 20f;
        root.Add(newStateDescriptionVis);

        Label newStateDescriptionLabel = new("State description:");
        newStateDescriptionVis.Add(newStateDescriptionLabel);

        TextField newStateDescription = new();
        newStateDescription.style.flexGrow = 1;
        newStateDescriptionVis.Add(newStateDescription);

        Button button = new();
        button.text = "Add new State";
        button.clicked += () => AddState(newStateText.text, newStateDescription.text);
        button.style.flexBasis = 17f;
        button.style.minHeight = 17f;
        root.Add(button);

        Button discardButton = new();
        discardButton.text = "Discard changes";
        discardButton.clicked += () => { OnEnable(); FillScrollViev(); };
        discardButton.style.flexBasis = 17f;
        discardButton.style.minHeight = 17f;
        root.Add(discardButton);

        scrollView = new ScrollView(ScrollViewMode.Vertical);
        root.Add(scrollView);

        FillScrollViev();
    }

    private void FillScrollViev()
    {
        scrollView.Clear();

        foreach (var State in States)
        {
            VisualElement vis = new();
            vis.style.flexDirection = FlexDirection.Row;
            vis.style.alignItems = Align.Center;
            vis.style.justifyContent = Justify.SpaceBetween;
            vis.style.borderBottomWidth = 0.1f;
            vis.style.borderBottomColor = Color.gray;
            scrollView.Add(vis);

            int lastIndexOf = State.Item1.LastIndexOf('.');
            int charCount = State.Item1.CountChars('.');

            //string labelText = "";
            //for(int i = 0; i < charCount; i++)
            //{
            //    labelText += " \u2011\u2011";
            //}
            string labelText = lastIndexOf != -1 ? State.Item1.Substring(lastIndexOf + 1, State.Item1.Length - 1 - lastIndexOf) : State.Item1;

            Label label = new(labelText);
            label.style.left = charCount * 17f;
            label.tooltip = State.Item2;
            vis.Add(label);

            VisualElement btns = new();
            btns.style.flexDirection = FlexDirection.Row;
            vis.Add(btns);

            Button aBtn = new();
            aBtn.text = "+";
            aBtn.style.flexBasis = StyleKeyword.Auto;
            aBtn.style.height = 12;
            aBtn.clicked += () => { newStateText.value = State.Item1 + "."; newStateText.Focus(); };
            btns.Add(aBtn);

            Button rBtn = new();
            rBtn.text = "-";
            rBtn.style.flexBasis = StyleKeyword.Auto;
            rBtn.style.height = 12;
            rBtn.clicked += () => { RemoveState(State.Item1); };
            btns.Add(rBtn);

            vis.Add(btns);
        }
    }

    private void AddState(string StateName, string StateDescription)
    {
        if(StateName.Contains(" "))
        {
            EditorUtility.DisplayDialog("States", "Wrong format", "Ok");
            return;
        }

        if(StateName.LastIndexOf(".") == StateName.Length - 1)
        {
            EditorUtility.DisplayDialog("States", "State is empty", "Ok");
            return;
        }

        int insertIndex = -1;

        for(int i = 0; i < States.Count; i++)
        {
            if (StateName == States[i].Item1)
            {
                EditorUtility.DisplayDialog("States", "State already exists. State description updated", "Ok");

                if (StateDescription != "")
                {
                    States[i] = (States[i].Item1, StateDescription);
                    SaveStates();
                }
                return;
            }
        }

        string parentStateName = StateName.LastIndexOf('.') != -1 ? StateName[..StateName.LastIndexOf('.')] : StateName;
        int entryCharCount = parentStateName.CountChars('.');

        for (int i = 0; i < States.Count; i++)
        {
            int charCount = States[i].Item1.CountChars('.');

            if(charCount == entryCharCount)
            {
                if(States[i].Item1 == parentStateName)
                {
                    insertIndex = i;
                    break;
                }
            }
        }

        if (insertIndex == -1)
        {
            string[] subStateNames = StateName.Split('.');

            for (int i = 1; i < subStateNames.Length - 1; i++)
            {
                subStateNames[i] = subStateNames[i - 1] + "." + subStateNames[i];
            }

            for (int i = 0; i < subStateNames.Length - 1; i++)
            {
                States.Add(new(subStateNames[i], ""));
            }

            States.Add(new(StateName, StateDescription));
        }
        else
        {
            States.Insert(insertIndex + 1, new(StateName, StateDescription));
        }

        wasChanged = true;
        FillScrollViev();
    }

    private void RemoveState(string StateName)
    {
        int removeIndex = -1;

        for (int i = 0; i < States.Count; i++)
        {
            if (StateName == States[i].Item1)
            {
                removeIndex = i;
                break;
            }
        }

        if( removeIndex == -1) { return; }

        List<string> StatesToRemove = new()
        {
            StateName
        };

        string[] entrySubStateNames = StateName.Split('.');

        for (int i = removeIndex + 1; i < States.Count; i++)
        {
            string[] subStateNames = States[i].Item1.Split('.');

            if (entrySubStateNames.Length > subStateNames.Length || entrySubStateNames.Length == subStateNames.Length) break;

            bool isSubStateOf = true;

            for (int j = 0; j < entrySubStateNames.Length; j++)
            {
                if (entrySubStateNames[j] == subStateNames[j])
                {
                    continue;
                }

                isSubStateOf = false;
                break;
            }

            if (!isSubStateOf) break;
            
            StatesToRemove.Add(States[i].Item1);
        }

        for(int i = 0; i < StatesToRemove.Count; i++)
        {
            for(int j = 0; j < States.Count; j++)
            {
                if (States[j].Item1 == StatesToRemove[i])
                {
                    States.RemoveAt(j);
                    break;
                }
            }
        }

        wasChanged = true;
        FillScrollViev();
    }

    private void SaveStates()
    {
        States.Sort();
        string[] StatesToWrite = new string[States.Count];

        for (int i = 0; i < States.Count; i++)
        {
            StatesToWrite[i] = States[i].Item1 + "," + States[i].Item2;
        }

        File.WriteAllLines("Assets/Resources/States.txt", StatesToWrite);
        AssetDatabase.Refresh();
    }
}

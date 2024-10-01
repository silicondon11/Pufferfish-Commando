using UnityEngine;

[CreateAssetMenu(fileName = "NewTutorialObject", menuName = "TutorialObject", order = 1)]
public class TutorialObject : ScriptableObject
{
    public int page;

    [TextArea(3, 10)]
    public string description;

}


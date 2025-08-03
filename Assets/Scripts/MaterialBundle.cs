using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class MaterialBundle : ScriptableObject
{
    public Material redMat;
    public Material greenMat;
    public Material blueMat;
    public Material purpleMat;
    public Material yellowMat;

    public Material FromBoxColor(BoxColor boxColor)
    {
        switch (boxColor)
        {
            case BoxColor.Red:
                return redMat;
            case BoxColor.Green:
                return greenMat;
            case BoxColor.Blue:
                return blueMat;
            case BoxColor.Purple:
                return purpleMat;
            default:
                return yellowMat;
        }
    }
}

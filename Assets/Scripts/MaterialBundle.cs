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
                break;
            case BoxColor.Green:
                return greenMat;
                break;
            case BoxColor.Blue:
                return blueMat;
                break;
            case BoxColor.Purple:
                return purpleMat;
                break;
            default:
                return yellowMat;
                break;
        }
    }
}

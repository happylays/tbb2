using UnityEngine;
public class UIClip
{
    public GameObject mainObject;
    public Transform mainTrans;

    public UIClip(GameObject mainObject)
    {
        this.mainObject = mainObject;
        mainTrans = this.mainObject.transform;
    }
        
    public void show() { }

    public void hide() { }

    virtual public void destroy() { }

    public GameObject getGameObject(string path)
    {
        return UITools.getGameObject(this.mainObject, path);
    }
}
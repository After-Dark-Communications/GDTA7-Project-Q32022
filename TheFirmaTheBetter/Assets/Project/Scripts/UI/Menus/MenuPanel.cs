using UnityEngine;
public class MenuPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject firstSelected;

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        //EventSystems.current.SetSelectedGameObject(firstSelected);
    }

    public void ClosePanel()
    {
        //EventSystems.current.SetSelectedGameObject(null);
        gameObject.SetActive(false);
    }
}

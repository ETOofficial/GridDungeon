using UnityEngine;
using UnityEngine.UI;

public class Close : MonoBehaviour
{
    private GameObject _currentPanel;

    private void Start()
    {
        _currentPanel = transform.parent.gameObject;
        
        var btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Destroy(_currentPanel);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dropdown : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {

    [SerializeField]
    private RectTransform container;

    private void Start()
    {
        ToggleDropDown(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToggleDropDown(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToggleDropDown(false);
    }

    private void ToggleDropDown(bool open)
    {                                                            //when open is true, open dropdown
        container.localScale = new Vector3(container.localScale.x, open? 1:0, container.localScale.y);
    }

    public void SetClickToAdd()
    {
        Manager.Instance.ChangeClickMode(Mode.Add);
        ToggleDropDown(false);
    }

    public void SetClickToRemove()
    {
        Manager.Instance.ChangeClickMode(Mode.Remove);
        ToggleDropDown(false);
    }

    public void SetClickToToggleFire()
    {
        Manager.Instance.ChangeClickMode(Mode.ToggleFire);
        ToggleDropDown(false);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UB
{
    public class MovePoint : MonoBehaviour
    {
        private void Awake()
        {

        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            GameManager.instance.activeCharacter.MoveToPoint(transform.position);

            MoveGrid.instance.HideMovePoints();

            PlayerActionMenu.instance.HideMenus();
        }
    }
}


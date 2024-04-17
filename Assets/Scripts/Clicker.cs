using UnityEngine;

public class Clicker : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out Dublicator dublicator))
                {
                    dublicator.OnClick();
                }
            }
        }
    }
}

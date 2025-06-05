using UnityEngine;

public class MapMoveToMarker : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private LayerMask _layerMask;

    private InputService _inputService;
    private Flag _flag;    

    public void Initialize(InputService inputService)
    {
        _inputService = inputService;
        _flag = Instantiate(_flagPrefab, Vector3.zero, Quaternion.identity);        
    }

    private void Update()
    {
        Vector3 flagPos = _inputService.GetClickedPointOnMap();

        if (flagPos != Vector3.zero)
            _flag.transform.position = flagPos;
    }
}
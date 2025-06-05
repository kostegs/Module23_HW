using UnityEngine;

public class MapMoveToMarker : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private ParticleSystem _clickEffectPrefab;
    
    private InputService _inputService;
    private Flag _flag;
    private ParticleSystem _clickEffect;

    public void Initialize(InputService inputService)
    {
        _inputService = inputService;
        _flag = Instantiate(_flagPrefab, Vector3.zero, Quaternion.identity);
        _flag.gameObject.SetActive(false);
        _clickEffect = Instantiate(_clickEffectPrefab, Vector3.zero, Quaternion.identity); 
    }

    private void Update()
    {
        if(_inputService.GetClickedPointOnMap(out Vector3 flagPos))
        {            
            _flag.transform.position = new Vector3(flagPos.x, 0, flagPos.z);
            _flag.gameObject.SetActive(true);
            _clickEffect.transform.position = _flag.transform.position + Vector3.up;
            _clickEffect.Play();
        }
            
    }
}
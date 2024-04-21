using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dublicator : MonoBehaviour
{
    [SerializeField] private float _dublicationChance;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _dublicantScaleRate;
    [SerializeField] private float _dublicantDublicationChanceRate;
    [SerializeField] private Dublicator _prefab;
    [SerializeField] private GameObject _explosionPrefab;

    private int _minDublicantAmount = 2;
    private int _maxDublicantAmount = 6;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        _explosionForce /= transform.localScale.x;
        _explosionRadius /= transform.localScale.x;
    }

    public void Init(Vector3 scale, float dublicationChance)
    {
        SetScale(scale);
        SetDublicationChance(dublicationChance);
    }

    public void OnClick()
    {
        if (Random.Range(0.0f, 1.0f) <= _dublicationChance)
        {
            Dublicate();
        }
        else
        {
            Explode();
        }

        
        Destroy(gameObject);
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void SetDublicationChance(float dublicationChance)
    {
        _dublicationChance = dublicationChance;
    }

    private void Dublicate()
    {
        int amount = Random.Range(_minDublicantAmount, _maxDublicantAmount + 1);

        for (int i = 0; i < amount; i++)
        {
            Dublicator dublicant = Instantiate(_prefab, transform.position, Quaternion.identity);
            dublicant.Init(transform.localScale * _dublicantScaleRate, _dublicationChance * _dublicantDublicationChanceRate);
        }
    }

    private void Explode()
    {
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.localScale = transform.localScale;
        Destroy(explosion, 1);

        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                hit.attachedRigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }
    }
}

using UnityEngine;

public class Dublicator : MonoBehaviour
{
    public float DublicationChance;

    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _dublicantScaleRate;
    [SerializeField] private float _dublicantDublicationChanceRate;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _explosionPrefab;

    private int _minDublicantAmount = 2;
    private int _maxDublicantAmount = 6;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.color = Random.ColorHSV();
    }

    public void OnClick()
    {
        if (Random.Range(0.0f, 1.0f) <= DublicationChance)
        {
            Dublicate();
        }

        Explode();
        Destroy(gameObject);
    }

    private void Dublicate()
    {
        int amount = Random.Range(_minDublicantAmount, _maxDublicantAmount + 1);

        for (int i = 0; i < amount; i++)
        {
            GameObject dublicant = Instantiate(_prefab, transform.position, Quaternion.identity);
            dublicant.transform.localScale = transform.localScale * _dublicantScaleRate;
            dublicant.GetComponent<Dublicator>().DublicationChance = DublicationChance * _dublicantDublicationChanceRate;
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

using System.Collections.Generic;
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
    private List<Dublicator> _dublicants;

    private void Awake()
    {
        _dublicants = new List<Dublicator>();
    }

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.color = Random.ColorHSV();
    }

    public void OnClick()
    {
        if (Random.Range(0.0f, 1.0f) <= _dublicationChance)
        {
            Dublicate();
        }

        Explode();
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
            dublicant.SetScale(transform.localScale * _dublicantScaleRate);
            dublicant.SetDublicationChance(_dublicationChance * _dublicantDublicationChanceRate);
            _dublicants.Add(dublicant);
        }
    }

    private void Explode()
    {
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.localScale = transform.localScale;
        Destroy(explosion, 1);

        foreach (Dublicator dublicant in _dublicants)
        {
            if (dublicant.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }
    }
}

using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] private float _existingTime;

    private void Start() => Invoke(nameof(Destroy), _existingTime);
    
    private void Destroy() => Destroy(gameObject);
}

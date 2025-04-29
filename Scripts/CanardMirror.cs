using UnityEngine;
using static F35Conversion.Logger;

namespace F35Conversion.Scripts;

public class CanardMirror : MonoBehaviour
{
    public Transform sourceCanard;
    private Quaternion _initialRotationOffset;
    private bool _initialized;

    private void Start()
    {
        if (sourceCanard == null)
        {
            LogWarn($"Source canard is null for {gameObject.name}");
            return;
        }

        // Calculate the initial rotation offset
        // This represents the difference between the fake canard's rotation and the source canard's rotation
        _initialRotationOffset = transform.localRotation * Quaternion.Inverse(sourceCanard.localRotation);
        _initialized = true;

        Log($"Initialized canard mirror for {gameObject.name} with rotation offset");
    }

    private void Update()
    {
        if (sourceCanard == null)
        {
            LogWarn($"Source canard is null for {gameObject.name}");
            return;
        }

        if (!_initialized) Start();

        // Extract the Euler angles from the source canard's rotation
        var sourceEuler = sourceCanard.localRotation.eulerAngles;

        // Create new rotation with inverted pitch but same yaw and roll
        var invertedRotation = Quaternion.Euler(-sourceEuler.x, sourceEuler.y, sourceEuler.z);

        // Apply with the initial offset
        transform.localRotation = _initialRotationOffset * invertedRotation;
    }
}
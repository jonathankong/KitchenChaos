using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AdjustColliderToChildrenVisuals : MonoBehaviour
{
    private readonly int roundToDecimalPlaces = 1;
    void Start()
    {
        Collider collider = GetComponent<Collider>();
        Bounds bounds = CalculateCombinedBounds();

        if (bounds.size == Vector3.zero)
        {
            Debug.LogWarning("No renderers found in children. Collider will not be adjusted.");
            return;
        }

        // Round the bounds center and size
        bounds.center = RoundVector3(bounds.center);
        bounds.size = RoundVector3(bounds.size);

        if (collider is BoxCollider boxCollider)
        {
            AdjustBoxCollider(boxCollider, bounds);
        }
        else if (collider is SphereCollider sphereCollider)
        {
            AdjustSphereCollider(sphereCollider, bounds);
        }
        else if (collider is CapsuleCollider capsuleCollider)
        {
            AdjustCapsuleCollider(capsuleCollider, bounds);
        }
        else
        {
            Debug.LogWarning("Unsupported collider type.");
        }
    }

    private Bounds CalculateCombinedBounds()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            return new Bounds(Vector3.zero, Vector3.zero);
        }

        Bounds combinedBounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }

        return combinedBounds;
    }

    private void AdjustBoxCollider(BoxCollider boxCollider, Bounds bounds)
    {
        boxCollider.size = bounds.size;
        boxCollider.center = transform.InverseTransformPoint(bounds.center);
    }

    private void AdjustSphereCollider(SphereCollider sphereCollider, Bounds bounds)
    {
        sphereCollider.center = transform.InverseTransformPoint(bounds.center);
        sphereCollider.radius = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
    }

    private void AdjustCapsuleCollider(CapsuleCollider capsuleCollider, Bounds bounds)
    {
        capsuleCollider.center = transform.InverseTransformPoint(bounds.center);

        // Choose the largest axis as the height and the other two as diameter
        Vector3 size = bounds.size;
        if (size.y >= size.x && size.y >= size.z)
        {
            capsuleCollider.direction = 1; // Y-Axis
            capsuleCollider.height = size.y;
            capsuleCollider.radius = Mathf.Max(size.x, size.z) / 2f;
        }
        else if (size.x >= size.y && size.x >= size.z)
        {
            capsuleCollider.direction = 0; // X-Axis
            capsuleCollider.height = size.x;
            capsuleCollider.radius = Mathf.Max(size.y, size.z) / 2f;
        }
        else
        {
            capsuleCollider.direction = 2; // Z-Axis
            capsuleCollider.height = size.z;
            capsuleCollider.radius = Mathf.Max(size.x, size.y) / 2f;
        }
    }

    // Function to round a Vector3 to the specified number of decimal places
    private Vector3 RoundVector3(Vector3 vector)
    {
        return new Vector3(
            Mathf.Round(vector.x * Mathf.Pow(10, roundToDecimalPlaces)) / Mathf.Pow(10, roundToDecimalPlaces),
            Mathf.Round(vector.y * Mathf.Pow(10, roundToDecimalPlaces)) / Mathf.Pow(10, roundToDecimalPlaces),
            Mathf.Round(vector.z * Mathf.Pow(10, roundToDecimalPlaces)) / Mathf.Pow(10, roundToDecimalPlaces)
        );
    }
}


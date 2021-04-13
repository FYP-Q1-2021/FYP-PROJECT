﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStandard : MonoBehaviour
{
    [Header("General")]
    [Tooltip("Radius of this projectile's collision detection")]
    public float Radius = 0.01f;

    [Tooltip("Transform representing the root of the projectile (used for accurate collision detection)")]
    public Transform Root;

    [Tooltip("Transform representing the tip of the projectile (used for accurate collision detection)")]
    public Transform Tip;

    [Tooltip("LifeTime of the projectile")]
    public float MaxLifeTime = 5f;

    [Tooltip("Clip to play on impact")] public AudioClip ImpactSfxClip;

    [Tooltip("Layers this projectile can collide with")]
    public LayerMask HittableLayers = -1;

    [Header("Movement")]
    [Tooltip("Speed of the projectile")]
    public float Speed = 20f;

    [Tooltip("Downward acceleration from gravity")]
    public float GravityDownAcceleration = 0f;

    [Tooltip(
        "Distance over which the projectile will correct its course to fit the intended trajectory (used to drift projectiles towards center of screen in First Person view). At values under 0, there is no correction")]
    public float TrajectoryCorrectionDistance = -1;

    [Tooltip("Determines if the projectile inherits the velocity that the weapon's muzzle had when firing")]
    public bool InheritWeaponVelocity = false;

    [Header("Damage")]
    [Tooltip("Damage of the projectile")]
    public float damage = 40f;

    [Header("Debug")]
    [Tooltip("Color of the projectile radius debug view")]
    public Color RadiusColor = Color.cyan * 0.2f;

    Projectile m_ProjectileBase;
    Vector3 m_LastRootPosition;
    Vector3 m_Velocity;
    bool m_HasTrajectoryOverride;
    float m_ShootTime;
    Vector3 m_TrajectoryCorrectionVector;
    Vector3 m_ConsumedTrajectoryCorrectionVector;
    List<Collider> m_IgnoredColliders;

    const QueryTriggerInteraction k_TriggerInteraction = QueryTriggerInteraction.Collide;

    void OnEnable()
    {
        m_ProjectileBase = GetComponent<Projectile>();

        m_ProjectileBase.OnShoot += OnShoot;

        Destroy(gameObject, MaxLifeTime);
    }

    void OnShoot()
    {
        m_ShootTime = Time.time;
        m_LastRootPosition = Root.position;
        m_Velocity = transform.forward * Speed;
        m_IgnoredColliders = new List<Collider>();
        transform.position += m_ProjectileBase.InheritedMuzzleVelocity * Time.deltaTime;

        // Ignore colliders of owner
        Collider[] ownerColliders = m_ProjectileBase.Owner.GetComponentsInChildren<Collider>();
        m_IgnoredColliders.AddRange(ownerColliders);

        // Handle case of player shooting (make projectiles not go through walls, and remember center-of-screen trajectory)
        PlayerWeaponsManager playerWeaponsManager = m_ProjectileBase.Owner.GetComponent<PlayerWeaponsManager>();
        if (playerWeaponsManager)
        {
            m_HasTrajectoryOverride = true;

            Vector3 cameraToMuzzle = (m_ProjectileBase.InitialPosition -
                                      playerWeaponsManager.weaponCamera.transform.position);

            m_TrajectoryCorrectionVector = Vector3.ProjectOnPlane(-cameraToMuzzle,
                playerWeaponsManager.weaponCamera.transform.forward);
            if (TrajectoryCorrectionDistance == 0)
            {
                transform.position += m_TrajectoryCorrectionVector;
                m_ConsumedTrajectoryCorrectionVector = m_TrajectoryCorrectionVector;
            }
            else if (TrajectoryCorrectionDistance < 0)
            {
                m_HasTrajectoryOverride = false;
            }

            if (Physics.Raycast(playerWeaponsManager.weaponCamera.transform.position, cameraToMuzzle.normalized,
                out RaycastHit hit, cameraToMuzzle.magnitude, HittableLayers, k_TriggerInteraction))
            {
                if (IsHitValid(hit))
                {
                    OnHit(hit.point, hit.normal, hit.collider);
                }
            }
        }
    }

    void Update()
    {
        // Move
        transform.position += m_Velocity * Time.deltaTime;
        if (InheritWeaponVelocity)
        {
            transform.position += m_ProjectileBase.InheritedMuzzleVelocity * Time.deltaTime;
        }

        // Drift towards trajectory override (this is so that projectiles can be centered 
        // with the camera center even though the actual weapon is offset)
        if (m_HasTrajectoryOverride && m_ConsumedTrajectoryCorrectionVector.sqrMagnitude <
            m_TrajectoryCorrectionVector.sqrMagnitude)
        {
            Vector3 correctionLeft = m_TrajectoryCorrectionVector - m_ConsumedTrajectoryCorrectionVector;
            float distanceThisFrame = (Root.position - m_LastRootPosition).magnitude;
            Vector3 correctionThisFrame =
                (distanceThisFrame / TrajectoryCorrectionDistance) * m_TrajectoryCorrectionVector;
            correctionThisFrame = Vector3.ClampMagnitude(correctionThisFrame, correctionLeft.magnitude);
            m_ConsumedTrajectoryCorrectionVector += correctionThisFrame;

            // Detect end of correction
            if (m_ConsumedTrajectoryCorrectionVector.sqrMagnitude == m_TrajectoryCorrectionVector.sqrMagnitude)
            {
                m_HasTrajectoryOverride = false;
            }

            transform.position += correctionThisFrame;
        }

        // Orient towards velocity
        transform.forward = m_Velocity.normalized;

        // Gravity
        if (GravityDownAcceleration > 0)
        {
            // add gravity to the projectile velocity for ballistic effect
            m_Velocity += Vector3.down * GravityDownAcceleration * Time.deltaTime;
        }

        // Hit detection
        {
            RaycastHit closestHit = new RaycastHit();
            closestHit.distance = Mathf.Infinity;
            bool foundHit = false;

            // Sphere cast
            Vector3 displacementSinceLastFrame = Tip.position - m_LastRootPosition;
            RaycastHit[] hits = Physics.SphereCastAll(m_LastRootPosition, Radius,
                displacementSinceLastFrame.normalized, displacementSinceLastFrame.magnitude, HittableLayers,
                k_TriggerInteraction);
            foreach (var hit in hits)
            {
                if (IsHitValid(hit) && hit.distance < closestHit.distance)
                {
                    foundHit = true;
                    closestHit = hit;
                }
            }

            if (foundHit)
            {
                // Handle case of casting while already inside a collider
                if (closestHit.distance <= 0f)
                {
                    closestHit.point = Root.position;
                    closestHit.normal = -transform.forward;
                }

                OnHit(closestHit.point, closestHit.normal, closestHit.collider);
            }
        }

        m_LastRootPosition = Root.position;
    }

    bool IsHitValid(RaycastHit hit)
    {
        // ignore hits with triggers that don't have a Damageable component
        if (hit.collider.isTrigger && hit.collider.GetComponent<Health>() == null)
        {
            return false;
        }

        // ignore hits with specific ignored colliders (self colliders, by default)
        if (m_IgnoredColliders != null && m_IgnoredColliders.Contains(hit.collider))
        {
            return false;
        }

        return true;
    }

    void OnHit(Vector3 point, Vector3 normal, Collider collider)
    {
        // point damage

        Health health = collider.gameObject.GetComponent<Health>();
        if (health)
        {
            Debug.Log("Hey");

            health.Damage(damage);
        }
        else
        {

            Hitbox hitbox = collider.gameObject.GetComponent<Hitbox>();
            if(hitbox)
            {
                Debug.Log("Hey2");

                hitbox.InflictDamage(damage);
            }
        }

        // Self Destruct
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        Health health = col.gameObject.GetComponent<Health>();
        if (health)
        {
            Debug.Log("Hey");

            health.Damage(damage);
        }
        else
        {

            Hitbox hitbox = col.gameObject.GetComponent<Hitbox>();
            if (hitbox)
            {
                Debug.Log("Hey2");

                hitbox.InflictDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = RadiusColor;
        Gizmos.DrawSphere(transform.position, Radius);
    }
}

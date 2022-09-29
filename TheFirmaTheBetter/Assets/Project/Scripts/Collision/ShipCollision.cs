using EventSystem;
using Projectiles;
using ShipParts.Ship;
using System;
using UnityEngine;

namespace Collisions
{
    public class ShipCollision : MonoBehaviour, ICollidable
    {
        private ShipBuilder shipBuilder;
        private Rigidbody rigidbody;

        private void Awake()
        {
            shipBuilder = GetComponent<ShipBuilder>();
            Channels.OnPlayerSpawned += ShipSpawned;

        }

        private void ShipSpawned(GameObject SpawnedShip, int playerIndex)
        {
            if (playerIndex == shipBuilder.PlayerNumber)
            {
                rigidbody = GetComponentInParent<Rigidbody>();
            }
        }

        public void DestroySelf()
        {

        }

        public void HandleCollision<T1>(T1 objectThatHit) where T1 : ICollidable
        {

            if (objectThatHit is Projectile)
            {
                HandleHitByProjectile(objectThatHit as Projectile);
            }

            if (objectThatHit is ShipCollision)
            {
                HandleHitByOtherShip(objectThatHit as ShipCollision);
            }
        }

        private void HandleHitByOtherShip(ShipCollision shipCollision)
        {
            if (shipCollision != null)
            {
                Rigidbody otherRigidbody = shipCollision.gameObject.GetComponentInParent<Rigidbody>();
                if (otherRigidbody)
                {
                    Vector3 bumpDir = transform.position - shipCollision.transform.position;

                    //apply force to both ships based on position delta
                    //TODO: adjust force based on (total?) weight of ship
                    rigidbody.AddForce(bumpDir.normalized * otherRigidbody.velocity.magnitude, ForceMode.Impulse);//issue, some bumps are too strong
                    //Debug.DrawLine(transform.position, transform.position + (bumpDir.normalized * otherRigidbody.velocity.magnitude), Color.red, 2f);
                }
            }
        }

        private void HandleHitByProjectile(Projectile projectileThatHit)
        {
            //Debug.Log($"took damage! ({Channels.OnPlayerTakeDamage?.GetInvocationList().Length})called");
            Channels.OnPlayerTakeDamage?.Invoke(shipBuilder, projectileThatHit.ProjectileDamage);

            projectileThatHit.DestroySelf();
        }
    }
}
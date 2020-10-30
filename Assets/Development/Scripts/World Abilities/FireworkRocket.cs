using UnityEngine;

public class FireworkRocket : MonoBehaviour
{
    [SerializeField] private float explosionSize;
    [SerializeField] private int damage;
    private Camera mainCam;
    private float camZ;
    private WorldAbilities worldAbilities;

    private void Start()
    {
        mainCam = Camera.main;
        camZ = mainCam.transform.position.y;

        worldAbilities = FindObjectOfType<WorldAbilities>();
    }

    private void Update1()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = camZ;

            Vector3 explosionPos = mainCam.ScreenToWorldPoint(mousePos);
            Collider[] enemies = Physics.OverlapSphere(explosionPos, explosionSize);

            for(int i = 0; i < enemies.Length; i++)
            {
                if(enemies[i].GetComponent<EnemyUnit>())
                {
                    enemies[i].GetComponent<EnemyUnit>().TakeDamage(damage);
                }
            }

            worldAbilities.ResetFireworkRocket();
        }
    }
}

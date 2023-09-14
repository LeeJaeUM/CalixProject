using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shot
{
    public class CircleShotGotoShot : MonoBehaviour
    {
        // �Ѿ��� ���� �� Target���� ���ư� ����
        public Transform Target;
        public Transform start;
        // �߻�� �Ѿ� ������Ʈ
        public GameObject Bullet;

        private void Start()
        {
            // 1�ʸ��� Shot() �Լ��� ȣ���մϴ�.
            InvokeRepeating("Shot", 0f, 1f);
        }

        private void Shot()
        {
            // Target �������� �߻�� ������Ʈ ����
            List<Transform> bullets = new List<Transform>();

            for (int i = 0; i < 360; i += 13)
            {
                // �Ѿ� ����
                GameObject temp = Instantiate(Bullet);

                // 2�� �� ����
                Destroy(temp, 2f);

                // �Ѿ� ���� ��ġ�� ���� ������Ʈ�� ��ġ�� �Ѵ�.
                Vector3 spawnPosition = start.position + Quaternion.Euler(0, 0, i) * (Vector3.right * 5); // ���÷� 2 ������ŭ ������ ��ġ�� ����
                temp.transform.position = spawnPosition;

                // ?�� �Ŀ� Target���� ���ư� ������Ʈ ����
                bullets.Add(temp.transform);

                // Yaw ȸ���� ����
                temp.transform.rotation = Quaternion.Euler(0, i, 0);
            }

            // �Ѿ��� Target �������� �̵���Ų��.
            StartCoroutine(BulletToTarget(bullets));
        }

        private IEnumerator BulletToTarget(IList<Transform> objects)
        {
            // 0.5�� �Ŀ� ����
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < objects.Count; i++)
            {
                // ���� �Ѿ��� ��ġ���� �÷��̾��� ��ġ�� ���Ͱ��� �y���Ͽ� ������ ����
                Vector3 targetDirection = Target.transform.position - objects[i].position;

                // ������ �ٶ󺸵��� ȸ��
                Quaternion rotation = Quaternion.LookRotation(targetDirection);
                objects[i].rotation = rotation;
            }

            // ������ ����
            objects.Clear();
        }
    }
}

using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] Transform target; // �^�[�Q�b�g�̃Q�[���I�u�W�F�N�g
    [SerializeField] float height = 10f; // �J�����̍���

    void Start()
    {
        if (target != null)
        {
            // �^�[�Q�b�g�̐^��ɃJ������z�u
            Vector3 newPosition = target.position;
            newPosition.y += height;
            transform.position = newPosition;

            // �J�����̌������^�[�Q�b�g�Ɍ�����
            transform.LookAt(target);
        }

        //�z�u���I�������폜
        Destroy(this);
    }
}


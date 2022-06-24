using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin_Example : MonoBehaviour
{
    [SerializeField] private GameObject[] Mannequin_Parts;
    [SerializeField] private GameObject Mannequin_Head;
    [SerializeField] private GameObject Mannequin_ArmL;
    [SerializeField] private GameObject Mannequin_ArmR;
    [SerializeField] private GameObject Mannequin_LegL;
    [SerializeField] private GameObject Mannequin_LegR;

    [SerializeField] private Material Red_Mat;
    [SerializeField] private Material Blue_Mat;
    [SerializeField] private Material Green_Mat;
    [SerializeField] private Material Default_Mat;

    [SerializeField] private ItemData[] Mannequin_PartsData;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject iter in Mannequin_Parts)
        {
            RandomMaterial_output(iter);
        }
    }

    public void GetMannequinData()
    {

    }
    public void RandomMaterial_output(GameObject Parts)
    {
        //��ü������ �����ʿ�
        //����ŷ ���� �� ���� �ϴ� ����
        //��ǰ ���� -> ������ ���� -> ������ ���� ItemData�ҷ��;� ��.(������)

        //������ ����ŷ�� ��ǰ ���� ��ó�� ������ �����͸� �� �迭������ ���� ����.
        //ex)�Ӹ������͸� �̾Ƽ� �ӽù迭������ �ְ� �ű⼭ �������� ����, ����ġ������ ItemData�� material��������� �ϸ�?
        Material mat;
        int rand = Random.Range(0, 4);

        switch(rand)
        {
            case 0:
                mat = Red_Mat;
                break;
            case 1:
                mat = Blue_Mat;
                break;
            case 2:
                mat = Green_Mat;
                break;
            default:
                mat = Default_Mat;
                break;
        }
        if (mat)
        {
            foreach (Renderer iter in Parts.GetComponentsInChildren<Renderer>())
            {
                iter.material = mat;
            }
        }
        
    }
}

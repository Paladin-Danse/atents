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
        //전체적으로 수정필요
        //마네킹 랜덤 색 지정 하는 순서
        //부품 지정 -> 랜덤색 지정 -> 지정된 색의 ItemData불러와야 됨.(복잡함)

        //오히려 마네킹에 부품 끼울 때처럼 아이템 데이터를 한 배열변수에 전부 저장.
        //ex)머리데이터만 뽑아서 임시배열변수에 넣고 거기서 랜덤값을 산출, 스위치문에서 ItemData와 material입히기까지 하면?
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

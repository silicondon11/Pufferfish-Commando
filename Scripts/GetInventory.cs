using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GetInventory : MonoBehaviour
{
    private void Start()
    {
        GetInventoryItems();
    }

    private void GetInventoryItems()
    {
        ShellType shellInNewScene = DataMove.Instance.CurrentShellType;
        List<string> pentsInNewScene = DataMove.Instance.CurrentPentTypes;

        if ((shellInNewScene == ShellType.SixA) || (shellInNewScene == ShellType.SixB) || (shellInNewScene == ShellType.Eight))
        {

            Transform hat = transform.Find("Details").Find("Cube.015");
            hat.gameObject.SetActive(false);
        }

        if (shellInNewScene == ShellType.Four)
        {
            InstantiatePentByName(pentsInNewScene[0], 0);
            InstantiatePentByName(pentsInNewScene[2], 2);
            InstantiatePentByName(pentsInNewScene[7], 7);
            InstantiatePentByName(pentsInNewScene[8], 8);
        }
        else if (shellInNewScene == ShellType.Five)
        {
            InstantiatePentByName(pentsInNewScene[0], 0);
            InstantiatePentByName(pentsInNewScene[2], 2);
            InstantiatePentByName(pentsInNewScene[4], 4);
            InstantiatePentByName(pentsInNewScene[5], 5);
            InstantiatePentByName(pentsInNewScene[6], 6);
        }
        else if (shellInNewScene == ShellType.SixA)
        {
            InstantiatePentByName(pentsInNewScene[0], 0);
            InstantiatePentByName(pentsInNewScene[2], 2);
            InstantiatePentByName(pentsInNewScene[5], 5);
            InstantiatePentByName(pentsInNewScene[7], 7);
            InstantiatePentByName(pentsInNewScene[8], 8);
            InstantiatePentByName(pentsInNewScene[9], 9);
        }
        else if (shellInNewScene == ShellType.SixB)
        {
            InstantiatePentByName(pentsInNewScene[0], 0);
            InstantiatePentByName(pentsInNewScene[2], 2);
            InstantiatePentByName(pentsInNewScene[4], 4);
            InstantiatePentByName(pentsInNewScene[5], 5);
            InstantiatePentByName(pentsInNewScene[6], 6);
            InstantiatePentByName(pentsInNewScene[9], 9);
        }
        else if (shellInNewScene == ShellType.Eight)
        {
            InstantiatePentByName(pentsInNewScene[0], 0);
            InstantiatePentByName(pentsInNewScene[1], 1);
            InstantiatePentByName(pentsInNewScene[2], 2);
            InstantiatePentByName(pentsInNewScene[3], 3);
            InstantiatePentByName(pentsInNewScene[4], 4);
            InstantiatePentByName(pentsInNewScene[5], 5);
            InstantiatePentByName(pentsInNewScene[6], 6);
            InstantiatePentByName(pentsInNewScene[9], 9);
        }

        
        
        
        //need to populate shell with pents then instatiate, here and in inventory preview
    }

    private void InstantiatePentByName(string name, int slot)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + name);

        if (prefab)
        {
            if (slot == 0)
            {
                UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(116.57f, 108f, 0f);
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(747f, 494f, 1204f);
                GameObject prefabInstance = Instantiate(prefab, pos, rot);
                prefabInstance.transform.SetParent(gameObject.transform);

            }
            else if (slot == 1)
            {
                UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(116.57f, 180f, 0f);
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(693f, 494f, 1164f);
                GameObject prefabInstance = Instantiate(prefab, pos, rot);
                prefabInstance.transform.SetParent(gameObject.transform);
            }
            else if (slot == 2)
            {
                UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(116.57f, 252f, 0f);
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(639f, 494f, 1204f);
                GameObject prefabInstance = Instantiate(prefab, pos, rot);
                prefabInstance.transform.SetParent(gameObject.transform);
            }
            else if (slot == 3)
            {
                UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(180f, 0f, 0f);
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(693f, 457f, 1222f);
                GameObject prefabInstance = Instantiate(prefab, pos, rot);
                prefabInstance.transform.SetParent(gameObject.transform);
            }
            else if (slot == 4)
            {
                UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(-63.43f, -108f, 0f);
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(748f, 553f, 1240f);
                GameObject prefabInstance = Instantiate(prefab, pos, rot);
                prefabInstance.transform.SetParent(gameObject.transform);
            }
            else if (slot == 5)
            {
                UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(-63.43f, -180f, 0f);
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(693f, 553f, 1280f);
                GameObject prefabInstance = Instantiate(prefab, pos, rot);
                prefabInstance.transform.SetParent(gameObject.transform);
            }
            else if (slot == 6)
            {
                UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(-63.43f, -252f, 0f);
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(638f, 553f, 1240f);
                GameObject prefabInstance = Instantiate(prefab, pos, rot);
                prefabInstance.transform.SetParent(gameObject.transform);
            }
            else if (slot == 7)
            {
                UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(116.57f, 36f, 0f);
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(727f, 494f, 1269f);
                GameObject prefabInstance = Instantiate(prefab, pos, rot);
                prefabInstance.transform.SetParent(gameObject.transform);
            }
            else if (slot == 8)
            {
                UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(116.57f, -36f, 0f);
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(659f, 494f, 1269f);
                GameObject prefabInstance = Instantiate(prefab, pos, rot);
                prefabInstance.transform.SetParent(gameObject.transform);
            }
            else if (slot == 9)
            {
                UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(0f, 0f, 0f);
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(693f, 589f, 1222f);
                GameObject prefabInstance = Instantiate(prefab, pos, rot);
                prefabInstance.transform.SetParent(gameObject.transform);
            }
            

        }
    }
}

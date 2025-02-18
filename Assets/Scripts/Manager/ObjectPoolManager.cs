
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public GameObject hideBox;
    [System.Serializable] // 클래스를 직렬화 시키는 법, 이전 정보를 저장해줌
    public class Pool // 풀링할 오브젝트
    {
        public string tag; // 태그 저장
        public GameObject prefab; // 풀링할 오브젝트
        public int size; // 어느정도 풀링할 건지
    }

    const float xSize = 4.5f;
    const float yMaxSize = 7.4f;
    const float yMinSize = 4.7f;

    float xSpawnValue;
    float ySpawnValue;
    Vector2 spawnValue;

    public List<Pool> pools; // 풀링할 객체들
    public Dictionary<string, Queue<GameObject>> poolDictionary; // 풀링 딕셔너리로 이름에 따라 풀링 딕셔러니에 보관

    private void Awake()
    {
        Initialize(); // 초기 값
    }

    private void Initialize()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>(); // 객체 생성

        foreach (Pool pool in pools) // 많은 풀링 돌리기
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); // 큐 객체 생성
            
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab); // 풀링된 오브젝트 생성
                obj.name = pool.prefab.name;
                obj.transform.SetParent(hideBox.transform); // 생성해서 폴더(빈옵젝) 안에 저장 
                obj.SetActive(false); // 비활성화 시켜놓기
                objectPool.Enqueue(obj); // 큐에 데이터 저장
            }

            poolDictionary.Add(pool.tag, objectPool); // 태그와 옵젝을 딕셔너리에 저장

        }
    }
    public GameObject SpawnFromPool(string tag) // 비활성화로 풀링된 오브젝트 활성화, 이름을 받아서 실행
    {
        if (!poolDictionary.ContainsKey(tag)) // 이름이 딕셔너리에 없으면 오류 구문 실행
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }
        if (poolDictionary[tag] == null || poolDictionary[tag].Count == 0) // 이름은 있는데 안에 풀링 오브젝트가 없을시 리턴
        {
            return null;
        }
        

        GameObject objectToSpawn = poolDictionary[tag].Dequeue(); // 앞에서 널 체크 이후 있기에 큐에서 꺼내 쓰기
        objectToSpawn.SetActive(true); // 꺼내고 활성화
        objectToSpawn.transform.position = RandomSpawn();

        return objectToSpawn; // 그리고 그 옵젝을 리턴
    }



    public GameObject SpawnFromPool(string tag,Vector2 FusionObj) // 비활성화로 풀링된 오브젝트 활성화, 이름을 받아서 실행
    {
        if (!poolDictionary.ContainsKey(tag)) // 이름이 딕셔너리에 없으면 오류 구문 실행
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }
        if (poolDictionary[tag] == null || poolDictionary[tag].Count == 0) // 이름은 있는데 안에 풀링 오브젝트가 없을시 리턴ㅋ
        {
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue(); // 앞에서 널 체크 이후 있기에 큐에서 꺼내 쓰기
        objectToSpawn.SetActive(true); // 꺼내고 활성화
        objectToSpawn.transform.position = FusionObj;

        return objectToSpawn; // 그리고 그 옵젝을 리턴
    }

    public Vector2 RandomSpawn()
    {
        xSpawnValue = Random.Range(-xSize, xSize);
        ySpawnValue = Random.Range(-yMinSize, yMaxSize);

        spawnValue = new Vector2(xSpawnValue, ySpawnValue);

        return spawnValue;
    }

    public void DeSpawnToPool(string tag, GameObject pool)
    {
        if (!poolDictionary.ContainsKey(tag)) // 이름이 딕셔너리에 없으면 오류 구문 실행
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
        }
        poolDictionary[tag].Enqueue(pool);
        pool.SetActive(false);
    }

}

using System.Buffers;
using UnityEngine;


public class MeshCreator : MonoBehaviour
{
    private void Awake()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.sharedMesh = new Mesh();
        GetMesh_Improved(filter.sharedMesh);
    }

    // 엄밀하게 따지자면, worst와 improved는 같은 기능을 수행하지 않음.
    // improved는 매개변수로 넘어온 mesh를 수정하는 것이고, worst는 아예 새로 생성하는 것
    // 매 시도마다 새로운 모양이 필요하다면, Mesh를 새로 생성하는 것 또한 가비지가 발생해 부담이 될 수 있음.
    private static void GetMesh_Improved(Mesh mesh)
    {
        // ArrayPool로부터 '최소' 크기가 3인 배열을 빌려 옴. 주어진 크기보다 크거나 같을 수 있음을 유의해야 함.
        Vector3[] vertices = ArrayPool<Vector3>.Shared.Rent(3);
        int[] triangles = ArrayPool<int>.Shared.Rent(3);

        vertices[0] = new Vector3(0, 1, 0);
        vertices[1] = new Vector3(1, -1, 0);
        vertices[2] = new Vector3(-1, -1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        // vertices와 triangles의 크기가 3이 아닐 수 있기 때문에 모두 복사시키는 건 문제를 발생시킬 수 있다
        // mesh.vertices = vertices;
        // mesh.triangles = triangles;

        // triangles에 새로 기록하려면 Clear가 반드시 필요하다.
        mesh.Clear();
        // vertices를 0번부터 시작하여 3개만큼 복사하도록 한다.
        mesh.SetVertices(vertices, 0, 3);
        // triangles를 0번부터 시작하여 3개만큼 복사하도록 한다. 마지막 0는 기본 submesh 번호
        mesh.SetTriangles(triangles, 0, 3, 0);

        // 풀 반납
        ArrayPool<Vector3>.Shared.Return(vertices);
        ArrayPool<int>.Shared.Return(triangles);
    }

    private static Mesh GetMesh_Worst()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0,1,0),
            new Vector3(1,-1,0),
            new Vector3(-1,-1,0)
        };

        int[] triangles = new int[] { 0, 1, 2 };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }
}
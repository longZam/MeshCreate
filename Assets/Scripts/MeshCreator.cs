using System.Buffers;
using UnityEngine;


public class MeshCreator : MonoBehaviour
{
    private void Awake()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = GetMesh_Improved();
    }

    private static Mesh GetMesh_Improved()
    {
        // Mesh를 매번 생성하는 것이 크게 문제 될 정도로 클래스가 크지 않음. 풀링해도 무방
        Mesh mesh = new Mesh();

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

        // vertices를 0번부터 시작하여 3개만큼 복사하도록 한다.
        mesh.SetVertices(vertices, 0, 3);
        // triangles를 0번부터 시작하여 3개만큼 복사하도록 한다. 마지막 0는 기본 submesh 번호
        mesh.SetTriangles(triangles, 0, 3, 0);

        // 풀 반납
        ArrayPool<Vector3>.Shared.Return(vertices);
        ArrayPool<int>.Shared.Return(triangles);

        return mesh;
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
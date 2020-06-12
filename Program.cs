using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exercise
{

    // 우선순위 큐
    // 우선순위가 가장 큰 것부터 나온다.
    class PriorityQueue
    {
        // 2진 힙트리
        List<int> _heap = new List<int>();

        public void Push(int data)
        {
            // 힙의 맨 끝에 새로운 데이터를 삽입한다.
            _heap.Add(data);

            int now = _heap.Count - 1;
            int next, temp;
            // 정렬
            while ( now > 0 )
            {
                next = (now - 1) / 2; // 부모노드
                if (_heap[next] > _heap[now]) // 부모노드가 더 크면 정렬 중단.
                    break;

                // 두 값을 교체
                temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                // 검사 위치를 이동한다.
                now = next;
            }
        }

        public int Pop()
        {
            // 반환 데이터를 따로 저장
            int ret = _heap[0];

            // 마지막 데이터를 루트로 이동한다.
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];

            // 마지막 노드 제거
            _heap.RemoveAt(lastIndex);
            lastIndex--;

            // 역으로 내려가는 정렬 시작
            int now = 0;
            int left, right, next, temp;
            while ( true )
            {
                left = (2 * now) + 1;
                right = (2 * now) + 2;

                next = now;
                // 왼쪽값이 현재 값 보다 크면, 왼쪽으로 이동
                if (left <= lastIndex && _heap[next] < _heap[left])
                    next = left;

                // 오른쪽값이 현재 값 보다 크면, 오른쪽으로 이동
                if (right <= lastIndex && _heap[next] < _heap[right])
                    next = right;

                // 왼쪽/오른쪽 모두 현재 값 보다 작으면 종료
                if (next == now)
                    break;

                temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                // 검사 위치 이동
                now = next;
            }


            return ret;
        }

        public int Count()
        {
            return _heap.Count;
        }
    }

    class TreeNode<T>
    {
        public T Data { get; set; }
        public List<TreeNode<T>> Children { get; set; } = new List<TreeNode<T>>();
    }

    class DijikstraGraph
    {
        int[,] adj = new int[6, 6]
        {
            { -1, 15, -1, 35, -1, -1 }, // 0 -> 1, 3
            { 15, -1, 05, 10, -1, -1 }, // 1 -> 0, 2, 3
            { -1, 05, -1, -1, -1, -1 }, // 2 -> 1
            { 35, 10, -1, -1, 05, -1 }, // 3 -> 0, 1, 4
            { -1, -1, -1, 05, -1, 05 }, // 4 -> 3, 5
            { -1, -1, -1, -1, 05, -1 }  // 5 -> 4
        };

        public void Dijikstra(int start)
        {
            bool[] visited = new bool[6]; // 방문했던 노드
            int[] distance = new int[6]; // 최단 거리
            int[] parent = new int[6];

            Array.Fill(distance, Int32.MaxValue); // 모든 인덱스에 int 최대값을 넣어준다.
            distance[start] = 0;
            parent[start] = start;

            int closest;
            int now;

            int nextDist;
            while ( true )
            {
                // 제일 좋은 후보를 찾는다. (가장 가까이 있는)
                // 가장 유력한 후보의 거리와 번호를 저장한다.
                closest = Int32.MaxValue;
                now = -1;
                for  ( int i = 0; i < 6; i++ )
                {
                    // 이미 방문한 정점은 스킵
                    if (visited[i])
                        continue;
                    // 이미 발견(예약)된 적이 없거나, 기존 후보보다 멀리있으면 스킵
                    if (distance[i] == Int32.MaxValue || distance[i] >= closest)
                        continue;
                    // 여태껏 발견한 가장 후보라는 의미, 정보를 갱신
                    closest = distance[i];
                    now = i;
                }

                // 다음 후보가 하나도 없다는 의미 -> 종료
                if (now == -1)
                    break;

                // 제일 좋은 후보를 찾음 -> 방문한다.
                visited[now] = true;

                // 방문한 정점과 인접한 정점들을 조사해서, 상황에 따라 발견한 최단거리를 갱신한다.
                for(int next = 0; next < 6; next++)
                {
                    // 연결되지 않은 정점 스킵
                    if (adj[now, next] == -1)
                        continue;
                    // 이미 방문한 정점은 스킵
                    if (visited[next])
                        continue;

                    // 새로 조사된 정점의 최단거리를 조사한다.
                    nextDist = distance[now] + adj[now, next];

                    // 만약 기존에 발견한 최단거리가 새로 조사될 최단거리보다 크면, 정보를 갱신
                    if (nextDist < distance[next])
                    {
                        distance[next] = nextDist;
                        parent[next] = now;
                    }
                }
            }
        }
    }
    
    class Graph
    {
        /**
         * 0 - 1 - 2
         *       - 3
         *   - 3
         * 4 - 5
         */
        int[,] adj = new int[6, 6]
        {
            { 0, 1, 0, 1, 0, 0 }, // 0 -> 1, 3
            { 1, 0, 1, 1, 0, 0 }, // 1 -> 0, 2, 3
            { 0, 1, 0, 0, 0, 0 }, // 2 -> 1
            { 1, 1, 0, 0, 0, 0 }, // 3 -> 0, 1
            { 0, 0, 0, 1, 0, 1 }, // 4 -> 3, 5
            { 0, 0, 0, 0, 1, 0 }  // 5 -> 4
        };

        /**
         * 0 - 1 - 2
         *       - 3
         *   - 3
         * 4 - 5
         */
        List<int>[] adj2 = new List<int>[] {
            new List<int> { 1, 3 },     // 0
            new List<int> { 0, 2, 3 },  // 1
            new List<int> { 1 },        // 2
            new List<int> { 0, 1 },     // 3
            new List<int> { 3, 5 },     // 4
            new List<int> { 4 }         // 5
        };

        /**
         * 0 - 1 - 2
         *       - 3 - 4
         *   - 3 - 4 - 5
         */
        int[,] adj3 = new int[6, 6]
        {
            { 0, 1, 0, 1, 0, 0 }, // 0 -> 1, 3
            { 1, 0, 1, 1, 0, 0 }, // 1 -> 0, 2, 3
            { 0, 1, 0, 0, 0, 0 }, // 2 -> 1
            { 1, 1, 0, 0, 1, 0 }, // 3 -> 0, 1, 4
            { 0, 0, 0, 1, 0, 1 }, // 4 -> 3, 5
            { 0, 0, 0, 0, 1, 0 }  // 5 -> 4
        };

        /**
         * 0 - 1 - 2
         *       - 3 - 4
         *   - 3 - 4 - 5
         */
        List<int>[] adj4 = new List<int>[] {
            new List<int> { 1, 3 },     // 0
            new List<int> { 0, 2, 3 },  // 1
            new List<int> { 1 },        // 2
            new List<int> { 0, 1, 4 },  // 3
            new List<int> { 3, 5 },     // 4
            new List<int> { 4 }         // 5
        };

        bool[] visited = new bool[6];

        /*
         * DFS (Depth First Search) 깊이 우선 검색
         * 1) 우선 now부터 방문하고
         * 2) now와 연결된 정점들을 하나씩 확인해서 [아직 미방문 상태라면] 방문한다.
         */
        public void DFS(int now)
        {
            Console.WriteLine(now);
            visited[now] = true; // 1) 우선 now부터 방문하고

            for ( int next = 0; next < adj.GetLength(0); next++ )
            {
                if (adj[now, next] == 0) // 연결되어 있지 않으면 스킵
                    continue;
                if (visited[next]) // 이미 방문했으면 스킵
                    continue;

                DFS(next); // [아직 미방문 상태라면] 방문한다.
            }
        }

        // DFS (Depth First Search) 깊이 우선 검색
        public void DFS2(int now)
        {
            Console.WriteLine(now);
            visited[now] = true; // 1) 우선 now부터 방문하고

            foreach ( int next in adj2[now] )
            {
                if (visited[next]) // 이미 방문했으면 스킵
                    continue;

                DFS2(next); // [아직 미방문 상태라면] 방문한다.
            }
        }

        /*
         * DFS (Depth First Search) 깊이 우선 검색 : 종단 연결을 일방향으로 진행한다.
         * 종단 연결이 끊어져 있는 경우 DFS 로만 모두 방문이 불가능하기 때문에
         * 모든 종단을 순회하며 방문되지 않은 것을 찾아 DFS 순환 시킨다.
         */
        public void SearchAllDFS()
        {
            visited = new bool[6];
            for ( int now = 0; now < 6; now++ )
                if ( !visited[now] )
                    DFS2(now);
        }

        // BFS (Breadth First Search) 너비 우선 검색
        // 시작점으로 부터 가까운 노드부터 탐색.
        // 길찾기 알고리즘에서 많이 사용된다.
        // 단점 : 모든 경로를 탐색한다. -> 비용문제 발생
        //        모든 경로를 탐색할 때만 사용 -> 가중치가 없을 때만 사용
        public void BFS(int start)
        {
            bool[] found = new bool[6];
            int[] parent = new int[6]; // 이전 노드
            int[] distance = new int[6]; // 노드간 거리

            Queue<int> q = new Queue<int>();
            q.Enqueue(start);
            found[start] = true;

            parent[start] = start;
            distance[start] = 0;

            int now;
            while ( q.Count > 0 )
            {
                now = q.Dequeue();
                Console.WriteLine(now);

                for ( int next = 0; next < adj3.GetLength(1); next++ )
                {
                    if (adj3[now, next] == 0) // 인접하지 않았으면 스킵
                        continue;
                    if (found[next]) // 이미 발견한 노드면 스킵
                        continue;

                    q.Enqueue(next);
                    found[next] = true; 

                    parent[next] = now;
                    distance[next] = distance[now] + 1;
                }
            }
        }

    }

    class Program
    {
        
        static TreeNode<string> MakeTree()
        {
            TreeNode<string> root = new TreeNode<string>() { Data = "R1 개발실" };
            {
                {
                    TreeNode<string> node = new TreeNode<string>() { Data = "디자인팀" };
                    node.Children.Add(new TreeNode<string>() { Data = "전투" });
                    node.Children.Add(new TreeNode<string>() { Data = "경제" });
                    node.Children.Add(new TreeNode<string>() { Data = "스토리" });
                    root.Children.Add(node);
                }

                {
                    TreeNode<string> node = new TreeNode<string>() { Data = "프로그래밍팀" };
                    node.Children.Add(new TreeNode<string>() { Data = "서버" });
                    node.Children.Add(new TreeNode<string>() { Data = "클라" });
                    node.Children.Add(new TreeNode<string>() { Data = "엔진" });
                    root.Children.Add(node);
                }

                {
                    TreeNode<string> node = new TreeNode<string>() { Data = "아트팀" };
                    node.Children.Add(new TreeNode<string>() { Data = "배경" });
                    node.Children.Add(new TreeNode<string>() { Data = "캐릭터" });
                    root.Children.Add(node);
                }

            }

            return root;
        }

        static void PrintTree(TreeNode<string> root)
        {
            Console.WriteLine(root.Data);

            foreach (TreeNode<string> child in root.Children)
                PrintTree(child);
        }

        static int GetHeight(TreeNode<string> root)
        {
            int height = 0;

            int newHeight;
            foreach (TreeNode<string> child in root.Children)
            {
                newHeight = GetHeight(child) + 1;
                height = Math.Max(height, newHeight);
            }

            return height;
        }

        static void Main(string[] args)
        {
            //Graph graph = new Graph();
            //graph.DFS(3);
            //graph.DFS2(3);
            //graph.SearchAllDFS();
            //graph.BFS(0);

            //DijikstraGraph g = new DijikstraGraph();
            //g.Dijikstra(0);

            //TreeNode<string> root = MakeTree();
            //PrintTree(root);
            //Console.WriteLine(GetHeight(root));

            /*
             * 이진 트리 특징
             * 1) 왼쪽으로 타고 가면 현재 값 보다 작다.
             * 2) 오른쪽으로 타고 가면 현재 값 보다 크다.
             * 
             * 힙 트리
             * 힙 트리 1 법칙: [부모 노드]가 가진 값을 항상 [자식 노드]가 가진 값 보다 크다.
             * 힙 트리 2 법칙: 노드 개수를 알면, 트리 구조는 무조건 확정할 수 있다.
             * 힙 트리 구조
             * 1) 마지막 레벨을 제외한 모든 레벨에 노드가 꽉 차 있다.
             * 2) 마지막 레벨에 노드가 있을 때는, 항상 왼쪽부터 순서대로 채워야 한다.
             * 
             * 힙 트리에 새로운 값 추가
             * 1) 우선 트리 구조부터 맞춘다. - 2법칙
             * 2) 트리 정렬을 진행한다 - 1법칙
             * 
             * 힙트리에서 최대값 꺼내기
             * 1) 가장 큰 값[0번 인덱스] 제거
             * 2) 가장 마지막 인덱스를 [0번 인덱스]로 이동
             * 3) 트리 정렬을 진행한다.
             */


            // 우선순위 큐
            PriorityQueue q = new PriorityQueue();
            q.Push(20);
            q.Push(10);
            q.Push(30);
            q.Push(90);
            q.Push(40);

            while ( q.Count() > 0 )
            {
                Console.WriteLine(q.Pop());
            }
        }

        static void StackAndQueue(string[] args)
        {
            Stack<int> stack = new Stack<int>(); // UI 중첩팝업 띄울때 많이 사용됨. (LIFO)
            Queue<int> queue = new Queue<int>(); // 네트워크 패킷을 순서대로 처리할 때 사용됨. (FIFO)
            
            
        }
    }
}

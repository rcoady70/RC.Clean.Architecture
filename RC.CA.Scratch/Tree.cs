using System.Diagnostics;

namespace RC.CA.Scratch
{
    internal class TreeTest
    {
        Tree _tree = new Tree();
        public void Run()
        {
            _tree.Add(1, 1);
            _tree.Add(1, 2);
            _tree.Add(1, 3);
            _tree.Add(1, 4);
            _tree.Add(2, 5);
            _tree.Add(2, 6);
            _tree.Add(3, 5);

            var ix = _tree.GetParent(5);
        }
    }
    public class Tree
    {
        System.Collections.Generic.Dictionary<int, List<int>> _tree = new Dictionary<int, List<int>>();

        public void Add(int parent, int child)
        {
            if (!_tree.ContainsKey(parent))
                _tree.Add(parent, new List<int>() { child });
            else
            {
                if (!_tree[parent].Contains(child))
                    _tree[parent].Add(child);
            }
        }
        public int GetParent(int child)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var x = _tree.Where(pair => pair.Value.Contains(child)).ToList();
            var y = _tree.Where(pair => pair.Value.Contains(child)).Select(p => p.Key).ToList();
            watch.Stop();
            return x[0].Key;
        }
    }



    //public class Node
    //{
    //    public int Id;
    //    System.Collections.Generic.List<int> Children = new List<int>();
    //    public Node(int id, System.Collections.Generic.List<int> children)
    //    {
    //        if (id > 123456)
    //            throw new ArgumentException($"Parent {nameof(Id)} cannot be greater than {123456}");
    //    }

    //}
}

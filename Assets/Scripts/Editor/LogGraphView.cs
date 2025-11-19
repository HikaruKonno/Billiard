using log4net.Util;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class LogGraphView : GraphView
{
    private NodeView _lastNode;
    private int _nodeCount = 0;

    public LogGraphView()
    {
        FlowLogger.OnLogAdded += AddLogNode;

        // その他 GraphView 設定（ズーム、グリッドなど）
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        Insert(0, new GridBackground());
    }

    public void ClearGraph()
    {
        DeleteElements(graphElements);
    }

    // ノード追加する
    public void AddLogNode(FlowLogger.LogEntry log)
    {
        var node = new NodeView(log);

        // 位置を設定：Xは固定、Yをインデックスでずらす
        float x = 100;
        float y = 100 + _nodeCount * 150; // 縦に150pxずつ並べる（必要に応じて調整）
        node.SetPosition(new Rect(x, y, 200, 100)); // 幅200、高さ100

        AddElement(node);

        if (_lastNode != null)
        {
            var edge = _lastNode.output.ConnectTo(node.input);
            AddElement(edge);
        }

        _lastNode = node;
        _nodeCount++;
    }
}
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using log4net.Util;

public class NodeView : Node
{
    public Port input;
    public Port output;
    public FlowLogger.LogEntry log;

    public NodeView(FlowLogger.LogEntry log)
    {
        this.log = log;
        this.title = log.Info.FileName;

        mainContainer.Add(new Label($"{log.Info.MethodName}"));

        input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));

        input.portName = "Prev";
        output.portName = "Next";

        // 入出力ポート追加（順番は自由）
        inputContainer.Add(input);
        outputContainer.Add(output);

        // タイトル表示
        var titleLabel = new Label(log.Info.FileName);

        // ラベル（追加情報）
        var detailLabel = new Label($"{log.Info.MethodName}\nFrame: {log.Frame} | Time: {log.Time:F2}");
        detailLabel.style.unityTextAlign = TextAnchor.MiddleLeft;

        // 拡張部分に追加（折りたたみ可能）
        extensionContainer.Add(detailLabel);

        RefreshExpandedState();
        RefreshPorts();
    }
}
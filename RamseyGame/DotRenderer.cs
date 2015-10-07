using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamseyGame
{
    public enum DotRenderType
    {
        pdf,
        png,
        svg,
        eps,
        ps
    }
    public class DotRenderer
    {
        public bool DrawCompleteGraph { get; set; }

        string _dotPath;
        public DotRenderer(string dotPath)
        {
            _dotPath = dotPath;
        }

        public string Render(IEnumerable<int> edgeWeights, string fileName, DotRenderType renderType)
        {
            return Render(ToDot(edgeWeights), fileName, renderType);
        }

        public string Render(string dot, string fileName, DotRenderType renderType)
        {
            var root = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrWhiteSpace(root))
                Directory.CreateDirectory(root);

            fileName = Path.Combine(root, Path.GetFileNameWithoutExtension(fileName) + "." + renderType);

            var tempFile = Path.GetTempFileName();
            using (var sw = new StreamWriter(tempFile))
                sw.Write(dot);

            var info = new ProcessStartInfo(_dotPath, string.Format(@"-T{0} ""{2}"" -o ""{1}""", renderType, fileName, tempFile));
            info.UseShellExecute = false;
            info.CreateNoWindow = true;

            var process = new Process();
            process.StartInfo = info;
            process.Start();
            process.WaitForExit();

            File.Delete(tempFile);

            return fileName;
        }

        string ToDot(IEnumerable<int> edgeWeights)
        {
            var ew = edgeWeights.ToList();
            var a = GraphUtility.GetAdjacencyMatrix(ew);
            var n = a.GetUpperBound(0) + 1;

            var sb = new StringBuilder();

            sb.AppendLine("graph G {");
            sb.AppendLine("splines=true;");
            sb.AppendLine("scale = 5;");
            sb.AppendLine("node[color=black shape=circle style = filled width=0.5];");
            sb.AppendLine("edge[color=black];");
            for (int v = 0; v < n; v++)
            {
                sb.AppendLine(string.Format("{0} [label=\"\"];", v));
            }

            int k = 0;
            for (int i = 0; i < n; i++)
                for (int j = i + 1; j < n; j++)
                {
                    if (a[i, j])
                    {
                        if (ew[k] == 1)
                            sb.AppendLine(string.Format("{0} -- {1} [penwidth = 8 color=red]", i, j));
                        else
                            sb.AppendLine(string.Format("{0} -- {1} [penwidth = 8 color=blue]", i, j));
                    }
                    else if (DrawCompleteGraph)
                    {
                        sb.AppendLine(string.Format("{0} -- {1} [penwidth = 8 color=grey]", i, j));
                    }
                    k++;
                }
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}

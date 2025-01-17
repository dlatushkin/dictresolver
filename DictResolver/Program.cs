using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace DictResolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var content = File.ReadAllText("wavemaker.json");

            //ProcessWithNewton(content);
            ProcessWithText(content);

            Console.WriteLine("done");
        }

        static void ProcessWithText(string content)
        {
            JsonNode forecastNode = JsonNode.Parse(content);
            var leaves = new List<Leaf>();
            VisitNode(forecastNode, leaves);
        }

        static void ProcessWithNewton(string content)
        {
            dynamic obj = JsonConvert.DeserializeObject(content);

            var leaves = new List<Leaf>();
            VisitNode(obj, leaves);

            foreach (var leaf in leaves)
            {
                Console.WriteLine("{0}=>{1}", leaf.Path, leaf.Val);
                var s = (string)leaf.Val.Value;
                if (s.Split('.', StringSplitOptions.RemoveEmptyEntries).Length < 2)
                {
                    continue;
                }

                var resolvedLeaf = leaves.FirstOrDefault(l => l.Path.EndsWith(s));
                if (resolvedLeaf == null)
                {
                    continue;
                }

                leaf.Val = resolvedLeaf.Val;
                Console.WriteLine("{0} substituted by {1}", s, resolvedLeaf.Val);
            }
        }

        static void VisitNode(JsonNode parentNode, List<Leaf> leaves)
        {
            if (parentNode is JsonObject parentObj)
            {
                foreach (var childObj in parentObj)
                {
                    VisitNode(childObj.Value, leaves);
                }
            }
            //else if (parentNode.GetType() == typeof(JsonElement))
            //{
            //    var element = (JsonElement)parentNode;
            //    foreach (var childObj in parentObj)
            //    {
            //        VisitNode(childObj.Value, leaves);
            //    }
            //}
        }

        //static void VisitNode(JsonValuePrimitive element, List<Leaf> leaves)
        //{

        //}

        static void VisitNode(JsonObject parentNode, List<Leaf> leaves)
        {
            //foreach (var childNode in parentNode.)
            //{
            //}
        }

        internal class Leaf
        {
            public string Path { get; set; }

            public JValue Val { get; set; }
        }

        static void VisitNode(JToken tok, List<Leaf> leaves)
        {
            if (tok is JValue val)
            {
                if (val.Value is string s)
                {
                    //Console.WriteLine("{0}=>{1}", val.Path, s);
                    leaves.Add( new Leaf { Path = val.Path, Val = val });
                }
            }
            else
            {
                foreach (var child in tok)
                {
                    VisitNode(child, leaves);
                }
            }
        }

        static void VisitNode(JObject obj, List<Leaf> leaves)
        {
            foreach (var child in obj)
            {
                VisitNode(child.Value, leaves);
            }
        }

        static void VisitNode(JArray arr, List<Leaf> leaves)
        {
            foreach (var child in arr)
            {
                VisitNode(child, leaves);
            }
        }

        static void VisitNode(JValue val, List<Leaf> leaves)
        {
            if (val.Value is string s)
            {
                Console.WriteLine(s);
            }
        }
    }
}

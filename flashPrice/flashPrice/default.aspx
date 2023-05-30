<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="flashPrice.default1" UICulture="id-ID" Culture="id-ID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #graph {
            width: 400px;
            height: 400px;
            border: 1px solid #ccc;
            margin-bottom: 10px;
        }

        #result {
            font-size: 16px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPh" runat="server">

    <div id="graph"></div>

    <div>
        <asp:Literal runat="server" ID="ResultLiteral"></asp:Literal>
    </div>

    <script>
        function drawGraph() {
            var graph = document.getElementById("graph");
            var ctx = graph.getContext("2d");

            var nodes = [
                { name: "Minimarket A", x: 50, y: 50 },
                { name: "Minimarket B", x: 200, y: 50 },
                { name: "Minimarket C", x: 200, y: 200 },
                { name: "Minimarket D", x: 50, y: 200 },
                { name: "Start Minimarket", x: 125, y: 125 }
            ];

            var edges = [
                { from: "Minimarket A", to: "Minimarket B" },
                { from: "Minimarket A", to: "Minimarket C" },
                { from: "Minimarket B", to: "Minimarket D" },
                { from: "Minimarket C", to: "Minimarket D" },
                { from: "Minimarket D", to: "Start Minimarket" }
            ];

            ctx.font = "14px Arial";
            ctx.fillStyle = "#000";

            nodes.forEach(function (node) {
                ctx.beginPath();
                ctx.arc(node.x, node.y, 15, 0, 2 * Math.PI);
                ctx.stroke();
                ctx.fillText(node.name, node.x - 30, node.y + 30);
            });

            edges.forEach(function (edge) {
                var fromNode = nodes.find(function (node) { return node.name === edge.from; });
                var toNode = nodes.find(function (node) { return node.name === edge.to; });

                ctx.beginPath();
                ctx.moveTo(fromNode.x, fromNode.y);
                ctx.lineTo(toNode.x, toNode.y);
                ctx.stroke();
            });
        }

        window.onload = function () {
            drawGraph();
        };
    </script>


</asp:Content>

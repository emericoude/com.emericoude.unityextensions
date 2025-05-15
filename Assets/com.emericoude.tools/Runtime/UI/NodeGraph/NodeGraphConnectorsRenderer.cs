using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emericoude.UI.NodeGraph
{
    //TODO: add an option to draw connectors as "pass-through" from a spline. Basically, draw a spline for each 'route'
    //and feed that manually
    [RequireComponent(typeof(NodeGraph))]
    public class NodeGraphConnectorsRenderer : MonoBehaviour
    {
        [SerializeField] private NodeGraph m_NodeGraph;
        [SerializeField] private Transform m_ConnectorsParent;
        [SerializeField] private UILineRendererController m_ConnectorPrefab;
        [SerializeField] private bool runOnStart = true;

        private List<UILineRendererController> connectors = new List<UILineRendererController>();

        private void Reset()
        {
            this.m_NodeGraph = this.GetComponent<NodeGraph>();
            this.m_ConnectorsParent = this.transform;
        }

        private void Start()
        {
            if (!this.runOnStart) return;
            this.RedrawConnectors();
        }

        public void RedrawConnectors()
        {
            this.ClearConnectors();
            var connectionGroups = m_NodeGraph.ConnectionsOut;
            foreach (var fromToGroup in connectionGroups)
            {
                var from = fromToGroup.Key;
                foreach (var to in fromToGroup.Value) {
                    var connector = Instantiate(this.m_ConnectorPrefab, this.m_ConnectorsParent);
                    connector.gameObject.name = $"Connector ({from.gameObject.name} to {to.gameObject.name})";
                    connector.FromRect = (RectTransform)from.transform;
                    connector.ToRect = (RectTransform)to.transform;
                    connector.RedrawPoints();
                    this.connectors.Add(connector);
                }
            }
        }

        public void ClearConnectors() {
            for (int i = this.connectors.Count - 1; i >= 0; i--)
            {
                Destroy(this.connectors[i]);
            }
        }
    }
}
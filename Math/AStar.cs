/*
 * 2018 Stompy Blondie Games
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace StompyBlondie.Math
{
    public class AStarNode
    {
        public Object value;
        public AStarNode previous;
        public float cost;
        public float pathCost;

        public override string ToString()
        {
            return $"value: '{value}', cost: {cost}, pathCost: {pathCost}, previous: {previous}";
        }
    }

    /*
     * Generic AStar implementation
     * Designed to be subclassed to provide specific node expansion and costing.
     *
     * CostNode(AStarNode node) and List<AStarNode> Expand(AStarNode node must be overridden.
     *
     * To perform searches the Search() method is passed a start node and an end node. It returns
     * null if a path is impossible otherwise a list of nodes.
     * Node values can be any object.
     *
     * You can optionally override Search(Object start, Object end) and IsEndNode(AStarNode node) but
     * the default implementations are functional.
     */
    public class AStar
    {
        protected AStarNode start;
        protected AStarNode end;
        private Dictionary<Object, AStarNode> openNodes;
        private Dictionary<Object, AStarNode> closedNodes;
        private List<AStarNode> foundPath;

        /*
         * Perform a search from the first node value to the second node value.
         * Values can be any object.
         *
         * @param System.Object start: The node to start the search at.
         * @param System.Object end: The desired end node to path to.
         * @return List<AStarNode>: A List of nodes representing the nodes that should be traversed to get to the end.
         *   Returns null if the path is impossible.
         */
        public virtual List<AStarNode> Search(Object start, Object end)
        {
            openNodes = new Dictionary<object, AStarNode>();
            closedNodes = new Dictionary<object, AStarNode>();
            foundPath = new List<AStarNode>();
            this.start = new AStarNode {value = start};
            this.end = new AStarNode {value = end};

            InitSearch();
            DoSearch();
            return foundPath.Count > 0 ? foundPath : null;
        }

        /*
         Should be overidden by subclasses to return the cost of a node, indicating to the algorithm how preferable the
         choice of move is.

         @param AStarNode node: The node to cost up.
         @return (float cost, float pathCost)?: A tuple containing cost and pathCost values respectively,
           or null if the node should not be considered at all (an impossible move).
           pathCost is the total cost to move along the path from the start to this node, and cost is pathCost plus an
           estimate of the distance to the goal. The pathCost can be found by adding the movement cost of this
           node to node.previous.pathCost, if available.
         */
        public virtual (float cost, float pathCost)? CostNode(AStarNode node)
        {
            return (cost: 0f, pathCost: node.previous?.pathCost ?? 0f);
        }

        /*
         Should be overidden by subclasses to return a List of nodes branching from the given node (whether already
         visited or not).

         @param AStarNode node: The node to branch from.
         @return List<AStarNode>: All nodes that can be traversed from this node, with their members set appropriately.
           The cost method can be used to calculate each node's pathCost and cost values.
           Note that cost will return null if a node represents an impossible move which should not be included in the
           branching nodes.
           */
        public virtual List<AStarNode> Expand(AStarNode node)
        {
            return new List<AStarNode>();
        }

        public virtual bool IsEndNode(AStarNode node)
        {
            return node.value == end.value;
        }

        private void InitSearch()
        {
            // Cost the initial node
            var startCosts = CostNode(start);
            if(startCosts == null)
                return;
            start.cost = startCosts.Value.cost;
            start.pathCost = startCosts.Value.pathCost;
            openNodes[start.value] = start;
        }

        private void DoSearch()
        {
            while(openNodes.Count > 0)
                ExpandNextNode();
        }

        private void ExpandNextNode()
        {
            // Get open node with lowest cost
            var sortedNodes = openNodes.OrderBy(v => v.Value.cost).ToList();
            var bestNode = sortedNodes[0].Value;

            // If this is the end then complete the path
            if(IsEndNode(bestNode))
            {
                foundPath = CompletePath(bestNode);
                openNodes.Clear();
                return;
            }

            // Close this node so it wont be considered again
            openNodes.Remove(bestNode.value);
            closedNodes[bestNode.value] = bestNode;

            // Expand this node and go through branches
            foreach(var nextNode in Expand(bestNode))
            {
                // Ignore if already expanded
                if(closedNodes.ContainsKey(nextNode.value))
                    continue;

                // Ignore if already open with a better path cost
                if(openNodes.ContainsKey(nextNode.value) && openNodes[nextNode.value].pathCost <= nextNode.pathCost)
                    continue;

                // To be considered so add to the open set
                openNodes[nextNode.value] = nextNode;
            }
        }

        private List<AStarNode> CompletePath(AStarNode endNode)
        {
            var path = new List<AStarNode>{endNode};
            var next = endNode.previous;
            while(next != null)
            {
                path.Insert(0, next);
                next = next.previous;
            }
            return path;
        }

    }
}
/*
 * 2018 Stompy Blondie Games
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using StompyBlondie.Common.Types;

namespace StompyBlondie.AI
{
    public struct NavigationPointLink
    {
        public Pos linkTo;
        public float costMultiplier;
    }

    public struct NavigationPoint
    {
        public Pos position;
        public List<NavigationPointLink> links;

        public void AddLink(Pos linkTo, float costMultiplier)
        {
            if(links.FindIndex(v => v.linkTo == linkTo) > -1)
                return;
            links.Add(new NavigationPointLink{
                linkTo = linkTo,
                costMultiplier = costMultiplier
            });
        }

        public void BreakLink(Pos linkToBreak)
        {
            var i = links.FindIndex(v => v.linkTo == linkToBreak);
            if(i == -1)
                return;
            links.RemoveAt(i);
        }
    }

    /*
     * Representation of a map of points and the links between them, for use in AStar.
     * Must be manually created by using the AddPoint and AddPointLink methods.
     */
    public class NavigationMap
    {
        public Dictionary<Pos, NavigationPoint> points = new Dictionary<Pos, NavigationPoint>();

        public (float minX,float minY, float minLayer, float maxX,float maxY, float maxLayer) Bounds => (
            points.Keys.Min(p => p.X),
            points.Keys.Min(p => p.Y),
            points.Keys.Min(p => p.Layer),
            points.Keys.Max(p => p.X),
            points.Keys.Max(p => p.Y),
            points.Keys.Max(p => p.Layer)
        );

        public NavigationMap()
        {
        }

        public NavigationMap(NavigationMap cloneFrom)
        {
            foreach(var point in cloneFrom.points)
            {
                var newPoint = new NavigationPoint{
                    position = point.Key,
                    links = new List<NavigationPointLink>(point.Value.links)
                };
                points[point.Key] = newPoint;
            }
        }

        public void Reset()
        {
            points = new Dictionary<Pos, NavigationPoint>();
        }

        /*
         * @returns: true if the point at position exists
         */
        public bool HasPoint(Pos position)
        {
            return points.ContainsKey(position);
        }

        /*
         * @returns: true if the point has been added, false if it already exists.
         */
        public bool AddPoint(Pos position)
        {
            if(HasPoint(position))
                return false;
            points[position] = new NavigationPoint{position = position, links = new List<NavigationPointLink>()};
            return true;
        }

        /*
         * Ensures the passed points have bidirectional links.
         * @return: true if the link was successful, false if either of the points don't exist
         */
        public bool AddPointLink(Pos pointA, Pos pointB, float costMultiplier = 1f)
        {
            if(!HasPoint(pointA) || !HasPoint(pointB))
                return false;
            points[pointA].AddLink(pointB, costMultiplier);
            points[pointB].AddLink(pointA, costMultiplier);
            return true;
        }

        /*
         * Removes the point passed and any links to and from it are broken.
         */
        public void RemovePoint(Pos position)
        {
            points.Remove(position);
            foreach(var p in points.Values)
                p.BreakLink(position);
        }

        /*
         * Breaks any link between two passed points
         */
        public void BreakPointLink(Pos pointA, Pos pointB)
        {
            if(HasPoint(pointA))
                points[pointA].BreakLink(pointB);
            if(HasPoint(pointB))
                points[pointB].BreakLink(pointA);
        }

        /*
         * Check determination of point distance
         */
        public float DistanceBetweenPoints(Pos pointA, Pos pointB)
        {
            return (float)System.Math.Sqrt(System.Math.Pow((pointB.X - pointA.X), 2) + System.Math.Pow((pointB.Y - pointA.Y), 2));
        }

        /*
         * Adds a navigation map on top of this navigation map
         */
        public bool SuperimposeNavigationMap(NavigationMap navMap, Pos position, EightDirection direction)
        {
            // TODO: Implement
            // ....
            return true;
        }

    }

    public class TilemapNavigation
    {

    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CyclicalSkipList
{
    public static class SkiplistUtilities
    {
        public static INode<T> Bottom<T>(this INode<T> node)
        {
            return EnumerateLevels(node).Last();
        }

        public static int DistanceTo<T>(this INode<T> start, INode<T> end)
        {
            var distance = 0;

            var currentNode = start;
            while (currentNode != end)
            {
                distance++;
                currentNode = currentNode.Right;
            }

            return distance;
        }

        public static INode<T> RightBy<T>(this INode<T> start, int count)
        {
            var currentNode = start;
            while (count > 0)
            {
                currentNode = currentNode.Right;
                count--;
            }

            return currentNode;
        }

        public static INode<T> Left<T>(this INode<T> start)
        {
            var currentNode = start;
            while (currentNode.Right != start)
            {
                currentNode = currentNode.Right;
            }

            return currentNode;
        }

        public static int DistanceToSelf<T>(this INode<T> node)
        {
            return node.Right.DistanceTo(node) + 1;
        }

        public static void InsertToRight<T>(this INode<T> node, INode<T> nodeToBeInserted)
        {
            nodeToBeInserted.Right = node.Right;
            node.Right = nodeToBeInserted;
        }

        public static int SizeOfGapTo<T>(this INode<T> start, INode<T> end)
        {
            return start.Down.Right.DistanceTo(end.Down) + 1;
        }

        public static int SizeOfBaseGapTo<T>(this INode<T> start, INode<T> end)
        {
            return start.Bottom().Right.DistanceTo(end.Bottom()) + 1;
        }

        public static IEnumerable<T> EnumerateKeysInLevel<T>(INode<T> start)
        {
            var currentNode = start;
            do
            {
                yield return currentNode.Key;
                currentNode = currentNode.Right;
            } 
            while (currentNode != start);
        }

        public static IEnumerable<INode<T>> EnumerateNodesInLevel<T>(INode<T> start)
        {
            var currentNode = start;
            do
            {
                yield return currentNode;
                currentNode = currentNode.Right;
            } 
            while (currentNode != start);
        }

        public static IEnumerable<INode<T>> EnumerateLevels<T>(INode<T> start)
        {
            var currentNode = start;
            do
            {
                yield return currentNode;
                currentNode = currentNode.Down;
            }
            while (currentNode != null);
        }
    }
}
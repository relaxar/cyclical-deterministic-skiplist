﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CyclicalSkipList
{
    public static class SkiplistStringFormatter
    {
        public static string ConvertToString<T>(Skiplist<T> skiplist)
        {
            var format = CreateFormatFor(skiplist);
            var levels = skiplist.EnumerateLevels();

            var levelStrings = levels.Select(level => FormatLevel(level, format) + "\n");

            return String.Concat(levelStrings).Replace(" ", "-");
        }

        private static string CreateFormatFor<T>(Skiplist<T> skiplist)
        {
            var bottomNodes = skiplist.Head.Bottom().EnumerateRightwards().ToList();

            var numberOfColumns = bottomNodes.Count();
            var columnWidth = bottomNodes.Max(node => node.Key.ToString().Length) + 1;

            var nodeFormat = Enumerable.Range(0, numberOfColumns).Select(i => "{" + i + ", -" + (columnWidth) + "}").ToArray();
            var levelFormat = string.Concat(nodeFormat);

            return levelFormat;
        }

        private static string FormatLevel<T>(INode<T> head, string format)
        {
            var nodes = head.EnumerateRightwards().ToList();
            var keys = nodes.ToDictionary(node => node, node => node.Key);
            var spacings = nodes.ToDictionary(node => node, node => node.Bottom().DistanceTo(node.Right.Bottom()) - 1);

            var spacedKeys = new List<string>();
            foreach (var node in nodes)
            {
                var key = keys[node].ToString();
                spacedKeys.Add(key);

                var spacing = Enumerable.Repeat(" ", spacings[node]);
                spacedKeys.AddRange(spacing);
            }

            var stringForThisLevel = String.Format(format, spacedKeys.ToArray());

            return stringForThisLevel;
        }
    }
}

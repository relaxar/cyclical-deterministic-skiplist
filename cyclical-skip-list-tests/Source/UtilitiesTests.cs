﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CyclicalSkipList;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;
using Xunit.Sdk;

namespace CyclicalSkipListTests
{
    public class UtilitiesTests
    {
        private const int LowerLengthBound = 3;
        private const int UpperLengthBound = 10;

        [Theory]
        [AutoIsolatedNodeData(LowerLengthBound, UpperLengthBound)]
        public void BottomOf_GivenAVerticalLinkedList_ReturnsTheBottomNode
            (List<INode<int>> nodes)
        {
            // Fixture setup
            if (nodes.Count > 1)
            {
                for (int i = 1; i < nodes.Count; i++)
                {
                    nodes[i].Down = nodes[i - 1];
                }
            }

            var listHead = nodes.Last();

            // Exercise system
            var result = Utilities.BottomOf(listHead);

            // Verify outcome
            Assert.Equal(nodes[0], result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void DistanceTo_GivenAHorizontalLinkedList_ReturnsTheDistanceBetweenTheSpecifiedNodes
            (List<INode<int>> nodes, INode<int> listHead)
        {
            // Fixture setup

            // Exercise system
            var result0 = listHead.DistanceTo(nodes[0]);
            var result1 = listHead.DistanceTo(nodes[1 % nodes.Count]);
            var result2 = listHead.DistanceTo(nodes[nodes.Count - 1]);

            // Verify outcome
            Assert.Equal(0 % nodes.Count, result0);
            Assert.Equal(1 % nodes.Count, result1);
            Assert.Equal(nodes.Count - 1, result2);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void DistanceToSelf_GivenAHorizontalLinkedList_ReturnsTheLengthOfTheList
            (List<INode<int>> nodes, INode<int> listHead)
        {
            // Fixture setup

            // Exercise system
            var result = listHead.DistanceToSelf();

            // Verify outcome
            Assert.Equal(nodes.Count, result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void MoveRightBy_GivenAHorizontalLinkedList_ReturnsANodeTheCorrectDistanceToTheRightOfSpecifiedNode
            (List<INode<int>> nodes, INode<int> listHead)
        {
            // Fixture setup

            // Exercise system
            var result0 = listHead.RightBy(0);
            var result1 = listHead.RightBy(2);
            var result2 = listHead.RightBy(nodes.Count + 1);

            // Verify outcome
            Assert.Equal(nodes[0 % nodes.Count], result0);
            Assert.Equal(nodes[2 % nodes.Count], result1);
            Assert.Equal(nodes[1 % nodes.Count], result2);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void Left_OnAHorizontalLinkedList_ReturnsTheNodeLeftOfTheSpecifiedNode
            (List<INode<int>> nodes, INode<int> sutHead)
        {
            // Fixture setup

            // Exercise system
            var result0 = nodes[0 % nodes.Count].Left();
            var result1 = nodes[2 % nodes.Count].Left();
            var result2 = nodes[nodes.Count - 1].Left();

            // Verify outcome
            Assert.Equal(nodes[nodes.Count - 1], result0);
            Assert.Equal(nodes[1 % nodes.Count], result1);
            Assert.Equal(nodes[(nodes.Count - 2) % nodes.Count], result2);

            // Teardown
        }

        [Theory]
        [AutoIsolatedNodeData]
        public void InsertToRight_WhenTwoNodesAreLinked_InsertsAnotherNodeInBetweenThem
            (INode<int> firstNode, INode<int> lastNode, INode<int> nodeToBeInserted)
        {
            // Fixture setup
            firstNode.Right = lastNode;

            // Exercise system
            firstNode.InsertToRight(nodeToBeInserted);

            // Verify outcome
            Assert.Equal(nodeToBeInserted, firstNode.Right);
            Assert.Equal(lastNode, nodeToBeInserted.Right);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void SizeOfGapTo_WhenStartAndEndHaveNodesBelowThem_ShouldReturnNumberOfInterveningNodes
            (List<INode<int>> nodes)
        {
	        // Fixture setup
            var start = Substitute.For<INode<int>>();
            start.Down = nodes.First();

            var end = Substitute.For<INode<int>>();
            end.Down = nodes.Last();

	        // Exercise system
            var result = start.SizeOfGapTo(end);

            // Verify outcome
            Assert.Equal(nodes.Count-1, result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void SizeOfBaseGapTo_WhenStartAndEndHaveNodesBelowThem_ShouldReturnNumberOfInterveningNodesAtBottomLevel
            (List<INode<int>> nodes)
        {
            // Fixture setup
            var start = Substitute.For<INode<int>>();
            var midlevelStart = Substitute.For<INode<int>>();
            start.Down = midlevelStart;
            midlevelStart.Down = nodes.First();

            var end = Substitute.For<INode<int>>();
            var midlevelEnd = Substitute.For<INode<int>>();
            end.Down = midlevelEnd;
            midlevelEnd.Down = nodes.Last();

            // Exercise system
            var result = start.SizeOfBaseGapTo(end);

            // Verify outcome
            Assert.Equal(nodes.Count - 1, result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void EnumerateKeysInLevel_GivenALinkedList_ShouldEnumerateKeysInThatLevel
            (List<INode<int>> nodes, INode<int> listHead)
        {
            // Fixture setup

            // Exercise system
            var result = Utilities.EnumerateKeysInLevel(listHead);

            // Verify outcome
            var expectedResult = nodes.Select(node => node.Key);

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [AutoINodeLinkedListData(LowerLengthBound, UpperLengthBound)]
        public void EnumerateNodesInLevel_GivenALinkedList_ShouldEnumerateNodesInThatLevel
            (List<INode<int>> nodes, INode<int> listHead)
        {
            // Fixture setup

            // Exercise system
            var result = Utilities.EnumerateNodesInLevel(listHead);

            // Verify outcome
            var expectedResult = nodes;

            Assert.Equal(expectedResult, result);

            // Teardown
        }

        [Theory]
        [AutoIsolatedNodeData(LowerLengthBound, UpperLengthBound)]
        public void EnumerateLevels_GivenAVerticalLinkedList_EnumeratesEachNodeBelowTheNodeSpecified
            (List<INode<int>> nodes)
        {
            // Fixture setup
            if (nodes.Count > 1)
            {
                for (int i = 1; i < nodes.Count; i++)
                {
                    nodes[i].Down = nodes[i - 1];
                }
            }

            var listHead = nodes.Last();
            
            // Exercise system
            var result = Utilities.EnumerateLevels(listHead).ToList();

            // Verify outcome
            result.Reverse();
            Assert.Equal(nodes, result);

            // Teardown
        }
    }
}

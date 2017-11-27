using System;
using System.Collections;
//using UnityEngine;

public class NodePriorityQueue
{

	private Node[] heap;
	private int count;
	private Hashtable heapContains;

	public NodePriorityQueue ()
	{
		heap = new Node[1];
		heapContains = new Hashtable();
		count = 0;
	}


	public void expandHeap(int size)
	{
		Node[] newHeap = new Node[size];
		heap.CopyTo (newHeap, 0);
		heap = newHeap;
	}

	public int getCount()
	{
		return count;
	}

	public void push(Node nr)
	{
		if (heap.Length == count) {
			expandHeap (count * 2);
		}

		heap [count] = nr;

		//parent index
		int pi = count;

		while (pi != 0)
		{
			//current node index
			int ni = pi;
			//calculate parent index
			pi = (pi % 2 == 0) ? (pi / 2 - 1) : ((pi + 1) / 2 - 1);

			//swap if needed
			if (heap [pi].estimatedTotalCost () > heap [ni].estimatedTotalCost ()) {
				Node temp = heap [ni];
				heap [ni] = heap [pi];
				heap [pi] = temp;
			} else {
				break;
			}

		}
			
		count++;

		heapContains[nr.strname] = nr;
	}

	public Node pop()
	{
		return pop(0);
	}

	public Node pop(int index)
	{
		Node root = heap [index];

		int curr = index;
		heap [index] = heap [count - 1];
		count--;

		while (true) {
			//if left child exists
			if (curr * 2 + 1 <= count) {
				//if right child exists
				if (curr * 2 + 2 <= count) {
					//if the left child is higher priority than the right child
					if (heap [curr * 2 + 1].estimatedTotalCost () < heap [curr * 2 + 2].estimatedTotalCost ()) {
						if (heap [curr * 2 + 1].estimatedTotalCost () < heap [curr].estimatedTotalCost ()) {
							//swap curr and curr * 2 + 1
							Node temp = heap [curr];
							heap [curr] = heap [curr * 2 + 1];
							heap [curr * 2 + 1] = temp;
							curr = curr * 2 + 1;
						} else {
							//no more swapping needed
							break;
						}
					} else {
						if (heap [curr * 2 + 2].estimatedTotalCost () < heap [curr].estimatedTotalCost ()) {
							//swap curr and curr * 2 + 2
							Node temp = heap [curr];
							heap [curr] = heap [curr * 2 + 2];
							heap [curr * 2 + 2] = temp;
							curr = curr * 2 + 2;
						} else {
							//no more swapping needed
							break;
						}
					}


				} else {

					if (heap [curr * 2 + 1].estimatedTotalCost () < heap [curr].estimatedTotalCost ())
					{
						//swap curr and curr * 2 + 1
						Node temp = heap[curr];
						heap [curr] = heap [curr * 2 + 1];
						heap [curr * 2 + 1] = temp;
						curr = curr * 2 + 1;

					}
					//we've reached the bottom
					break;
				}
			} else {
				//we've reached the bottom
				break;
			}
		}

		heapContains.Remove(root.strname);

		return root;

	}

	public bool hasNode(string namestr)
	{
		return heapContains.Contains(namestr);
	}

	//O(n)...
	public int findNode(string namestr)
	{
		if (heapContains.Contains(namestr))
		{
			for (int i = 0; i < count; i++)
			{
				if (heap[i].strname.Equals(namestr))
				{
					return i;
				}
			}
		}

		return -1;
	}

	public Node getNodeAtIndex(int i)
	{
		return heap[i];
	}


//		public void debugPrintHeap()
//		{
//			String s = "";
//			for (int i = 0; i < count; i++) {
//				s += heap[i].nodeName + ":" + heap[i].estimatedTotalCost() + " ";
//
//			}
//
//			Debug.Log (s);
//		}


}


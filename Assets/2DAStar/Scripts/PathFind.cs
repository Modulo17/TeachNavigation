using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/* Simple A* implementation using an array
 * Code Richard Leinfellner richardl@uel.ac.uk
 * Check https://www.raywenderlich.com/4946/introduction-to-a-pathfinding
 * and https://en.wikipedia.org/wiki/A*_search_algorithm for some background
 * The algorithm assumes integer positions to simplify the search set
 * Any non null array entries are assumed blocked
 * Diagonals are counted as 1.4 straights
 * Uses 3 nested classes to simplify variable handling
 * One of these; Waypoint is used to pass a list containing the path back to the caller
 */

public class PathFind : MonoBehaviour{


	/* Neighbour Helper class to specify costs of getting to a neighbouring cell
	 * X & Y are relatives offsets to current cell
	 * Cost is the relative cost of getting there
	 * 10 is a good cost for a straight line and 14 for a diagonal
	*/
	public	class Neighbour {
		private	int	mX;		//Once set there are read only, so protection reflects this
		private	int	mY;
		private	int	mCost;

		public	int	X { get { return mX; } }		//Allow Read only access
		public	int	Y { get { return mY; } }
		public	int	Cost { get { return mCost; } }	//Relative cost for getting to this cell

		public	Neighbour(int vX,int vY, int vCost) {
			mX=vX;
			mY=vY;
			mCost=vCost;
		}
	}


	/* Node Helper class which represents a potential interim destination
	 * As much as possible this class handles all the node specific functionality
	 * 
	 * 
	 * 
	 * 
	 * 
	*/
	public	class Node {
		private Node mParent;		//Parent for this cell, this is used later to constuct the path in reverse
        private int mX;				//Read only X position of this Node
		private int mY;				//Read only X position of this Node

		public int X { get { return mX; } }	//Allows access to position variables
		public int Y { get { return mY; } }
		public	Node	Parent { get { return mParent; } } //Used to build up path from destination to start position

		private int mG;		//Relative cost to get to this node
		private int mH;		//Predicted cost to destination
		private int mF;		//Total cost

		public	int	H { get { return mH; } set { mH = value; mF = mG + mH;} }	//Allow access and also update total cost if any component changes
		public	int	G { get { return mG; } set { mG = value; mF = mG + mH;} }
		public	int	F { get { return mF; } }			//Read only all calculations are done inside this class when needed


		public	bool	SamePosition(Node vOther) {		//Helper function to decide if 2 nodes are in same position
			return	(mX==vOther.mX && mY==vOther.mY);	//In our case this means comparing both X & Y coordinates, safer then doing this in code in multiple places
		}

		public	bool	IsCloser(Node vOtherNode) {	//Helper function to decide if we are closer than another node
			return	mF<vOtherNode.mF;
		}

		public	Node(Waypoint vWP, Node vParent=null) {		//Set up a new node and optionally parent it
			mX=vWP.X;
			mY=vWP.Y;
			mParent=vParent;
		}

		public	Node(int vX,int vY, Node vParent=null) {		//Set up a new node and optionally parent it
			mX=vX;
			mY=vY;
			mParent=vParent;
		}
		public	Node	NeighbourNode(Neighbour vNeighbour) {		//Make a new node out of a Neighbour cell
			Node	tNeighbour = new Node (mX + vNeighbour.X, mY + vNeighbour.Y, this);		//Parent it to our cell
			tNeighbour.G = mG + vNeighbour.Cost;	//Allocate it a cost based on its location relative to this one
            return tNeighbour;
		}
		public	bool	UpdateIfCloser(Node vOther) {
			if (vOther.mG+mH < mF) {    		 //If other G Score with this H would make the this F lower use it
				mG = vOther.mG;					//Use the other G score
				mF = mG+mH;         			 //Work out new F Score based on new G
				mParent = vOther.mParent;			//Re-parent to new other one (new one)
				return	true;		//Means we updated
			}
			return	false;
		}
		/* Uses the Manhatten / taxicab technique see https://en.wikipedia.org/wiki/Taxicab_geometry and http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html 
		 * This method tries to approximate the distance to the target following a permissible path (which may not be the best path, this is fine as long as its consistent for all targets)
		 * By knowing H from different location we can pick the best guess for the optimal path
		 * 
		*/

		public	int    CalculateH(Node vDestination)     {		//This method approximates the cost to the destination 1 square at the time
			int tCount = 0;
			int tX = X;		//Keep temp copy as this changes as we walk closer to destination
			int tY = Y;
			do {
				if (tX != vDestination.X) {
					int tDX = Math.Sign(vDestination.X - tX);		//NOTE use Math.Sign() (.NET) not MathF.Sign() (Unity) as Unity does not handle 0 correctly, returning 1 for 0
					tX += tDX;
					tCount+=10;										//We don't allow diagonals for this count
				}
				if (tY != vDestination.Y) {
					int tDY = Math.Sign(vDestination.Y - tY);
					tY += tDY;
					tCount+=10;
				}
			} while (tX != vDestination.X || tY != vDestination.Y);
			return	tCount;		//Cost of path taken
		}
	}

	public	class Waypoint{		//This helper class is used to set up initial From-To and also to pass the path back to the caller
		private	int mX;			//It's all read only once set up
		private int	mY;
		public	int	X { get { return mX; } }		//Ensure Read Only
		public	int	Y {	get { return mY; } }

		public	Waypoint(Node vNode) {		//Used to initialise with a Node
			mX=vNode.X;
			mY=vNode.Y;
		}
		public	Waypoint(int vX, int vY) {	//Used to initialise with X & Y
			mX=vX;
			mY=vY;
		}
	}


	/* Now into the main pathfinding code
	 * The helper functions are used to make this code more readable and hide away (encapsulate) variables which may have side effects
	 * A* has lots of scope for optimisation, especially on the frequent list searches, to keep the code readable this has not been implemented
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	*/

	private	static	List<Neighbour>	mNeighbours;		//This is a list of possible neighbours, for speed its only set up the first time this class is used, as it does not change
	private	List<Node>	mOpen;							//Current list of open paths, ie ones we have not decided on yet
	private	List<Node>	mClosed;						//Current list of closed paths, ie ones we consider candidates
	private GameObject[,]		mMap;					//Map of game objects, this is used to decided what is navigateable
	private Node        mDestination;					//Where we are heading
	private	int			mWidth;							//Width of array
	private	int			mHeight;						//Height of array
	private	bool		mDebug;							//Should we show search path

	/* This will find a path from To - From if it exists
	 * If it succeeds it will return a list of Waypoints, tile by tile, which if followed will lead to the destination 
	 * If it fails the list will be empty
	 * Any non null entry in the array will block the movement
	 * if Debug is true , this will draw DebugDots on the path
	*/

	public	List<Waypoint>    Find(Waypoint vFrom,Waypoint vTo,GameObject[,] vMap, bool vDebug=false) {
		mOpen = new List<Node> ();		//Keep a list of open paths to explore
		mClosed = new List<Node> ();	//List of closed paths, I.E. ones which have been explored
		mMap = vMap;					//Keep a pointer to the map of blocking tiles
        mDestination = new Node(vTo);	//Make a destination node, so we can check we have arrived
		mWidth = mMap.GetLength (0);	//Cache size of map to make bounds check faster
		mHeight = mMap.GetLength (1);
		mNeighbours = SetNeighbours ();	//Cache list of relative neighbours, only happens first time as its static
        Node tCurrent = new Node(vFrom);		//Add starting node to the Open list
        mOpen.Add(tCurrent);        //Add starting point
		mDebug=vDebug;				//Should we draw debug circles
		if(mDebug) {
			DebugHelper.DebugDot(vFrom.X,vFrom.Y,Color.yellow,2.0f);		//Show starting position
			DebugHelper.DebugDot(vTo.X,vTo.Y,Color.cyan,2.0f);				//Show finish position
		}
        return  CalculatePath();					//Run A*
    }
		
    private  Node    FindShortOpen() {		//Find the path with the lowest F score, if any
        Node tShort = mOpen.First();
        foreach(Node tCurrent in mOpen) {
			if (tCurrent.IsCloser(tShort))
                tShort = tCurrent;
        }
        return tShort;
    }

    public Node IsInClosed(Node vNode) {		//Find this Node if its in the Closed List
		return  mClosed.Find(tN => tN.SamePosition(vNode));
    }

	public Node IsInOpen(Node vNode) {	//Find this Node if its in the Open List
		return mOpen.Find(tN => tN.SamePosition(vNode));
    }

    private List<Neighbour>	SetNeighbours() {		//Makes a list of Neighbour squares with relative offsets & costs, uses a static so this only makes one list for all searches
		if (mNeighbours!=null)
			return	mNeighbours;
		List<Neighbour> tNeighbours= new List<Neighbour> ();
		tNeighbours.Add (new Neighbour (-1, 0, 10));
		tNeighbours.Add (new Neighbour (0, 1, 10));
		tNeighbours.Add (new Neighbour (1, 0, 10));
		tNeighbours.Add (new Neighbour (0, -1, 10));
		tNeighbours.Add (new Neighbour (-1, 1, 14));
		tNeighbours.Add (new Neighbour (1, 1, 14));
		tNeighbours.Add (new Neighbour (1, -1, 14));
		tNeighbours.Add (new Neighbour (-1, -1, 14));
		return	tNeighbours;
	}

	bool	IsOnMap(int vX,int vY) {		//Is this a valid Map Position?
		return	(vX>=0 && vX < mWidth && vY>=0 && vY < mHeight); 
	}

    bool IsOnTarget(Node tNode) {			//Is this node the target?
		return (tNode.SamePosition(mDestination));
    }

	bool	IsAccessible(Node vNode) {		//Is this node walkable, in this case any GameObject will block
		if (IsOnMap (vNode.X, vNode.Y)) {
			return	(mMap[vNode.X,vNode.Y]==null);
		}
		return	false;
	}

    public  List<Waypoint>    CalculatePath() {		//Main A* routine, makes a path to the target
        bool    Quit = false;						//Used to exit infinite loop when debugging, set to true to stop Unity locking up
        List<Waypoint> tPath = new List<Waypoint>();	//List for storing found path
        do {
            Node tShortNode = FindShortOpen();      //Find current open path with shortest F
            mOpen.Remove(tShortNode);               //Move it to closed list
            mClosed.Add(tShortNode);				//
			if(mDebug) {			//Draw a Dot to show current path progress
				DebugHelper.DebugDot(tShortNode.X,tShortNode.Y,Color.gray,1.0f);
			}
            Node tAtDestination = IsInClosed(mDestination);		//Check if we have reached the destination
			if(tAtDestination!=null) {		//If the destination is in closed list we are there
                Node tNext;
                do {						//Construct list of waypoints for caller
					tNext = tAtDestination.Parent;		//Follow links from destination to start (reverse order)
                    if(tNext!= null) {			//We dont need to include start position and when we have reached it parent will be null and list is complete
						Waypoint tWayPoint=new Waypoint(tAtDestination);		//Add waypoint from current Destination as we walk the list back to the origin
						if(mDebug) {		//Show Path if debugging
							DebugHelper.DebugDot(tWayPoint.X,tWayPoint.Y,Color.red,2.0f);
						}
						tPath.Insert(0,tWayPoint);	//Add waypoint, by adding at start of list we are reversing the Node list, which is what we want as its back to front when generated
													//ie. the nodes go from the end point to the start
                        tAtDestination = tNext;		//Following the parent reference will take us back to the start
                    } else {
                        break;		//Done, we can leave the loop
                    }
                } while (true);
				mOpen=null;		//Release resources
				mClosed=null;
				mDestination=null;
                return tPath;		//Send Path to caller
            }
            List<Node> tWalkableNodes = Walkable(tShortNode);		//If we are not there yet, find all walkable squares centered around current position
            foreach(Node tCurrent in tWalkableNodes) { 
                if (IsInClosed(tCurrent) != null) {					//If we have this square in closed list already, we can skip it
                    continue;
                }
                Node tOpen = IsInOpen(tCurrent);			//If its in open list get it
                if (tOpen == null) {
                    mOpen.Add(tCurrent);                    //Add to new open list if its not already there
                } else {
					tOpen.UpdateIfCloser(tCurrent);			//If we have it check if this one would give a better F Score (by having a lower G), if so replace & reparent
                }
            }
        } while (mOpen.Count > 0 && !Quit);			//Keep going while we have open destinations, if there are none, it his means the path does not exist, also allow debugger to exit by setting Quit true, stops Unity Hanging
        return tPath;		//If we get here we are going to return an empty path, as success exit is higher up
    }

    public	List<Node>	Walkable(Node vNode) {		//Make a list of Neighbour nodes, which are on the map and  empty
		List<Node> tWalkableNodes = new List<Node> ();
		foreach (Neighbour tN in mNeighbours) {
            Node tNeighbourNode = vNode.NeighbourNode(tN);
            if (IsAccessible(tNeighbourNode)) {			//Is is valid, ie. can it be walked on
				tNeighbourNode.H = vNode.CalculateH(mDestination);	//Calculate H from this node
                tWalkableNodes.Add(tNeighbourNode);			//Add to list
            }
		}
		return	tWalkableNodes;
	}
}

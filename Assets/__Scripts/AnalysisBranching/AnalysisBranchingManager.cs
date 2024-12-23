using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BarbarO.Utils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi{
	[ScriptTiming(501)]
	public class AnalysisBranchingManager : MonoBehaviour
	{
		public RefAction<AnalysisBranch> OnNewBranchSelected = new RefAction<AnalysisBranch>();
		public List<AnalysisBranch> branches = new List<AnalysisBranch>();
		
		public AnalysisBranch currBranch;

		public Transform branchesContainer;

		public GameObject newBranchPrefab;
		public Button prevBranch_btn;
		public Button nextBranch_btn;
		public Button createNewBranch_btn;
		public Button deleteBranch_btn;

		protected ShogiGame shogiGame;

		void Awake() {
			shogiGame = FindObjectOfType<ShogiGame>();

			Debug.Assert(FindObjectOfType<SceneManager>() != null, "No Scene Manager in the scene. \nSomeone needs to enable AnalysisBranchingManager to make it work. It's supposed to be the SceneManager's job" );
		}

		void Start()
		{
			if(currBranch != null){
				branches.Add( currBranch );
			}

			prevBranch_btn.interactable = false;
	
			EnableBranch( currBranch );
		}

		void OnEnable(){
			prevBranch_btn.onClick.AddListener( GoToPreviousBranching );
			nextBranch_btn.onClick.AddListener( GoToNextBranching );
			createNewBranch_btn.onClick.AddListener( ForkSelectedEntry_ToNewBranch );
			deleteBranch_btn.onClick.AddListener( DeleteCurrentBranch );

			branches.ForEach( b => b.gameObject.SetActive(false));
		}

		void OnDisable(){
			prevBranch_btn.onClick.RemoveListener( GoToPreviousBranching );
			nextBranch_btn.onClick.RemoveListener( GoToNextBranching );
			createNewBranch_btn.onClick.RemoveListener( ForkSelectedEntry_ToNewBranch );
			deleteBranch_btn.onClick.RemoveListener( DeleteCurrentBranch );

			currBranch?.gameObject?.SetActive(true);
		}

		protected virtual AnalysisBranch CreateNewBranch(){
			var newBranchObj = Instantiate( newBranchPrefab );
			var branch = newBranchObj.GetComponent<AnalysisBranch>();
			return branch;
		}

		private AnalysisBranch CloneBranch_UpToMove( AnalysisBranch toCopy, int turnsToCopy ) {
			AnalysisBranch result = CreateNewBranch();

			CopyBranch_UpToMove(toCopy, turnsToCopy, ref result);
			return result;
		}

		private void CopyBranch_UpToMove( AnalysisBranch toCopy, int turnsToCopy, ref AnalysisBranch targetBranch ) {
			var entriesToCarryOver = toCopy.entries.Take( turnsToCopy );
			targetBranch.ClearEntries();
			foreach (var entry in entriesToCarryOver) {
				targetBranch.CreateAndAppend_MoveEntry( entry.associatedMove );
			}
			targetBranch.currentlySelectedEntry = targetBranch.entries.Last();

			GameHistory trimmedGameHistory_copy = toCopy.BranchGameHistory.Clone( turnsToCopy );
			targetBranch.BranchGameHistory = trimmedGameHistory_copy;
		}

		public void EnableBranch( AnalysisBranch branchToEnable ) {
			if (branches.Contains( branchToEnable ) == false) {
				branches.Add( branchToEnable );
				branchToEnable.BranchName = "Branch " + ( branchesContainer.childCount - 1 );
				
				branchToEnable.transform.SetParent(branchesContainer, false);
				branchToEnable.GetComponent<RectTransform>().SetAnchor( AnchorPresets.StretchAll );
				branchToEnable.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
				branchToEnable.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
				
			}
			EnableBranch( branches.IndexOf( branchToEnable ) );
		}

		public void EnableBranch( int index ) {
			var branchToEnable = branches [index];

			currBranch?.gameObject?.SetActive( false );
			branchToEnable.gameObject.SetActive( true );
			// branch.enabled = true;

			//TODO: bug here
			shogiGame.gameHistory = branchToEnable.BranchGameHistory;
			shogiGame.BeginGame( branchToEnable.BranchGameHistory.GetPlayer_WhoMovesNext(), branchToEnable.BranchGameHistory);

			if (currBranch != null) {
				currBranch.OnHeadDetached -= HandleHeadDetached;
			}
			branchToEnable.OnHeadDetached += HandleHeadDetached;

			currBranch = branchToEnable;
			OnNewBranchSelected.Invoke( branchToEnable );
			SetButtonsInteractable( index );


			void SetButtonsInteractable( int index ) {
				if (index == 0) {
					prevBranch_btn.interactable = false;
				}
				if (index == branches.Count - 1) {
					nextBranch_btn.interactable = false;
				}
				if (index - 1 >= 0) {
					prevBranch_btn.interactable = true;
				}
				if (index + 1 <= branches.Count - 1) {
					nextBranch_btn.interactable = true;
				}
			}
		}

		protected virtual void HandleHeadDetached(AnalysisEntry entry){
			Logger.Log( "[Analysis] New Branch" );
			shogiGame.OnBeforeActionExecuted -= ForkSelectedEntry_ToNewBranch;
			shogiGame.OnBeforeActionExecuted += ForkSelectedEntry_ToNewBranch;
		}

		protected void ForkSelectedEntry_ToNewBranch( AShogiAction _ ) {
			ForkSelectedEntry_ToNewBranch();
		}

		protected void ForkSelectedEntry_ToNewBranch(){
			ForkEntry_ToNewBranch(currBranch.currentlySelectedEntry);
		}

		protected void ForkEntry_ToNewBranch(AnalysisEntry entry){
			Logger.Log( "[Analysis] Move detected: Fork" );

			var forkedBranch = CloneBranch_UpToMove(currBranch, entry.moveNumber);

			shogiGame.OnBeforeActionExecuted -= ForkSelectedEntry_ToNewBranch;
			EnableBranch( forkedBranch );
		}

		public void GoToNextBranching(){
			int index = branches.IndexOf( currBranch ) + 1;
			if(index < 0 || index >= branches.Count){
				return;
			}

			EnableBranch( index );
			branches[index].currentlySelectedEntry?.SelectEntry();
		}

		public void GoToPreviousBranching(){
			int index = branches.IndexOf( currBranch ) - 1;
			if (index < 0 || index >= branches.Count) {
				return;
			}

			EnableBranch( index );
			branches [index].currentlySelectedEntry?.SelectEntry();
		}

		public void DeleteCurrentBranch(){
			if(branches.Count == 1){
				Debug.LogError("NO! You can't delete ALL of the branches. That would be genocide. Leave at least 1 alive please");
				return;
			}

			int currBranchIndex = branches.IndexOf( currBranch );
			branches.RemoveAt( currBranchIndex );
			Destroy( currBranch.gameObject );

			EnableBranch( currBranchIndex - 1);
			branches [currBranchIndex - 1].currentlySelectedEntry?.SelectEntry();
		}
	}
}
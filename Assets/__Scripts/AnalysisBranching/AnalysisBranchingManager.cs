using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Shogi{
	public class AnalysisBranchingManager : MonoBehaviour
	{
		public RefAction<AnalysisBranching> OnNewBranchSelected = new RefAction<AnalysisBranching>();
		public List<AnalysisBranching> branches = new List<AnalysisBranching>();
		public AnalysisBranching currBranch;
		private AnalysisBranching detachedHeadBranch;

		public Transform branchesContainer;

		public GameObject newBranchPrefab;
		public Button prevBranch_btn;
		public Button nextBranch_btn;
		public Button createNewBranch_btn;
		public Button deleteBranch_btn;

		private ShogiGame shogiGame;

		void Awake() {
			shogiGame = FindObjectOfType<ShogiGame>();

			detachedHeadBranch = CreateDetachedBranch();
			currBranch.OnHeadDetached += UpdateDetachedBranch;

		}
		private AnalysisBranching CreateDetachedBranch() {
			var newBranchObj = Instantiate( newBranchPrefab, this.transform );
			var branch = newBranchObj.GetComponent<AnalysisBranching>();
			// branch.enabled = false;
			branch.GetComponentInChildren<Canvas>().enabled = false;
			branch.BranchName = "Detached Branch";
			return branch;
		}

		void Start()
		{
			if(currBranch != null){
				branches.Add( currBranch );
			}

			prevBranch_btn.interactable = false;
		}

		void OnEnable(){
			prevBranch_btn.onClick.AddListener( GoToPreviousBranching );
			nextBranch_btn.onClick.AddListener( GoToNextBranching );
			createNewBranch_btn.onClick.AddListener( CreateNewBranch );
			deleteBranch_btn.onClick.AddListener( DeleteCurrentBranch );
		}

		void OnDisable(){
			prevBranch_btn.onClick.RemoveListener( GoToPreviousBranching );
			nextBranch_btn.onClick.RemoveListener( GoToNextBranching );
			createNewBranch_btn.onClick.RemoveListener( CreateNewBranch );
			deleteBranch_btn.onClick.RemoveListener( DeleteCurrentBranch );
		}

		[Button]
		public void CreateNewBranch() {
			var newBranchObj = Instantiate( newBranchPrefab );
			var branch = newBranchObj.GetComponent<AnalysisBranching>();

			CopyCurrBranch_UpToCurrSelectedEntry( ref branch );
			
			EnableBranch( branch );
		}

		private void CopyCurrBranch_UpToCurrSelectedEntry( ref AnalysisBranching targetBranch ) {
			int selectedEntry_turn = currBranch.currentlySelectedEntry.moveNumber;

			var entriesToCarryOver = currBranch.entries.Take( selectedEntry_turn );
			targetBranch.ClearEntries();
			foreach (var entry in entriesToCarryOver) {
				targetBranch.CreateAndAppend_MoveEntry( entry.associatedMove );
			}
			targetBranch.currentlySelectedEntry = targetBranch.entries.Last();

			//Copy game history
			GameHistory trimmedGameHistory = currBranch.branchGameHistory.Clone( selectedEntry_turn );
			targetBranch.branchGameHistory = trimmedGameHistory;
		}

		public void EnableBranch( AnalysisBranching branchToEnable ) {
			if (branches.Contains( branchToEnable ) == false) {
				branches.Add( branchToEnable );
				branchToEnable.transform.SetParent(branchesContainer);
				branchToEnable.BranchName = "Branch " + ( branchesContainer.childCount - 1 );
			}
			EnableBranch( branches.IndexOf( branchToEnable ) );
		}

		public void EnableBranch( int index ) {
			var branchToEnable = branches [index];

			currBranch?.gameObject?.SetActive( false );
			branchToEnable.gameObject.SetActive( true );

			shogiGame.gameHistory = branchToEnable.branchGameHistory;
			shogiGame.BeginGame( branchToEnable.branchGameHistory.GetPlayer_WhoMovesNext() );

			if (currBranch != null) {
				currBranch.OnHeadDetached -= UpdateDetachedBranch;
			}
			branchToEnable.OnHeadDetached += UpdateDetachedBranch;

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

		private void UpdateDetachedBranch(AnalysisEntry entry){
			Debug.Log( "New Branch" );
			CopyCurrBranch_UpToCurrSelectedEntry( ref detachedHeadBranch );
			shogiGame.OnBeforeActionExecuted -= ForkSelectedEntry_ToNewBranch;
			shogiGame.OnBeforeActionExecuted += ForkSelectedEntry_ToNewBranch;
		}

		private void ForkSelectedEntry_ToNewBranch(AShogiAction action){
			Debug.Log("Move detected: Fork");
			detachedHeadBranch.GetComponent<Canvas>().enabled = true;

			shogiGame.OnBeforeActionExecuted -= ForkSelectedEntry_ToNewBranch;
			EnableBranch( detachedHeadBranch );

			detachedHeadBranch = CreateDetachedBranch();
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
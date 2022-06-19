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
		
		protected AnalysisBranch currBranch;
		private AnalysisBranch detachedHeadBranch;

		public Transform branchesContainer;

		public GameObject newBranchPrefab;
		public Button prevBranch_btn;
		public Button nextBranch_btn;
		public Button createNewBranch_btn;
		public Button deleteBranch_btn;

		private ShogiGame shogiGame;

		void Awake() {
			shogiGame = FindObjectOfType<ShogiGame>();

		}

		void Start()
		{
			if(currBranch != null){
				branches.Add( currBranch );
			}

			detachedHeadBranch = CreateDetachedBranch();
			prevBranch_btn.interactable = false;
			CreateMainBranch();
		}

		void OnEnable(){
			prevBranch_btn.onClick.AddListener( GoToPreviousBranching );
			nextBranch_btn.onClick.AddListener( GoToNextBranching );
			createNewBranch_btn.onClick.AddListener( ForkCurrentBranch_UpToSelectedEntry );
			deleteBranch_btn.onClick.AddListener( DeleteCurrentBranch );

			branches.ForEach( b => b.gameObject.SetActive(false));
		}

		void OnDisable(){
			prevBranch_btn.onClick.RemoveListener( GoToPreviousBranching );
			nextBranch_btn.onClick.RemoveListener( GoToNextBranching );
			createNewBranch_btn.onClick.RemoveListener( ForkCurrentBranch_UpToSelectedEntry );
			deleteBranch_btn.onClick.RemoveListener( DeleteCurrentBranch );

			currBranch?.gameObject?.SetActive(true);
		}

		private AnalysisBranch CreateDetachedBranch() {
			var newBranchObj = Instantiate( newBranchPrefab, this.transform );
			var branch = newBranchObj.GetComponent<AnalysisBranch>();
			branch.gameObject.SetActive( false );
			branch.BranchName = "Detached Branch";
			return branch;
		}

		[Button]
		public void ForkCurrentBranch_UpToSelectedEntry() {
			var newBranch = CreateNewBranch();
			CopyCurrBranch_UpToCurrSelectedEntry( ref newBranch );
			
			EnableBranch( newBranch );
		}

		public void CreateMainBranch(){
			var newBranch = CreateNewBranch();
			newBranch.BranchGameHistory = shogiGame.gameHistory;

			EnableBranch( newBranch );
			newBranch.BranchName = "Stein;s Gate";
		}

		private AnalysisBranch CreateNewBranch(){
			var newBranchObj = Instantiate( newBranchPrefab );
			var branch = newBranchObj.GetComponent<AnalysisBranch>();
			return branch;
		}

		private void CopyCurrBranch_UpToCurrSelectedEntry( ref AnalysisBranch targetBranch ) {
			int selectedEntry_turn = currBranch.currentlySelectedEntry.moveNumber;

			var entriesToCarryOver = currBranch.entries.Take( selectedEntry_turn );
			targetBranch.ClearEntries();
			foreach (var entry in entriesToCarryOver) {
				targetBranch.CreateAndAppend_MoveEntry( entry.associatedMove );
			}
			targetBranch.currentlySelectedEntry = targetBranch.entries.Last();

			GameHistory trimmedGameHistory_copy = currBranch.BranchGameHistory.Clone( selectedEntry_turn );
			targetBranch.BranchGameHistory = trimmedGameHistory_copy;
		}

		public void EnableBranch( AnalysisBranch branchToEnable ) {
			if (branches.Contains( branchToEnable ) == false) {
				branches.Add( branchToEnable );
				branchToEnable.BranchName = "Branch " + ( branchesContainer.childCount - 1 );
				
				branchToEnable.transform.SetParent(branchesContainer);
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
			Debug.Log( "New Branch" );
			CopyCurrBranch_UpToCurrSelectedEntry( ref detachedHeadBranch );
			shogiGame.OnBeforeActionExecuted -= ForkSelectedEntry_ToNewBranch;
			shogiGame.OnBeforeActionExecuted += ForkSelectedEntry_ToNewBranch;
		}

		protected void ForkSelectedEntry_ToNewBranch(AShogiAction action){
			Debug.Log("Move detected: Fork");
			// detachedHeadBranch.GetComponent<Canvas>().enabled = true;
			detachedHeadBranch.gameObject.SetActive( true );

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
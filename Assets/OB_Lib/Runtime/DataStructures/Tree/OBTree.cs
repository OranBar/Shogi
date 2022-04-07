
/**<summary>Tree Implementation. Each leaf can have a different amount of children. 
Doesn't allow for duplicate entries. </summary>
* <author>Oran Bar</author>
*/
namespace BarbarO.DataStructures {
	public class OBTree<E> {

		public const int DEPTH_FIRST_SEARCH = 0, BREADTH_FIRST_SEARCH = 1;

		public TreeNode<E> Root { get; set; }

		public OBTree() {
			this.Root = new TreeNode<E>();
		}

		public OBTree(TreeNode<E> root) {
			this.Root = root;
		}

		public OBTree(ATreeValuesGetter<E> valuesGetter) {
			E rootValue = valuesGetter.GetRoot();
			Root = new TreeNode<E>(rootValue, null);

			foreach (E nodeValue in valuesGetter.GetNodesValues()) {
				E parentValue = valuesGetter.GetParentValue(nodeValue);
				new TreeNode<E>(nodeValue, FindNode(parentValue));
			}
		}

		public bool IsRoot(TreeNode<E> node) {
			return this.Root.Equals(node);
		}

		public bool IsChild(TreeNode<E> node) {
			return IsRoot(node) == false;
		}

		public bool IsLeaf(TreeNode<E> node) {
			return node.Children.Count == 0;
		}

		public TreeNode<E> FindNode(E value, int searchMode = 1/*BREADTH_FIRST_SEARCH*/) {
			TreeNode<E> result = null;

			if (Root.Value.Equals(value)) {
				return Root;
			}

			if (searchMode == DEPTH_FIRST_SEARCH) {
				result = FindNodeRecursiveDFS(Root, value);
			}
			else
			if (searchMode == BREADTH_FIRST_SEARCH) {
				result = FindNodeRecursiveBFS(Root, value);
			}

			return result;
		}

		private TreeNode<E> FindNodeRecursiveBFS(TreeNode<E> node, E value) {
			foreach (TreeNode<E> child in node.Children) {
				if (child.Value.Equals(value)) {
					return child;
				}
			}

			foreach (TreeNode<E> child in node.Children) {
				TreeNode<E> result = FindNodeRecursiveBFS(child, value);
				if (result != null) {
					return result;
				}
			}

			return null;
		}

		private TreeNode<E> FindNodeRecursiveDFS(TreeNode<E> node, E value) {
			throw new System.NotImplementedException();
		}

	}
}
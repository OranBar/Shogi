using System;
using System.Collections.Generic;
using System.Linq;					 
using UnityEngine;
using Object = UnityEngine.Object;

public static class TransformEx {
	
	#region Position

	public static void SetX(this Transform transform, float x) {
		var newPosition = new Vector3(x, transform.position.y, transform.position.z);

		transform.position = newPosition;
	}
	
	public static void SetY(this Transform transform, float y) {
		var newPosition = new Vector3(transform.position.x, y, transform.position.z);

		transform.position = newPosition;
	}

	public static void SetZ(this Transform transform, float z) {
		var newPosition = new Vector3(transform.position.x, transform.position.y, z);

		transform.position = newPosition;
	}

	public static void SetXY(this Transform transform, float x, float y) {
		var newPosition = new Vector3(x, y, transform.position.z);
		transform.position = newPosition;
	}

	public static void SetXZ(this Transform transform, float x, float z) {
		var newPosition = new Vector3(x, transform.position.y, z);
		transform.position = newPosition;
	}

	public static void SetYZ(this Transform transform, float y, float z) {
		var newPosition = new Vector3(transform.position.x, y, z);
		transform.position = newPosition;
	}

	public static void SetXYZ(this Transform transform, float x, float y, float z) {
		var newPosition = new Vector3(x, y, z);
		transform.position = newPosition;
	}

	public static void TranslateX(this Transform transform, float x) {
		var offset = new Vector3(x, 0, 0);

		transform.position += offset;
	}

	public static void TranslateY(this Transform transform, float y) {
		var offset = new Vector3(0, y, 0);

		transform.position += offset;
	}

	public static void TranslateZ(this Transform transform, float z) {
		var offset = new Vector3(0, 0, z);
		transform.position += offset;
	}

	public static void TranslateXY(this Transform transform, float x, float y) {
		var offset = new Vector3(x, y, 0);
		transform.position += offset;
	}

	public static void TranslateXZ(this Transform transform, float x, float z) {
		var offset = new Vector3(x, 0, z);
		transform.position += offset;
	}

	public static void TranslateYZ(this Transform transform, float y, float z) {
		var offset = new Vector3(0, y, z);
		transform.position += offset;
	}

	public static void TranslateXYZ(this Transform transform, float x, float y, float z) {
		var offset = new Vector3(x, y, z);
		transform.position += offset;
	}

	public static void SetLocalX(this Transform transform, float x) {
		var newPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
		transform.localPosition = newPosition;
	}

	public static void SetLocalY(this Transform transform, float y) {
		var newPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
		transform.localPosition = newPosition;
	}

	public static void SetLocalZ(this Transform transform, float z) {
		var newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
		transform.localPosition = newPosition;
	}

	public static void SetLocalXY(this Transform transform, float x, float y) {
		var newPosition = new Vector3(x, y, transform.localPosition.z);
		transform.localPosition = newPosition;
	}

	public static void SetLocalXZ(this Transform transform, float x, float z) {
		var newPosition = new Vector3(x, transform.localPosition.z, z);
		transform.localPosition = newPosition;
	}

	public static void SetLocalYZ(this Transform transform, float y, float z) {
		var newPosition = new Vector3(transform.localPosition.x, y, z);
		transform.localPosition = newPosition;
	}

	public static void SetLocalXYZ(this Transform transform, float x, float y, float z) {
		var newPosition = new Vector3(x, y, z);
		transform.localPosition = newPosition;
	}

	public static void ResetPosition(this Transform transform) {
		transform.position = Vector3.zero;
	}

	public static void ResetLocalPosition(this Transform transform) {
		transform.localPosition = Vector3.zero;
	}

	#endregion

	#region Scale

	public static void SetScaleX(this Transform transform, float x) {
		var newScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
		transform.localScale = newScale;
	}

	public static void SetScaleY(this Transform transform, float y) {
		var newScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
		transform.localScale = newScale;
	}

	public static void SetScaleZ(this Transform transform, float z) {
		var newScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
		transform.localScale = newScale;
	}

	public static void SetScaleXY(this Transform transform, float x, float y) {
		var newScale = new Vector3(x, y, transform.localScale.z);
		transform.localScale = newScale;
	}

	public static void SetScaleXZ(this Transform transform, float x, float z) {
		var newScale = new Vector3(x, transform.localScale.y, z);
		transform.localScale = newScale;
	}

	public static void SetScaleYZ(this Transform transform, float y, float z) {
		var newScale = new Vector3(transform.localScale.x, y, z);
		transform.localScale = newScale;
	}

	public static void SetScaleXYZ(this Transform transform, float x, float y, float z) {
		var newScale = new Vector3(x, y, z);
		transform.localScale = newScale;
	}

	public static void ScaleByX(this Transform transform, float x) {
		transform.localScale = new Vector3(transform.localScale.x * x, transform.localScale.y, transform.localScale.z);
	}

	public static void ScaleByY(this Transform transform, float y) {
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * y, transform.localScale.z);
	}

	public static void ScaleByZ(this Transform transform, float z) {
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * z);
	}

	public static void ScaleByXY(this Transform transform, float x, float y) {
		transform.localScale = new Vector3(transform.localScale.x * x, transform.localScale.y * y, transform.localScale.z);
	}

	public static void ScaleByXZ(this Transform transform, float x, float z) {
		transform.localScale = new Vector3(transform.localScale.x * x, transform.localScale.y, transform.localScale.z * z);
	}

	public static void ScaleByYZ(this Transform transform, float y, float z) {
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * y, transform.localScale.z * z);
	}

	public static void ScaleByXY(this Transform transform, float r) {
		transform.ScaleByXY(r, r);
	}

	public static void ScaleByXZ(this Transform transform, float r) {
		transform.ScaleByXZ(r, r);
	}

	public static void ScaleByYZ(this Transform transform, float r) {
		transform.ScaleByYZ(r, r);
	}

	public static void ScaleByXYZ(this Transform transform, float x, float y, float z) {
		transform.localScale = new Vector3(
			x, y, z);
	}

	public static void ScaleByXYZ(this Transform transform, float r) {
		transform.ScaleByXYZ(r, r, r);
	}


	public static void ResetScale(this Transform transform) {
		transform.localScale = Vector3.one;
	}

	#endregion

	#region FlipScale

	public static void FlipX(this Transform transform) {
		transform.SetScaleX(-transform.localScale.x);
	}

	public static void FlipY(this Transform transform) {
		transform.SetScaleY(-transform.localScale.y);
	}

	public static void FlipZ(this Transform transform) {
		transform.SetScaleZ(-transform.localScale.z);
	}

	public static void FlipXY(this Transform transform) {
		transform.SetScaleXY(-transform.localScale.x, -transform.localScale.y);
	}

	public static void FlipXZ(this Transform transform) {
		transform.SetScaleXZ(-transform.localScale.x, -transform.localScale.z);
	}

	public static void FlipYZ(this Transform transform) {
		transform.SetScaleYZ(-transform.localScale.y, -transform.localScale.z);
	}

	public static void FlipXYZ(this Transform transform) {
		transform.SetScaleXYZ(-transform.localScale.z, -transform.localScale.y, -transform.localScale.z);
	}

	#endregion

	#region Rotation

	public static void RotateAroundX(this Transform transform, float angle) {
		var rotation = new Vector3(angle, 0, 0);
		transform.Rotate(rotation);
	}

	public static void RotateAroundY(this Transform transform, float angle) {
		var rotation = new Vector3(0, angle, 0);
		transform.Rotate(rotation);
	}

	public static void RotateAroundZ(this Transform transform, float angle) {
		var rotation = new Vector3(0, 0, angle);
		transform.Rotate(rotation);
	}

	public static void SetRotationX(this Transform transform, float angle) {
		transform.eulerAngles = new Vector3(angle, 0, 0);
	}

	public static void SetRotationY(this Transform transform, float angle) {
		transform.eulerAngles = new Vector3(0, angle, 0);
	}

	public static void SetRotationZ(this Transform transform, float angle) {
		transform.eulerAngles = new Vector3(0, 0, angle);
	}

	public static void SetLocalRotationX(this Transform transform, float angle) {
		transform.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));
	}

	public static void SetLocalRotationY(this Transform transform, float angle) {
		transform.localRotation = Quaternion.Euler(new Vector3(0, angle, 0));
	}

	public static void SetLocalRotationZ(this Transform transform, float angle) {
		transform.localEulerAngles =  new Vector3(0, 0, angle);
	}

	public static void ResetRotation(this Transform transform) {
		transform.rotation = Quaternion.identity;
	}

	public static void ResetLocalRotation(this Transform transform) {
		transform.localRotation = Quaternion.identity;
	}

	#endregion

	#region All

	public static void ResetLocal(this Transform transform) {
		transform.ResetLocalRotation();
		transform.ResetLocalPosition();
		transform.ResetScale();

	}

	public static void Reset(this Transform transform) {
		transform.ResetRotation();
		transform.ResetPosition();
		transform.ResetScale();
	}

    #endregion

    #region Children

    public static int CountDecendants(this Transform transform)
    {
        int childCount = transform.childCount;// direct child count.
        foreach (Transform child in transform)
        {
            childCount += CountDecendants(child);// add child direct children count.
        }
        return childCount;
    }

    public static void DestroyChildren(this Transform transform) {
		//Add children to list before destroying
		//otherwise GetChild(i) may bomb out
		var children = new List<Transform>();

		for (var i = 0; i < transform.childCount; i++) {
			var child = transform.GetChild(i);
			children.Add(child);
		}

		foreach (var child in children) {
			Object.Destroy(child.gameObject);
		}
	}

	public static void DestroyChildrenImmediate(this Transform transform) {
		//Add children to list before destroying
		//otherwise GetChild(i) may bomb out
		var children = new List<Transform>();

		for (var i = 0; i < transform.childCount; i++) {
			var child = transform.GetChild(i);
			children.Add(child);
		}

		foreach (var child in children) {
			Object.DestroyImmediate(child.gameObject);
		}
	}

	public static List<Transform> GetChildren(this Transform transform) {
		var children = new List<Transform>();

		for (var i = 0; i < transform.childCount; i++) {
			var child = transform.GetChild(i);
			children.Add(child);
		}

		return children;
	}

	public static List<Transform> GetAllChildren(this Transform transform, bool alsoInactiveObjects, bool recursive){
		if(recursive){
			return transform.GetAllChildren_Recursive(alsoInactiveObjects);
		} else {
			return transform.GetAllChildren_NonRecursive(alsoInactiveObjects);
		}
	}

	public static List<Transform> GetAllChildren_NonRecursive(this Transform transform, bool alsoInactiveObjects)
    {
        var children = new List<Transform>();
        Transform[] tmpChildren = transform.GetComponentsInChildren<Transform>(alsoInactiveObjects);
        children = tmpChildren.Skip(1).ToList(); //Take myself out of the list
		children = children.Where(c => c.transform.parent == transform).ToList(); //Remove all nefews 

		return children;
    }

	public static List<Transform> GetAllChildren_Recursive(this Transform transform, bool alsoInactiveObjects)
    {
        var children = new List<Transform>();
        Transform[] tmpChildren = transform.GetComponentsInChildren<Transform>(alsoInactiveObjects);
        children = tmpChildren.Skip(1).ToList();

        return children;
    }

	public static Transform FindChildWithTag(this Transform transform, string tag, bool includeInactive=false)
	{
		return transform.GetAllChildren_Recursive(includeInactive).FirstOrDefault(child => child.tag == tag);
	}

	public static List<Transform> FindChildrenWithTag( this Transform transform, string tag, bool includeInactive=false ) {
		return transform.GetAllChildren_Recursive(includeInactive).Where(child => child.tag == tag).ToList();
	}

    public static void Sort(this Transform transform, Func<Transform, IComparable> sortFunction) {
		var children = transform.GetChildren();
		var sortedChildren = children.OrderBy(sortFunction).ToList();

		for (int i = 0; i < sortedChildren.Count(); i++) {
			sortedChildren[i].SetSiblingIndex(i);
		}
	}

	public static void SortAlphabetically(this Transform transform) {
		transform.Sort(t => t.name);
	}


	/// <summary>
	/// A lazy enumerable of this objects transform, and all it's children down the hierarchy. Useful for LINQ usage.
	/// </summary>
	/// <param name="transform"></param>
	/// <returns></returns>
	public static IEnumerable<Transform> ToEnumerable(this Transform transform) {
		foreach (Transform child in transform) {
			yield return child;
		}
	}
    #endregion

    #region Parent
    public static int GetNumberOfParents(this Transform transf)
    {
        int count = 0;
        Transform current = transf;
        while(current.parent != null)
        {
            count++;
            current = current.parent;
        }
        return count;
    }

    /// <summary>
    /// Returns the child index as if the hiearchy had only 1 level of depth, starting from the ancestor. Returns -1 if the target is not found
    /// </summary>
    /// <param name="ancestor"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static int GetHiearchyIndex(this Transform me, Transform ancestor)
    {
        Debug.Log(string.Format("{0} is the {1}th brother of {2}", me.name, GetHiearchyIndex_Recursive(ancestor, me, -1), ancestor.name));
        return GetHiearchyIndex_Recursive(ancestor, me, -1);
    }

    //NOTE by Oran: This method's performance can be improved by flattening an array where each element is a list of gameobjects (the children).
    private static int GetHiearchyIndex_Recursive(Transform current, Transform target, int indexSoFar)
    {
        foreach (Transform t in current)
        {
            indexSoFar++;
            if (t == target)
            {
                return indexSoFar;
            }
            else
            {
                if (GetHiearchyIndex_Recursive(t, target, indexSoFar) != -1)
                {
                    return GetHiearchyIndex_Recursive(t, target, indexSoFar);
                }
                else
                {
                    indexSoFar += t.CountDecendants();
                }
            }
        }
        return -1;
    }

    #endregion

	public static void SetPivotToChildPosition(this Transform t, Transform targetPivot){
		Debug.Assert(t.GetAllChildren_Recursive(false).Contains(targetPivot), "Target Pivot needs to be a child of the calling transform");
		// In verita', se l'asserzione non va a asegno, potrei sistemare la faccenda da codice con un setparent?

		List<Transform> myChildren = t.GetAllChildren_NonRecursive(false);

		// SetParent(null) => diventa oggetto di scena, senza padre.
		myChildren.ForEach( c => c.SetParent(null) );

		// Sposto la transform di partenza (ora senza figli) nella posizione corretta
		t.position = targetPivot.position;

		// Riparento tutti i figli alla transform
		myChildren.ForEach(c => c.SetParent(t));
	}
}

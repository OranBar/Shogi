using UnityEngine;

public static class ColorEx {
	public static Color SetAlpha( this Color color, float alpha ) {
		var alphaColor = color;
		alphaColor.a = alpha;
		return alphaColor;
	}
}
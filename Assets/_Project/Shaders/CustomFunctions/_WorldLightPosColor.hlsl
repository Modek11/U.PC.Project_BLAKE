//CustomLight.hlsl

void CustomLight_float(out half3 direction)
{
	#ifdef SHADERGRAPH_PREVIEW
		direction = half3(0, 1, 0);
	#else
		#if defined(UNIVERSAL_LIGHTING_INCLUDED)
			Light mainLight = GetMainLight();
			direction = mainLight.direction;
		#endif
	#endif
}
#include "pch-cpp.hpp"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <limits>



struct String_t;

IL2CPP_EXTERN_C RuntimeClass* InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C const RuntimeMethod* Math_ThrowMinMaxException_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mBC7732632C280D3AEE2B08C470A78B9C5C4CBD77_RuntimeMethod_var;


IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
struct U3CModuleU3E_tA32484217F2E5D2EE6846EDE80E8FFCDDEDEB3CE 
{
};
struct InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8  : public RuntimeObject
{
};
struct ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2  : public RuntimeObject
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F  : public RuntimeObject
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_pinvoke
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_com
{
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22 
{
	bool ___m_value;
};
struct Double_tE150EF3D1D43DEE85D533810AB4C742307EEDE5F 
{
	double ___m_value;
};
struct Int32_t680FF22E76F6EFAD4375103CBBFFA0421349384C 
{
	int32_t ___m_value;
};
struct Single_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C 
{
	float ___m_value;
};
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915 
{
	union
	{
		struct
		{
		};
		uint8_t Void_t4861ACF8F4594C3437BB48B6E56783494B843915__padding[1];
	};
};
struct InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_StaticFields
{
	int32_t ___s_MaxNrBigTileLightsPlusOne;
	int32_t ___s_LightListMaxCoarseEntries;
	int32_t ___s_LightClusterMaxCoarseEntries;
	int32_t ___s_LightDwordPerFptlTile;
	int32_t ___s_LightClusterPackingCountBits;
	int32_t ___s_LightClusterPackingCountMask;
	int32_t ___s_LightClusterPackingOffsetBits;
	int32_t ___s_LightClusterPackingOffsetMask;
};
struct ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields
{
	int32_t ___s_CameraRelativeRendering;
	int32_t ___s_PreExposition;
	int32_t ___s_XrMaxViews;
	int32_t ___s_PrecomputedAtmosphericAttenuation;
	int32_t ___s_AreaLights;
	int32_t ___s_BarnDoor;
	bool ___s_GlobalMipBias;
	int32_t ___FPTLMaxLightCount;
	int32_t ___PathTracingMaxLightCount;
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_StaticFields
{
	String_t* ___TrueString;
	String_t* ___FalseString;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif


IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Math_ThrowMinMaxException_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mBC7732632C280D3AEE2B08C470A78B9C5C4CBD77_gshared (int32_t ___0_min, int32_t ___1_max, const RuntimeMethod* method) ;

IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Math_Clamp_mAB687477D3AAC0E7243D724F45626026980CE2FF_inline (int32_t ___0_value, int32_t ___1_min, int32_t ___2_max, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Mathf_NextPowerOfTwo_mA1CE7F3EEF9B0B07AB2D586C030ED236D578F485 (int32_t ___0_value, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR float Mathf_Log_m116F062EEBF1C53EC8D18C9B1748E999EF9424EF_inline (float ___0_f, float ___1_p, const RuntimeMethod* method) ;
inline void Math_ThrowMinMaxException_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mBC7732632C280D3AEE2B08C470A78B9C5C4CBD77 (int32_t ___0_min, int32_t ___1_max, const RuntimeMethod* method)
{
	((  void (*) (int32_t, int32_t, const RuntimeMethod*))Math_ThrowMinMaxException_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mBC7732632C280D3AEE2B08C470A78B9C5C4CBD77_gshared)(___0_min, ___1_max, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR double Math_Log_m5A3BBBF06AB82F25C885812E07D27B473CF43054 (double ___0_a, double ___1_newBase, const RuntimeMethod* method) ;
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ShaderConfig__cctor_m154F4E71F5E2CE730B813C440928D01A3D51AA00 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___s_CameraRelativeRendering = 1;
		((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___s_PreExposition = 1;
		((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___s_XrMaxViews = 2;
		((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___s_PrecomputedAtmosphericAttenuation = 0;
		((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___s_AreaLights = 1;
		((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___s_BarnDoor = 0;
		((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___s_GlobalMipBias = (bool)1;
		((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___FPTLMaxLightCount = ((int32_t)63);
		((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___PathTracingMaxLightCount = ((int32_t)16);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void InternalLightCullingDefs__cctor_mAA179C0BE7DE721174786E4A121D0EE84BB08E20 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		il2cpp_codegen_runtime_class_init_inline(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var);
		int32_t L_0 = ((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___FPTLMaxLightCount;
		il2cpp_codegen_runtime_class_init_inline(Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		int32_t L_1;
		L_1 = Math_Clamp_mAB687477D3AAC0E7243D724F45626026980CE2FF_inline(((int32_t)il2cpp_codegen_multiply(((int32_t)il2cpp_codegen_add(L_0, 1)), 8)), ((int32_t)512), ((int32_t)1024), NULL);
		((InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_StaticFields*)il2cpp_codegen_static_fields_for(InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var))->___s_MaxNrBigTileLightsPlusOne = L_1;
		int32_t L_2 = ((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___FPTLMaxLightCount;
		int32_t L_3;
		L_3 = Math_Clamp_mAB687477D3AAC0E7243D724F45626026980CE2FF_inline(((int32_t)il2cpp_codegen_add(L_2, 1)), ((int32_t)64), ((int32_t)256), NULL);
		((InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_StaticFields*)il2cpp_codegen_static_fields_for(InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var))->___s_LightListMaxCoarseEntries = L_3;
		int32_t L_4 = ((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___FPTLMaxLightCount;
		int32_t L_5;
		L_5 = Math_Clamp_mAB687477D3AAC0E7243D724F45626026980CE2FF_inline(((int32_t)il2cpp_codegen_multiply(((int32_t)il2cpp_codegen_add(L_4, 1)), 2)), ((int32_t)128), ((int32_t)256), NULL);
		((InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_StaticFields*)il2cpp_codegen_static_fields_for(InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var))->___s_LightClusterMaxCoarseEntries = L_5;
		int32_t L_6 = ((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___FPTLMaxLightCount;
		((InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_StaticFields*)il2cpp_codegen_static_fields_for(InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var))->___s_LightDwordPerFptlTile = ((int32_t)(((int32_t)il2cpp_codegen_add(L_6, 1))/2));
		int32_t L_7 = ((ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_StaticFields*)il2cpp_codegen_static_fields_for(ShaderConfig_tB0071DEDAFA079DC303735CCD4DCD1C33F9075A2_il2cpp_TypeInfo_var))->___FPTLMaxLightCount;
		int32_t L_8;
		L_8 = Mathf_NextPowerOfTwo_mA1CE7F3EEF9B0B07AB2D586C030ED236D578F485(L_7, NULL);
		float L_9;
		L_9 = Mathf_Log_m116F062EEBF1C53EC8D18C9B1748E999EF9424EF_inline(((float)L_8), (2.0f), NULL);
		float L_10;
		L_10 = ceilf(L_9);
		((InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_StaticFields*)il2cpp_codegen_static_fields_for(InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var))->___s_LightClusterPackingCountBits = il2cpp_codegen_cast_double_to_int<int32_t>(L_10);
		int32_t L_11 = ((InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_StaticFields*)il2cpp_codegen_static_fields_for(InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var))->___s_LightClusterPackingCountBits;
		((InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_StaticFields*)il2cpp_codegen_static_fields_for(InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var))->___s_LightClusterPackingCountMask = ((int32_t)il2cpp_codegen_subtract(((int32_t)(1<<((int32_t)(L_11&((int32_t)31))))), 1));
		int32_t L_12 = ((InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_StaticFields*)il2cpp_codegen_static_fields_for(InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var))->___s_LightClusterPackingCountBits;
		((InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_StaticFields*)il2cpp_codegen_static_fields_for(InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var))->___s_LightClusterPackingOffsetBits = ((int32_t)il2cpp_codegen_subtract(((int32_t)32), L_12));
		int32_t L_13 = ((InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_StaticFields*)il2cpp_codegen_static_fields_for(InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var))->___s_LightClusterPackingOffsetBits;
		((InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_StaticFields*)il2cpp_codegen_static_fields_for(InternalLightCullingDefs_t3578B94998CE824C90BF77810328DAEF2FFB02F8_il2cpp_TypeInfo_var))->___s_LightClusterPackingOffsetMask = ((int32_t)il2cpp_codegen_subtract(((int32_t)(1<<((int32_t)(L_13&((int32_t)31))))), 1));
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Math_Clamp_mAB687477D3AAC0E7243D724F45626026980CE2FF_inline (int32_t ___0_value, int32_t ___1_min, int32_t ___2_max, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Math_ThrowMinMaxException_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mBC7732632C280D3AEE2B08C470A78B9C5C4CBD77_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		int32_t L_0 = ___1_min;
		int32_t L_1 = ___2_max;
		if ((((int32_t)L_0) <= ((int32_t)L_1)))
		{
			goto IL_000b;
		}
	}
	{
		int32_t L_2 = ___1_min;
		int32_t L_3 = ___2_max;
		il2cpp_codegen_runtime_class_init_inline(Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		Math_ThrowMinMaxException_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mBC7732632C280D3AEE2B08C470A78B9C5C4CBD77(L_2, L_3, Math_ThrowMinMaxException_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mBC7732632C280D3AEE2B08C470A78B9C5C4CBD77_RuntimeMethod_var);
	}

IL_000b:
	{
		int32_t L_4 = ___0_value;
		int32_t L_5 = ___1_min;
		if ((((int32_t)L_4) >= ((int32_t)L_5)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_6 = ___1_min;
		return L_6;
	}

IL_0011:
	{
		int32_t L_7 = ___0_value;
		int32_t L_8 = ___2_max;
		if ((((int32_t)L_7) <= ((int32_t)L_8)))
		{
			goto IL_0017;
		}
	}
	{
		int32_t L_9 = ___2_max;
		return L_9;
	}

IL_0017:
	{
		int32_t L_10 = ___0_value;
		return L_10;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR float Mathf_Log_m116F062EEBF1C53EC8D18C9B1748E999EF9424EF_inline (float ___0_f, float ___1_p, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	float V_0 = 0.0f;
	{
		float L_0 = ___0_f;
		float L_1 = ___1_p;
		il2cpp_codegen_runtime_class_init_inline(Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		double L_2;
		L_2 = Math_Log_m5A3BBBF06AB82F25C885812E07D27B473CF43054(((double)L_0), ((double)L_1), NULL);
		V_0 = ((float)L_2);
		goto IL_000e;
	}

IL_000e:
	{
		float L_3 = V_0;
		return L_3;
	}
}

------ Common settings for all quality levels
applySettings([=[
]=])


------ Texture Quality
applyQualitySettings('TextureQuality', {
	[Quality.Low]=[=[
		VisualTerrain.TextureRenderJobCount 1
		VisualTerrain.TextureRenderJobsLaunchedPerFrameCountMax 1
		VisualTerrain.TextureSkipMipSpeed 20
		VisualTerrain.TextureCompressionQuality 0
		VisualTerrain.Decal3dFarDrawDistanceScaleFactor 1.0
		WorldRender.SkyCelestialQuality QualityLevel_Low
		RvmSystem.DefaultQualityLevel QualityLevel_Low
	]=],
	[Quality.Medium]=[=[
		VisualTerrain.TextureRenderJobCount 2
		VisualTerrain.TextureRenderJobsLaunchedPerFrameCountMax 2
		VisualTerrain.TextureSkipMipSpeed 20
		VisualTerrain.TextureCompressionQuality 0
		VisualTerrain.Decal3dFarDrawDistanceScaleFactor 1.2
		WorldRender.SkyCelestialQuality QualityLevel_Medium
		RvmSystem.DefaultQualityLevel QualityLevel_Medium
	]=],
	[Quality.High]=[=[
		VisualTerrain.TextureRenderJobCount 4
		VisualTerrain.TextureRenderJobsLaunchedPerFrameCountMax 4
		VisualTerrain.TextureSkipMipSpeed 30
		VisualTerrain.TextureCompressionQuality 0
		VisualTerrain.Decal3dFarDrawDistanceScaleFactor 1.7
		WorldRender.SkyCelestialQuality QualityLevel_High
		RvmSystem.DefaultQualityLevel QualityLevel_High
	]=],
	[Quality.Ultra]=[=[
		VisualTerrain.TextureRenderJobCount 4
		VisualTerrain.TextureRenderJobsLaunchedPerFrameCountMax 4
		VisualTerrain.TextureSkipMipSpeed 40
		VisualTerrain.TextureCompressionQuality -1
		VisualTerrain.Decal3dFarDrawDistanceScaleFactor 2
		WorldRender.SkyCelestialQuality QualityLevel_Ultra
		RvmSystem.DefaultQualityLevel QualityLevel_Ultra
	]=],
})


------ Lighting Quality
applyQualitySettings('LightingQuality', {
	[Quality.Low]=[=[
		Mesh.CastPlanarReflectionQualityLevel Low
		Mesh.CastDynamicReflectionQualityLevel Low
		Mesh.CastStaticReflectionQualityLevel Low
		WorldRender.SubSurfaceScatteringEnable 0
		WorldRender.LocalLightTranslucencyEnable 0
		WorldRender.LocalLightCastVolumetricLevel QualityLevel_Low
		WorldRender.PunctualLightCastVolumetricShadowmapEnableLevel QualityLevel_Low
		WorldRender.AreaLightCastVolumetricShadowmapEnableLevel QualityLevel_Low
		WorldRender.VolumetricCloudsQuality QualityLevel_Low
		WorldRender.VolumetricCloudsRenderTargetResolutionDivider 4
		WorldRender.VolumetricCloudsReflectionRenderTargetResolutionDivider 4
		WorldRender.IndirectSpecularIntensity 0
		WorldRender.IndirectSpecularReflectanceScale 0
		WorldRender.IndirectSpecularProbesIntensity 0
		WorldRender.IndirectSpecularProbesReflectanceScale 0
	]=],
	[Quality.Medium]=[=[
		Mesh.CastPlanarReflectionQualityLevel Medium
		Mesh.CastDynamicReflectionQualityLevel Medium
		Mesh.CastStaticReflectionQualityLevel Medium
		WorldRender.SubSurfaceScatteringEnable 0
		WorldRender.LocalLightTranslucencyEnable 0
		WorldRender.LocalLightCastVolumetricLevel QualityLevel_Medium
		WorldRender.PunctualLightCastVolumetricShadowmapEnableLevel QualityLevel_Medium
		WorldRender.AreaLightCastVolumetricShadowmapEnableLevel QualityLevel_Medium
		WorldRender.VolumetricCloudsQuality QualityLevel_Medium
		WorldRender.VolumetricCloudsRenderTargetResolutionDivider 4
		WorldRender.VolumetricCloudsReflectionRenderTargetResolutionDivider 4
		WorldRender.IndirectSpecularIntensity 0
		WorldRender.IndirectSpecularReflectanceScale 0
		WorldRender.IndirectSpecularProbesIntensity 0
		WorldRender.IndirectSpecularProbesReflectanceScale 0
	]=],
	[Quality.High]=[=[
		Mesh.CastPlanarReflectionQualityLevel High
		Mesh.CastDynamicReflectionQualityLevel High
		Mesh.CastStaticReflectionQualityLevel High
		WorldRender.SubSurfaceScatteringEnable 1
		WorldRender.LocalLightTranslucencyEnable 1
		WorldRender.LocalLightCastVolumetricLevel QualityLevel_High
		WorldRender.PunctualLightCastVolumetricShadowmapEnableLevel QualityLevel_High
		WorldRender.AreaLightCastVolumetricShadowmapEnableLevel QualityLevel_High
		WorldRender.VolumetricCloudsQuality QualityLevel_High
		WorldRender.VolumetricCloudsRenderTargetResolutionDivider 3
		WorldRender.VolumetricCloudsReflectionRenderTargetResolutionDivider 3
		WorldRender.IndirectSpecularIntensity 0
		WorldRender.IndirectSpecularReflectanceScale 0
		WorldRender.IndirectSpecularProbesIntensity 0
		WorldRender.IndirectSpecularProbesReflectanceScale 0
	]=],
	[Quality.Ultra]=[=[
		Mesh.CastPlanarReflectionQualityLevel Ultra
		Mesh.CastDynamicReflectionQualityLevel Ultra
		Mesh.CastStaticReflectionQualityLevel Ultra
		WorldRender.SubSurfaceScatteringEnable 1
		WorldRender.LocalLightTranslucencyEnable 1
		WorldRender.LocalLightCastVolumetricLevel QualityLevel_Ultra
		WorldRender.PunctualLightCastVolumetricShadowmapEnableLevel QualityLevel_Ultra
		WorldRender.AreaLightCastVolumetricShadowmapEnableLevel QualityLevel_Ultra
		WorldRender.VolumetricCloudsQuality QualityLevel_Ultra
		WorldRender.VolumetricCloudsRenderTargetResolutionDivider 3
		WorldRender.VolumetricCloudsReflectionRenderTargetResolutionDivider 3
		WorldRender.IndirectSpecularIntensity 1
		WorldRender.IndirectSpecularReflectanceScale 1
		WorldRender.IndirectSpecularProbesIntensity 1
		WorldRender.IndirectSpecularProbesReflectanceScale 1
	]=],
})


------ Shadow Quality
applyQualitySettings('ShadowQuality', {
	[ShadowQuality.Low]=[=[
		WorldRender.SunShadowmapLevel QualityLevel_Low
		WorldRender.ShadowmapViewDistance 20
		WorldRender.ShadowmapResolution 608
		WorldRender.ShadowmapSliceCount 3
		WorldRender.ShadowmapQuality 0
		WorldRender.TransparencyShadowmapsEnable 0
		WorldRender.ShadowmapForegroundEnable 0
		WorldRender.SpotLightShadowmapResolution 256
		WorldRender.SpotLightShadowmapQuality 0
		WorldRender.SpotLightShadowmapLevel QualityLevel_Low
		WorldRender.PunctualLightShadowLevel QualityLevel_Low
		WorldRender.AreaLightShadowLevel QualityLevel_Low
		Mesh.CastShadowQualityLevel Low
		VegetationSystem.UseShadowLodOffset 1

		WorldRender.NVIDIAShadowsPCSSEnable false
		WorldRender.NVIDIAShadowsHFTSEnable false
	]=],
	[ShadowQuality.Medium]=[=[
		WorldRender.SunShadowmapLevel QualityLevel_Medium
		WorldRender.ShadowmapViewDistance 50
		WorldRender.ShadowmapResolution 896
		WorldRender.ShadowmapSliceCount 4
		WorldRender.ShadowmapQuality 1
		WorldRender.TransparencyShadowmapsEnable 0
		WorldRender.ShadowmapForegroundEnable 0			
		WorldRender.SpotLightShadowmapResolution 256
		WorldRender.SpotLightShadowmapQuality 1
		WorldRender.SpotLightShadowmapLevel QualityLevel_Medium
		WorldRender.PunctualLightShadowLevel QualityLevel_Medium
		WorldRender.AreaLightShadowLevel QualityLevel_Medium
		Mesh.CastShadowQualityLevel Medium
		VegetationSystem.UseShadowLodOffset 1

		WorldRender.NVIDIAShadowsPCSSEnable false
		WorldRender.NVIDIAShadowsHFTSEnable false
	]=],
	[ShadowQuality.High]=[=[
		WorldRender.SunShadowmapLevel QualityLevel_High
		WorldRender.ShadowmapViewDistance 70
		WorldRender.ShadowmapResolution 1408
		WorldRender.ShadowmapSliceCount 5
		WorldRender.ShadowmapQuality 1
		WorldRender.TransparencyShadowmapsEnable 1
		WorldRender.ShadowmapForegroundEnable 0	
		WorldRender.SpotLightShadowmapResolution 512
		WorldRender.SpotLightShadowmapQuality 1
		WorldRender.SpotLightShadowmapLevel QualityLevel_High
		WorldRender.PunctualLightShadowLevel QualityLevel_High
		WorldRender.AreaLightShadowLevel QualityLevel_High
		Mesh.CastShadowQualityLevel High
		VegetationSystem.UseShadowLodOffset 0

		WorldRender.NVIDIAShadowsPCSSEnable false
		WorldRender.NVIDIAShadowsHFTSEnable false
	]=],
	[ShadowQuality.Ultra]=[=[
		WorldRender.SunShadowmapLevel QualityLevel_Ultra
		WorldRender.ShadowmapViewDistance 100
		WorldRender.ShadowmapResolution 2048
		WorldRender.ShadowmapSliceCount 5
		WorldRender.ShadowmapQuality 1
		WorldRender.TransparencyShadowmapsEnable 1
		WorldRender.ShadowmapForegroundEnable 0
		WorldRender.SpotLightShadowmapResolution 512
		WorldRender.SpotLightShadowmapQuality 1
		WorldRender.SpotLightShadowmapLevel QualityLevel_Ultra
		WorldRender.PunctualLightShadowLevel QualityLevel_Ultra
		WorldRender.AreaLightShadowLevel QualityLevel_Ultra
		Mesh.CastShadowQualityLevel Ultra
		VegetationSystem.UseShadowLodOffset 0

		WorldRender.NVIDIAShadowsPCSSEnable false
		WorldRender.NVIDIAShadowsHFTSEnable false
	]=],
	[ShadowQuality.PCSS]=[=[
		WorldRender.SunShadowmapLevel QualityLevel_Ultra
		WorldRender.ShadowmapViewDistance 100
		WorldRender.ShadowmapResolution 2048
		WorldRender.ShadowmapSliceCount 5
		WorldRender.ShadowmapQuality 1
		WorldRender.TransparencyShadowmapsEnable 1
		WorldRender.ShadowmapForegroundEnable 0
		WorldRender.SpotLightShadowmapResolution 512
		WorldRender.SpotLightShadowmapQuality 1
		WorldRender.SpotLightShadowmapLevel QualityLevel_Ultra
		WorldRender.PunctualLightShadowLevel QualityLevel_Ultra
		WorldRender.AreaLightShadowLevel QualityLevel_Ultra
		Mesh.CastShadowQualityLevel Ultra
		VegetationSystem.UseShadowLodOffset 0

		WorldRender.NVIDIAShadowsPCSSEnable true
		WorldRender.NVIDIAShadowsHFTSEnable false
	]=],
	[ShadowQuality.HFTS]=[=[
		WorldRender.SunShadowmapLevel QualityLevel_Ultra
		WorldRender.ShadowmapViewDistance 100
		WorldRender.ShadowmapResolution 2048
		WorldRender.ShadowmapSliceCount 5
		WorldRender.ShadowmapQuality 1
		WorldRender.TransparencyShadowmapsEnable 1
		WorldRender.ShadowmapForegroundEnable 0
		WorldRender.SpotLightShadowmapResolution 512
		WorldRender.SpotLightShadowmapQuality 1
		WorldRender.SpotLightShadowmapLevel QualityLevel_Ultra
		WorldRender.PunctualLightShadowLevel QualityLevel_Ultra
		WorldRender.AreaLightShadowLevel QualityLevel_Ultra
		Mesh.CastShadowQualityLevel Ultra
		VegetationSystem.UseShadowLodOffset 0

		WorldRender.NVIDIAShadowsPCSSEnable false
		WorldRender.NVIDIAShadowsHFTSEnable true
	]=],
})


------ Effects Quality
applyQualitySettings('EffectsQuality', {
	[Quality.Low]=[=[
		EmitterSystem.QuadHalfResEnable 1
		EmitterSystem.QuadMaxCount 6000
		EmitterSystem.MeshMaxCount 2000
		EmitterSystem.CollisionRayCastMaxCount 50
		EmitterSystem.EmitterQualityLevel Low
		EffectSystem.EffectQualityLevel Low
                        
	]=],
	[Quality.Medium]=[=[
		EmitterSystem.QuadHalfResEnable 1
		EmitterSystem.QuadMaxCount 8000
		EmitterSystem.MeshMaxCount 3000
		EmitterSystem.CollisionRayCastMaxCount 100
		EmitterSystem.EmitterQualityLevel Medium
		EffectSystem.EffectQualityLevel Medium
                        
	]=],
	[Quality.High]=[=[
		EmitterSystem.QuadHalfResEnable 1
		EmitterSystem.QuadMaxCount 12000
		EmitterSystem.MeshMaxCount 4000
		EmitterSystem.CollisionRayCastMaxCount 150
		EmitterSystem.EmitterQualityLevel High
		EffectSystem.EffectQualityLevel High

	]=],
	[Quality.Ultra]=[=[
		EmitterSystem.QuadHalfResEnable 1
		EmitterSystem.QuadMaxCount 16000
		EmitterSystem.MeshMaxCount 8000
		EmitterSystem.CollisionRayCastMaxCount 250
		EmitterSystem.EmitterQualityLevel Ultra
		EffectSystem.EffectQualityLevel Ultra

	]=],
})


------ PostProcess Quality
applyQualitySettings('PostProcessQuality', {
	[Quality.Low]=[=[
		PostProcess.SpriteDofEnable 0
		PostProcess.ScreenSpaceRaytraceEnable 0
		PostProcess.DofMethod DofMethod_Gaussian
		PostProcess.BlurMethod BlurMethod_Gaussian
		PostProcess.BlurPyramidQuarterResEnable 1		
		WorldRender.DistortionEnable 1
		WorldRender.MotionBlurEnable 0
		WorldRender.FastHdrEnable 1
		Render.RenderScaleResampleMode ScaleResampleMode_Linear
		WorldRender.TranslucencyAutoThicknessEnable 0
	]=],
	[Quality.Medium]=[=[
		PostProcess.SpriteDofEnable 0
		PostProcess.ScreenSpaceRaytraceEnable 1
		PostProcess.ScreenSpaceRaytraceFullresEnable 0
		PostProcess.DofMethod DofMethod_Gaussian
		PostProcess.BlurMethod BlurMethod_Gaussian
		PostProcess.BlurPyramidQuarterResEnable 0
		WorldRender.DistortionEnable 1
		WorldRender.MotionBlurEnable 0
		WorldRender.FastHdrEnable 1
		Render.RenderScaleResampleMode ScaleResampleMode_BicubicSharp
		WorldRender.TranslucencyAutoThicknessEnable 1
	]=],
	[Quality.High]=[=[
		PostProcess.SpriteDofEnable 0
		PostProcess.DofMethod DofMethod_Gaussian
		PostProcess.ScreenSpaceRaytraceEnable 1
		PostProcess.ScreenSpaceRaytraceFullresEnable 1
		PostProcess.BlurMethod BlurMethod_Gaussian
		PostProcess.BlurPyramidQuarterResEnable 0
		WorldRender.DistortionEnable 1
		WorldRender.MotionBlurEnable 1
		WorldRender.TiledMotionBlurEnable 0
		WorldRender.MotionBlurMaxSampleCount 16
		WorldRender.FastHdrEnable 1
		Render.RenderScaleResampleMode ScaleResampleMode_BicubicSharp
		WorldRender.TranslucencyAutoThicknessEnable 1
	]=],
	[Quality.Ultra]=[=[
		PostProcess.SpriteDofEnable 0
		PostProcess.DofMethod BlurMethod_Gaussian
		PostProcess.ScreenSpaceRaytraceEnable 1
		PostProcess.ScreenSpaceRaytraceQuality 2
		PostProcess.BlurMethod BlurMethod_Gaussian
		PostProcess.BlurPyramidQuarterResEnable 0
		WorldRender.DistortionEnable 1
		WorldRender.MotionBlurEnable 1
		WorldRender.TiledMotionBlurEnable 0
		WorldRender.MotionBlurMaxSampleCount 20
		WorldRender.FastHdrEnable 1
		Render.RenderScaleResampleMode ScaleResampleMode_BicubicSharp
		WorldRender.TranslucencyAutoThicknessEnable 1
	]=],
})


------ Mesh Quality
applyQualitySettings('MeshQuality', {
	[Quality.Low]=[=[
		WorldRender.CullScreenAreaScale 0.7
		Mesh.GlobalLodScale 0.425
		Mesh.ShadowDistanceScale 0.425
		Mesh.TessellationEnable 0
		VegetationSystem.MaxActiveDistance 50
		VegetationSystem.MaxPreSimsPerJob 2
		VegetationSystem.SimulationMemKbClient 2048
		Render.EdgeModelViewDistance 50
		WaterInteract.WaterQualityLevel Low
		Mesh.ProceduralAnimationMaxDistance 25
	]=],
	[Quality.Medium]=[=[
		WorldRender.CullScreenAreaScale 0.9
		Mesh.GlobalLodScale 0.75
		Mesh.ShadowDistanceScale 0.65
		Mesh.TessellationEnable 0
		VegetationSystem.MaxActiveDistance 50
		VegetationSystem.MaxPreSimsPerJob 3
		VegetationSystem.SimulationMemKbClient 2048
		Render.EdgeModelViewDistance 70
		WaterInteract.WaterQualityLevel Medium
		Mesh.ProceduralAnimationMaxDistance 25
	]=],
	[Quality.High]=[=[
		WorldRender.CullScreenAreaScale 1.2
		Mesh.GlobalLodScale 1.2
		Mesh.ShadowDistanceScale 1
		Mesh.TessellationEnable 1
		Mesh.TessellationMaxFactor 7
		Mesh.TessellationMaxDistance 6
		VegetationSystem.MaxActiveDistance 100
		VegetationSystem.MaxPreSimsPerJob 4
		VegetationSystem.SimulationMemKbClient 4096
		Render.EdgeModelViewDistance 100
		WaterInteract.WaterQualityLevel High
		Mesh.ProceduralAnimationMaxDistance 50
	]=],
	[Quality.Ultra]=[=[
		WorldRender.CullScreenAreaScale 2.2
		Mesh.GlobalLodScale 1.75
		Mesh.ShadowDistanceScale 1
		Mesh.TessellationEnable 1
		Mesh.TessellationMaxFactor 9
		Mesh.TessellationMaxDistance 8
		VisualTerrain.MeshScatteringDistanceScaleFactor 1.2
		VegetationSystem.MaxActiveDistance 150
		VegetationSystem.MaxPreSimsPerJob 8
		VegetationSystem.SimulationMemKbClient 4096
		Render.EdgeModelViewDistance 150
		WaterInteract.WaterQualityLevel High
		Mesh.ProceduralAnimationMaxDistance 100
	]=],
})


------ Terrain Quality
applyQualitySettings('TerrainQuality', {
	[Quality.Low]=[=[
		VisualTerrain.DetailDisplacementEnable 0
		VisualTerrain.TessellatedTriWidth 12
		VisualTerrain.LodScale 1.0
		VisualTerrain.TerrainShadowsQualityLevel QualityLevel_Low
		VisualTerrain.DetailDisplacementQualityLevel Low
	]=],
	[Quality.Medium]=[=[
		VisualTerrain.DetailDisplacementEnable 1
		VisualTerrain.TessellatedTriWidth 12
		VisualTerrain.LodScale 1.1
		VisualTerrain.TerrainShadowsQualityLevel QualityLevel_Medium
		VisualTerrain.DetailDisplacementQualityLevel Medium
		VisualTerrain.DetailDisplacementFadeRange 5
	]=],
	-- Only on DX11 cards
	[Quality.High]=[=[
		VisualTerrain.DetailDisplacementEnable 1
		VisualTerrain.TessellatedTriWidth 12
		VisualTerrain.LodScale 1.1
		VisualTerrain.TerrainShadowsQualityLevel QualityLevel_high
		VisualTerrain.DetailDisplacementQualityLevel High
		VisualTerrain.DetailDisplacementFadeRange 5
	]=],
	[Quality.Ultra]=[=[
		VisualTerrain.DetailDisplacementEnable 1
		VisualTerrain.TessellatedTriWidth 8
		VisualTerrain.LodScale 1.1
		VisualTerrain.TerrainShadowsQualityLevel QualityLevel_Ultra
		VisualTerrain.DetailDisplacementQualityLevel Ultra
		VisualTerrain.DetailDisplacementFadeRange 5
	]=],
})


------ Terrain Groundcover
applyQualitySettings('TerrainGroundcover', {
	[Quality.Low]=[=[
		VisualTerrain.MeshScatteringBuildChannelCount 2
		VisualTerrain.MeshScatteringBuildChannelsLaunchedPerFrameCountMax 1
		VisualTerrain.MeshScatteringDensityScaleFactor 0.5
		VisualTerrain.MeshScatteringDistanceScaleFactor 0.43
		VisualTerrain.MeshScatteringInstancesPerCellMax 2048
		VisualTerrain.MeshScatteringShadowViewCullSize 0.1
		VisualTerrain.MeshScatteringQualityLevel Low
	]=],
	[Quality.Medium]=[=[
		VisualTerrain.MeshScatteringBuildChannelCount 4
		VisualTerrain.MeshScatteringBuildChannelsLaunchedPerFrameCountMax 2
		VisualTerrain.MeshScatteringDensityScaleFactor 0.6
		VisualTerrain.MeshScatteringDistanceScaleFactor 0.57
		VisualTerrain.MeshScatteringInstancesPerCellMax 3072
		VisualTerrain.MeshScatteringShadowViewCullSize 0.05
		VisualTerrain.MeshScatteringQualityLevel Medium
	]=],
	[Quality.High]=[=[
		VisualTerrain.MeshScatteringBuildChannelCount 8
		VisualTerrain.MeshScatteringBuildChannelsLaunchedPerFrameCountMax 4
		VisualTerrain.MeshScatteringDensityScaleFactor 0.75
		VisualTerrain.MeshScatteringDistanceScaleFactor 0.74
		VisualTerrain.MeshScatteringInstancesPerCellMax 4096
		VisualTerrain.MeshScatteringShadowViewCullSize 0.03
		VisualTerrain.MeshScatteringQualityLevel High
	]=],
	[Quality.Ultra]=[=[
		VisualTerrain.MeshScatteringBuildChannelCount 8
		VisualTerrain.MeshScatteringBuildChannelsLaunchedPerFrameCountMax 4
		VisualTerrain.MeshScatteringDensityScaleFactor 1
		VisualTerrain.MeshScatteringDistanceScaleFactor 1
		VisualTerrain.MeshScatteringInstancesPerCellMax 4096
		VisualTerrain.MeshScatteringShadowViewCullSize 0.01
		VisualTerrain.MeshScatteringQualityLevel Ultra
	]=],
})


------ AntiAliasingPost
applyQualitySettings('AntiAliasingPost', {
	[AntiAliasingPost.Off]=[=[
		WorldRender.PostProcessAntialiasingMode PostProcessAAMode_None
	]=],
	[AntiAliasingPost.Low]=[=[
		WorldRender.PostProcessAntialiasingMode PostProcessAAMode_TemporalAA
		WorldRender.TemporalAAQuality 0
	]=],
	[AntiAliasingPost.High]=[=[
		WorldRender.PostProcessAntialiasingMode PostProcessAAMode_TemporalAA
		WorldRender.TemporalAAQuality 1
	]=],
})


------ Ambient Occlusion
applyQualitySettings('AmbientOcclusion', {
	[AmbientOcclusion.Off]=[=[
		PostProcess.DynamicAOEnable 0
	]=],
	[AmbientOcclusion.AdvancedAO]=[=[
		PostProcess.DynamicAOEnable 1
		PostProcess.DynamicAOMethod DynamicAOMethod_AdvancedAO
	]=],
	[AmbientOcclusion.HBAO]=[=[
		PostProcess.DynamicAOEnable 1
		PostProcess.DynamicAOMethod DynamicAOMethod_HBAO
		PostProcess.DynamicAOHorizonBased true
		PostProcess.DynamicAOTemporalFilterEnable 0
		PostProcess.DynamicAOSampleDirCount 4
		PostProcess.DynamicAOSampleStepCount 2
		PostProcess.DynamicAOBilateralBlurRadius 6
	]=],
	[AmbientOcclusion.HBAOFull]=[=[
		PostProcess.DynamicAOEnable 1
		PostProcess.DynamicAOMethod DynamicAOMethod_HBAO
		PostProcess.DynamicAOHorizonBased true
		PostProcess.DynamicAOTemporalFilterEnable 0
		PostProcess.DynamicAOSampleDirCount  5
		PostProcess.DynamicAOSampleStepCount 5
		PostProcess.DynamicAOBilateralBlurRadius 6
		PostProcess.DynamicAOHalfResEnable 0
	]=],
})

------ Memory
applyQualitySettings('GPUMemory', {
	[Quality.Low]=[=[
		Texture.SkipMipmapCount 2
		TextureStreaming.PoolSize 6942069
		MeshStreaming.PoolSize 6942069
		VisualTerrain.TextureAtlasSampleCountXFactor 1
		VisualTerrain.TextureAtlasSampleCountYFactor 1
		TerrainStreaming.HeightfieldAtlasSampleCountXFactor 1
		TerrainStreaming.HeightfieldAtlasSampleCountYFactor 1
		TerrainStreaming.MaskAtlasSampleCountXFactor 1
		TerrainStreaming.MaskAtlasSampleCountYFactor 1
		TerrainStreaming.ColorAtlasSampleCountXFactor 1
		TerrainStreaming.ColorAtlasSampleCountYFactor 1
		VisualTerrain.DetailDisplacementEnable 0
		WorldRender.SkyEnvmapResolution 128
		WorldRender.LocalIBLResolution 128 -- Needs to be the same resolution on all configs - baked LRVs don't support different resolutions
		WorldRender.LocalIBLShadowmapSliceCount	3
		WorldRender.LocalIBLShadowmapResolution 256
		DynamicTextureAtlas.EmitterBaseWidth 6144
		DynamicTextureAtlas.EmitterBaseHeight 4096
		DynamicTextureAtlas.EmitterBaseMipmapCount 5
		DynamicTextureAtlas.EmitterBaseSkipmipsCount 1
		DynamicTextureAtlas.EmitterNormalWidth 512
		DynamicTextureAtlas.EmitterNormalHeight 512
		DynamicTextureAtlas.EmitterNormalMipmapCount 4
		DynamicTextureAtlas.EmitterNormalSkipmipsCount 1
		Decal.MaxDecalObjectPrims 10000
	]=],
	[Quality.Medium]=[=[
		Texture.SkipMipmapCount 1
		TextureStreaming.PoolSize 6942069
		MeshStreaming.PoolSize 6942069
		VisualTerrain.TextureAtlasSampleCountXFactor 1
		VisualTerrain.TextureAtlasSampleCountYFactor 1
		TerrainStreaming.HeightfieldAtlasSampleCountXFactor 1
		TerrainStreaming.HeightfieldAtlasSampleCountYFactor 1
		TerrainStreaming.MaskAtlasSampleCountXFactor 1
		TerrainStreaming.MaskAtlasSampleCountYFactor 1
		TerrainStreaming.ColorAtlasSampleCountXFactor 1
		TerrainStreaming.ColorAtlasSampleCountYFactor 1
		VisualTerrain.DetailDisplacementEnable 1
		WorldRender.SkyEnvmapResolution 256
		WorldRender.LocalIBLResolution 128 -- Needs to be the same resolution on all configs - baked LRVs don't support different resolutions
		WorldRender.LocalIBLShadowmapSliceCount	4
		WorldRender.LocalIBLShadowmapResolution 512
		DynamicTextureAtlas.EmitterBaseWidth 6144
		DynamicTextureAtlas.EmitterBaseHeight 4096
		DynamicTextureAtlas.EmitterBaseMipmapCount 5
		DynamicTextureAtlas.EmitterBaseSkipmipsCount 1
		DynamicTextureAtlas.EmitterNormalWidth 512
		DynamicTextureAtlas.EmitterNormalHeight 512
		DynamicTextureAtlas.EmitterNormalMipmapCount 4
		DynamicTextureAtlas.EmitterNormalSkipmipsCount 1
		Decal.MaxDecalObjectPrims 20000
	]=],	
	[Quality.High]=[=[
		Texture.SkipMipmapCount 0
		TextureStreaming.PoolSize 6942069
		MeshStreaming.PoolSize 6942069
		VisualTerrain.TextureAtlasSampleCountXFactor 1
		VisualTerrain.TextureAtlasSampleCountYFactor 2
		TerrainStreaming.HeightfieldAtlasSampleCountXFactor 1
		TerrainStreaming.HeightfieldAtlasSampleCountYFactor 1
		TerrainStreaming.MaskAtlasSampleCountXFactor 1
		TerrainStreaming.MaskAtlasSampleCountYFactor 1
		TerrainStreaming.ColorAtlasSampleCountXFactor 1
		TerrainStreaming.ColorAtlasSampleCountYFactor 1
		VisualTerrain.DetailDisplacementEnable 1
		WorldRender.SkyEnvmapResolution 512
		WorldRender.LocalIBLResolution 128 -- Needs to be the same resolution on all configs - baked LRVs don't support different resolutions
		WorldRender.LocalIBLShadowmapSliceCount	4
		WorldRender.LocalIBLShadowmapResolution 512
		DynamicTextureAtlas.EmitterBaseWidth 12288
		DynamicTextureAtlas.EmitterBaseHeight 8192
		DynamicTextureAtlas.EmitterBaseMipmapCount 6
		DynamicTextureAtlas.EmitterBaseSkipmipsCount 0
		DynamicTextureAtlas.EmitterNormalWidth 1024
		DynamicTextureAtlas.EmitterNormalHeight 1024
		DynamicTextureAtlas.EmitterNormalMipmapCount 5
		DynamicTextureAtlas.EmitterNormalSkipmipsCount 0
		Decal.MaxDecalObjectPrims 30000
	]=],
	[Quality.Ultra]=[=[
		Texture.SkipMipmapCount 0
		TextureStreaming.PoolSize 6942069
		MeshStreaming.PoolSize 6942069
		VisualTerrain.TextureAtlasSampleCountXFactor 2
		VisualTerrain.TextureAtlasSampleCountYFactor 2
		TerrainStreaming.HeightfieldAtlasSampleCountXFactor 1
		TerrainStreaming.HeightfieldAtlasSampleCountYFactor 1
		TerrainStreaming.MaskAtlasSampleCountXFactor 1
		TerrainStreaming.MaskAtlasSampleCountYFactor 1
		TerrainStreaming.ColorAtlasSampleCountXFactor 1
		TerrainStreaming.ColorAtlasSampleCountYFactor 1
		VisualTerrain.DetailDisplacementEnable 1
		WorldRender.SkyEnvmapResolution 512
		WorldRender.LocalIBLResolution 128 -- Needs to be the same resolution on all configs - baked LRVs don't support different resolutions
		WorldRender.LocalIBLShadowmapSliceCount	4
		WorldRender.LocalIBLShadowmapResolution 512
		DynamicTextureAtlas.EmitterBaseWidth 12288
		DynamicTextureAtlas.EmitterBaseHeight 8192
		DynamicTextureAtlas.EmitterBaseMipmapCount 6
		DynamicTextureAtlas.EmitterBaseSkipmipsCount 0
		DynamicTextureAtlas.EmitterNormalWidth 1024
		DynamicTextureAtlas.EmitterNormalHeight 1024
		DynamicTextureAtlas.EmitterNormalMipmapCount 5
		DynamicTextureAtlas.EmitterNormalSkipmipsCount 0
		Decal.MaxDecalObjectPrims 60000
	]=],
})


------ Anisotropic filtering
ShaderSystem = ShaderSystem or {}
ShaderSystem.MaxAnisotropy = 2^settings['AnisotropicFilter'] -- 1, 2, 4, 8, 16


------- Motion Blur
WorldRender = WorldRender or {}
WorldRender.MotionBlurEnable = settings['MotionBlurEnabled']
WorldRender.MotionBlurScale = (settings['MotionBlur'] or 0.5) 


------- Screen resolution
RenderDevice = RenderDevice or {}
RenderDevice.FullscreenWidth = settings['ResolutionWidth']
RenderDevice.FullscreenHeight = settings['ResolutionHeight']
RenderDevice.FullscreenRefreshRate = settings['FullscreenRefreshRate']
RenderDevice.Fullscreen = settings['FullscreenEnabled']
RenderDevice.FullscreenOutputIndex = settings['FullscreenScreen']
RenderDevice.WindowBordersEnable = settings['WindowBordersEnable']
RenderDevice.VSyncEnable = settings['VSyncEnabled']
Render.VsyncEnable = settings['VSyncEnabled']

if settings['EnableHDR'] == 0 then
	applySettings("RenderDevice.DisplayDynamicRange DisplayDynamicRange_SDR")
else
	applySettings("RenderDevice.DisplayDynamicRange DisplayDynamicRange_Auto")
end

-------- Resolution scale
Render.ResolutionScale = settings['ResolutionScale']
Render.DynamicResolutionScaleEnable = settings['EnableDynamicResolution']

-------- Brightness
PostProcess.UIBrightnessNorm = settings['Brightness']

Render.FovMultiplier = settings['FieldOfView']

-------- PC vendor specific settings
if dice.getCurrentPlatformName() == 'Win32' then
	
	if rendererType == "RendererType_Dx12" then
		if detectedVendorId == Vendors.Intel then
			applySettings [=[
				Render.DynamicResolutionScaleTargetTime 33.3
			]=]
		end
	end

--	TODO test these on AMD - might be valid for them also. especially LightTileAsyncComputeCulling 0
	if detectedVendorId == Vendors.Nvidia then
		applySettings [=[
			WorldRender.LightTileAsyncComputeCulling 0
		]=]
	end

end

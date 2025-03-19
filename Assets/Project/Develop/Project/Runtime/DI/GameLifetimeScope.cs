using Runtime.Attributes;
using Runtime.Data.Constants.Enums;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.DI.Core;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Factories.Core;
using Runtime.Infrastructure.Services;
using Runtime.Infrastructure.Services.App;
using Runtime.Infrastructure.Services.AssetsProvider.Containers;
using Runtime.Infrastructure.Services.AssetsProvider.Containers.Core;
using Runtime.Infrastructure.Services.Input;
using Runtime.Infrastructure.Services.Localization;
using Runtime.Infrastructure.Services.SaveProgressServices;
using Runtime.Infrastructure.Services.UIServices;
using Runtime.Visual.UI.UIDocumentWrappers.Screens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Runtime.MonoBehaviours.LifetimeScopes
{
	[DefaultExecutionOrder(-99)]
	internal sealed class GameLifetimeScope : CustomLifetimeScope
	{
		[Header("Assets Containers")]
		[SerializeField]
		private UIDocumentInfoAssetsContainer<ScreenType> _screensInfoAssetsContainer;
		[SerializeField]
		private UIDocumentInfoAssetsContainer<PopupType> _popupsInfoAssetsContainer;
		[SerializeField]
		private PrefabAssetsContainer<VFXType, GameObject> _vfxPrefabAssetsContainer;
		[SerializeField]
		private PrefabAssetsContainer<PrefabType, GameObject> _prefabAssetsContainer;
		[SerializeField]
		private PrefabAssetsContainer<TemplateID, VisualTreeAsset> _templateAssetsContainer;
		[Header("Configs")]
		/*		[SerializeField]
				private InventoryConfig _inventoryConfig;
				[SerializeField]
				private ProgressConfig _progressConfig;
				[SerializeField]
				private LightingConfig _lightingConfig;
				[SerializeField]
				private GraphicsConfig _graphicsConfig;
				[SerializeField]
				private CameraConfig _cameraConfig;
				[SerializeField]
				private QuestsConfig _questsConfig;
				[SerializeField]
				private EnemyConfig _enemyConfig;
				[SerializeField]
				private AudioConfig _audioConfig;
				[SerializeField]
				private InputConfig _inputConfig;
				[SerializeField]
				private GameConfig _gameConfig;
				[SerializeField]
				private FogConfig _fogConfig;
				[SerializeField]
				private UIConfig _uiConfig;*/
		[Header("Components")]
		[SerializeField]
		private UIDocument _loadingScreenUIDocument;
		[SerializeField]
		[GetComponent]
		private LoopsService _loopsService;
		/*		[SerializeField]
				[GetComponent]
				private TimerService _timerManager;*/
		[SerializeField]
		private EventSystem _eventSystem;

		private LoadingScreen _loadingScreen;


		protected override void Configure(IContainerBuilder containerBuilder)
		{
			base.Configure(containerBuilder);

			_loadingScreen = new LoadingScreen(_loadingScreenUIDocument);

			containerBuilder.RegisterBuildCallback(objectResolver =>
			{
				objectResolver.Inject(_loadingScreen);
				//objectResolver.Inject(_progressConfig);
			});

			RegisterAssetsContainers(containerBuilder);
			RegisterFactories(containerBuilder);
			RegisterServices(containerBuilder);
			RegisterConfigs(containerBuilder);

			DontDestroyOnLoad(gameObject);
		}

		private void RegisterAssetsContainers(IContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterInstance(_screensInfoAssetsContainer).As<IUIDocumentInfoAssetsContainer<ScreenType>>();
			containerBuilder.RegisterInstance(_popupsInfoAssetsContainer).As<IUIDocumentInfoAssetsContainer<PopupType>>();
			containerBuilder.RegisterInstance(_prefabAssetsContainer).As<IAsyncAssetsContainer<PrefabType, GameObject>>();
			containerBuilder.RegisterInstance(_vfxPrefabAssetsContainer).As<IAsyncAssetsContainer<VFXType, GameObject>>();
			containerBuilder.RegisterInstance(_templateAssetsContainer).As<IAsyncAssetsContainer<TemplateID, VisualTreeAsset>>();
		}

		private void RegisterFactories(IContainerBuilder containerBuilder)
		{
			containerBuilder.Register<UIDocumentsFactory<ScreenType>>(Lifetime.Singleton).As<IUIDocumentsFactory<ScreenType>>();
			containerBuilder.Register<UIDocumentsFactory<PopupType>>(Lifetime.Singleton).As<IUIDocumentsFactory<PopupType>>();
			/*			containerBuilder.Register<ComponentsFactory>(Lifetime.Singleton).As<IComponentsFactory>();
						containerBuilder.Register<TemplatesFactory>(Lifetime.Singleton).As<ITemplatesFactory>();
			*/
			containerBuilder.Register<ProgressFactory>(Lifetime.Singleton).As<IProgressFactory>();
			containerBuilder.UseEntryPoints(ConfigureEntryPoints);

			return;

			void ConfigureEntryPoints(EntryPointsBuilder entryPointsBuilder)
			{
				/*				entryPointsBuilder.Add<PrefabsFactory<PrefabType, GameObject>>();
								entryPointsBuilder.Add<PrefabsFactory<VFXType, GameObject>>();
				*/
			}
		}

		private void RegisterServices(IContainerBuilder containerBuilder)
		{
			containerBuilder.Register<PopupsService>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(transform);
			containerBuilder.Register<LocalizationService>(Lifetime.Singleton).AsImplementedInterfaces();
			containerBuilder.Register<SystemRandomService>(Lifetime.Singleton).AsImplementedInterfaces();
			containerBuilder.Register<FileSystemService>(Lifetime.Singleton).AsImplementedInterfaces();
			containerBuilder.Register<SceneLoadService>(Lifetime.Singleton).AsImplementedInterfaces();
			containerBuilder.Register<Pauseable>(Lifetime.Singleton).AsImplementedInterfaces();

			containerBuilder.RegisterComponent(_loopsService).AsImplementedInterfaces();
			//containerBuilder.RegisterComponent(_timerManager).AsImplementedInterfaces();
			containerBuilder.UseEntryPoints(ConfigureEntryPoints);

			return;

			void ConfigureEntryPoints(EntryPointsBuilder entryPointsBuilder)
			{
				entryPointsBuilder.Add<ScreensService>().WithParameter(_loadingScreen).WithParameter(transform);
				entryPointsBuilder.Add<PersistentProgressService>();
				entryPointsBuilder.Add<InputService>();
				entryPointsBuilder.Add<GameService>();

#if UNITY_EDITOR
				entryPointsBuilder.Add<EditorApplicationService>();
#else
				entryPointsBuilder.Add<RuntimeApplicationService>();
#endif
			}
		}

		private void RegisterConfigs(IContainerBuilder containerBuilder)
		{
			/*			containerBuilder.RegisterInstance(_inventoryConfig).AsImplementedInterfaces();
						containerBuilder.RegisterInstance(_lightingConfig).AsImplementedInterfaces();
						containerBuilder.RegisterInstance(_progressConfig).AsImplementedInterfaces();
						containerBuilder.RegisterInstance(_graphicsConfig).AsImplementedInterfaces();
						containerBuilder.RegisterInstance(_questsConfig).AsImplementedInterfaces();
						containerBuilder.RegisterInstance(_cameraConfig).AsImplementedInterfaces();
						containerBuilder.RegisterInstance(_inputConfig).AsImplementedInterfaces();
						containerBuilder.RegisterInstance(_audioConfig).AsImplementedInterfaces();
						containerBuilder.RegisterInstance(_gameConfig).AsImplementedInterfaces();
						containerBuilder.RegisterInstance(_fogConfig).AsImplementedInterfaces();
						containerBuilder.RegisterInstance(_uiConfig).AsImplementedInterfaces();*/
		}
	}
}
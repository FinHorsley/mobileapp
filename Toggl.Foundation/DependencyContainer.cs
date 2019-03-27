﻿using System;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using Toggl.Foundation.Analytics;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Diagnostics;
using Toggl.Foundation.Interactors;
using Toggl.Foundation.Login;
using Toggl.Foundation.Services;
using Toggl.Foundation.Shortcuts;
using Toggl.Foundation.Suggestions;
using Toggl.Foundation.Sync;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;
using Toggl.PrimeRadiant;
using Toggl.Ultrawave;
using Toggl.Ultrawave.Network;
using Toggl.PrimeRadiant.Settings;
using Toggl.Foundation.Autocomplete;

namespace Toggl.Foundation
{
    public abstract class DependencyContainer
    {
        private readonly UserAgent userAgent;
        private readonly CompositeDisposable disposeBag = new CompositeDisposable();

        // Require recreation during login/logout
        private Lazy<ITogglApi> api;
        private Lazy<ISyncManager> syncManager;
        private Lazy<IInteractorFactory> interactorFactory;

        // Normal dependencies
        private readonly Lazy<IApiFactory> apiFactory;
        private readonly Lazy<ITogglDatabase> database;
        private readonly Lazy<ITimeService> timeService;
        private readonly Lazy<IPlatformInfo> platformInfo;
        private readonly Lazy<ITogglDataSource> dataSource;
        private readonly Lazy<IGoogleService> googleService;
        private readonly Lazy<IRatingService> ratingService;
        private readonly Lazy<ICalendarService> calendarService;
        private readonly Lazy<ILicenseProvider> licenseProvider;
        private readonly Lazy<IUserPreferences> userPreferences;
        private readonly Lazy<IRxActionFactory> rxActionFactory;
        private readonly Lazy<IAnalyticsService> analyticsService;
        private readonly Lazy<IBackgroundService> backgroundService;
        private readonly Lazy<ISchedulerProvider> schedulerProvider;
        private readonly Lazy<IStopwatchProvider> stopwatchProvider;
        private readonly Lazy<INotificationService> notificationService;
        private readonly Lazy<IRemoteConfigService> remoteConfigService;
        private readonly Lazy<IAutocompleteProvider> autocompleteProvider;
        private readonly Lazy<IErrorHandlingService> errorHandlingService;
        private readonly Lazy<ILastTimeUsageStorage> lastTimeUsageStorage;
        private readonly Lazy<IApplicationShortcutCreator> shortcutCreator;
        private readonly Lazy<IBackgroundSyncService> backgroundSyncService;
        private readonly Lazy<IIntentDonationService> intentDonationService;
        private readonly Lazy<IAutomaticSyncingService> automaticSyncingService;
        private readonly Lazy<ISyncErrorHandlingService> syncErrorHandlingService;
        private readonly Lazy<IPrivateSharedStorageService> privateSharedStorageService;
        private readonly Lazy<ISuggestionProviderContainer> suggestionProviderContainer;
        
        // Non lazy
        public virtual IUserAccessManager UserAccessManager { get; }
        public ApiEnvironment ApiEnvironment { get; }

        public ISyncManager SyncManager => syncManager.Value;
        public IInteractorFactory InteractorFactory => interactorFactory.Value;

        public IApiFactory ApiFactory => apiFactory.Value;
        public ITogglDatabase Database => database.Value;
        public ITimeService TimeService => timeService.Value;
        public IPlatformInfo PlatformInfo => platformInfo.Value;
        public ITogglDataSource DataSource => dataSource.Value;
        public IGoogleService GoogleService => googleService.Value;
        public IRatingService RatingService => ratingService.Value;
        public ICalendarService CalendarService => calendarService.Value;
        public ILicenseProvider LicenseProvider => licenseProvider.Value;
        public IUserPreferences UserPreferences => userPreferences.Value;
        public IRxActionFactory RxActionFactory => rxActionFactory.Value;
        public IAnalyticsService AnalyticsService => analyticsService.Value;
        public IStopwatchProvider StopwatchProvider => stopwatchProvider.Value;
        public IBackgroundService BackgroundService => backgroundService.Value;
        public ISchedulerProvider SchedulerProvider => schedulerProvider.Value;
        public IApplicationShortcutCreator ShortcutCreator => shortcutCreator.Value;
        public INotificationService NotificationService => notificationService.Value;
        public IRemoteConfigService RemoteConfigService => remoteConfigService.Value;
        public IAutocompleteProvider AutocompleteProvider => autocompleteProvider.Value;
        public IErrorHandlingService ErrorHandlingService => errorHandlingService.Value;
        public ILastTimeUsageStorage LastTimeUsageStorage => lastTimeUsageStorage.Value;
        public IBackgroundSyncService BackgroundSyncService => backgroundSyncService.Value;
        public IIntentDonationService IntentDonationService => intentDonationService.Value;
        public IAutomaticSyncingService AutomaticSyncingService => automaticSyncingService.Value;
        public ISyncErrorHandlingService SyncErrorHandlingService => syncErrorHandlingService.Value;
        public IPrivateSharedStorageService PrivateSharedStorageService => privateSharedStorageService.Value;
        public ISuggestionProviderContainer SuggestionProviderContainer => suggestionProviderContainer.Value;

        protected DependencyContainer(ApiEnvironment apiEnvironment, UserAgent userAgent)
        {
            this.userAgent = userAgent;

            ApiEnvironment = apiEnvironment;


            database = new Lazy<ITogglDatabase>(CreateDatabase);
            apiFactory = new Lazy<IApiFactory>(CreateApiFactory);
            syncManager = new Lazy<ISyncManager>(CreateSyncManager);
            timeService = new Lazy<ITimeService>(CreateTimeService);
            dataSource = new Lazy<ITogglDataSource>(CreateDataSource);
            platformInfo = new Lazy<IPlatformInfo>(CreatePlatformInfo);
            googleService = new Lazy<IGoogleService>(CreateGoogleService);
            ratingService = new Lazy<IRatingService>(CreateRatingService);
            calendarService = new Lazy<ICalendarService>(CreateCalendarService);
            licenseProvider = new Lazy<ILicenseProvider>(CreateLicenseProvider);
            rxActionFactory = new Lazy<IRxActionFactory>(CreateRxActionFactory);
            userPreferences = new Lazy<IUserPreferences>(CreateUserPreferences);
            analyticsService = new Lazy<IAnalyticsService>(CreateAnalyticsService);
            interactorFactory = new Lazy<IInteractorFactory>(CreateInteractorFactory);
            stopwatchProvider = new Lazy<IStopwatchProvider>(CreateStopwatchProvider);
            backgroundService = new Lazy<IBackgroundService>(CreateBackgroundService);
            schedulerProvider = new Lazy<ISchedulerProvider>(CreateSchedulerProvider);
            shortcutCreator = new Lazy<IApplicationShortcutCreator>(CreateShortcutCreator);
            notificationService = new Lazy<INotificationService>(CreateNotificationService);
            remoteConfigService = new Lazy<IRemoteConfigService>(CreateRemoteConfigService);
            autocompleteProvider = new Lazy<IAutocompleteProvider>(CreateAutocompleteProvider);
            errorHandlingService = new Lazy<IErrorHandlingService>(CreateErrorHandlingService);
            lastTimeUsageStorage = new Lazy<ILastTimeUsageStorage>(CreateLastTimeUsageStorage);
            backgroundSyncService = new Lazy<IBackgroundSyncService>(CreateBackgroundSyncService);
            intentDonationService = new Lazy<IIntentDonationService>(CreateIntentDonationService);
            automaticSyncingService = new Lazy<IAutomaticSyncingService>(CreateAutomaticSyncingService);
            syncErrorHandlingService = new Lazy<ISyncErrorHandlingService>(CreateSyncErrorHandlingService);
            privateSharedStorageService = new Lazy<IPrivateSharedStorageService>(CreatePrivateSharedStorageService);
            suggestionProviderContainer = new Lazy<ISuggestionProviderContainer>(CreateSuggestionProviderContainer);

            api = apiFactory.Select(factory => factory.CreateApiWith(Credentials.None));
            UserAccessManager = new UserAccessManager(
                apiFactory,
                database,
                googleService,
                privateSharedStorageService);

            UserAccessManager
                .UserLoggedIn
                .Subscribe(recreateLazyDependenciesForLogin)
                .DisposedBy(disposeBag);

            UserAccessManager
                .UserLoggedOut
                .Subscribe(_ => recreateLazyDependenciesForLogout())
                .DisposedBy(disposeBag);
        }
        
        protected abstract ITogglDatabase CreateDatabase();
        protected abstract IPlatformInfo CreatePlatformInfo();
        protected abstract IGoogleService CreateGoogleService();
        protected abstract IRatingService CreateRatingService();
        protected abstract ICalendarService CreateCalendarService();
        protected abstract ILicenseProvider CreateLicenseProvider();
        protected abstract IUserPreferences CreateUserPreferences();
        protected abstract IAnalyticsService CreateAnalyticsService();
        protected abstract IStopwatchProvider CreateStopwatchProvider();
        protected abstract ISchedulerProvider CreateSchedulerProvider();
        protected abstract INotificationService CreateNotificationService();
        protected abstract IRemoteConfigService CreateRemoteConfigService();
        protected abstract IErrorHandlingService CreateErrorHandlingService();
        protected abstract ILastTimeUsageStorage CreateLastTimeUsageStorage();
        protected abstract IApplicationShortcutCreator CreateShortcutCreator();
        protected abstract IBackgroundSyncService CreateBackgroundSyncService();
        protected abstract IIntentDonationService CreateIntentDonationService();
        protected abstract IPrivateSharedStorageService CreatePrivateSharedStorageService();
        protected abstract ISuggestionProviderContainer CreateSuggestionProviderContainer();

        protected virtual ITimeService CreateTimeService()
            => new TimeService(SchedulerProvider.DefaultScheduler);

        protected virtual IBackgroundService CreateBackgroundService()
            => new BackgroundService(TimeService, AnalyticsService);

        protected virtual IAutomaticSyncingService CreateAutomaticSyncingService()
            => new AutomaticSyncingService(BackgroundService, TimeService);

        protected virtual ISyncErrorHandlingService CreateSyncErrorHandlingService()
            => new SyncErrorHandlingService(ErrorHandlingService);

        protected virtual IAutocompleteProvider CreateAutocompleteProvider()
            => new AutocompleteProvider(InteractorFactory);

        protected virtual ITogglDataSource CreateDataSource()
            => new TogglDataSource(Database, TimeService, AnalyticsService);

        protected virtual IRxActionFactory CreateRxActionFactory()
            => new RxActionFactory(SchedulerProvider);

        protected virtual IApiFactory CreateApiFactory()
            => new ApiFactory(ApiEnvironment, userAgent);

        protected virtual ISyncManager CreateSyncManager()
        {
            var syncManager = TogglSyncManager.CreateSyncManager(
                Database,
                api.Value,
                DataSource,
                TimeService,
                AnalyticsService,
                LastTimeUsageStorage,
                SchedulerProvider.DefaultScheduler,
                StopwatchProvider,
                AutomaticSyncingService
            );
            SyncErrorHandlingService.HandleErrorsOf(syncManager);

            syncManager.ForceFullSync().GetAwaiter().GetResult();
            return syncManager;
        }

        protected virtual IInteractorFactory CreateInteractorFactory() => new InteractorFactory(
            api.Value,
            UserAccessManager,
            database.Select(database => database.IdProvider),
            database,
            timeService,
            syncManager,
            platformInfo,
            dataSource,
            calendarService,
            userPreferences,
            analyticsService,
            stopwatchProvider,
            notificationService,
            lastTimeUsageStorage,
            shortcutCreator,
            intentDonationService,
            privateSharedStorageService
        );

        private void recreateLazyDependenciesForLogin(ITogglApi api)
        {
            this.api = new Lazy<ITogglApi>(() => api);

            syncManager = new Lazy<ISyncManager>(CreateSyncManager);
            interactorFactory = new Lazy<IInteractorFactory>(CreateInteractorFactory);
        }

        private void recreateLazyDependenciesForLogout()
        {
            api = apiFactory.Select(factory => factory.CreateApiWith(Credentials.None));

            syncManager = new Lazy<ISyncManager>(CreateSyncManager);
            interactorFactory = new Lazy<IInteractorFactory>(CreateInteractorFactory);
        }
    }
}